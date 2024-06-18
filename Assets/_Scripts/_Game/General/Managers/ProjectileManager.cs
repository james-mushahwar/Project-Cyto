using _Scripts._Game.General.Projectile;
using _Scripts._Game.General.Projectile.AI.BombDroid;
using _Scripts._Game.General.Projectile.Player;
using _Scripts._Game.General.Projectile.Pools;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.General.Managers{
    
    public enum EProjectileType
    {
        //Player
        BasicAttack             = 0,
        //AI
        BombDroidBombDrop       = 1000,
        BombDroidSuperBombDrop        ,
        //Environment
        COUNT                   = 100000
    }

    public class ProjectileManager : Singleton<ProjectileManager>, IManager
    {
        #region Projectile properties
        [Header("Basic attack projectile")]
        [SerializeField]
        private float _basicAttackLifetime;
        [SerializeField]
        private float _bombDroidBombDropAttackLifetime;

        public float BasicAttackLifetime { get => _basicAttackLifetime; }
        public float BombDroidBombDropAttackLifetime { get => _bombDroidBombDropAttackLifetime; }
        #endregion

        #region Pools
        private BasicAttackPoolComponentManager _basicAttackProjectilePool;
        private BombDroidBombDropPoolComponentManager _bdBombDropProjectilePool;
        private BombDroidSuperBombDropPoolComponentManager _bdSuperBombDropProjectilePool;

        private List<IManagedPool> _managedProjectilePools;
        #endregion

        protected override void Awake()
        {
            base.Awake();

            _managedProjectilePools = new List<IManagedPool>();
        }

        public void ManagedTick()
        {
            foreach (IManagedPool projectilePool in _managedProjectilePools)
            {
                projectilePool.ManagedTick();
            }
        }

        protected void Start()
        {
            _basicAttackProjectilePool       = GetComponentInChildren<BasicAttackPoolComponentManager>();
            _bdBombDropProjectilePool        = GetComponentInChildren<BombDroidBombDropPoolComponentManager>();
            _bdSuperBombDropProjectilePool   = GetComponentInChildren<BombDroidSuperBombDropPoolComponentManager>();

            //_particleTypePools.Add(EParticleType.BasicAttack, _basicAttackProjectilePool);

            _managedProjectilePools.Add(_basicAttackProjectilePool);
            _managedProjectilePools.Add(_bdBombDropProjectilePool);
            _managedProjectilePools.Add(_bdSuperBombDropProjectilePool);
        }


        public void SpawnProjectileByType(EProjectileType particleType, Vector3 spawnPosition, EEntityType instigator, IDamageable damageableTarget = null, int index = 0)
        {
            switch (particleType)
            {
                case EProjectileType.BasicAttack:
                    TryBasicAttackProjectile(damageableTarget, spawnPosition, index);
                    break;
                case EProjectileType.BombDroidBombDrop:
                    TryBombDroidBombDropProjectile(instigator, spawnPosition);
                    break;
                
                default: break;
            }
        }

        public bool TryBasicAttackProjectile(IDamageable damageable, Vector3 startPosition, int comboIndex)
        {
            return _basicAttackProjectilePool.TryBasicAttackProjectile(damageable, startPosition, comboIndex);
        }

        public void TryBombDroidBombDropProjectile(EEntityType instigator, Vector3 startPosition)
        {
            bool found = _bdBombDropProjectilePool.TryBombDroidBombDropProjectile(instigator, startPosition);
            if (found)
            {
                AudioManager.Instance.TryPlayAudioSourceAtLocation(EAudioType.SFX_Enemy_BombDroid_BombDropAttack, startPosition);
            }
        }

        public void TryBombDroidSuperBombDropProjectile(EEntityType instigator, Vector3 startPosition)
        {
            bool found = _bdSuperBombDropProjectilePool.TryBombDroidSuperBombDropProjectile(instigator, startPosition);
            if (found)
            {
                AudioManager.Instance.TryPlayAudioSourceAtLocation(EAudioType.SFX_Enemy_BombDroid_BombDropAttack, startPosition);
            }
        }

        public void ManagedPreInGameLoad()
        {
             
        }

        public void ManagedPostInGameLoad()
        {
             
        }

        public void ManagedPreMainMenuLoad()
        {
             
        }

        public void ManagedPostMainMenuLoad()
        {
             
        }
    }

}
