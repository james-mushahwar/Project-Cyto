using _Scripts._Game.General.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.General.Projectile{
    
    public class BaseProjectile : MonoBehaviour, ITickGroup, ITeleportEntity
    {
        protected GameObject _instigator;
        protected EEntityType _instigatorType;
        public EEntityType InstigatorType { get => _instigatorType; set => _instigatorType = value; }

        private readonly UniqueTickGroup _uniqueTickGroup = new UniqueTickGroup();

        public UniqueTickGroup UniqueTickGroup { get => _uniqueTickGroup; }
        protected float ProjectileLifetime { get; set; }
        protected float ProjectileLifetimeTimer { get; set; }

        public bool IsActive()
        {
            return ProjectileLifetimeTimer <= ProjectileLifetime;
        }

        public void Teleport(ITeleporter teleporter, Vector3 position, Vector3 direction)
        {
            transform.position = position;
            Vector3 angleBetween = Quaternion.FromToRotation(transform.rotation.eulerAngles, direction).eulerAngles;

            //transform.rotation = Quaternion.LookRotation(direction.normalized);
            //transform.Rotate(angleBetween);
            transform.rotation *= Quaternion.FromToRotation(-transform.up, direction);
            transform.rotation *= Quaternion.FromToRotation(transform.forward, Vector3.forward);
        }
    }
    
}
