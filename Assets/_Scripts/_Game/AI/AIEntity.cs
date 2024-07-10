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
using _Scripts._Game.Events;
using EZCameraShake;

namespace _Scripts._Game.AI{

    public abstract class AIEntity : MonoBehaviour, IBondable, IPossessable, IDamageable, ITickGroup, IExposable, ISpawnable
    {
        [Header("Entity")]
        protected EEntity _entity;
        private bool _isPossessed;

        public EEntity Entity { get => _entity; }

        public T GetEntity<T>() where T : AIEntity
        {
            return this as T;
        }

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
        protected AIMovementStateMachineBase _movementSM;
        protected AIAttackStateMachineBase _attackSM;
        protected SpriteAnimator _spriteAnimator;

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
        public IExposable Exposable
        {
            get => this;
        }
        private Vector2 _damageDirection = Vector2.zero;

        public Vector2 DamageDirection { get => _damageDirection; set => _damageDirection = value; }
        public Transform Transform { get => transform; }

        public bool FacingRight { get => !_spriteAnimator.Renderer.flipX; }

        [SerializeField]
        private List<EDamageType> _damageTypesToIgnore;
        [SerializeField]
        private List<EDamageType> _damageTypesToAccept;

        public List<EDamageType> DamageTypesToIgnore => _damageTypesToIgnore;
        public List<EDamageType> DamageTypesToAccept => _damageTypesToAccept;

        //Exposable
        [Header("Exposable")]
        [SerializeField]
        private UnityEvent _onExposedEvent;
        [SerializeField]
        private UnityEvent _onUnexposedEvent;

        public UnityEvent OnExposedEvent => _onExposedEvent;

        // IBondable
        public EBondBehaviourType BondBehaviourType => EBondBehaviourType.Possess;
        public Transform BondTargetTransform => Transform;
        [SerializeField]
        private float _sqDistanceToCompleteBond = 1.0f;

        public IPossessable Possessable => this;
        public IBounceable Bounceable => null;
        public IPhaseable Phaseable => null;

        public float SqDistanceToCompleteBond => _sqDistanceToCompleteBond;

        [SerializeField]
        [HideInInspector]
        private UnityEvent _onStartBondEvent;
        public UnityEvent OnStartBondEvent => _onStartBondEvent;

        protected virtual void Awake()
        {
            
        }

        public virtual void Tick()
        {
            _movementSM.Tick();
            _attackSM.Tick();
            _spriteAnimator.Tick();
        }

        public bool CanBeBonded()
        {
            return !_isPossessed && ((_enemyHealthStats.IsAlive() && !_enemyBondableHealthStats.IsAlive()) || DebugManager.Instance.DebugSettings.AlwaysBondable);
        }

        public void OnStartBond()
        {
            _onStartBondEvent.Invoke();
        }

        // IPossessable
        public bool IsPossessed()
        {
            return _isPossessed;
        }

        public virtual bool CanBePossessed()
        {
            return !_isPossessed && ((_enemyHealthStats.IsAlive() && !_enemyBondableHealthStats.IsAlive()) || DebugManager.Instance.DebugSettings.AlwaysBondable) ;
        }

        public void OnPossess()
        {
            AudioManager.Instance.TryPlayAudioSourceAtLocation(EAudioType.SFX_Player_PossessStart, PlayerEntity.Instance.transform.position);

            // possess control of this AI
            _movementSM.OnPossess();
            _movementSM.CurrentState.ExitState();
            _movementSM.CurrentBondedState.EnterState();
            _movementSM.Collider.isTrigger = false;

            _attackSM.OnPossess();
            _attackSM.CurrentState.ExitState();
            _attackSM.CurrentBondedState.EnterState();
            //InputManager.Instance.TryEnableActionMap(EInputSystem.BondedPlayer);
            _onPossessedEvent.Invoke();

            _isPossessed = true;
        }

        public void OnDispossess()
        {
            // dispossess this AI
            _movementSM.OnDispossess();
            _movementSM.CurrentBondedState.ExitState();
            _movementSM.CurrentState.EnterState();
            _movementSM.Collider.isTrigger = _movementSM.DefaultIsTrigger;

            _attackSM.OnDispossess();
            //InputManager.Instance.TryDisableActionMap(EInputSystem.BondedPlayer);
            _isPossessed = false;

            bool bond = TargetManager.Instance.LockedBondableTarget != null;

            PlayerEntity.Instance.OnPossess(bond); 
            AudioManager.Instance.TryPlayAudioSourceAtLocation(EAudioType.SFX_Player_BondExit, PlayerEntity.Instance.transform.position);

            if (MovementSM.Waypoints == null)
            {
                Despawn();
            }
        }

        public void Spawn()
        {
            //reset states
            _movementSM.Spawn();
            _attackSM.Spawn();

            FEntityStats entityStats = StatsManager.Instance.GetEntityStat(_entity);
            _enemyHealthStats = new EnemyHealthStats(entityStats.MaxHealth, entityStats.MaxHealth, EHealthStatType.EnemyHealth);
            _enemyBondableHealthStats = new EnemyHealthStats(entityStats.MaxBondableHealth, entityStats.MaxBondableHealth, EHealthStatType.BondableHealth);

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

            _spawnPoint = null;
            _spawnPointID = "";
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
            if (DamageManager.CanBeDamaged(damageType, this) == false)
            {
                return;
            }

            float resultHealth = 100.0f;
            bool killedOrBroken = false;

            bool IsInstakill = damageType == EDamageType.Laser_Instakill;
            float damageAmount = DamageManager.GetDamageFromTypeToEntity(damageType, this);

            if (_enemyBondableHealthStats.IsAlive() && !IsInstakill)
            {
                //where is the damage coming from?
                DamageDirection = (Transform.position - damagePosition).normalized;
                if (CanTakeDamage())
                {
                    resultHealth = _enemyBondableHealthStats.RemoveHitPoints(damageAmount, true);
                }

                bool brokenShield = resultHealth <= 0.0f;

                if (brokenShield)
                {
                    OnExposed();
                    killedOrBroken = true;
                    if (causer == EEntityType.Player || causer == EEntityType.BondedEnemy)
                    {
                        PlayerEntity.Instance.AttackingSM.RestartChargedMode();
                        VolumeManager.Instance.OnExposed();
                        FollowCamera.Instance.OnExposed();
                    }
                }
                //broken shield
                FeedbackManager.Instance.TryFeedbackPattern(brokenShield ? EFeedbackPattern.Game_OnAIExposed : EFeedbackPattern.Game_BasicAttackLight);
                
                _spriteAnimator.DamageFlash();
                _onHitEvent.Invoke(gameObject);
                if (!killedOrBroken)
                {
                    VolumeManager.Instance.OnBondableHit();
                }
            }
            else
            {
                //where is the damage coming from?
                DamageDirection = (Transform.position - damagePosition).normalized;
                if (CanTakeDamage())
                {
                    resultHealth = _enemyHealthStats.RemoveHitPoints(damageAmount, true);
                }

                bool killed = resultHealth <= 0.0f;

                FeedbackManager.Instance.TryFeedbackPattern(killed ? EFeedbackPattern.Game_BasicAttackHeavy : EFeedbackPattern.Game_BasicAttackLight);

                if (killed)
                {
                    // death reaction needed
                    Despawn(true);
                    killedOrBroken = true;
                }
                else
                {
                    _onTakeDamageEvent.Invoke(gameObject);
                    VolumeManager.Instance.OnBondableHit();
                }
                //_spriteAnimator.DamageFlash();
            }

            CameraShaker.Instance.ShakeOnce(killedOrBroken ? 4.0f : 2.0f, killedOrBroken ? 0.5f : 0.2f, 0.0f, 0.15f);
        }

        public bool CanTakeDamage()
        {
            return (!DebugManager.Instance.DebugSettings.EnemiesImmune);
        }

        public bool IsAlive()
        {
            return _enemyHealthStats.IsAlive();
        }

        public bool IsExposed()
        {
            return !_enemyBondableHealthStats.IsAlive();
        }

        public void OnExposed()
        {
            TimeManager.Instance.TryRequestTimeScale(ETimeImportance.Low, 0.25f, 0.0f, 0.1f, 0.1f);
            OnExposedEvent.Invoke();
            //play exposed vfx
            ParticleManager.Instance.TryPlayParticleSystem(EParticleType.Exposed, transform.position, 0.0f);
            AudioManager.Instance.TryPlayAudioSourceAtLocation(EAudioType.SFX_Player_Exposed, transform.position);
        }

        public void OnUnexposed()
        {
            _onUnexposedEvent.Invoke();
        }

    }
    
}
