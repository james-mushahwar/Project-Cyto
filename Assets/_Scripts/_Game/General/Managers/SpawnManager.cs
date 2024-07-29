using _Scripts._Game.AI;
using _Scripts._Game.General.Spawning.AI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts._Game.General.Managers{
    
    public class SpawnManager : Singleton<SpawnManager>, IManager
    {
        #region General
        private Dictionary<int, List<string>> _sceneSpawnPointsDict = new Dictionary<int, List<string>>();
        private Dictionary<string, AIEntity> _spawnPointEntityDict = new Dictionary<string, AIEntity>();
        private Dictionary<string, float> _spawnPointRespawnTimersDict = new Dictionary<string, float>();
        //private List<string> _removeIDTimers = new List<string>();

        private List<AISpawner> _activeSpawners = new List<AISpawner>();
        private List<SpawnPoint> _activeSpawnPoints = new List<SpawnPoint>();

        #endregion

        protected override void Awake()
        {
            base.Awake();

            _activeSpawnPoints = new List<SpawnPoint>();

            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        public void ManagedTick()
        {
            foreach (AISpawner spawner in _activeSpawners)
            {
                if (spawner != null && spawner.isActiveAndEnabled)
                {
                    spawner.Tick();
                }
            }

            foreach (SpawnPoint spawnPoint in _activeSpawnPoints)
            {
                if (spawnPoint != null && spawnPoint.isActiveAndEnabled)
                {
                    spawnPoint.Tick();
                }
            }

            UpdateRespawnTimers();
        }

        private void UpdateRespawnTimers()
        {
            //update respawn timers;
            for (int i = _spawnPointRespawnTimersDict.Count - 1; i >= 0; i--)
            {
                KeyValuePair<string, float> entry = _spawnPointRespawnTimersDict.ElementAt(i);
                float timer = entry.Value;
                timer -= Time.deltaTime;

                if (timer <= 0.0f)
                {
                    _spawnPointRespawnTimersDict.Remove(entry.Key);
                }
                else
                {
                    _spawnPointRespawnTimersDict[entry.Key] = timer;
                }
            }
        }

        private void OnSceneUnloaded(Scene current)
        {
            int sceneIndex = current.buildIndex;
            List<string> spawnPointIDs;

            if (_sceneSpawnPointsDict.TryGetValue(sceneIndex, out spawnPointIDs))
            {
                // handle spawn points and ai despawn
                foreach (string id in spawnPointIDs)
                {

                }
            }
        }

        public void AssignSpawner(AISpawner spawner)
        {
            if (!_activeSpawners.Contains(spawner))
            {
                _activeSpawners.Add(spawner);
            }
        }

        public void UnassignSpawner(AISpawner spawner)
        {
            if (_activeSpawners.Contains(spawner))
            {
                _activeSpawners.Remove(spawner);
            }
        }

        public void AssignSpawnPoint(int sceneIndex, SpawnPoint spawnPoint)
        {
            List<string> spawnPointIDs;

            if (!_sceneSpawnPointsDict.TryGetValue(sceneIndex, out spawnPointIDs))
            {
                _sceneSpawnPointsDict.Add(sceneIndex, spawnPointIDs = new List<string>());
                _sceneSpawnPointsDict[sceneIndex].Add(spawnPoint.RuntimeID.Id);
                _spawnPointEntityDict.TryAdd(spawnPoint.RuntimeID.Id, null);
            }

            if (!_activeSpawnPoints.Contains(spawnPoint))
            {
                _activeSpawnPoints.Add(spawnPoint);
            }
        }

        public void UnassignSpawnPoint(int sceneIndex, SpawnPoint spawnPoint)
        {
            if (_activeSpawnPoints.Contains(spawnPoint))
            {
                _activeSpawnPoints.Remove(spawnPoint);
            }
        }

        public void RegisterSpawnPointEntity(SpawnPoint spawnPoint, AIEntity entity)
        {
            _spawnPointEntityDict[spawnPoint.RuntimeID.Id] = entity;
        }

        public void UnregisterSpawnPointEntity(string id)
        {
            if (_spawnPointEntityDict.ContainsKey(id))
            {
                _spawnPointEntityDict.Remove(id);
            }

        }

        public AIEntity TryGetRegisteredEntity(SpawnPoint spawnPoint)
        {
            AIEntity entity = null;
            _spawnPointEntityDict.TryGetValue(spawnPoint.RuntimeID.Id, out entity);
            return entity;
        }

        public void TryRemoveRegisteredEntity(SpawnPoint spawnPoint)
        {
            string id = spawnPoint.RuntimeID.Id;
            if (_spawnPointEntityDict.ContainsKey(id))
            {
                _spawnPointEntityDict[id] = null;
            }
        }

        public void RegisterSpawnPointRespawnTimer(string id)
        {
            float timer = -1.0f;
            if (!_spawnPointRespawnTimersDict.TryGetValue(id, out timer))
            {
                _spawnPointRespawnTimersDict.Add(id, 5.0f);
            }
        }

        public bool TryHasRespawnTimerElapsed(SpawnPoint spawnPoint)
        {
            float timer = -1.0f;
            _spawnPointRespawnTimersDict.TryGetValue(spawnPoint.RuntimeID.Id, out timer);
            return timer <= 0.0f;
        }

        public void ManagedPreInGameLoad()
        {
            _activeSpawnPoints = new List<SpawnPoint>();
        }

        public void ManagedPostInGameLoad()
        {
             
        }

        public void ManagedPreMainMenuLoad()
        {
             
        }

        public void ManagedPostMainMenuLoad()
        {
             
        }
    }
    
}
