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
        [SerializeField]
        private GameObject _bombDroidProjectilePrefab;

        protected override void Awake()
        {
            base.Awake();
            Debug.Log("PoolComponentManager<T> awake check");
            for (int i = 0; i < m_PoolCount; ++i)
            {
                GameObject newGO = GameObject.Instantiate(_bombDroidProjectilePrefab);
                newGO.transform.parent = this.gameObject.transform;

                BombDroidBombDropProjectile comp = newGO.GetComponent<BombDroidBombDropProjectile>();
                m_Pool.Push(comp);
                newGO.SetActive(false);
            }
        }

        protected override bool IsActive(BombDroidBombDropProjectile component)
        {
            return component.IsActive();
        }

    }

}
