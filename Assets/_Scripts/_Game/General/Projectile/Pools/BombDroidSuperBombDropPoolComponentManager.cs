using _Scripts._Game.General.Managers;
using _Scripts._Game.General.Projectile.AI.BombDroid;
using _Scripts._Game.General.Projectile.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.General.Projectile.Pools{
    
    public class BombDroidSuperBombDropPoolComponentManager : PoolComponentManager<BombDroidSuperBombDropProjectile>
    {
        [Header("Prefab")]
        [SerializeField]
        private GameObject _bombDroidSuperProjectilePrefab;

        protected override void Awake()
        {
            base.Awake();
            //Debug.Log("PoolComponentManager<T> awake check");
            for (int i = 0; i < m_PoolCount; ++i)
            {
                GameObject newGO = GameObject.Instantiate(_bombDroidSuperProjectilePrefab);
                newGO.transform.parent = this.gameObject.transform;

                BombDroidSuperBombDropProjectile comp = newGO.GetComponent<BombDroidSuperBombDropProjectile>();
                comp.UniqueTickGroup.AssignID((short)i);
                comp.UniqueTickGroup.TickMaster = this;

                m_Pool.Push(comp);
                newGO.SetActive(false);
            }
        }

        public override void ManagedTick()
        {
            base.ManagedTick();
        }

        protected override bool IsActive(BombDroidSuperBombDropProjectile component)
        {
            return component.IsActive() && (component.Collided == false || component.ExplodeElapsed == false);
        }

        public bool TryBombDroidSuperBombDropProjectile(EEntityType instigator, Vector3 startPosition)
        {
            bool found = false;
            BombDroidSuperBombDropProjectile pooledComp = GetPooledComponent();

            if (pooledComp)
            {
                pooledComp.InstigatorType = instigator;
                pooledComp.transform.position = startPosition;
                pooledComp.gameObject.SetActive(true);
                found = true;
            }
            else
            {
                Debug.Log("No more super bomb drop projectiles");
            }

            return found;
        }

        public override void ManagedPreInGameLoad()
        {
            
        }

        public override void ManagedPostInGameLoad()
        {
            
        }

        public override void ManagedPreMainMenuLoad()
        {
            
        }

        public override void ManagedPostMainMenuLoad()
        {
            
        }
    }

}
