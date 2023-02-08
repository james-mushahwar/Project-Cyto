using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.General.Managers{
    
    public class ParticlePool : PoolComponentManager<ParticleSystem>
    {
        #region Prefab
        [Header("Prefab")]
        [SerializeField]
        private ParticleSystem _particlePrefab;
        [SerializeField]
        private EParticleType _particleType;
        [SerializeField]
        private float _degreesToUpwardDirection;

        public float DegreesToUpwardDirection { get => _degreesToUpwardDirection; }
        #endregion

        protected override void Awake()
        {
            base.Awake();
            Debug.Log("PoolComponentManager<T> awake check");
            for (int i = 0; i < m_PoolCount; ++i)
            {
                GameObject newGO = Instantiate(_particlePrefab.gameObject);
                newGO.transform.parent = this.gameObject.transform;

                ParticleSystem comp = newGO.GetComponent(typeof(ParticleSystem)) as ParticleSystem;
                comp.Stop();
                m_Pool.Push(comp);
            }

            ParticleManager.Instance.AssignParticlePool(_particleType, this);
        }

        protected override bool IsActive(ParticleSystem component)
        {
            return component.IsAlive();
        }

        public ParticleSystem GetParticleSystem()
        {
            return GetPooledComponent();
        }
    }
    
}
