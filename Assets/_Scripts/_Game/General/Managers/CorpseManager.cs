using _Scripts._Game.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.General.Managers{
    
    public class CorpseManager : Singleton<CorpseManager>
    {
        #region AI
        private CorpsePool _bombDroidCorpsePool;
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
    }

}
