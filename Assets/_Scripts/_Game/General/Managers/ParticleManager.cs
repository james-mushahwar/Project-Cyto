using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.General.Managers{
    
    public class ParticleManager : PoolComponentManager<ParticleSystem>
    {
        protected new void Awake()
        {
            base.Awake();
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
