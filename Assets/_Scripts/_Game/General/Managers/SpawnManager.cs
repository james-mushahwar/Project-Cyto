using _Scripts._Game.AI;
using _Scripts._Game.General.Spawning.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts._Game.General.Managers{
    
    public class SpawnManager : Singleton<SpawnManager>
    {
        #region General
        private Dictionary<int, List<SpawnPoint>> _sceneSpawnPointsDict = new Dictionary<int, List<SpawnPoint>>();
        private Dictionary<string, AIEntity> _spawnPointEntityDict = new Dictionary<string, AIEntity>();

        #endregion

        private void Awake()
        {
            base.Awake();

            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        private void OnSceneUnloaded(Scene current)
        {
            int sceneIndex = current.buildIndex;
            List<SpawnPoint> spawnPoints;

            if (_sceneSpawnPointsDict.TryGetValue(sceneIndex, out spawnPoints))
            {
                // handle spawn points and ai despawn
                foreach (SpawnPoint spawnPoint in spawnPoints)
                {

                }
            }
        }

        public void AssignSpawnPoint(int sceneIndex, SpawnPoint spawnPoint)
        {
            List<SpawnPoint> spawnPoints;

            if (!_sceneSpawnPointsDict.TryGetValue(sceneIndex, out spawnPoints))
            {
                _sceneSpawnPointsDict.Add(sceneIndex, spawnPoints = new List<SpawnPoint>());
                _sceneSpawnPointsDict[sceneIndex].Add(spawnPoint);
                _spawnPointEntityDict.TryAdd(spawnPoint.RuntimeID.Id, null);
            }
        }

        public void RegisterSpawnPointEntity(SpawnPoint spawnPoint, AIEntity entity)
        {
            _spawnPointEntityDict[spawnPoint.RuntimeID.Id] = entity;
        }

        public AIEntity TryGetRegisteredEntity(SpawnPoint spawnPoint)
        {
            AIEntity entity = null;
            _spawnPointEntityDict.TryGetValue(spawnPoint.RuntimeID.Id, out entity);
            return entity;
        }
    }
    
}
