using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using _Scripts._Game.General;
using _Scripts._Game.General.SaveLoad;
using _Scripts._Game.General.Managers;
using _Scripts._Game.AI.Bonding;
using UnityEngine.Rendering;

namespace _Scripts._Game.Player.AttackingStateMachine{
    
    public class PlayerAttackingStateMachine : Singleton<PlayerAttackingStateMachine>, ISaveable
    {
        #region Input
        //attack
        private bool _isAttackPressed = false;
        private bool _isAttackInputValid = false;
        //aim L stick
        private Vector2 _currentAimInput = Vector2.zero;

        public bool IsAttackPressed { get => _isAttackPressed; }
        public bool IsAttackInputValid { get => _isAttackInputValid; }
        public Vector2 CurrentAimInput { get => _currentAimInput; }
        #endregion

        #region State Machine
        private BaseAttackingState _abilityAttackCurrentState; // states that determine player movement
        private BaseAttackingState _basicAttackCurrentState; // just for X attack - doesn't affect movement

        public BaseAttackingState AbilityAttackCurrentState { get => _abilityAttackCurrentState; set => _abilityAttackCurrentState = value; }
        public BaseAttackingState BasicAttackCurrentState { get => _basicAttackCurrentState; set => _basicAttackCurrentState = value; }

        private PlayerAttackingStateMachineFactory _states;
        #endregion

        #region Basic Attack Properties
        [SerializeField]
        private int _basicComboLimit;
        [SerializeField]
        private float[] _basicComboWaitTimes = new float[5]; // wait for next combo to be ready
        [SerializeField]
        private float[] _basicComboElapseTimes = new float[5]; // wait to return to idle state
        [SerializeField]
        private float[] _basicComboBufferTimes = new float[5]; // how long can the buffer be open for the next combo
        private int _currentBasicAttackCombo = 0;

        public int BasicComboLimit { get => _basicComboLimit; }
        public float[] BasicComboWaitTimes { get => _basicComboWaitTimes; }
        public float[] BasicComboElapseTimes { get => _basicComboElapseTimes; }
        public float[] BasicComboBufferTimes { get => _basicComboBufferTimes; }
        public int CurrentBasicAttackCombo { get => _currentBasicAttackCombo; set => _currentBasicAttackCombo = value; }

        [Header("Damageable Range properties")]
        [SerializeField]
        private float _damageableOverlapRange;
        [SerializeField]
        private ContactFilter2D _damageableContactFilter;

        private Collider2D[] _aiColliders = new Collider2D[20];
        private IDamageable _damageableTarget;

        public IDamageable DamageableTarget { get => _damageableTarget; }
        #endregion

        #region VFX
        [Header("VFX")]
        [SerializeField]
        private ParticleSystem _targetHighlightPS;
        #endregion

        protected override void Awake()
        {
            base.Awake();

            PlayerInput playerInput = InputManager.Instance.PlayerInput;
            //setup player input callbacks
            playerInput.Player.Attack.started += OnAttackInput;
            playerInput.Player.Attack.canceled += OnAttackInput;

            playerInput.Player.Movement.started += OnAimInput;
            playerInput.Player.Movement.performed += OnAimInput;
            playerInput.Player.Movement.canceled += OnAimInput;

            _states = new PlayerAttackingStateMachineFactory(this);
            _basicAttackCurrentState = _states.GetState(AttackingState.Basic_Idle);
            //_abilityAttackCurrentState = _states.GetState(AttackingState.Ability_Idle);

            PlayerEntity.Instance.AttackingSM = this;
        }

        void FixedUpdate()
        {
            IDamageable newTarget = FindBestDamageable();
            bool differentTarget = false;
            if (newTarget != _damageableTarget)
            {
                _damageableTarget = newTarget;
                differentTarget = true;

                if (_damageableTarget != null)
                {
                    _targetHighlightPS.gameObject.SetActive(true);
                    _targetHighlightPS.Stop();
                    _targetHighlightPS.transform.parent = _damageableTarget.Transform;
                    _targetHighlightPS.transform.localPosition = Vector3.zero;
                    _targetHighlightPS.Play();
                }
            }

            if (_damageableTarget == null)
            {
                if (_targetHighlightPS.isPlaying)
                {
                    _targetHighlightPS.Stop();
                    _targetHighlightPS.transform.parent = gameObject.transform;
                    _targetHighlightPS.transform.localPosition = Vector3.zero;
                    _targetHighlightPS.gameObject.SetActive(false);
                }
            }

            _basicAttackCurrentState.ManagedStateTick();
            //_abilityAttackCurrentState.ManagedStateTick();
        }

        IDamageable FindBestDamageable()
        {
            int aiOverlapCount = Physics2D.OverlapCircle(transform.position, _damageableOverlapRange, _damageableContactFilter, _aiColliders);

            if (aiOverlapCount > 0)
            {
                float bestScore = -1.0f;
                IDamageable bestDamageable = null;
                IDamageable currentDamageable = null;

                Collider2D col = null;

                for (int i = 0; i < aiOverlapCount; i++)
                {
                    col = _aiColliders[i];
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

                        if (bestDamageable == null)
                        {
                            bestDamageable = currentDamageable;
                            continue;
                        }

                        // calculate then compare scores
                        float currentScore = TargetManager.Instance.GetDamageableTargetScore(PlayerEntity.Instance, currentDamageable);
                        // dot poduct aim direction
                        if (currentScore > bestScore)
                        {
                            bestScore = currentScore;
                            bestDamageable = currentDamageable;
                        }
                    }
                }

                return bestDamageable;
            }
            else
            {
                return null;
            }
        }

        void OnAttackInput(InputAction.CallbackContext context)
        {
            _isAttackPressed = context.ReadValueAsButton();
            _isAttackInputValid = _isAttackPressed;
        }

        void OnAimInput(InputAction.CallbackContext context)
        {
            Vector2 movementInput = context.ReadValue<Vector2>().normalized;
            _currentAimInput = PlayerEntity.Instance.IsAlive() && movementInput.sqrMagnitude >= 0.4 ? movementInput : Vector2.zero;
        }

        public void NullifyInput(AttackingState state)
        {
            switch (state)
            {
                case AttackingState.Basic_Attack:
                    _isAttackInputValid = false;
                    break;
                default:
                    break;
            }
        }

        //ISaveable

        [System.Serializable]
        private struct SaveData
        {
            
        }

        public void LoadState(object state)
        {
            
        }

        public object SaveState()
        {
            return new SaveData();
        }
    }
    
}
