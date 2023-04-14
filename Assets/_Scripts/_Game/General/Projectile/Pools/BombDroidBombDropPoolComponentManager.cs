using _Scripts._Game.General.Managers;
using _Scripts._Game.General.Projectile.AI.BombDroid;
using _Scripts._Game.General.Projectile.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.General.Projectile.Pools{
    
    public class BombDroidBombDropPoolComponentManager : PoolComponentManager<BombDroidBombDropProjectile>
    {
        [Header("Prefab")]
        [SerializeField]
        private GameObject _bombDroidProjectilePrefab;

        protected override void Awake()
        {
            base.Awake();
            //Debug.Log("PoolComponentManager<T> awake check");
            for (int i = 0; i < m_PoolCount; ++i)
            {
                GameObject newGO = GameObject.Instantiate(_bombDroidProjectilePrefab);
                newGO.transform.parent = this.gameObject.transform;

                BombDroidBombDropProjectile comp = newGO.GetComponent<BombDroidBombDropProjectile>();
                comp.UniqueTickGroup.AssignID((short)i);
                comp.UniqueTickGroup.TickMaster = this;

                m_Pool.Push(comp);
                newGO.SetActive(false);
            }
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            if (m_lastCheckFrame != Time.frameCount)
            {
                m_lastCheckFrame = Time.frameCount;
                CheckPools();
            }
        }

        protected override bool IsActive(BombDroidBombDropProjectile component)
        {
            return component.IsActive() && (component.Collided == false || component.ExplodeElapsed == false);
        }

        public void TryBombDroidBombDropProjectile(EEntityType instigator, Vector3 startPosition)
        {
            BombDroidBombDropProjectile pooledComp = GetPooledComponent();

            if (pooledComp)
            {
                pooledComp.InstigatorType = instigator;
                pooledComp.transform.position = startPosition;
                pooledComp.gameObject.SetActive(true);
            }
            else
            {
                Debug.Log("No more bomb drop projectiles");
            }
            //return pooledComp;
        }

    }

}
