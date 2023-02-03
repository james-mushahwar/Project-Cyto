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

            foreach (BombDroidBombDropProjectile bombDropProjectile in m_Pool)
            {
                bombDropProjectile.gameObject.SetActive(false);
            }
        }

        protected override bool IsActive(BombDroidBombDropProjectile component)
        {
            return component.IsActive();
        }

    }

}
