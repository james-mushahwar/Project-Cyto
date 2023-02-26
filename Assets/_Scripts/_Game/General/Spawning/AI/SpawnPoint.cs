using _Scripts._Game.AI;
using _Scripts._Game.General.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts._Game.General.Spawning.AI{
    
    public class SpawnPoint : MonoBehaviour
    {
        [Header("Spawn properties")]
        [SerializeField]
        private EEntity _entity;
        [SerializeField]
        private Waypoints _waypoints;

        private bool _isEntitySpawned;
        private AIEntity _entitySpawned;


        public Waypoints Waypoints { get => _waypoints; set => _waypoints = value; }

        private void Awake()
        {
            SpawnManager.Instance.AssignSpawnPoint(gameObject.scene.buildIndex, this);
        }

        private void OnEnable()
        {
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
            }
        }

        private void OnDisable()
        {
            if (_isEntitySpawned && _entitySpawned != null)
            {
                if (!_entitySpawned.IsPossessed())
                {
                    _entitySpawned.gameObject.SetActive(false);
                }
                _isEntitySpawned = false;
                _entitySpawned = null;
            }
        }
    }
    
}
