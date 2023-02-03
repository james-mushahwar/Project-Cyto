﻿using _Scripts._Game.General.Managers;
using _Scripts._Game.General.Projectile.Pools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.General.Projectile.Player{

    public class BasicAttackProjectile : BaseProjectile
    {
        private Transform _targetTransform;
        private Vector3 _startPosition;
        private Vector3 _endPosition;
        private bool _inverseDisplacement;

        [Header("Projectile movement")]
        [SerializeField]
        private AnimationCurve _speedCurve;
        [SerializeField]
        private AnimationCurve _displacementCurve;
        [SerializeField]
        private AnimationCurve _magnitudeCurve;

        public Transform TargetTransform { get => _targetTransform; set => _targetTransform = value; }
        public Vector3 StartPosition { get => _startPosition; set => _startPosition = value; }
        public Vector3 EndPosition { get => _endPosition; set => _endPosition = value; }

        private void Awake()
        {
            ProjectileLifetime = ProjectileManager.Instance.BasicAttackLifetime;
        }

        private void OnEnable()
        {
            ProjectileLifetimeTimer = 0.0f;
            _inverseDisplacement = (Random.Range(0, 2) == 1);
        }

        private void FixedUpdate()
        {
            ProjectileLifetimeTimer += Time.deltaTime;

            #region Projectile Movement
            float step = _speedCurve.Evaluate(ProjectileLifetimeTimer) * Time.deltaTime;
            
            Vector2 straightPathPosition = Vector2.MoveTowards(transform.position, _targetTransform.position, step);

            Vector2 perpendicularDirection = Vector2.Perpendicular((_targetTransform.position - transform.position).normalized);

            float displacement = _displacementCurve.Evaluate(ProjectileLifetimeTimer);
            float magnitude = _magnitudeCurve.Evaluate(ProjectileLifetimeTimer);

            Vector2 resultingPostion = straightPathPosition + (perpendicularDirection * displacement * magnitude * (_inverseDisplacement ? -1 : 1));
        
            transform.position = resultingPostion;
            #endregion
        }
    }
}
