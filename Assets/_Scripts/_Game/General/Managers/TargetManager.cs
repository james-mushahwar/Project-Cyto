using _Scripts._Game.AI.MovementStateMachine;
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
        Bondable,
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

        [Header("Damageable Range properties")]
        [SerializeField]
        private float _damageableOverlapRange;
        [SerializeField]
        private ContactFilter2D _damageableContactFilter;

        private Collider2D[] _damageColliders = new Collider2D[20];
        private IDamageable _damageableTarget;

        public IDamageable DamageableTarget { get => _damageableTarget; }

        [Header("Bonding properties")]
        [SerializeField]
        private float _bondingOverlapRange;
        [SerializeField]
        private ContactFilter2D _bondingContactFilter;

        private Collider2D[] _bondingColliders = new Collider2D[20];
        private IBondable _bondableTarget;
        private IBondable _lockedBondableTarget;

        public IBondable BondableTarget { get => _bondableTarget; }
        public IBondable LockedBondableTarget { get => _lockedBondableTarget; set => _lockedBondableTarget = value; }

        #region VFX
        [Header("VFX")]
        [SerializeField]
        private ParticleSystem _bondHighlightPS;

        [SerializeField]
        private ParticleSystem _targetHighlightPS;
        #endregion

        private void Start()
        {
            var targets = FindObjectsOfType<MonoBehaviour>().OfType<ITarget>();

            for (int i = 0; i < targets.Count(); ++i)
            {
                _targetsDict.Add(targets.ElementAt(i).TargetType, targets.ElementAt(i));
            }
        }

        private void Update()
        {
            ManagedTargetsTick();

            // bondable target
            FindBestBondable();

            //Debug.Log("TargetManager fixed update: deltaTime is: " + Time.deltaTime + " and fixedDeltaTime is: " + Time.fixedDeltaTime);

            // damageable target
            FindBestDamageable();
        }

        private void FindBestBondable()
        {
            IBondable newBondable = null;

            int aiOverlapCount = Physics2D.OverlapCircle(PlayerEntity.Instance.Transform.position, _bondingOverlapRange, _bondingContactFilter, _bondingColliders);

            if (aiOverlapCount > 0)
            {
                float bestScore = -1.0f;
                IBondable currentBondable = null;

                Collider2D col = null;

                for (int i = 0; i < aiOverlapCount; i++)
                {
                    col = _bondingColliders[i];
                    if (col == null)
                    {
                        continue;
                    }
                    currentBondable = col.gameObject.GetComponent<IBondable>();

                    if (currentBondable != null)
                    {
                        if (currentBondable.CanBeBonded() == false)
                        {
                            continue;
                        }

                        float currentScore = GetBondableTargetScore(PlayerEntity.Instance, currentBondable);
                        
                        if (newBondable == null)
                        {
                            bestScore = currentScore;
                            newBondable = currentBondable;
                            continue;
                        }

                        // calculate then compare scores
                        // dot poduct aim direction
                        if (currentScore > bestScore)
                        {
                            bestScore = currentScore;
                            newBondable = currentBondable;
                        }
                    }
                }

            }

            if (newBondable != _bondableTarget)
            {
                _bondableTarget = newBondable;

                if (_bondableTarget != null)
                {
                    _bondHighlightPS.gameObject.SetActive(true);
                    _bondHighlightPS.Stop();
                    _bondHighlightPS.transform.parent = _bondableTarget.BondTargetTransform;
                    _bondHighlightPS.transform.localPosition = Vector3.zero;
                    _bondHighlightPS.Play();
                }
            }

            if (_bondableTarget == null)
            {
                if (_bondHighlightPS.isPlaying)
                {
                    _bondHighlightPS.Stop();
                    _bondHighlightPS.transform.parent = gameObject.transform;
                    _bondHighlightPS.transform.localPosition = Vector3.zero;
                    _bondHighlightPS.gameObject.SetActive(false);
                }
            }
        }

        private void FindBestDamageable()
        {
            IDamageable newDamageable = null;

            int aiOverlapCount = Physics2D.OverlapCircle(PlayerEntity.Instance.Transform.localPosition, _damageableOverlapRange, _damageableContactFilter, _damageColliders);

            if (aiOverlapCount > 0)
            {
                float bestScore = -1.0f;
                IDamageable currentDamageable = null;

                Collider2D col = null;

                for (int i = 0; i < aiOverlapCount; i++)
                {
                    col = _damageColliders[i];
                    if (col == null)
                    {
                        continue;
                    }
                    currentDamageable = col.gameObject.GetComponent<IDamageable>();

                    if (currentDamageable != null)
                    {
                        if (currentDamageable.IsAlive() == false)
                        {
                            continue;
                        }

                        // calculate then compare scores
                        float currentScore = GetDamageableTargetScore(PlayerEntity.Instance, currentDamageable);

                        if (newDamageable == null)
                        {
                            bestScore = currentScore;
                            newDamageable = currentDamageable;
                            continue;
                        }

                        // dot poduct aim direction
                        if (currentScore > bestScore)
                        {
                            bestScore = currentScore;
                            newDamageable = currentDamageable;
                        }
                    }
                }

            }

            if (newDamageable != _damageableTarget)
            {
                _damageableTarget = newDamageable;

                if (_damageableTarget != null)
                {
                    _targetHighlightPS.gameObject.SetActive(true);
                    _targetHighlightPS.Stop();
                    _targetHighlightPS.transform.parent = _damageableTarget.Transform;
                    _targetHighlightPS.transform.localPosition = Vector3.zero;
                    _targetHighlightPS.Play();
                }
            }

            if (_damageableTarget == null || !PlayerEntity.Instance.IsPossessed())
            {
                if (_targetHighlightPS.isPlaying)
                {
                    _targetHighlightPS.Stop();
                    _targetHighlightPS.transform.parent = gameObject.transform;
                    _targetHighlightPS.transform.localPosition = Vector3.zero;
                    _targetHighlightPS.gameObject.SetActive(false);
                }
            }
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

        private float GetBondableTargetScore(PlayerEntity pInstigator, IBondable pTarget)
        {
            TargetingParameters tp = GetTargetingParameters(ETargetType.Bondable);
            Vector2 inputDirection = pInstigator.GetMovementInput().normalized;
            bool noPlayerInput = inputDirection.sqrMagnitude == 0.0f;
            if (noPlayerInput)
            {
                inputDirection = pInstigator.FacingRight ? new Vector2(1.0f, 0.0f) : new Vector2(-1.0f, 0.0f);
            }

            float finalScore = 0.0f;
            float distanceScore = 0.0f;
            float angleScore = 0.0f;

            Vector2 targetVector = pTarget.BondTargetTransform.position - pInstigator.Transform.position;

            if (!tp.IgnoreDotProductScore && !noPlayerInput)
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
                    angleScore = 0.0f;
                }
            }

            if (!tp.IgnoreDistanceScore)
            {
                float distanceToTarget = targetVector.SqrMagnitude();
                if (distanceToTarget < tp.MaxSqDistance)
                {
                    distanceScore = Mathf.Lerp(1.0f, 0.1f, (distanceToTarget / tp.MaxSqDistance)) * (noPlayerInput ? 1.0f : tp.DistanceScoreMultiplier);
                }
                else
                {
                    distanceScore = 0.0f;
                }
            }

            finalScore = (distanceScore + angleScore);

            return finalScore;
        }

        private float GetDamageableTargetScore(IDamageable dInstigator, IDamageable dTarget)
        {
            TargetingParameters tp = GetTargetingParameters(ETargetType.Damageable);
            Vector2 inputDirection = dInstigator.GetMovementInput();
            bool noPlayerInput = inputDirection.sqrMagnitude == 0.0f;
            if (noPlayerInput)
            {
                inputDirection = dInstigator.FacingRight ? new Vector2(1.0f, 0.0f) : new Vector2(-1.0f, 0.0f);
            }

            float finalScore = 0.0f;
            float distanceScore = 0.0f;
            float angleScore = 0.0f;

            Vector2 targetVector = dTarget.Transform.position - dInstigator.Transform.position;

            if (!tp.IgnoreDotProductScore && !noPlayerInput)
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
                    return 0.0f;
                }
            }

            if (!tp.IgnoreDistanceScore)
            {
                float distanceToTarget = targetVector.SqrMagnitude();
                if (distanceToTarget < tp.MaxSqDistance)
                {
                    distanceScore = Mathf.Lerp(1.0f, 0.1f, (distanceToTarget / tp.MaxSqDistance)) * (noPlayerInput ? 1.0f : tp.DistanceScoreMultiplier);
                }
                else
                {
                    return 0.0f;
                }
            }

            finalScore = (distanceScore + angleScore);

            return finalScore;
        }

        private TargetingParameters GetTargetingParameters(ETargetType targetType)
        {
            switch (targetType)
            {
                case ETargetType.Bondable:
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
    
    
}
