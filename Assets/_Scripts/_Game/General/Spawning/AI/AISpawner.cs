using _Scripts._Game.General.Identification;
using _Scripts._Game.General.LogicController;
using _Scripts._Game.General.Managers;
using NaughtyAttributes;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace _Scripts._Game.General.Spawning.AI{

    [RequireComponent(typeof(RuntimeID))]
    public class AISpawner : MonoBehaviour
    {
        private RuntimeID _runtimeID;

        public RuntimeID RuntimeID { get => _runtimeID; }

        [Header("Logic Entity")]
        [SerializeField]
        private bool _enableSpawnOnInputChanged = false;
        [SerializeField]
        private bool _trySpawnOnInputValid = false;
        [SerializeField]
        private bool _trySpawnOnInputInvalid = false;
        private ILogicEntity _logicEntity;

        [Header("Spawn properties")]
        private List<SpawnPoint> _spawnPoints;

        [SerializeField]
        private bool _spawnerControlsSpawning = false;
        [SerializeField]
        private bool _trySpawnAutomatically = true; // use tick to check whether to spawn

        //[SerializeField]
        //private bool _limitMaxActiveSpawns;
        [SerializeField]//, ShowIf("_limitMaxActiveSpawns")]
        private int _maxActiveSpawns = 1;

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
            if (_runtimeID == null)
            {
                _runtimeID = GetComponent<RuntimeID>();
            }
            RuntimeIDManager.Instance.RegisterRuntimeSpawner(this);
            SpawnManager.Instance.AssignSpawner(this);

            _logicEntity = GetComponent<LogicEntity>();

            _logicEntity.OnInputChanged.AddListener(OnLogicInputChanged);

            _spawnPoints = new List<SpawnPoint>();
            GetComponentsInChildren<SpawnPoint>(_spawnPoints);

            foreach(SpawnPoint spawnPoint in _spawnPoints)
            {
                spawnPoint.SpawnerID = _runtimeID.Id;
            }
        }

        private void OnDisable()
        {
            SpawnManager.Instance?.UnassignSpawner(this);
        }

        private void OnLogicInputChanged()
        {
            if (_spawnerControlsSpawning && _enableSpawnOnInputChanged)
            {
                bool input = _logicEntity.IsInputLogicValid;

                if ((input && _trySpawnOnInputValid) || (!input && _trySpawnOnInputInvalid))
                {
                    if (CanAISpawnerSpawn(false))
                    {
                        TrySpawnFromSpawnPoints();
                    }
                }
            }
        }

        // Update is called once per frame
        public void Tick()
        {
            bool canAISpawnerSpawn = CanAISpawnerSpawn(true);
            if (canAISpawnerSpawn)
            {
                TrySpawnFromSpawnPoints();
            }
        }

        private bool GetActiveSpawnLimitReached()
        {
            bool activeSpawnLimitReached = false;
            if (_maxActiveSpawns > 0)
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

        private bool CanAISpawnerSpawn(bool autoSpawn)
        {
            bool activeSpawnLimitReached = GetActiveSpawnLimitReached();

            bool autoSpawnCheck = (autoSpawn && _trySpawnAutomatically) || !autoSpawn;

            bool canAISpawnerSpawn = _spawnerControlsSpawning && !activeSpawnLimitReached && autoSpawnCheck;

            return canAISpawnerSpawn;
        }

        public bool TrySpawnFromSpawnPoints()
        {
            bool success = false;
            foreach (SpawnPoint spawnPoint in _spawnPoints)
            {
                bool spawned = TrySpawnFromSpawnPoint(spawnPoint);

                if (spawned && !success)
                {
                    success = true;
                }
            }

            return success;
        }

        public bool TrySpawnFromSpawnPoint(SpawnPoint spawnPoint)
        {
            bool success = false;
            if (spawnPoint.isActiveAndEnabled == false)
            {
                return false;
            }

            bool spawnPointHasSpawnedEntity = spawnPoint.IsEntitySpawned;
            bool respawnTimerElapsed = SpawnManager.Instance.TryHasRespawnTimerElapsed(spawnPoint);

            bool canSpawnPointSpawn = !spawnPointHasSpawnedEntity && respawnTimerElapsed;

            if (canSpawnPointSpawn)
            {
                bool spawn = spawnPoint.Spawn();
                success = spawn;
            }

            return success;
        }

        public void OnSpawnKilled(SpawnPoint spawnPoint)
        {
            _onSpawnKilledEvent.Invoke();
        }
    }
    
}
