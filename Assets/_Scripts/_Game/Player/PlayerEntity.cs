using _Scripts._Game.Animation;
using _Scripts._Game.Animation.Character.Player;
using _Scripts._Game.General;
using _Scripts._Game.General.Managers;
using _Scripts._Game.Player.AttackingStateMachine;
using System.Collections;
using System.Collections.Generic;
using _Scripts._Game.Events;
using _Scripts._Game.General.Settings;
using Pathfinding.Examples;
using UnityEngine;

namespace _Scripts._Game.Player{
    
    public enum ERespawnReason
    {
        None,
        Death,
        QuickRespawn,
        OutOfBounds,
    }
    public struct RespawnReason
    {
        public bool _isRespawning;
        public ERespawnReason _reaspawnReason;
        public float _respawnTimer;
    }

    public class PlayerEntity : Singleton<PlayerEntity>, IDamageable, IDamageCauser
    {
        #region Properties
        private bool _isPossessed;
        private IPossessable _possessed; //save what we're possessing
        private RespawnReason _playerRespawnReason;

        public IPossessable Possessed { get => _possessed; }

        private Transform _playerParentTransform;
        public Transform PlayerParentTransform
        {
            get { return _playerParentTransform; }
        }

        #endregion

        #region State Machines
        private PlayerMovementStateMachine _movementSM;
        private PlayerAttackingStateMachine _attackingSM;
        private PlayerSpriteAnimator _spriteAnimator;

        public PlayerMovementStateMachine MovementSM { get => _movementSM; set => _movementSM = value; }
        public PlayerAttackingStateMachine AttackingSM { get => _attackingSM; set => _attackingSM = value; }
        public PlayerSpriteAnimator SpriteAnimator { get => _spriteAnimator; set => _spriteAnimator = value; }
        #endregion

        #region Player Components
        private PlayerHealthStats _playerHealthStats;
        private EnergyStats _playerEnergyStats;

        public PlayerHealthStats PlayerHealthStats { get => _playerHealthStats; }
        #endregion

        #region Damage Properties
        [SerializeField]
        private float _invulnerableDuration = 0.8f;
        private float _isInvulnerableTimer;
        #endregion

        //IDamageable
        private Vector2 _damageDirection = Vector2.zero;

        public Vector2 DamageDirection { get => _damageDirection; set => _damageDirection = value; }
        public Transform Transform { get => transform; }

        public bool FacingRight
        {
            get
            {
                return (!IsPossessed() ? _possessed.FacingRight : _movementSM.IsFacingRight);
            }
            
        }

        public IExposable Exposable
        {
            get => null;
        }

        protected override void Awake()
        {
            base.Awake();

            //FreezeAllMovement(true);

            _playerParentTransform = transform.parent;

            FEntityStats playerEntityStats = StatsManager.Instance.GetEntityStat(EEntity.Player);
            _playerHealthStats = new PlayerHealthStats(playerEntityStats.MaxHealth, playerEntityStats.MaxHealth);
            _playerEnergyStats = new EnergyStats(1.0f, 1.0f);
        }

        void Start()
        {
            OnPossess();
        }

        private void FixedUpdate() 
        {
            //timers
            if (_playerRespawnReason._isRespawning == true)
            {
                if ((_playerRespawnReason._respawnTimer -= Time.deltaTime) < 0.0f)
                {
                    //respawn
                    VolumeManager.Instance.OnPlayerRespawned();
                    //transform.position = new Vector3(26.0f, 146.0f, 0.0f);
                    RespawnManager.Instance.RespawnObject(gameObject);
                    _playerRespawnReason._reaspawnReason = ERespawnReason.None;
                    _playerRespawnReason._isRespawning = false;

                    FEntityStats playerEntityStats = StatsManager.Instance.GetEntityStat(EEntity.Player);
                    _playerHealthStats.AddHitPoints(playerEntityStats.MaxHealth);
                    _isInvulnerableTimer = 0.0f;

                }
            }

            if (_isInvulnerableTimer > 0.0f)
            {
                _isInvulnerableTimer -= Time.deltaTime;
            }
        }

        public GameObject GetControlledGameObject()
        {
            if (!IsPossessed())
            {
                return _possessed.Transform.gameObject; 
            }
            else
            {
                return Transform.gameObject;

            }
        }

        // IPossessable
        public bool IsPossessed()
        {
            return _isPossessed;
        }

        public bool CanBePossessed()
        {
            return !_isPossessed && _playerHealthStats.IsAlive();
        }

        public void OnPossess(bool bondPressed = false)
        {
            InputManager.Instance.TryEnableActionMap(EInputSystem.Player);
            _isPossessed = true;
            _possessed = null;

            gameObject.transform.SetParent(PlayerParentTransform);

            _movementSM.Rb.isKinematic = false;
            _movementSM.enabled = true;
            _attackingSM.enabled = true;
            _spriteAnimator.enabled = true;

            if (bondPressed)
            {
                _movementSM.OverrideState(MovementState.Phasing);
            }

            TargetManager.Instance.LockedBondableTarget = null;
        }

        public void OnDispossess()
        {
            IBondable bondableTarget = TargetManager.Instance.LockedBondableTarget;
            if (bondableTarget != null && bondableTarget.BondBehaviourType == EBondBehaviourType.Possess)
            {
                IPossessable possessable = bondableTarget.Possessable;

                if (possessable != null)
                {
                    // dispossess this player for something else
                    InputManager.Instance.TryDisableActionMap(EInputSystem.Player);
                    _isPossessed = false;

                    _possessed = possessable;
                    _possessed.OnPossess();

                    gameObject.transform.SetParent(TargetManager.Instance.LockedBondableTarget.BondTargetTransform);

                    _movementSM.Rb.isKinematic = true;
                    _movementSM.enabled = false;
                    _attackingSM.enabled = false;
                    _spriteAnimator.enabled = false;
                }

            }
        }

        public Vector2 GetMovementInput()
        {
            return (!IsPossessed() ? _possessed.GetMovementInput() : _movementSM.CurrentMovementInput);
        }

        public bool IsAlive()
        {
            return _playerHealthStats.IsAlive() && !_playerRespawnReason._isRespawning;
        }

        public void TakeDamage(EDamageType damageType, EEntityType causer, Vector3 damagePosition)
        {
            float resultsHealth = 0.0f;

            if (!CanTakeDamage())
            {
                return;
            }

            //where is the damage coming from?
            DamageDirection = (damagePosition - Transform.position).normalized; 

            if (_possessed != null)
            {
                resultsHealth = _playerEnergyStats.RemoveEnergyPoints(1.0f, false);

                if (resultsHealth <= 0.0f)
                {
                    // lost all energy - dispossess
                    _possessed.OnDispossess();
                }
            }
            else
            {
                resultsHealth = _playerHealthStats.RemoveHitPoints(1.0f, false);

                if (resultsHealth <= 0.0f)
                {
                    //dead reaction
                    if (_playerRespawnReason._isRespawning == false)
                    {
                        _playerRespawnReason._reaspawnReason = ERespawnReason.Death;
                        OnRespawnStart();
                    }
                }
            }

            AudioManager.Instance.TryPlayAudioSourceAtLocation(EAudioType.SFX_Player_TakeDamage, FollowCamera.Instance.GetAudioListener().transform.position);

            FeedbackManager.Instance.TryFeedbackPattern(_playerRespawnReason._isRespawning ? EFeedbackPattern.Game_TakeDamageHeavy : EFeedbackPattern.Game_TakeDamageLight);
            if (_playerRespawnReason._isRespawning)
            {
                VolumeManager.Instance.OnPlayerKilled();
            }
            else
            {
                VolumeManager.Instance.OnPlayerTakeDamage();
            }
            TimeManager.Instance.TryRequestTimeScale(ETimeImportance.High, 0.0f, 0.0f, 0.025f, 0.2f);
            _isInvulnerableTimer = _invulnerableDuration;
        }

        private bool CanTakeDamage()
        {
            return !DebugManager.Instance.DebugSettings.PlayerImmune && _isInvulnerableTimer <= 0.0f && IsAlive();
        }

        public void OnRespawnStart()
        {
            // kill velocity
            MovementSM.Rb.velocity = Vector2.zero;

            _playerRespawnReason._isRespawning = true;
            if (_playerRespawnReason._reaspawnReason == ERespawnReason.Death)
            {
                _playerRespawnReason._respawnTimer = 3.0f;
            }
            else
            {
                _playerRespawnReason._respawnTimer = 1.0f;
            }
        }

        public void FreezeAllMovement(bool set)
        {
            _movementSM.Rb.velocity = Vector2.zero;
            if (set)
            {
                _movementSM.Rb.gravityScale = 0.0f;
            }
            _movementSM.Capsule.isTrigger = set;

            if (!set)
            {
                _movementSM.OverrideState(MovementState.Grounded);
            }
        }
    }
    
}
