using _Scripts._Game.AI;
using _Scripts._Game.AI.Entity.Bosses;
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

        //boss ai
        [SerializeField]
        private GigaBombDroidAIEntity _gigaBombDroidPrefab;
        private GigaBombDroidAIEntity _gigaBombDroid;

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

            _aiPoolDict.Add(entity, pool);
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
            AIEntity aiEntity = null;
            if (StatsManager.IsBossAIEntity(entity))
            {
                //boss ai
                switch (entity)
                {
                    case EEntity.GigaBombDroid:
                        aiEntity = _gigaBombDroid;
                        break;
                    default:   
                        break;
                }
            }
            else
            {
                //ai
                if (_aiPoolDict.TryGetValue(entity, out aiPool))
                {
                    aiEntity = aiPool.GetAIEntity();
                }
            }

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

            return aiEntity;
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
            foreach (AIPool pool in _activeAIPools)
            {
                pool.ManagedTick();
            }

            foreach (AIEntity aiEntity in _activeAIEntities)
            {
                aiEntity.Tick();
            }
        }

        public void OnCreated()
        {
            if (!_gigaBombDroid)
            {
                _gigaBombDroid = Instantiate<GigaBombDroidAIEntity>(_gigaBombDroidPrefab);
                _gigaBombDroid.transform.parent = this.gameObject.transform;
                _gigaBombDroid.gameObject.SetActive(false);
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
