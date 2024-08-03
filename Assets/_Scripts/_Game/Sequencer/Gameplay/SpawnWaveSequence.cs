using _Scripts._Game.AI;
using _Scripts._Game.General.Identification;
using _Scripts._Game.General.Managers;
using _Scripts._Game.General.Spawning.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.Sequencer.Gameplay{
    
    public class SpawnWaveSequence : Sequenceable
    {
        [SerializeField]
        private List<AISpawner> _spawners = new List<AISpawner>();
        [SerializeField]
        private List<SpawnPoint> _spawnPoints = new List<SpawnPoint>();
        private List<string> _spawnPointIDs;
        private List<string> _activeSpawnPointIDs;

        [Header("Properties")]
        [SerializeField]
        private bool _useSpawnsKilledToCompleteCheck;
        [SerializeField]
        private int _spawnsKilledToComplete;
        private int _spawnsKilledCount;

        [SerializeField, Tooltip("Are there no more active spawns?")]
        private bool _useNoRemainingSpawnsToCompleteCheck = true;
        [SerializeField]
        private bool _flagStopSpawningWhenKillCountReached;

        private bool _isStarted;
        private bool _isComplete;

        private RuntimeID _runtimeID;
        public override string RuntimeID => _runtimeID.Id;

        private void Start()
        {
            _runtimeID = GetComponent<RuntimeID>();
            _spawnPointIDs = new List<string>();
            _activeSpawnPointIDs = new List<string>();
        }

        private void SetupActiveSpawners()
        {
            _spawnPointIDs.Clear(); ;
            _activeSpawnPointIDs.Clear();

            foreach (AISpawner spawner in _spawners)
            {
                foreach (SpawnPoint spawnPoint in spawner.SpawnPoints)
                {
                    if (_spawnPointIDs.Contains(spawnPoint.RuntimeID.Id) == false)
                    {
                        _spawnPointIDs.Add(spawnPoint.RuntimeID.Id);
                    }
                }
            }

            foreach (SpawnPoint spawnPoint in _spawnPoints)
            {
                if (_spawnPointIDs.Contains(spawnPoint.RuntimeID.Id) == false)
                {
                    _spawnPointIDs.Add(spawnPoint.RuntimeID.Id);
                }
            }
        }

        public override void Begin()
        {
            _isStarted = true;
            
            SetupActiveSpawners();

            List<AISpawner> spawners = new List<AISpawner>();

            for (int i = 0; i < _spawnPointIDs.Count; i++)
            {
                string id = _spawnPointIDs[i];
                
                _activeSpawnPointIDs.Add(id);
                SpawnPoint spawnPoint = RuntimeIDManager.Instance.GetRuntimeSpawnPoint(id);
                if (spawnPoint.Spawner != null)
                { 
                    if (spawners.Contains(spawnPoint.Spawner)  == false)
                    {
                        spawners.Add(spawnPoint.Spawner);
                        spawnPoint.Spawner.OnSpawnKilledEvent.AddListener(SpawnKilled);
                    }
                }
            }
        }

        public override bool IsStarted()
        {
            return _isStarted;
        }

        public override bool IsComplete()
        {
            return _isComplete;
        }

        public override void Stop()
        {
            _isStarted = false;
            _isComplete = false;

            for (int i = 0; i < _activeSpawnPointIDs.Count; i++)
            {
                string id = _activeSpawnPointIDs[i];
                SpawnPoint spawnPoint = RuntimeIDManager.Instance.GetRuntimeSpawnPoint(id);

                if (spawnPoint != null && spawnPoint.Spawner != null)
                {
                    spawnPoint.Spawner.OnSpawnKilledEvent.RemoveListener(SpawnKilled);
                }
            }
            _activeSpawnPointIDs.Clear();
        }

        public override void Tick()
        {
            bool hasWaveFinished = true;

            bool spawnsKilledCheck = true;
            bool noRemainingSpawnsCheck = true;

            if (_useSpawnsKilledToCompleteCheck)
            {
                if (_spawnsKilledCount < _spawnsKilledToComplete)
                {
                    spawnsKilledCheck = false;
                }
            }

            if (_useNoRemainingSpawnsToCompleteCheck)
            {
                foreach (string id in _activeSpawnPointIDs)
                {
                    SpawnPoint spawnPoint = RuntimeIDManager.Instance.GetRuntimeSpawnPoint(id);

                    if (spawnPoint == null)
                    {
                        continue;
                    }

                    AIEntity entity = SpawnManager.Instance.TryGetRegisteredEntity(spawnPoint);

                    if (entity == null)
                    {
                        continue;
                    }
                    if (entity != null)
                    {
                        if (spawnPoint.WaveCompleteOnAllAIExposed)
                        {
                            noRemainingSpawnsCheck = entity.IsExposed();
                        }
                        else
                        {
                            noRemainingSpawnsCheck = false;
                        }
                    }

                    if (!noRemainingSpawnsCheck)
                    {
                        break;
                    }
                }
            }

            hasWaveFinished = spawnsKilledCheck && noRemainingSpawnsCheck;

            _isComplete = hasWaveFinished;
        }

        private void SpawnKilled(string spawnerId)
        {
            _spawnsKilledCount++;

            if (_useSpawnsKilledToCompleteCheck && _spawnsKilledCount >= _spawnsKilledToComplete)
            {
                if (_flagStopSpawningWhenKillCountReached)
                {
                    AISpawner spawner = RuntimeIDManager.Instance.GetRuntimeSpawner(spawnerId);

                    if (spawner != null)
                    {
                        spawner.SetSpawnAutomatically(false);
                    }
                }
            }
        }

    }
    
}
