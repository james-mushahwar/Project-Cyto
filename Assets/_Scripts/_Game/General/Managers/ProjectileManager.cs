using _Scripts._Game.General.Projectile;
using _Scripts._Game.General.Projectile.Player;
using _Scripts._Game.General.Projectile.Pools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.General.Managers{
    
    public enum EProjectileType
    {
        BasicAttack,
        BombDroidBombDrop,
        COUNT
    }

    public abstract class ProjectileManager<T> : Singleton<ProjectileManager<T>> where T : BaseProjectile
    {
        #region Projectiles
        [SerializeField]
        private GameObject _playerBasicAttackProjectilePrefab;
        [SerializeField]
        private GameObject _bombDroidProjectilePrefab;

        private Dictionary<EProjectileType, PoolComponentManager<T>> _projectilePoolDict = new Dictionary<EProjectileType, PoolComponentManager<T>>();
        #endregion

        protected override void Awake()
        {
            base.Awake();
            for (int i = 0; i < (int)EProjectileType.COUNT; i++)
            {
                if (i ==  (int)EProjectileType.BasicAttack)
                {

                }
                else if (i == (int)EProjectileType.BombDroidBombDrop)
                {

                }
            }
        }


        public void RegisterProjectilePool(PoolComponentManager<T> projectilePool)
        {
            if (projectilePool is BasicAttackPoolComponentManager)
            {
                _projectilePoolDict.Add(EProjectileType.BasicAttack, projectilePool);
            }
            else if (projectilePool as BasicAttackPoolComponentManager)
            {
                _projectilePoolDict.Add(EProjectileType.BombDroidBombDrop, projectilePool);
            }
        }
    }

}
