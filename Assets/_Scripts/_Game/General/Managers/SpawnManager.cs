using _Scripts._Game.General.Spawning.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts._Game.General.Managers{
    
    public class SpawnManager : Singleton<SpawnManager>
    {
        #region General
        private Dictionary<int, List<SpawnPoint>> _spawnPointsDict = new Dictionary<int, List<SpawnPoint>>();

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

            if (_spawnPointsDict.TryGetValue(sceneIndex, out spawnPoints))
            {
                // handle spawn points and ai despawn
            }
        }
    }
    
}
