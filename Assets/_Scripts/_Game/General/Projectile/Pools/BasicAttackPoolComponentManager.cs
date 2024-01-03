﻿using _Scripts._Game.General.Managers;
using _Scripts._Game.General.Projectile.AI.BombDroid;
using _Scripts._Game.General.Projectile.Player;
using _Scripts._Game.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.General.Projectile.Pools{

    public class BasicAttackPoolComponentManager : PoolComponentManager<BasicAttackProjectile>
    {
        #region Projectile
        [Header("Prefab")]
        [SerializeField]
        private GameObject _playerBasicAttackProjectilePrefab;

        #endregion

        protected override void Awake()
        {
            base.Awake();
            //Debug.Log("PoolComponentManager<T> awake check");
            for (int i = 0; i < m_PoolCount; ++i)
            {
                GameObject newGO = GameObject.Instantiate(_playerBasicAttackProjectilePrefab);
                newGO.transform.parent = this.gameObject.transform;

                BasicAttackProjectile comp = newGO.GetComponent<BasicAttackProjectile>();
                m_Pool.Push(comp);
                newGO.SetActive(false);
            }
        }

        public override void ManagedTick()
        {
            base.ManagedTick();
        }

        protected override bool IsActive(BasicAttackProjectile component)
        {
            return component.IsActive() && component.HitTarget == false;
        }

        public bool TryBasicAttackProjectile(IDamageable damageable, Vector3 startPosition, int comboIndex = 0)
        {
            BasicAttackProjectile pooledComp = GetPooledComponent();

            if (pooledComp)
            {
                pooledComp.DamageableTarget = damageable;
                pooledComp.TargetTransform = damageable.Transform;
                pooledComp.StartPosition = startPosition;
                pooledComp.EndPosition = damageable.Transform.position;
                pooledComp.transform.position = startPosition;
                pooledComp.gameObject.SetActive(true);

                pooledComp.ComboIndex = comboIndex;
                EAudioType audioType = comboIndex < 2 ? (comboIndex < 1 ? EAudioType.SFX_Player_BasicAttack1 : EAudioType.SFX_Player_BasicAttack2) : EAudioType.SFX_Player_BasicAttack3;
                AudioManager.Instance.TryPlayAudioSourceAtLocation(audioType, transform.position);
                return true;
            }
            else
            {
                Debug.Log("No more basic attack projectiles");
            }
            return false;
        }
    }

}
