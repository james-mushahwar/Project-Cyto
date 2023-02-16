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
        private bool _explodeElapsed;

        public EEntityType Instigator { get => _instigator; set => _instigator = value; }
        public bool Collided { get => _collided; }
        public bool ExplodeElapsed { get => _explodeElapsed; }

        [Header("Components")]
        [SerializeField]
        private SpriteRenderer _spriteRenderer;    

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
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        private void OnEnable()
        {
            ProjectileLifetimeTimer = 0.0f;
            _collided = false;
            _explodeElapsed = false;
            _spriteRenderer.enabled = true;
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
                
                #endregion
            }
            else if (!_explodeElapsed)
            {
                if (UniqueTickGroup.CanTick())
                {
                    
                    
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            GameObject collidedGO = collision.gameObject;
            if (_collided)
            {
                return;
            }

            if (_instigator == EEntityType.Enemy)
            {
                if ((_playerLayerMask.value & (1 << collidedGO.layer)) > 0)
                {
                    _collided = true;
                    _explodeElapsed = true;
                    PlayerEntity.Instance.TakeDamage(1.0f, EEntityType.Enemy);
                }
            }
            else if (_instigator == EEntityType.BondedEnemy)
            {
                if ((_aiLayerMask.value & (1 << collidedGO.layer)) > 0)
                {
                    _collided = true;
                    _explodeElapsed = true;
                    IDamageable damageable = collidedGO.GetComponent<IDamageable>();
                    if (damageable != null)
                    {
                        damageable.TakeDamage(1.0f, EEntityType.Player);
                    }
                }
            }

            if (LayerMask.NameToLayer("Ground") == collidedGO.layer)
            {
                _collided = true;
                ParticleManager.Instance.TryPlayParticleSystem(EParticleType.BombDroidBombDrop, collision.ClosestPoint(transform.position), 0.0f);
                _spriteRenderer.enabled = false;
            }
        }

    }
    
}
