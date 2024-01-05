using _Scripts._Game.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.General.Managers{
    
    public class CorpseManager : Singleton<CorpseManager>, IManager
    {
        #region AI
        //private CorpsePool _bombDroidCorpsePool;
        //private TeleportCorpsePool _bombDroidTeleportCorpsePool;
        #endregion

        private Dictionary<EEntity, CorpsePool> _corpsePoolDict = new Dictionary<EEntity, CorpsePool>();
        private Dictionary<EEntity, TeleportCorpsePool> _teleportCorpsePoolDict = new Dictionary<EEntity, TeleportCorpsePool>();

        private List<IManagedPool> _activeCorpsePools = new List<IManagedPool>();
        private List<Corpse> _activeCorpses = new List<Corpse>();

        protected override void Awake()
        {
            base.Awake();

            _activeCorpsePools = new List<IManagedPool>();
            _activeCorpses = new List<Corpse>();
        }

        public void ManagedTick()
        {
            foreach (IManagedPool corpsePool in _activeCorpsePools)
            {
                corpsePool.ManagedTick();
            }

            foreach (Corpse corpse in _activeCorpses)
            {
                corpse.Tick();
            }
        }

        public void AssignCorpsePool(EEntity entity, CorpsePool pool)
        {
            if (!_corpsePoolDict.ContainsKey(entity))
            {
                _corpsePoolDict.TryAdd(entity, pool);
                _activeCorpsePools.Add(pool);
            }
        }

        private CorpsePool GetCorpsePool(EEntity entity)
        {
            CorpsePool pool = null;

            _corpsePoolDict.TryGetValue(entity, out pool);

            return pool;
        }

        public void TrySpawnCorpse(EEntity entity, Vector2 position)
        {
            CorpsePool pool = GetCorpsePool(entity);
            Corpse corpse = pool.GetCorpse();
            if (corpse != null)
            {
                corpse.transform.position = position + pool.PositionOffset;
                corpse.gameObject.SetActive(true);
                _activeCorpses.Add(corpse);
            }
        }

        public void AssignTeleportCorpsePool(EEntity entity, TeleportCorpsePool pool)
        {
            if (!_teleportCorpsePoolDict.ContainsKey(entity))
            {
                _teleportCorpsePoolDict.TryAdd(entity, pool);
                _activeCorpsePools.Add(pool);
            }
        }

        private TeleportCorpsePool GetTeleportCorpsePool(EEntity entity)
        {
            TeleportCorpsePool pool = null;

            _teleportCorpsePoolDict.TryGetValue(entity, out pool);

            return pool;
        }

        public void TrySpawnTeleportCorpse(EEntity entity, Vector2 position)
        {
            TeleportCorpsePool pool = GetTeleportCorpsePool(entity);
            TeleportCorpse teleportCorpse = pool.GetTeleportCorpse();
            if (teleportCorpse != null)
            {
                teleportCorpse.transform.position = position + pool.PositionOffset;
                teleportCorpse.gameObject.SetActive(true);
                _activeCorpses.Add(teleportCorpse);
            }
        }

        public void UnassignCorpse(Corpse corpse)
        {
            if (_activeCorpses.Contains(corpse))
            {
                _activeCorpses.Remove(corpse);
            }
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
