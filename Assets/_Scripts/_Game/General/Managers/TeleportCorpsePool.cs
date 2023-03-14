using System.Collections;
using System.Collections.Generic;
using _Scripts._Game.AI;
using UnityEngine;

namespace _Scripts._Game.General.Managers{
    
    public class TeleportCorpsePool : PoolComponentManager<TeleportCorpse>
    {
        [Header("Teleport Corpse properties")]
        [SerializeField]
        private EEntity _entity;
        [SerializeField]
        private Vector2 _positionOffset;

        public Vector2 PositionOffset { get => _positionOffset; }

        [SerializeField]
        private TeleportCorpse _corpsePrefab;

        protected override void Awake()
        {
            for (int i = 0; i < m_PoolCount; ++i)
            {
                GameObject newGO = Instantiate(_corpsePrefab.gameObject);
                newGO.transform.parent = this.gameObject.transform;

                TeleportCorpse comp = newGO.GetComponent(typeof(TeleportCorpse)) as TeleportCorpse;
                m_Pool.Push(comp);
                newGO.SetActive(false);
            }

            CorpseManager.Instance.AssignTeleportCorpsePool(_entity, this);
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            if (m_lastCheckFrame != Time.frameCount)
            {
                m_lastCheckFrame = Time.frameCount;
                CheckPools();
            }
        }

        protected override bool IsActive(TeleportCorpse component)
        {
            return component.IsActive();
        }

        public TeleportCorpse GetTeleportCorpse()
        {
            return GetPooledComponent();
        }
    }
    
}
