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

        public override void ManagedTick()
        {
            base.ManagedTick();
        }

        protected override bool IsActive(BombDroidBombDropProjectile component)
        {
            return component.IsActive() && (component.Collided == false || component.ExplodeElapsed == false);
        }

        public bool TryBombDroidBombDropProjectile(EEntityType instigator, Vector3 startPosition, Vector3 direction)
        {
            bool found = false;
            BombDroidBombDropProjectile pooledComp = GetPooledComponent();

            if (pooledComp)
            {
                pooledComp.InstigatorType = instigator;
                pooledComp.transform.position = startPosition;
                pooledComp.transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
                pooledComp.gameObject.SetActive(true);
                found = true;
            }
            else
            {
                Debug.Log("No more bomb drop projectiles");
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
