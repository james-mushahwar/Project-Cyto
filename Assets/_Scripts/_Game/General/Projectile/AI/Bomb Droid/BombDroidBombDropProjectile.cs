using _Scripts._Game.AI;
using _Scripts._Game.General.Managers;
using _Scripts._Game.Player;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

#if UNITY_EDITOR
using _Scripts.Editortools.Draw;
#endif

namespace _Scripts._Game.General.Projectile.AI.BombDroid{
    
    public class BombDroidBombDropProjectile : BaseProjectile
    {
        private bool _collided;
        private bool _explodeElapsed;

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
        private LayerMask _destructableLayerMask;
        [SerializeField]
        private LayerMask _playerLayerMask;

        [Header("Explosion")]
        [SerializeField]
        private float _explosionOverlapRange;
        [SerializeField]
        private ContactFilter2D _explosionContactFilter;
        [SerializeField]
        private float _explosionTimeDuration = 1.0f;
        private float _explosionTimer;

        private List<IDamageable> _damageablesHit = new List<IDamageable>();
        private Collider2D[] _hitColliders = new Collider2D[8];

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
            _explosionTimer = 0.0f;
            _spriteRenderer.enabled = true;

            //transform.rotation = Quaternion.LookRotation(Vector3.right);

            for (int i = 0; i < _hitColliders.Length; i++)
            {
                _hitColliders[i] = null;
            }
            _damageablesHit.Clear();
        }

        void OnDrawGizmos()
        {
            // scene debug updates
            if (_collided && !_explodeElapsed)
            {
            #if UNITY_EDITOR
                DrawGizmos.DrawSphereDebug(transform.position, _explosionOverlapRange);
            #endif
            }
        }

        private void FixedUpdate()
        {
            ProjectileLifetimeTimer += Time.deltaTime;

            if (!_collided)
            {
                #region Projectile Movement

                float step = _fallSpeedCurve.Evaluate(ProjectileLifetimeTimer) * Time.deltaTime;
                Vector2 newPosition = Vector2.MoveTowards(transform.position, transform.position + (-transform.up * 100.0f), step);
                transform.position = newPosition;
                
                #endregion
            }
            else if (!_explodeElapsed)
            {
                if (UniqueTickGroup.CanTick())
                {
                    int hitOverlapCount = Physics2D.OverlapCircle(transform.position, _explosionOverlapRange, _explosionContactFilter, _hitColliders);

                    if (hitOverlapCount > 0)
                    {
                        Collider2D col = null;
                        IDamageable damageable = null;

                        for (int i = 0; i < hitOverlapCount; i++)
                        {
                            col = _hitColliders[i];
                            if (col == null)
                            {
                                continue;
                            }
                            damageable = col.gameObject.GetComponent<IDamageable>();

                            if (damageable != null)
                            {
                                if (_damageablesHit.Contains(damageable))
                                {
                                    continue;
                                }

                                IPossessable possessable = col.gameObject.GetComponent<IPossessable>();
                                bool isPlayerPossessed = possessable != null && (PlayerEntity.Instance.Possessed != possessable);

                                if ((damageable is AIEntity && !isPlayerPossessed && (_instigatorType == EEntityType.BondedEnemy)) || (damageable is PlayerEntity && (_instigatorType == EEntityType.Enemy)))
                                {
                                    damageable.TakeDamage(EDamageType.BombDroid_BombDrop_Explosion, _instigatorType, transform.position);
                                    _damageablesHit.Add(damageable);
                                }
                                
                            }
                        }
                    }
                }

                _explosionTimer += Time.deltaTime;
                if (_explosionTimer >= _explosionTimeDuration)
                {
                    _explodeElapsed = true;
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

            bool hitFloor = false;

            if (_instigatorType == EEntityType.Enemy)
            {
                if ((_playerLayerMask.value & (1 << collidedGO.layer)) > 0)
                {
                    _collided = true;
                    _explodeElapsed = true;
                    PlayerEntity.Instance.TakeDamage(EDamageType.BombDroid_BombDrop_DirectHit, EEntityType.Enemy, transform.position);
                }
            }
            else if (_instigatorType == EEntityType.BondedEnemy)
            {
                if ((_destructableLayerMask.value & (1 << collidedGO.layer)) > 0)
                {
                    IDamageable destructable = collidedGO.GetComponent<IDamageable>();
                    if (destructable != null)
                    {
                        _collided = true;
                        _explodeElapsed = true;
                        hitFloor = true;

                        IDamageable damageable = collidedGO.GetComponent<IDamageable>();
                        if (damageable != null)
                        {
                            damageable.TakeDamage(EDamageType.BombDroid_BombDrop_DirectHit, EEntityType.BondedEnemy, transform.position);
                        }
                    }
                }
                else if ((_aiLayerMask.value & (1 << collidedGO.layer)) > 0)
                {
                    IPossessable possessable = collidedGO.GetComponent<IPossessable>();

                    bool isPlayerPossessed = possessable != null && (PlayerEntity.Instance.Possessed == possessable);

                    if (!isPlayerPossessed)
                    {
                        _collided = true;
                        _explodeElapsed = true;

                        IDamageable damageable = collidedGO.GetComponent<IDamageable>();
                        if (damageable != null)
                        {
                            damageable.TakeDamage(EDamageType.BombDroid_BombDrop_DirectHit, EEntityType.BondedEnemy, transform.position);
                        }
                    }
                    
                }
            }

            Vector2 closestCollisionPoint = collision.ClosestPoint(transform.position);

            if (hitFloor || LayerMask.NameToLayer("Ground") == collidedGO.layer)
            {
                _collided = true;
                ParticleManager.Instance.TryPlayParticleSystem(EParticleType.BombDroidBombDrop, closestCollisionPoint, 0.0f);
                _spriteRenderer.enabled = false;
                ProjectileLifetimeTimer = ProjectileLifetime - _explosionTimeDuration;
            }

            if (_collided)
            {
                AudioManager.Instance.TryPlayAudioSourceAtLocation(EAudioType.SFX_Environment_BombDroidBomb_Explosion, closestCollisionPoint);
            }
        }

    }
    
}
