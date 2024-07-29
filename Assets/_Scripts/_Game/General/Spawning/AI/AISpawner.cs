using _Scripts._Game.General.Managers;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace _Scripts._Game.General.Spawning.AI{
    
    public class AISpawner : MonoBehaviour
    {
        private List<SpawnPoint> _spawnPoints;

        [Header("Spawn properties")]
        [SerializeField]
        private bool _spawnerControlsSpawning = false;

        [SerializeField]
        private bool _limitMaxActiveSpawns = false;
        [ShowIf("_limitMaxActiveSpawns"), SerializeField]
        private int _maxActiveSpawns;

        [SerializeField]
        private float _delayUntilRespawn = 5.0f; 

        public bool SpawnerControlsSpawning { get { return _spawnerControlsSpawning; } }
        public float DelayUntilRespawn {  get { return _delayUntilRespawn; } }

        [Header("Custom")]
        [SerializeField]
        private GameObject _spawnPointPrefab;

        [SerializeField]
        private UnityEvent _onSpawnKilledEvent;

        public UnityEvent OnSpawnKilledEvent { get => _onSpawnKilledEvent; }

        public void CreateSpawnPoint()
        {
            GameObject spawnPoint = Instantiate(_spawnPointPrefab);
            spawnPoint.transform.parent = transform;
            spawnPoint.transform.localPosition = Vector3.zero;
        }

        private void OnEnable()
        {
            SpawnManager.Instance.AssignSpawner(this);

            GetComponentsInChildren<SpawnPoint>(_spawnPoints);
        }

        private void OnDisable()
        {
            SpawnManager.Instance.UnassignSpawner(this);
        }

        // Update is called once per frame
        public void Tick()
        {
            bool activeSpawnLimitReached = GetActiveSpawnLimitReached();

            bool canAISpawnerSpawn = _spawnerControlsSpawning && !activeSpawnLimitReached;
            if (canAISpawnerSpawn)
            {
                foreach (SpawnPoint spawnPoint in _spawnPoints)
                {
                    if (spawnPoint.isActiveAndEnabled == false)
                    {
                        continue;
                    }

                    bool spawnPointHasSpawnedEntity = spawnPoint.IsEntitySpawned;
                    bool respawnTimerElapsed = SpawnManager.Instance.TryHasRespawnTimerElapsed(spawnPoint);
                    
                    bool canSpawnPointSpawn = !spawnPointHasSpawnedEntity && respawnTimerElapsed;

                    if (canSpawnPointSpawn)
                    {
                        spawnPoint.Spawn();
                    }
                }
                
            }
        }

        private bool GetActiveSpawnLimitReached()
        {
            bool activeSpawnLimitReached = false;
            if (_limitMaxActiveSpawns)
            {
                int activeSpawnCount = 0;
                foreach (SpawnPoint spawnPoint in _spawnPoints)
                {
                    if (activeSpawnCount >= _maxActiveSpawns)
                    {
                        break;
                    }

                    if (spawnPoint.IsEntitySpawned)
                    {
                        activeSpawnCount++;
                    }
                }

                if (activeSpawnCount >= _maxActiveSpawns)
                {
                    activeSpawnLimitReached = true;
                }
            }

            return activeSpawnLimitReached;
        }

        public bool Spawn()
        {
            return false;
        }

        public void OnSpawnKilled(SpawnPoint spawnPoint)
        {
            _onSpawnKilledEvent.Invoke();
        }
    }
    
}
