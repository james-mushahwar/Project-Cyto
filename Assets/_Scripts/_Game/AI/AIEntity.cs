using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _Scripts._Game.Player;
using _Scripts._Game.AI.MovementStateMachine;
using _Scripts._Game.Animation;
using _Scripts._Game.General;
using _Scripts._Game.General.Managers;
using _Scripts._Game.AI.AttackStateMachine;
using _Scripts._Game.General.Spawning.AI;
using System;
using _Scripts._Game.General.Settings;
using UnityEngine.Events;
using System.Runtime.CompilerServices;

namespace _Scripts._Game.AI{

    public class AIEntity : MonoBehaviour, IBondable, IPossessable, IDamageable, ITickGroup
    {
        [Header("Entity")]
        [SerializeField]
        private EEntity _entity;
        private bool _isPossessed;

        public EEntity Entity { get => _entity; }

        private string _spawnPointID = "";
        private SpawnPoint _spawnPoint;

        public string SpawnPointID { get => _spawnPointID; set => _spawnPointID = value; }

        [Header("Damage")]
        [SerializeField]
        private UnityEvent<GameObject> _onHitEvent; // hit without damage
        [SerializeField] 
        private UnityEvent<GameObject> _onTakeDamageEvent; // hit with damage

        [Header("Possess")]
        [SerializeField]
        private UnityEvent _onPossessedEvent;
        public SpawnPoint SpawnPoint
        {
            get
            {
                if (_spawnPoint == null)
                {
                    _spawnPoint = RuntimeIDManager.Instance.GetRuntimeSpawnPoint(_spawnPointID);
                }
                return _spawnPoint;
            }
        }

        #region TickGroup 
        private readonly UniqueTickGroup _uniqueTickGroup = new UniqueTickGroup();
        public UniqueTickGroup UniqueTickGroup { get => _uniqueTickGroup; }
        #endregion

        #region State Machines
        private AIMovementStateMachineBase _movementSM;
        private AIAttackStateMachineBase _attackSM;
        private SpriteAnimator _spriteAnimator;

        public AIMovementStateMachineBase MovementSM { get => _movementSM; set => _movementSM = value; }
        public AIAttackStateMachineBase AttackSM { get => _attackSM; set => _attackSM = value; }
        public SpriteAnimator SpriteAnimator { get => _spriteAnimator; set => _spriteAnimator = value; }
        #endregion

        #region AI Components
        private EnemyHealthStats _enemyHealthStats;
        private EnemyHealthStats _enemyBondableHealthStats;

        public EnemyHealthStats EnemyHealthStats { get => _enemyHealthStats; }
        #endregion

        //IDamageable
        private Vector2 _damageDirection = Vector2.zero;

        public Vector2 DamageDirection { get => _damageDirection; set => _damageDirection = value; }
        public Transform Transform { get => transform; }

        public bool FacingRight { get => !_spriteAnimator.Renderer.flipX; }

        // IBondable
        public EBondBehaviourType BondBehaviourType => EBondBehaviourType.Possess;
        public Transform BondTargetTransform => Transform;
        [SerializeField]
        private float _sqDistanceToCompleteBond = 1.0f;

        public IPossessable Possessable => this;
        public IBounceable Bounceable => null;
        public IPhaseable Phaseable => null;

        public float SqDistanceToCompleteBond => _sqDistanceToCompleteBond;

        protected void Awake()
        {
            _enemyHealthStats         = new EnemyHealthStats(3.0f, 3.0f, EHealthStatType.EnemyHealth);
            _enemyBondableHealthStats = new EnemyHealthStats(3.0f, 3.0f, EHealthStatType.BondableHealth);
        }

        public bool CanBeBonded()
        {
            return !_isPossessed && ((_enemyHealthStats.IsAlive() && !_enemyBondableHealthStats.IsAlive()) || DebugManager.Instance.DebugSettings.AlwaysBondable);
        }

        // IPossessable
        public bool IsPossessed()
        {
            return _isPossessed;
        }

        public bool CanBePossessed()
        {
            return !_isPossessed && ((_enemyHealthStats.IsAlive() && !_enemyBondableHealthStats.IsAlive()) || DebugManager.Instance.DebugSettings.AlwaysBondable) ;
        }

        public void OnPossess()
        {
            AudioSource pooledSource = (AudioManager.Instance as AudioManager).TryPlayAudioSourceAtLocation(EAudioType.SFX_Player_PossessStart, PlayerEntity.Instance.transform.position);

            // possess control of this AI
            _movementSM.OnBonded();
            _movementSM.CurrentState.ExitState();
            _movementSM.CurrentBondedState.EnterState();
            _movementSM.Collider.isTrigger = false;

            _attackSM.OnBonded();
            _attackSM.CurrentState.ExitState();
            _attackSM.CurrentBondedState.EnterState();
            //InputManager.Instance.TryEnableActionMap(EInputSystem.BondedPlayer);
            _onPossessedEvent.Invoke();

            _isPossessed = true;
        }

        public void OnDispossess()
        {
            // dispossess this AI
            _movementSM.OnUnbonded();
            _movementSM.CurrentBondedState.ExitState();
            _movementSM.CurrentState.EnterState();
            _movementSM.Collider.isTrigger = true;

            _attackSM.OnUnbonded();
            //InputManager.Instance.TryDisableActionMap(EInputSystem.BondedPlayer);
            _isPossessed = false;

            bool bond = TargetManager.Instance.BondableTarget != null;
            
            PlayerEntity.Instance.OnPossess(bond);
            AudioSource pooledSource = (AudioManager.Instance as AudioManager).TryPlayAudioSourceAtLocation(EAudioType.SFX_Player_BondExit, PlayerEntity.Instance.transform.position);

            if (MovementSM.Waypoints == null)
            {
                Despawn();
            }
        }

        public void Spawn()
        {
            _enemyHealthStats.RestoreHitPoints();
            _enemyBondableHealthStats.RestoreHitPoints();
            gameObject.SetActive(true);
        }

        public void Despawn(bool killed = false)
        {
            gameObject.SetActive(false);
            
            SpawnManager.Instance.UnregisterSpawnPointEntity(_spawnPointID);
            if (killed)
            {
                SpawnManager.Instance.RegisterSpawnPointRespawnTimer(_spawnPointID);
                // spawnpoint is still active so tell it that the spawn is dead
                if (SpawnPoint)
                {
                    SpawnPoint.OnSpawnKilled();
                }

                //spawn corpse
                CorpseManager.Instance.TrySpawnCorpse(_entity, transform.position);
            }
            else
            {
                CorpseManager.Instance.TrySpawnTeleportCorpse(_entity, transform.position);
            }
        }

        public Vector2 GetMovementInput()
        {
            if (_isPossessed)
            {
                return _movementSM.CurrentMovementInput;
            }
            else
            {
                return new Vector2(0,0);
            }
        }

        public void TakeDamage(EDamageType damageType, EEntityType causer, Vector3 damagePosition)
        {
            float resultHealth = 100.0f;
            if (_enemyBondableHealthStats.IsAlive())
            {
                //where is the damage coming from?
                DamageDirection = (Transform.position - damagePosition).normalized;
                if (CanTakeDamage())
                {
                    resultHealth = _enemyBondableHealthStats.RemoveHitPoints(1.0f, true);
                }

                bool brokenShield = resultHealth <= 0.0f;
                
                //broken shield
                FeedbackManager.Instance.TryFeedbackPattern(brokenShield ? EFeedbackPattern.Game_BasicAttackHeavy : EFeedbackPattern.Game_BasicAttackLight);
                
                _spriteAnimator.DamageFlash();
                _onHitEvent.Invoke(gameObject);
            }
            else
            {
                //where is the damage coming from?
                DamageDirection = (Transform.position - damagePosition).normalized;
                if (CanTakeDamage())
                {
                    resultHealth = _enemyHealthStats.RemoveHitPoints(1.0f, true);
                }

                bool killed = resultHealth <= 0.0f;

                FeedbackManager.Instance.TryFeedbackPattern(killed ? EFeedbackPattern.Game_BasicAttackHeavy : EFeedbackPattern.Game_BasicAttackLight);

                if (killed)
                {
                    // death reaction needed
                    Despawn(true);
                }
                else
                {
                    _onTakeDamageEvent.Invoke(gameObject);
                }
                //_spriteAnimator.DamageFlash();
            }

            
        }

        public bool CanTakeDamage()
        {
            return (!DebugManager.Instance.DebugSettings.EnemiesImmune);
        }

        public bool IsAlive()
        {
            return _enemyHealthStats.IsAlive();
        }

    }
    
}
