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
        private List<SpawnPoint> _spawners = new List<SpawnPoint>();
        private List<SpawnPoint> _activeSpawners = new List<SpawnPoint>();

        [Header("Properties")]
        [SerializeField]
        private bool _useSpawnsKilledToComplete;
        [SerializeField]
        private int _spawnsKilledToComplete;
        private int _spawnsKilledCount;

        private bool _isStarted;
        private bool _isComplete;

        private RuntimeID _runtimeID;
        public override string RuntimeID => _runtimeID.Id;

        private void Awake()
        {
            _runtimeID = GetComponent<RuntimeID>();
        }

        public override void Begin()
        {
            _isStarted = true;
            _activeSpawners.Clear();

            foreach (SpawnPoint spawnPoint in _spawners)
            {
                AIEntity entity = SpawnManager.Instance.TryGetRegisteredEntity(spawnPoint);
                if (entity != null)
                {
                    _activeSpawners.Add(spawnPoint);
                    if (spawnPoint.Spawner != null)
                    {
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

            for (int i = 0; i < _activeSpawners.Count; i++)
            {
                SpawnPoint spawnPoint = _activeSpawners[i];

                if (spawnPoint != null && spawnPoint.Spawner != null)
                {
                    spawnPoint.Spawner.OnSpawnKilledEvent.RemoveListener(SpawnKilled);
                }
            }
            _activeSpawners.Clear();
        }

        public override void Tick()
        {
            bool hasWaveFinished = true;

            if (_useSpawnsKilledToComplete)
            {
                if (_spawnsKilledCount < _spawnsKilledToComplete)
                {
                    hasWaveFinished = false;
                }
            }
            else
            {
                foreach (SpawnPoint spawnPoint in _activeSpawners)
                {
                    AIEntity entity = SpawnManager.Instance.TryGetRegisteredEntity(spawnPoint);

                    if (entity == null)
                    {
                        continue;
                    }
                    if (entity != null)
                    {
                        if (spawnPoint.WaveCompleteOnAllAIExposed)
                        {
                            hasWaveFinished = entity.IsExposed();
                        }
                        else
                        {
                            hasWaveFinished = false;
                        }
                    }

                    if (!hasWaveFinished)
                    {
                        break;
                    }
                }

                _isComplete = hasWaveFinished;
            }
        }

        private void SpawnKilled()
        {
            _spawnsKilledCount++;
        }

    }
    
}
