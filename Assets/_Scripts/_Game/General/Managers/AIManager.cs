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

        public void AssignAIPool(EEntity entity, AIPool pool)
        {
            AIPool aiPool = null;
            if (_aiPoolDict.TryGetValue(entity, out aiPool) == true)
            {
                Debug.LogWarning("AI Manager: Trying to add pool that already exists");
                return;
            }

            _aiPoolDict[entity] = pool;
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
                    return aiEntity;
                }
                else
                {
                    Debug.Log("Couldn't get AI entity of type: " + entity);
                }
            }
            return null;
        }

        public void PreInGameLoad()
        {
             
        }

        public void PostInGameLoad()
        {
             
        }

        public void PreMainMenuLoad()
        {
             
        }

        public void PostMainMenuLoad()
        {
             
        }
    }
    
}
