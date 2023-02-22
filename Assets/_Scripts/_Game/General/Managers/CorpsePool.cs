﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _Scripts._Game.AI;

namespace _Scripts._Game.General.Managers{
    
    public class CorpsePool : PoolComponentManager<Corpse>
    {
        [Header("Corpse properties")]
        [SerializeField]
        private EEntity _entity;
        [SerializeField]
        private Vector2 _positionOffset;

        public Vector2 PositionOffset { get => _positionOffset; }

        [SerializeField]
        private Corpse _corpsePrefab;

        protected override void Awake()
        {
            for (int i = 0; i < m_PoolCount; ++i)
            {
                GameObject newGO = Instantiate(_corpsePrefab.gameObject);
                newGO.transform.parent = this.gameObject.transform;
                newGO.SetActive(false);

                Corpse comp = newGO.GetComponent(typeof(Corpse)) as Corpse;
                m_Pool.Push(comp);
            }

            CorpseManager.Instance.AssignCorpsePool(_entity, this);
            //ParticleManager.Instance.AssignParticlePool(_particleType, this);
        }
        protected override bool IsActive(Corpse component)
        {
            return component.IsActive();
        }

        public Corpse GetCorpse()
        {
            return GetPooledComponent();
        }
    }
    
}
