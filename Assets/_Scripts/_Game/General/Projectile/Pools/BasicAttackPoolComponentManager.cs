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
        private GameObject _playerBasicAttackProjectilePrefab;

        #endregion

        protected override void Awake()
        {
            base.Awake();
            Debug.Log("PoolComponentManager<T> awake check");
            for (int i = 0; i < m_PoolCount; ++i)
            {
                GameObject newGO = GameObject.Instantiate(_playerBasicAttackProjectilePrefab);
                newGO.transform.parent = this.gameObject.transform;

                BasicAttackProjectile comp = newGO.GetComponent<BasicAttackProjectile>();
                m_Pool.Push(comp);
                newGO.SetActive(false);
            }
        }

        protected override void FixedUpdate()
        {
            if (m_lastCheckFrame != Time.frameCount)
            {
                m_lastCheckFrame = Time.frameCount;
                CheckPools();
            }
        }

        protected override bool IsActive(BasicAttackProjectile component)
        {
            return component.IsActive() && component.HitTarget == false;
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
