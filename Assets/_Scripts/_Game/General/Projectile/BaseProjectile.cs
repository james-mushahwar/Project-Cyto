using _Scripts._Game.General.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.General.Projectile{
    
    public class BaseProjectile : MonoBehaviour, ITickGroup
    {
        private readonly UniqueTickGroup _uniqueTickGroup = new UniqueTickGroup();

        public UniqueTickGroup UniqueTickGroup { get => _uniqueTickGroup; }
        protected float ProjectileLifetime { get; set; }
        protected float ProjectileLifetimeTimer { get; set; }

        public bool IsActive()
        {
            return ProjectileLifetimeTimer <= ProjectileLifetime;
        }
    }
    
}
