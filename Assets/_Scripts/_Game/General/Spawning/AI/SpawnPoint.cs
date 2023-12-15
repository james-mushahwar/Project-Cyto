using _Scripts._Game.AI;
using _Scripts._Game.General.Identification;
using _Scripts._Game.General.Managers;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.EventSystems.EventTrigger;

namespace _Scripts._Game.General.Spawning.AI{
    
    public class SpawnPoint : MonoBehaviour, IRuntimeId
    {
        #region General
        [Header("Spawn properties")]
        [SerializeField]
        private EEntity _entity;

        [SerializeField]
        //runtime waypoint
        private Waypoints _waypoints;
        private string _waypointsID = "";

        private bool _isEntitySpawned;
        private AIEntity _entitySpawned;

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

        private void Awake()
        {
            _runtimeID = GetComponent<RuntimeID>();
            RuntimeIDManager.Instance.RegisterRuntimeSpawnPoint(this);

            SpawnManager.Instance.AssignSpawnPoint(gameObject.scene.buildIndex, this);
        }

        private void FixedUpdate()
        {
            // check level is running
            if (GameStateManager.Instance.IsGameRunning == false)
            {
                return;
            }
            // timer has elapsed and need to respawn
            if (_isEntitySpawned == false || _entitySpawned == null)
            {
                bool respawnEntity = SpawnManager.Instance.TryHasRespawnTimerElapsed(this);
                if (respawnEntity)
                {
                    Debug.Log("Respawn timer elapsed, respawning Entity");

                    AIEntity entity = SpawnManager.Instance.TryGetRegisteredEntity(this);
                    if (entity == null)
                    {
                        Spawn();
                    }
                    else
                    {
                        _isEntitySpawned = true;
                        _entitySpawned = entity;
                    }
                }
            }
        }

        private void Start()
        {
            if (_waypointsID == "")
            {
                _waypointsID = _waypoints.RuntimeID.Id;
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

        private void Spawn()
        {
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
        }
    }
    
}
