using _Scripts._Game.General.Projectile;
using _Scripts._Game.General.Projectile.AI.BombDroid;
using _Scripts._Game.General.Projectile.Player;
using _Scripts._Game.General.Projectile.Pools;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.General.Managers{
    
    public enum EProjectileType
    {
        BasicAttack,
        BombDroidBombDrop,
        COUNT
    }

    public class ProjectileManager : Singleton<ProjectileManager>
    {
        #region Projectile properties
        [Header("Basic attack projectile")]
        [SerializeField]
        private float _basicAttackLifetime;
        [SerializeField]
        private float _bombDroidBombDropAttackLifetime;

        public float BasicAttackLifetime { get => _basicAttackLifetime; }
        public float BombDroidBombDropAttackLifetime { get => _bombDroidBombDropAttackLifetime; }
        #endregion

        #region Pools
        private BasicAttackPoolComponentManager _basicAttackProjectilePool;
        private BombDroidBombDropPoolComponentManager _bdBombDropProjectilePool;
        #endregion

        protected override void Awake()
        {
            base.Awake();
        }

        protected void Start()
        {
            _basicAttackProjectilePool = GetComponentInChildren<BasicAttackPoolComponentManager>();
            _bdBombDropProjectilePool = GetComponentInChildren<BombDroidBombDropPoolComponentManager>();
        }

        public bool TryBasicAttackProjectile(IDamageable damageable, Vector3 startPosition, int comboIndex)
        {
            return _basicAttackProjectilePool.TryBasicAttackProjectile(damageable, startPosition, comboIndex);
        }

        public void TryBombDroidBombDropProjectile(EEntityType instigator, Vector3 startPosition)
        {
            _bdBombDropProjectilePool.TryBombDroidBombDropProjectile(instigator, startPosition);
        }
    }

}
