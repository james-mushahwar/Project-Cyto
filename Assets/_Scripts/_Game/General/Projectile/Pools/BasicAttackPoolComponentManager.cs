using _Scripts._Game.General.Managers;
using _Scripts._Game.General.Projectile.AI.BombDroid;
using _Scripts._Game.General.Projectile.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.General.Projectile.Pools{

    public class BasicAttackPoolComponentManager : PoolComponentManager<BasicAttackProjectile>
    {
        #region Projectile
        [SerializeField]
        private GameObject _bombDroidProjectilePrefab;
        
        #endregion

        protected override void Awake()
        {
            base.Awake();

            foreach (BasicAttackProjectile basicAttackProjectile in m_Pool)
            {
                basicAttackProjectile.gameObject.SetActive(false);
            }
        }

        protected override bool IsActive(BasicAttackProjectile component)
        {
            return component.IsActive();
        }

        public void TryBasicAttackProjectile(Transform targetTransform, Vector3 startPosition)
        {
            BasicAttackProjectile pooledComp = GetPooledComponent();

            if (pooledComp)
            {
                pooledComp.TargetTransform = targetTransform;
                pooledComp.StartPosition = startPosition;
                pooledComp.EndPosition = targetTransform.position;
                pooledComp.transform.position = startPosition;
                pooledComp.gameObject.SetActive(true);
            }
            else
            {
                Debug.Log("No more basic attack projectiles");
            }
            //return pooledComp;
        }
    }

}
