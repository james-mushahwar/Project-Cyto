﻿using _Scripts._Game.General.Projectile;
using _Scripts._Game.General.Projectile.AI.BombDroid;
using _Scripts._Game.General.Projectile.Player;
using _Scripts._Game.General.Projectile.Pools;
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
        [Header("Bsaic attack projectile")]
        [SerializeField]
        private float _basicAttackLifetime;

        public float BasicAttackLifetime { get => _basicAttackLifetime; }
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

        public void TryBasicAttackProjectile(Transform targetTransform, Vector3 startPosition)
        {
            _basicAttackProjectilePool.TryBasicAttackProjectile(targetTransform, startPosition);
        }

        //public void RegisterProjectilePool(ProjectilePoolComponentManager<T> projectilePool)
        //{
        //    if (projectilePool as BasicAttackProjectilePoolComponentManager<BasicAttackProjectile>)
        //    {
        //        _projectilePoolDict.Add(EProjectileType.BasicAttack, projectilePool);
        //    }
        //    else if (projectilePool as BasicAttackProjectilePoolComponentManager<BasicAttackProjectile>)
        //    {
        //        _projectilePoolDict.Add(EProjectileType.BombDroidBombDrop, projectilePool);
        //    }
        //}
    }

}
