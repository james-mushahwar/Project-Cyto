using _Scripts._Game.General.Managers;
using _Scripts._Game.General.Projectile.Pools;
using _Scripts._Game.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

#if UNITY_EDITOR
using _Scripts.Editortools.Draw;
#endif

namespace _Scripts._Game.General.Projectile.Player{

    public class BasicAttackProjectile : BaseProjectile
    {
        private IDamageable _damageableTarget;
        private Transform _targetTransform;
        private Vector3 _startPosition;
        private Vector3 _endPosition;
        private bool _inverseDisplacement;
        private bool _hitTarget;
        private int _comboIndex;

        [Header("Projectile movement")]
        [SerializeField]
        private AnimationCurve _speedCurve;
        [SerializeField]
        private AnimationCurve _displacementCurve;
        [SerializeField]
        private AnimationCurve _magnitudeCurve;

        [Header("Collision")]
        [SerializeField]
        private float _sqrDistanceToCollision = 1.0f;

        public IDamageable DamageableTarget { get => _damageableTarget; set => _damageableTarget = value; }
        public Transform TargetTransform { get => _targetTransform; set => _targetTransform = value; }
        public Vector3 StartPosition { get => _startPosition; set => _startPosition = value; }
        public Vector3 EndPosition { get => _endPosition; set => _endPosition = value; }
        public bool HitTarget { get => _hitTarget; }
        public int ComboIndex { get => _comboIndex; set => _comboIndex = value; }

        private void Awake()
        {
            ProjectileLifetime = ProjectileManager.Instance.BasicAttackLifetime;
        }

        private void OnEnable()
        {
            ProjectileLifetimeTimer = 0.0f;
            _hitTarget = false;
            _inverseDisplacement = (Random.Range(0, 2) == 1);
        }

        private void FixedUpdate()
        {
            ProjectileLifetimeTimer += Time.deltaTime;

            #region Projectile Movement
            if (_hitTarget == false)
            {
                float step = _speedCurve.Evaluate(ProjectileLifetimeTimer) * Time.deltaTime;

                Vector2 straightPathPosition = Vector2.MoveTowards(transform.position, _targetTransform.position, step);

                Vector2 direction = (_targetTransform.position - transform.position).normalized;
                Vector2 perpendicularDirection = Vector2.Perpendicular(direction);

                float displacement = _displacementCurve.Evaluate(ProjectileLifetimeTimer);
                float magnitude = _magnitudeCurve.Evaluate(ProjectileLifetimeTimer);

                Vector2 resultingPostion = straightPathPosition + (perpendicularDirection * displacement * magnitude * (_inverseDisplacement ? -1 : 1));

                transform.position = resultingPostion;

                #region Collision detection
                if (Vector2.SqrMagnitude(transform.position - _targetTransform.position) < _sqrDistanceToCollision)
                {
                    _hitTarget = true;
                    float vfxRotation = Vector2.Angle(Vector2.up, direction);
                    ParticleManager.Instance.TryPlayParticleSystem(EParticleType.BasicAttack, transform.position, vfxRotation);
                    _damageableTarget.TakeDamage(EDamageType.Player_BasicAttack, EEntityType.Player, transform.position);

                    EAudioType audioType = _comboIndex < 2 ? (_comboIndex < 1 ? EAudioType.SFX_BasicAttackImpact1 : EAudioType.SFX_BasicAttackImpact2) : EAudioType.SFX_BasicAttackImpact3;
                    if (_damageableTarget.Exposable != null)
                    {
                        if (_damageableTarget.Exposable.IsExposed())
                        {
                            audioType = _comboIndex < 2 ? (_comboIndex < 1 ? EAudioType.SFX_BasicAttackImpactWithLiquid1 : EAudioType.SFX_BasicAttackImpactWithLiquid2) : EAudioType.SFX_BasicAttackImpactWithLiquid3;
                        }
                    }
                    ((AudioManager)AudioManager.Instance).TryPlayAudioSourceAtLocation(audioType, transform.position);

                }
                #endregion
            }
            #endregion
        }

        void OnDrawGizmos()
        {
            // scene debug updates
        #if UNITY_EDITOR
            DrawGizmos.DrawSphereDebug(transform.position, Mathf.Sqrt(_sqrDistanceToCollision));
        #endif
        }
    }
}
