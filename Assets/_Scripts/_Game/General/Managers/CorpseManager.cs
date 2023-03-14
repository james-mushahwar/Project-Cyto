using _Scripts._Game.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.General.Managers{
    
    public class CorpseManager : Singleton<CorpseManager>
    {
        #region AI
        private CorpsePool _bombDroidCorpsePool;
        private TeleportCorpsePool _bombDroidTeleportCorpsePool;
        #endregion

        protected override void Awake()
        {
            base.Awake();
        }

        public void AssignCorpsePool(EEntity entity, CorpsePool pool)
        {
            if (entity == EEntity.BombDroid)
            {
                if (_bombDroidCorpsePool == null)
                {
                    _bombDroidCorpsePool = pool;
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
            }
        }

        public void AssignTeleportCorpsePool(EEntity entity, TeleportCorpsePool pool)
        {
            if (entity == EEntity.BombDroid)
            {
                if (_bombDroidTeleportCorpsePool == null)
                {
                    _bombDroidTeleportCorpsePool = pool;
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
            }
        }
    }

}
