﻿using _Scripts._Game.AI.MovementStateMachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using _Scripts._Game.Player;
using _Scripts._Game.AI.Bonding;
using System;
using UnityEngine.InputSystem;
using _Scripts.Editortools.Draw;

namespace _Scripts._Game.General.Managers{
    
    public enum ETargetType
    {
        AbovePlayer,
        Possessable,
        Damageable,
        INVALID,
    }

    public class TargetManager : Singleton<TargetManager> 
    {
        [System.Serializable]
        private struct TargetingParameters
        {
            [Header("Target parameters")]
            [SerializeField]
            ETargetType _targetType;
            [SerializeField]
            float _maxSqDistance;
            [SerializeField]
            float _minDotProduct;

            [Header("Score Multiplers")]
            [SerializeField]
            float _threatScoreMultiplier;
            [SerializeField]
            float _angleScoreMultiplier;
            [SerializeField]
            float _distanceScoreMultiplier;

            [Header("Modifiers")]
            [SerializeField]
            bool _ignoreDotProductScore;
            [SerializeField]
            bool _ignoreDistanceScore;

            public ETargetType TargetType { get => _targetType; }
            public float MaxSqDistance { get => _maxSqDistance; }
            public float MinDotProduct { get => _minDotProduct; }

            public float ThreatScoreMultiplier { get => _threatScoreMultiplier; }
            public float AngleScoreMultiplier { get => _angleScoreMultiplier; }
            public float DistanceScoreMultiplier { get => _distanceScoreMultiplier; }

            public bool IgnoreDotProductScore { get => _ignoreDotProductScore; }
            public bool IgnoreDistanceScore { get => _ignoreDistanceScore; }
        }

        [Header("Targeting Parameters")]
        [SerializeField]
        private TargetingParameters _possessableTargetParameters;
        [SerializeField]
        private TargetingParameters _damageableTargetParameters;

        [Header("Target type transforms")]
        private Dictionary<ETargetType, ITarget> _targetsDict = new Dictionary<ETargetType, ITarget>();
        private int _targetsIndex = 0;

        private void Start()
        {
            var targets = FindObjectsOfType<MonoBehaviour>().OfType<ITarget>();

            for (int i = 0; i < targets.Count(); ++i)
            {
                _targetsDict.Add(targets.ElementAt(i).TargetType, targets.ElementAt(i));
            }
        }

        private void FixedUpdate()
        {
            ManagedTargetsTick();
        }

        private void ManagedTargetsTick()
        {
            ETargetType tickType = (ETargetType)_targetsIndex;
            _targetsDict.TryGetValue(tickType, out ITarget target);

            if (target != null)
            {
                target.ManagedTargetTick();
            }

            _targetsIndex++;
            if (_targetsIndex >= (int)ETargetType.INVALID)
            {
                _targetsIndex = 0;
            }
        }

        public float GetPossessableTargetScore(IPossessable pInstigator, IPossessable pTarget)
        {
            TargetingParameters tp = GetTargetingParameters(ETargetType.Possessable);
            Vector2 inputDirection = pInstigator.GetMovementInput().normalized;
            if (inputDirection.sqrMagnitude == 0.0f)
            {
                inputDirection = pInstigator.FacingRight ? new Vector2(1.0f, 0.0f) : new Vector2(-1.0f, 0.0f);
            }
            #if UNITY_EDITOR
            DrawGizmos.ForPointsDebug(pInstigator.Transform.position, pInstigator.Transform.position + (Vector3)(inputDirection * 10.0f));
            DrawGizmos.ForDirectionDebug(pInstigator.Transform.position, pInstigator.Transform.position + (Vector3.right * 10.0f));
            DrawGizmos.ForDirectionGizmo(pInstigator.Transform.position, pInstigator.Transform.position + (Vector3.right * 10.0f));
            
            #endif

            float finalScore = 0.0f;
            float distanceScore = 0.0f;
            float angleScore = 0.0f;

            Vector2 targetVector = pTarget.Transform.position - pInstigator.Transform.position;

            if (!tp.IgnoreDotProductScore)
            {
                float dotProductDiff = Vector2.Dot(targetVector.normalized, inputDirection.normalized);

                if (dotProductDiff > tp.MinDotProduct)
                {
                    float dotRange = 1 - tp.MinDotProduct;
                    float alpha = Mathf.Lerp(0.1f, 1.0f, (dotProductDiff - tp.MinDotProduct) / dotRange);
                    angleScore = alpha * tp.AngleScoreMultiplier;
                }
                else
                {
                    return -100.0f;
                }
            }

            if (!tp.IgnoreDistanceScore)
            {
                float distanceToTarget = targetVector.SqrMagnitude();
                if (distanceToTarget < tp.MaxSqDistance)
                {
                    distanceScore = 1.0f - (distanceToTarget / tp.MaxSqDistance) * tp.DistanceScoreMultiplier;
                }
                else
                {
                    return -100.0f;
                }
            }

            finalScore = (distanceScore + angleScore);

            return finalScore;
        }

        public float GetDamageableTargetScore(IDamageable dInstigator, IDamageable dTarget)
        {
            TargetingParameters tp = GetTargetingParameters(ETargetType.Damageable);
            Vector2 inputDirection = dInstigator.GetMovementInput();
            if (inputDirection.sqrMagnitude == 0.0f)
            {
                inputDirection = dInstigator.FacingRight ? new Vector2(1.0f, 0.0f) : new Vector2(-1.0f, 0.0f);
            }

            float finalScore = 0.0f;
            float distanceScore = 0.0f;
            float angleScore = 0.0f;

            Vector2 targetVector = dTarget.Transform.position - dInstigator.Transform.position;

            if (!tp.IgnoreDotProductScore)
            {
                float dotProductDiff = Vector2.Dot(targetVector.normalized, inputDirection.normalized);

                if (dotProductDiff > tp.MinDotProduct)
                {
                    float dotRange = 1 - tp.MinDotProduct;
                    float alpha = Mathf.Lerp(0.1f, 1.0f, (dotProductDiff - tp.MinDotProduct) / dotRange);
                    angleScore = alpha * tp.AngleScoreMultiplier;
                }
                else
                {
                    return -100.0f;
                }
            }

            if (!tp.IgnoreDistanceScore)
            {
                float distanceToTarget = targetVector.SqrMagnitude();
                if (distanceToTarget < tp.MaxSqDistance)
                {
                    distanceScore = 1.0f - (distanceToTarget / tp.MaxSqDistance) * tp.DistanceScoreMultiplier;
                }
                else
                {
                    return -100.0f;
                }
            }

            finalScore = (distanceScore + angleScore);

            return finalScore;
        }

        private TargetingParameters GetTargetingParameters(ETargetType targetType)
        {
            switch (targetType)
            {
                case ETargetType.Possessable:
                    return _possessableTargetParameters;
                case ETargetType.Damageable:
                    return _damageableTargetParameters;
                default:
                    return _damageableTargetParameters;
            }
        }

        public Transform GetTargetTypeTransform(ETargetType type)
        {
            _targetsDict.TryGetValue(type, out ITarget target);
            if (target != null)
            {
                return target.GetTargetTransform();
            }
            return transform;
        }

    }
    
    public interface ITarget
    {
        public ETargetType TargetType { get; }
        public Transform GetTargetTransform();
        public void ManagedTargetTick();
    }
}
