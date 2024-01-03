﻿using _Scripts._Game.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.General.Managers{
    
    public class AIPool : PoolComponentManager<AIEntity>
    {
        #region Prefab
        [Header("Prefab")]
        [SerializeField]
        private AIEntity _entityPrefab;
        #endregion

        protected override void Awake()
        {
            for (int i = 0; i < m_PoolCount; ++i)
            {
                GameObject newGO = Instantiate(_entityPrefab.gameObject);
                newGO.name = newGO.name + i;
                newGO.transform.parent = this.gameObject.transform;

                AIEntity comp = newGO.GetComponent(typeof(AIEntity)) as AIEntity;
                comp.UniqueTickGroup.AssignID((short)i);
                comp.UniqueTickGroup.TickMaster = this;
                newGO.SetActive(false);
                m_Pool.Push(comp);
            }

            AIManager.Instance.AssignAIPool(_entityPrefab.Entity, this);
        }

        public override void ManagedTick()
        {
            base.ManagedTick();
        }

        protected override void CheckPools()
        {
            LinkedListNode<AIEntity> node = m_Inuse.First;
            while (node != null)
            {
                LinkedListNode<AIEntity> current = node;
                node = node.Next;

                if (!IsActive(current.Value))
                {
                    current.Value.gameObject.SetActive(false);
                    current.Value.transform.parent = transform;
                    current.Value.transform.localPosition = Vector3.zero;
                    m_Pool.Push(current.Value);
                    m_Inuse.Remove(current);
                    m_NodePool.Push(current);

                    AIManager.Instance.UnassignSpawnedEntity(current.Value);
                }
            }
        }

        protected override bool IsActive(AIEntity entity)
        {
            return entity.IsAlive() && entity.gameObject.activeSelf;
        }

        public AIEntity GetAIEntity()
        {
            return GetPooledComponent();
        }
    }
    
}
