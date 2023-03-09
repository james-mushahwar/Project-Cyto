using _Scripts._Game.AI;
using _Scripts._Game.General.Identification;
using _Scripts._Game.General.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts._Game.General.Spawning.AI{
    
    public class SpawnPoint : MonoBehaviour
    {
        #region General
        [Header("Spawn properties")]
        [SerializeField]
        private EEntity _entity;
        [SerializeField]
        private Waypoints _waypoints;

        private bool _isEntitySpawned;
        private AIEntity _entitySpawned;

        public Waypoints Waypoints { get => _waypoints; set => _waypoints = value; }
        #endregion

        #region ID
        private RuntimeID _runtimeID;

        public RuntimeID RuntimeID { get => _runtimeID; }
        #endregion

        private void Awake()
        {
            _runtimeID = GetComponent<RuntimeID>();
            SpawnManager.Instance.AssignSpawnPoint(gameObject.scene.buildIndex, this);
        }

        private void OnEnable()
        {
            AIEntity entity = SpawnManager.Instance.TryGetRegisteredEntity(this);
            if (entity != null)
            {
                _isEntitySpawned = true;
                _entitySpawned = entity;
                Debug.LogWarning("Entity already exists - no need to spawn another");
                return;
            }

            if (_entitySpawned && _isEntitySpawned)
            {
                return;
            }

            AIEntity aiEntity = AIManager.Instance.TrySpawnAI(_entity, transform.position);
            if (aiEntity != null)
            {
                _isEntitySpawned = true;
                _entitySpawned = aiEntity;
                _entitySpawned.MovementSM.Waypoints = _waypoints;
                SpawnManager.Instance.RegisterSpawnPointEntity(this, _entitySpawned);
            }
        }

        private void OnDisable()
        {
            if (_isEntitySpawned && _entitySpawned != null)
            {
                if (!_entitySpawned.IsPossessed())
                {
                    _entitySpawned.gameObject.SetActive(false);
                    _isEntitySpawned = false;
                    _entitySpawned = null;
                }
            }
        }
    }
    
}
