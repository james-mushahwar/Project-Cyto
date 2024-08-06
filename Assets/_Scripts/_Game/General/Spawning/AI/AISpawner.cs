using _Scripts._Game.General.Identification;
using _Scripts._Game.General.LogicController;
using _Scripts._Game.General.Managers;
using NaughtyAttributes;
using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace _Scripts._Game.General.Spawning.AI{

    [Serializable]
    public struct FAISpawnerSpawnSettings
    {
        //Options
        private bool _canEditAtRuntime;

        //Spawn
        [SerializeField]
        private bool _spawnAutomatically; // true - spawn in tick. false - spawn some other way - Logic IO?
        [SerializeField]
        private bool _spawnAutomaticallyInputsAllValidCheck;
        [SerializeField]
        private bool _spawnOnInputChanged;
        [SerializeField]
        private bool _spawnOnInputChangedToValid;
        [SerializeField]
        private bool _spawnOnInputChangedToInvalid;

        //limits
        [SerializeField]
        private int _maxActiveSpawns;

        //delays
        [SerializeField]
        private float _delayBetweenSpawns;
        [SerializeField]
        private float _delayUntilRespawn;

        #region Properties
        public bool CanEditAtRuntime { get => _canEditAtRuntime; set => _canEditAtRuntime = value; }

        public bool SpawnAutomatically 
        {   
            get => _spawnAutomatically; 
            set
            {
                if (_canEditAtRuntime)
                {
                    _spawnAutomatically = value;
                }
            }
        }
        public bool SpawnAutomaticallyInputsAllValidCheck 
        { 
            get => _spawnAutomaticallyInputsAllValidCheck; 
            set
            {
                if (_canEditAtRuntime)
                {
                    _spawnAutomaticallyInputsAllValidCheck = value;
                }
            }
        }
        public bool SpawnOnInputChanged 
        { 
            get => _spawnOnInputChanged;
            set
            {
                if (_canEditAtRuntime)
                {
                    _spawnOnInputChanged = value;
                }
            }
        }
        public bool SpawnOnInputChangedToValid 
        {
            get => _spawnOnInputChangedToValid;
            set
            {
                if (_canEditAtRuntime)
                {
                    _spawnOnInputChangedToValid = value;
                }
            }
        }
        public bool SpawnOnInputChangedToInvalid 
        { 
            get => _spawnOnInputChangedToInvalid;
            set
            {
                if (_canEditAtRuntime)
                {
                    _spawnOnInputChangedToInvalid = value;
                }
            }
        }

        public int MaxActiveSpawns 
        { 
            get => _maxActiveSpawns;
            set
            {
                if (_canEditAtRuntime)
                {
                    _maxActiveSpawns = value;
                }
            }
        }

        public float DelayBetweenSpawns 
        { 
            get => _delayBetweenSpawns;
            set
            {
                if (_canEditAtRuntime)
                {
                    _delayBetweenSpawns = value;
                }
            }
        }
        public float DelayUntilRespawn 
        {  
            get => _delayUntilRespawn;
            set
            {
                if (_canEditAtRuntime)
                {
                    _delayUntilRespawn = value;
                }
            }
        }
        #endregion
    }

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
        [SerializeField]
        private GameObject _waveCompletedOutputLogicGO;
        private ILogicEntity _waveCompletedOutputLogicEntity;
        [SerializeField]
        private GameObject _spawnKilledOutputLogicGO;
        private ILogicEntity _spawnKilledOutputLogicEntity;

        private List<SpawnPoint> _spawnPoints = new List<SpawnPoint>();

        public List<SpawnPoint> SpawnPoints { get => _spawnPoints; }

        [Header("Spawn properties")]
        private bool _spawnerControlsSpawning = false;
        [SerializeField]
        private FAISpawnerSpawnSettings _defaultSpawnSettings;
        private float _delayBetweenSpawnsTimer = 0.0f;
        public bool SpawnerControlsSpawning { get { return _spawnerControlsSpawning; } }

        [Header("Wave properties")]
        [SerializeField]
        private FAISpawnerSpawnSettings _waveSpawnSettings;
        private bool _isWaveActive = false;
        [SerializeField]
        private bool _waveCompleteOnNoActiveSpawns; // complete when all active spawns in the wave are killed
        [SerializeField]
        private bool _waveCompleteOnKilledCountReached; // complete when x spawns are killed

        #region Runtime properties
        private FAISpawnerSpawnSettings _runtimeSpawnSettings;

        private int _spawnsKilledCount;
        private int _waveSpawnsKilledCount;

        private bool SpawnAutomatically { get => _isWaveActive ? _waveSpawnSettings.SpawnAutomatically : _defaultSpawnSettings.SpawnAutomatically; }
        private bool SpawnAutomaticallyInputsAllValidCheck { get => _isWaveActive ? _waveSpawnSettings.SpawnAutomaticallyInputsAllValidCheck : _defaultSpawnSettings.SpawnAutomaticallyInputsAllValidCheck; }
        private bool SpawnOnInputChanged { get => _isWaveActive ? _waveSpawnSettings.SpawnOnInputChanged : _defaultSpawnSettings.SpawnOnInputChanged; }
        private bool SpawnOnInputChangedToValid { get => _isWaveActive ? _waveSpawnSettings.SpawnOnInputChangedToValid : _defaultSpawnSettings.SpawnOnInputChangedToValid; }
        private bool SpawnOnInputChangedToInvalid { get => _isWaveActive ? _waveSpawnSettings.SpawnOnInputChangedToInvalid : _defaultSpawnSettings.SpawnOnInputChangedToInvalid; }

        private int MaxActiveSpawns { get => _isWaveActive ? _waveSpawnSettings.MaxActiveSpawns : _defaultSpawnSettings.MaxActiveSpawns; }

        public float DelayBetweenSpawns { get => _isWaveActive ? _waveSpawnSettings.DelayBetweenSpawns : _defaultSpawnSettings.DelayBetweenSpawns; }
        public float DelayUntilRespawn { get => _isWaveActive ? _waveSpawnSettings.DelayUntilRespawn : _defaultSpawnSettings.DelayUntilRespawn; }
        #endregion

        [Header("Custom")]
        [SerializeField]
        private GameObject _spawnPointPrefab;

        [SerializeField]
        private UnityEvent<string> _onSpawnKilledEvent;

        public UnityEvent<string> OnSpawnKilledEvent { get => _onSpawnKilledEvent; }

        private void Awake()
        {
            _runtimeSpawnSettings = new FAISpawnerSpawnSettings();

            if (_waveCompletedOutputLogicGO)
            {
                _waveCompletedOutputLogicEntity = _waveCompletedOutputLogicGO.GetComponent<LogicEntity>();
            }

            if (_spawnKilledOutputLogicGO)
            {
                _spawnKilledOutputLogicEntity = _spawnKilledOutputLogicGO.GetComponent<LogicEntity>();
            }
        }

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

            _delayBetweenSpawnsTimer = DelayBetweenSpawns;
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
            TickTimers();

            //bool canAISpawnerSpawn = CanAISpawnerSpawn(true);
            //if (canAISpawnerSpawn)
            //{
            //}
            TrySpawnFromSpawnPoints();
        }

        private void TickTimers()
        {
            if (_delayBetweenSpawnsTimer > 0.0f)
            {
                _delayBetweenSpawnsTimer -= Time.deltaTime;

                if (_delayBetweenSpawnsTimer < 0.0f)
                {
                    _delayBetweenSpawnsTimer = 0.0f;
                }
            }
        }

        private bool GetActiveSpawnLimitReached()
        {
            bool activeSpawnLimitReached = false;
            if (MaxActiveSpawns > 0)
            {
                int activeSpawnCount = 0;
                foreach (SpawnPoint spawnPoint in _spawnPoints)
                {
                    if (activeSpawnCount >= MaxActiveSpawns)
                    {
                        break;
                    }

                    if (spawnPoint.IsEntitySpawned)
                    {
                        activeSpawnCount++;
                    }
                }

                if (activeSpawnCount >= MaxActiveSpawns)
                {
                    activeSpawnLimitReached = true;
                }
            }

            return activeSpawnLimitReached;
        }

        private bool CanAISpawnerSpawn(bool autoSpawn)
        {
            bool activeSpawnLimitReached = GetActiveSpawnLimitReached();

            bool autoSpawnCheck = (autoSpawn && SpawnAutomatically) || !autoSpawn;

            bool noTimeDelay = DelayBetweenSpawns <= 0.0f || _delayBetweenSpawnsTimer <= 0.0f;

            bool canAISpawnerSpawn = _spawnerControlsSpawning && !activeSpawnLimitReached && autoSpawnCheck && noTimeDelay;

            return canAISpawnerSpawn;
        }

        public bool TrySpawnFromSpawnPoints()
        {
            bool success = false;
            foreach (SpawnPoint spawnPoint in _spawnPoints)
            {
                if (CanAISpawnerSpawn(true) == false)
                {
                    break;
                }

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

                if (success)
                {
                    _delayBetweenSpawnsTimer = DelayBetweenSpawns;
                }
            }

            return success;
        }

        public void OnSpawnKilled(SpawnPoint spawnPoint)
        {
            _onSpawnKilledEvent.Invoke(_runtimeID.Id);
        }

        public void SetSpawnAutomatically(bool set)
        {
            //_runtimeTrySpawnAutomatically = set;
        }
    }
    
}
