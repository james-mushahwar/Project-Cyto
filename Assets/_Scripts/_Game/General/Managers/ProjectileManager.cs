using _Scripts._Game.General.Projectile;
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
        #region Pools
        private PoolComponentManager<BasicAttackProjectile> _basicAttackProjectilePool;
        private PoolComponentManager<BombDroidBombDropProjectile> _bdBombDropProjectilePool;
        #endregion

        protected override void Awake()
        {
            base.Awake();
            
        }

        protected void Start()
        {
            _basicAttackProjectilePool = GetComponentInChildren<PoolComponentManager<BasicAttackProjectile>>();
            _bdBombDropProjectilePool = GetComponentInChildren<PoolComponentManager<BombDroidBombDropProjectile>>();
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
