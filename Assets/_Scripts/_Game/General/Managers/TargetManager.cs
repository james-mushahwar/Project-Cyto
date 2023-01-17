using _Scripts._Game.AI.MovementStateMachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using _Scripts._Game.Player;

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

        private Dictionary<ETargetType, ITarget> _targetsDict = new Dictionary<ETargetType, ITarget>();

        [Header("Targeting Parameters")]
        [SerializeField]
        private TargetingParameters _possessableTargetParameters;
        [SerializeField]
        private TargetingParameters _damageableTargetParameters;

        private void Start()
        {
            var targets = FindObjectsOfType<MonoBehaviour>().OfType<ITarget>();

            for (int i = 0; i < targets.Count(); ++i)
            {
                _targetsDict.Add(targets.ElementAt(i).TargetType, targets.ElementAt(i));
            }
        }

        public float GetPossessableTargetScore(IPossessable pInstigator, IPossessable pTarget)
        {
            TargetingParameters tp = GetTargetingParameters(ETargetType.Possessable);
            Vector2 inputDirection = pInstigator.GetMovementInput();

            float finalScore = 0.0f;
            float distanceScore = 0.0f;
            float angleScore = 0.0f;

            Vector2 targetVector = pTarget.PossessableTransform.position - pInstigator.PossessableTransform.position;

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
                    break;
                case ETargetType.Damageable:
                    return _damageableTargetParameters;
                    break;
                default:
                    return _damageableTargetParameters;
                    break;
            }
        }
    }
    
    public interface ITarget
    {
        public ETargetType TargetType { get; }
        public Transform GetTargetTransform();
    }
}
