using _Scripts._Game.General.Managers;
using _Scripts._Game.Player;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace _Scripts._Game.General.Projectile.AI.BombDroid{
    
    public class BombDroidBombDropProjectile : BaseProjectile
    {
        private EEntityType _instigator;
        private bool _collided;

        public EEntityType Instigator { get => _instigator; set => _instigator = value; }
        public bool Collided { get => _collided; }

        [Header("Projectile movement")]
        [SerializeField]
        private AnimationCurve _fallSpeedCurve;

        [Header("Collision")]
        [SerializeField]
        private float _sqrDistanceToCollision = 1.0f;
        [SerializeField]
        private LayerMask _aiLayerMask;
        [SerializeField]
        private LayerMask _playerLayerMask;

        private void Awake()
        {
            ProjectileLifetime = ProjectileManager.Instance.BombDroidBombDropAttackLifetime;
        }

        private void OnEnable()
        {
            ProjectileLifetimeTimer = 0.0f;
            _collided = false;
        }

        private void FixedUpdate()
        {
            ProjectileLifetimeTimer += Time.deltaTime;

            if (!_collided)
            {
                #region Projectile Movement

                float step = _fallSpeedCurve.Evaluate(ProjectileLifetimeTimer) * Time.deltaTime;
                Vector2 newPosition = Vector2.MoveTowards(transform.position, transform.position + (Vector3.down * 100.0f), step);
                transform.position = newPosition;

                    #region Collision detection
                    //if (Vector2.SqrMagnitude(transform.position - _targetTransform.position) < _sqrDistanceToCollision)
                    //{
                    //    _hitTarget = true;
                    //    float vfxRotation = Vector2.Angle(Vector2.up, direction);
                    //    ParticleManager.Instance.TryPlayParticleSystem(EParticleType.BasicAttack, transform.position, vfxRotation);
                    //    PlayerEntity.Instance.AttackingSM.DamageableTarget.TakeDamage(1.0f, EEntityType.Player);
                    //}
                    #endregion
           
                #endregion
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log("Trigger enter 2d");
            GameObject collidedGO = collision.gameObject;


            if (_instigator == EEntityType.Enemy)
            {
                if ((_playerLayerMask.value & (1 << collidedGO.layer)) > 0)
                {
                    _collided = true;
                }
            }
            else if (_instigator == EEntityType.BondedEnemy)
            {
                if ((_aiLayerMask.value & (1 << collidedGO.layer)) > 0)
                {
                    _collided = true;
                }
            }

            if (LayerMask.NameToLayer("Ground") == collidedGO.layer)
            {
                _collided = true;
            }
        }

    }
    
}
