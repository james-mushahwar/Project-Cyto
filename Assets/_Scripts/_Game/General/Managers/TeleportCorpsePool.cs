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

        public override void ManagedTick()
        {
            base.ManagedTick();
        }

        protected override void CheckPools()
        {
            LinkedListNode<TeleportCorpse> node = m_Inuse.First;
            while (node != null)
            {
                LinkedListNode<TeleportCorpse> current = node;
                node = node.Next;

                if (!IsActive(current.Value))
                {
                    current.Value.gameObject.SetActive(false);
                    current.Value.transform.parent = transform;
                    current.Value.transform.localPosition = Vector3.zero;
                    m_Pool.Push(current.Value);
                    m_Inuse.Remove(current);
                    m_NodePool.Push(current);

                    CorpseManager.Instance.UnassignCorpse(current.Value);
                }
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
