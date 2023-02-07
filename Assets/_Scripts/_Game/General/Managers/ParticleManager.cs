using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.General.Managers{
    
    public enum EParticleType
    {
        BasicAttack,
        BombDroidBombDrop,
        COUNT
    }

    public class ParticleManager : Singleton<ParticleManager>
    {
        #region Pools
        #region Player
        private ParticlePool _basicAttackPool;
        #endregion

        #region AI
        private ParticlePool _bombDroidBombDropPool;
        #endregion

        #endregion

        protected override void Awake()
        {
            base.Awake();
        }

        public void AssignParticlePool(EParticleType particleType, ParticlePool pool)
        {
            if (particleType == EParticleType.BasicAttack)
            {
                if (_basicAttackPool == null)
                {
                    _basicAttackPool = pool;
                }
            }
            else if (particleType == EParticleType.BombDroidBombDrop)
            {
                if (_bombDroidBombDropPool == null)
                {
                    _bombDroidBombDropPool = pool;
                }
            }
            else
            {

            }
        }

        public void TryPlayParticleSystem(EParticleType particleType)
        {

        }

    }
    
}
