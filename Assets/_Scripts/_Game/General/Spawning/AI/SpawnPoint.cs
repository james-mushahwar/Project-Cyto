using _Scripts._Game.AI;
using _Scripts._Game.General.Identification;
using _Scripts._Game.General.LogicController;
using _Scripts._Game.General.Managers;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using static UnityEngine.EventSystems.EventTrigger;

namespace _Scripts._Game.General.Spawning.AI{

    public class SpawnPoint : MonoBehaviour, IRuntimeId
    {
        private AISpawner _spawner;

        public AISpawner Spawner { get { return _spawner; } }

        #region General
        [Header("Spawn properties")]
        [SerializeField]
        private EEntity _entity = (EEntity)1000;
        [SerializeField]
        private bool _trySpawnAutomatically = true;
        [SerializeField]
        private bool _overrideRespawnDelay;
        [ShowIf("_overrideRespawnDelay"), SerializeField]
        private float _respawnTimerDelay = 5.0f;
        //runtime waypoint
        [SerializeField]
        private Waypoints _waypoints;
        private string _waypointsID = "";

        private bool _isEntitySpawned;
        private AIEntity _entitySpawned;

        public bool IsEntitySpawned { get { return _isEntitySpawned; } }
        public AIEntity EntitySpawned { get { return _entitySpawned; } }

        [Header("Logic Entity")]
        [SerializeField]
        private bool _enableSpawnOnInputChanged = false;
        [SerializeField]
        private bool _trySpawnOnInputValid = false;
        [SerializeField]
        private bool _trySpawnOnInputInvalid = false;
        private ILogicEntity _logicEntity;

        [Header("Wave properties")]
        [SerializeField]
        private bool _waveCompleteOnAllAIExposed = false;

        public bool WaveCompleteOnAllAIExposed { get => _waveCompleteOnAllAIExposed; }


        //public Waypoints Waypoints 
        //{ 
        //    get
        //    {
        //        if (_waypoints == null)
        //        {
        //            _waypoints = RuntimeIDManager.Instance.GetRuntimeWaypoints(_waypointsID);
        //        }
        //        return _waypoints;
        //    }
        //}
        #endregion

        #region ID
        private RuntimeID _runtimeID;

        public RuntimeID RuntimeID { get => _runtimeID; }

        public EEntity Entity
        {
            get { return _entity; }
        }

        #endregion

        public bool HasSpawnAuthority()
        {
            return Spawner == null || (Spawner != null && Spawner.SpawnerControlsSpawning == false);
        }

        public float GetRespawnDelay()
        {
            float delay = _respawnTimerDelay;
            
            if (_overrideRespawnDelay == false && _spawner != null)
            {
                delay = _spawner.DelayUntilRespawn;
            }

            return delay;
        }

        private void Awake()
        {
            _spawner = GetComponentInParent<AISpawner>();
            if (_spawner == null)
            {
                Debug.LogWarning("No AI Spawner attached to this spawnpoint");
            }

            _logicEntity = GetComponent<LogicEntity>();

            _logicEntity.OnInputChanged.AddListener(OnLogicInputChanged);

            _runtimeID = GetComponent<RuntimeID>();
            RuntimeIDManager.Instance.RegisterRuntimeSpawnPoint(this);

            SpawnManager.Instance.AssignSpawnPoint(gameObject.scene.buildIndex, this);
        }

        private void OnLogicInputChanged()
        {
            if (HasSpawnAuthority() && _enableSpawnOnInputChanged)
            {
                bool input = _logicEntity.IsInputLogicValid;

                if ((input && _trySpawnOnInputValid) || (!input && _trySpawnOnInputInvalid))
                {
                    Spawn();
                }
            }
        }

        public void Tick()
        {
            // check level is running
            if (GameStateManager.Instance.IsGameRunning == false )
            {
                return;
            }

            if (HasSpawnAuthority())
            {
                // timer has elapsed and need to respawn
                if (_isEntitySpawned == false || _entitySpawned == null)
                {
                    if (_trySpawnAutomatically)
                    {
                        bool respawnEntity = SpawnManager.Instance.TryHasRespawnTimerElapsed(this);
                        if (respawnEntity)
                        {
                            //Debug.Log("Respawn timer elapsed, respawning Entity");
                            Spawn();
                        }
                    }
                }
            }

        }

        private void Start()
        {
            if (_waypointsID == "")
            {
                if (_waypoints)
                {
                   _waypointsID = _waypoints.RuntimeID.Id;
                }
            }

            AIEntity entity = SpawnManager.Instance.TryGetRegisteredEntity(this);
            if (entity != null)
            {
                _isEntitySpawned = true;
                _entitySpawned = entity;
                //Debug.LogWarning("Entity already exists - no need to spawn another");
                return;
            }

            if (_entitySpawned && _isEntitySpawned)
            {
                return;
            }

            //Spawn();
        }

        private void OnDisable()
        {
            // if application quitting ignore
            if (GameStateManager.IsQuitting)
            {
                return;
            }

            SpawnManager.Instance?.UnassignSpawnPoint(gameObject.scene.buildIndex, this);

            if (_isEntitySpawned && _entitySpawned != null)
            {
                if (!_entitySpawned.IsPossessed())
                {
                    _entitySpawned.Despawn();
                    _isEntitySpawned = false;
                    _entitySpawned = null;

                    SpawnManager.Instance?.UnregisterSpawnPointEntity(RuntimeID.Id);
                }
            }

            RuntimeIDManager.Instance?.UnregisterRuntimeSpawnPoint(this);
        }

        public void Spawn()
        {
            AIEntity entity = SpawnManager.Instance.TryGetRegisteredEntity(this);
            if (entity != null)
            {
                return;
            }

            AIEntity aiEntity = AIManager.Instance.TrySpawnAI(Entity, transform.position, RuntimeID.Id, _waypointsID);
            if (aiEntity != null)
            {
                _isEntitySpawned = true;
                _entitySpawned = aiEntity;
                SpawnManager.Instance.RegisterSpawnPointEntity(this, _entitySpawned);
            }
        }

        public void OnSpawnKilled()
        {
            _isEntitySpawned = false;
            _entitySpawned = null;
            SpawnManager.Instance?.TryRemoveRegisteredEntity(this);

            if (_spawner != null)
            {
                _spawner.OnSpawnKilled(this);
            }
        }
    }
    
}
