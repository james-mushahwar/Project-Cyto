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
            _activeSpawners.Clear();
        }

        public override void Tick()
        {
            bool hasWaveFished = true;

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
                        hasWaveFished = entity.IsExposed();
                    }
                    else
                    {
                        hasWaveFished = false;
                    }
                }

                if (!hasWaveFished)
                {
                    break;
                }
            }

            _isComplete = hasWaveFished;
        }

    }
    
}
