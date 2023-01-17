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

        public float GetTargetScore(ETargetType targetType, Transform targetTransform, IPossessable possessable)
        {
            TargetingParameters tp = GetTargetingParameters(targetType);
            //Vector2 inputDirection =;
            return 1.0f;
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

        [System.Serializable]
        private struct TargetingParameters
        {
            [Header("Target parameters")]
            ETargetType _targetType;
            float _maxDistance;
            float _minDotProduct;

            [Header("Score Multiplers")]
            float _threatScoreMultiplier;
            float _angleScoreMultiplier;
            float _distanceScoreMultiplier;

            [Header("Modifiers")]
            bool _ignoreDistanceScore;
        }
    }
    
    public interface ITarget
    {
        public ETargetType TargetType { get; }
        public Transform GetTargetTransform();
    }
}
