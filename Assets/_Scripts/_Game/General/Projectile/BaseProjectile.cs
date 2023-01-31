using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.General.Projectile{
    
    public abstract class BaseProjectile : MonoBehaviour
    {
        protected float ProjectileLifetime { get; set; }
        protected float ProjectileLifetimeTimer { get; set; }
        public bool IsActive()
        {
            return ProjectileLifetimeTimer >= ProjectileLifetime;
        }
    }
    
}
