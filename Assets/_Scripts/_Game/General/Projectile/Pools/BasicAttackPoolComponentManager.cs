using _Scripts._Game.General.Managers;
using _Scripts._Game.General.Projectile.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.General.Projectile.Pools{
    
    public class BasicAttackPoolComponentManager : PoolComponentManager<BasicAttackProjectile>
    {
        // Start is called before the first frame update
        void Start()
        {
            
        }
    
        // Update is called once per frame
        void Update()
        {
            
        }

        protected override bool IsActive(BasicAttackProjectile component)
        {
            return component.IsActive();
        }
    }
    
}
