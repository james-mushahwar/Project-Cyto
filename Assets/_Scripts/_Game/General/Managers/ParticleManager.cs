using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.General.Managers{
    
    public enum EParticleType
    {
        BasicAttack,
        Exposed,
        TargetHighlight,
        BombDroidBombDrop,
        COUNT
    }

    public class ParticleManager : Singleton<ParticleManager>
    {
        #region Pools
        #region Player
        private ParticlePool _basicAttackPool;
        private ParticlePool _exposedPool;
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
            else if (particleType == EParticleType.Exposed)
            {
                if (_exposedPool == null)
                {
                    _exposedPool = pool;
                }
            }
        }

        private ParticlePool GetParticlePool(EParticleType particleType)
        {
            if (particleType == EParticleType.BasicAttack)
            {
                return _basicAttackPool;
            }
            else if (particleType == EParticleType.BombDroidBombDrop)
            {
                return _bombDroidBombDropPool;
            }
            else if (particleType == EParticleType.Exposed)
            {
                return _exposedPool;
            }
            else
            {
                return null;
            }
        }

        public void TryPlayParticleSystem(EParticleType particleType, Vector2 position, float rotationDeg)
        {
            ParticlePool pool = GetParticlePool(particleType);
            ParticleSystem ps = pool.GetParticleSystem();
            if (ps != null)
            {
                ps.transform.position = position + pool.PositionOffset;
                var main = ps.main;
                main.startRotation = rotationDeg + pool.DegreesToUpwardDirection;
                ps.Play();
            }
        }

    }
    
}
