using _Scripts._Game.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.General.Managers{
    
    public class CorpseManager : Singleton<CorpseManager>, IManager
    {
        #region AI
        private CorpsePool _bombDroidCorpsePool;
        private TeleportCorpsePool _bombDroidTeleportCorpsePool;
        #endregion

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
            if (entity == EEntity.BombDroid)
            {
                if (_bombDroidCorpsePool == null)
                {
                    _bombDroidCorpsePool = pool;
                    _activeCorpsePools.Add(pool);
                }
            }
            else
            {

            }
        }

        private CorpsePool GetCorpsePool(EEntity entity)
        {
            if (entity == EEntity.BombDroid)
            {
                return _bombDroidCorpsePool;
            }
            else
            {
                return null;
            }
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
            if (entity == EEntity.BombDroid)
            {
                if (_bombDroidTeleportCorpsePool == null)
                {
                    _bombDroidTeleportCorpsePool = pool;
                    _activeCorpsePools.Add(pool);
                }
            }
            else
            {

            }
        }

        private TeleportCorpsePool GetTeleportCorpsePool(EEntity entity)
        {
            if (entity == EEntity.BombDroid)
            {
                return _bombDroidTeleportCorpsePool;
            }
            else
            {
                return null;
            }
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
