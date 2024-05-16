using _Scripts._Game.AI;
using _Scripts._Game.General.Identification;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.General.Managers{
    
    public class AIManager : Singleton<AIManager>, IManager
    {
        [Header("Pools")]
        private Dictionary<EEntity, AIPool> _aiPoolDict = new Dictionary<EEntity, AIPool>();

        private List<AIPool> _activeAIPools = new List<AIPool>();
        private List<AIEntity> _activeAIEntities = new List<AIEntity>();

        public void AssignAIPool(EEntity entity, AIPool pool)
        {
            AIPool aiPool = null;

            if (!_activeAIPools.Contains(pool))
            {
                _activeAIPools.Add(pool);
            }

            if (_aiPoolDict.TryGetValue(entity, out aiPool) == true)
            {
                Debug.LogWarning("AI Manager: Trying to add pool that already exists");
                return;
            }

            _aiPoolDict[entity] = pool;
        }

        public void UnasignAIPool(AIPool pool)
        {
            if (_activeAIPools.Contains(pool))
            {
                _activeAIPools.Remove(pool);
            }
        }

        public AIEntity TrySpawnAI(EEntity entity, Vector2 spawnLocation, string spawnPointID, string waypointID)
        {
            AIPool aiPool = null;
            if (_aiPoolDict.TryGetValue(entity, out aiPool))
            {
                AIEntity aiEntity = aiPool.GetAIEntity();
                if (aiEntity)
                {
                    aiEntity.transform.position = spawnLocation;
                    aiEntity.SpawnPointID = spawnPointID;
                    aiEntity.MovementSM.WaypointsID = waypointID;
                    aiEntity.Spawn();

                    if (!_activeAIEntities.Contains(aiEntity))
                    {
                        _activeAIEntities.Add(aiEntity);
                    }

                    return aiEntity;
                }
                else
                {
                    Debug.Log("Couldn't get AI entity of type: " + entity);
                }
            }
            return null;
        }

        public void UnassignSpawnedEntity(AIEntity aiEntity)
        {
            if (_activeAIEntities.Contains(aiEntity))
            {
                _activeAIEntities.Remove(aiEntity);
            }
        }

        public void ManagedTick()
        {
            if (GameStateManager.Instance.IsLoadInProgress || !GameStateManager.Instance.IsGameRunning)
            {
                return;
            }

            foreach (AIPool pool in _activeAIPools)
            {
                pool.ManagedTick();
            }

            foreach (AIEntity aiEntity in _activeAIEntities)
            {
                aiEntity.Tick();
            }
        }

        public void ManagedPreInGameLoad()
        {
            _activeAIEntities = new List<AIEntity>();
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
