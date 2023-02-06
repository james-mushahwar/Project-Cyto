using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.General.Managers{
    
    public class ParticleManager : PoolComponentManager<ParticleSystem>
    {
        #region Prefabs
        [Header("Prefabs")]
        [SerializeField]
        private ParticleSystem _basicAttackPS; 

        #endregion


        protected override void Awake()
        {
            base.Awake();
            Debug.Log("PoolComponentManager<T> awake check");
            for (int i = 0; i < m_PoolCount; ++i)
            {
                GameObject newGO = new GameObject("Pooled Particle System" + i);
                newGO.transform.parent = this.gameObject.transform;

                ParticleSystem comp = newGO.AddComponent(typeof(ParticleSystem)) as ParticleSystem;
                comp.Stop();
                comp = CopyComponent(_basicAttackPS, comp);
                m_Pool.Push(comp);
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }
    
        // Update is called once per frame
        void Update()
        {
            
        }

        protected override bool IsActive(ParticleSystem component)
        {
            return component.IsAlive();
        }

    }
    
}
