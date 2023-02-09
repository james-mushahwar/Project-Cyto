using _Scripts._Game.General.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.General.Projectile.AI.BombDroid{
    
    public class BombDroidBombDropProjectile : BaseProjectile
    {
        private EEntityType _instigator;

        public EEntityType Instigator { get => _instigator; set => _instigator = value; }

        [Header("Projectile movement")]
        [SerializeField]
        private AnimationCurve _fallSpeedCurve;

        [Header("Collision")]
        [SerializeField]
        private float _sqrDistanceToCollision = 1.0f;

        private void Awake()
        {
            ProjectileLifetime = ProjectileManager.Instance.BombDroidBombDropAttackLifetime;
        }

        private void OnEnable()
        {
            ProjectileLifetimeTimer = 0.0f;
        }
    }
    
}
