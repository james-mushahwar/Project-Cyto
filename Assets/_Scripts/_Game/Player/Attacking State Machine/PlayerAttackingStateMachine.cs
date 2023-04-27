using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using _Scripts._Game.General;
using _Scripts._Game.General.SaveLoad;
using _Scripts._Game.General.Managers;
using _Scripts._Game.AI.Bonding;
using UnityEngine.Rendering;
using _Scripts._Game.Events;

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
        [SerializeField]
        private float _basicAttackRecentTimer;

        private int _currentBasicAttackCombo = 0;
        private bool _basicAttackBuffered = false;

        public int BasicComboLimit { get => _basicComboLimit; }
        public float[] BasicComboWaitTimes { get => _basicComboWaitTimes; }
        public float[] BasicComboElapseTimes { get => _basicComboElapseTimes; }
        public float[] BasicComboBufferTimes { get => _basicComboBufferTimes; }
        public float BasicAttackRecentTimer { get => _basicAttackRecentTimer; }
        public int CurrentBasicAttackCombo { get => _currentBasicAttackCombo; set => _currentBasicAttackCombo = value; }
        public bool BasicAttackBuffered { get => _basicAttackBuffered; set => _basicAttackBuffered = value; }


        [Header("Damageable Range properties")]
        [SerializeField]
        private float _damageableOverlapRange;
        [SerializeField]
        private ContactFilter2D _damageableContactFilter;

        private Collider2D[] _aiColliders = new Collider2D[20];
        #endregion

        #region Combo mode
        [Header("Combo mode")]
        private bool _comboModeActive = false;
        private float _comboModeTimer;
        [SerializeField]
        private float _comboModeDuration;
        [SerializeField]
        private GameEvent _comboModeStartedGameEvent;
        [SerializeField]
        private GameEvent _comboModeEndedGameEvent;


        public float ComboModeTimer
        {
            get => _comboModeTimer;
        }
        #endregion

        #region General
        private float _recentAttackTimer = 0.0f;

        public float RecentAttackTimer
        {
            get => _recentAttackTimer;
            set
            {
                if (value > _recentAttackTimer)
                {
                    _recentAttackTimer = value;
                }
            }
        }
        #endregion

        //#region VFX
        //[Header("VFX")]
        //[SerializeField]
        //private ParticleSystem _targetHighlightPS;
        //#endregion

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

        void Update()
        {
            // timers
            TickTimers();

            if (PlayerEntity.Instance.IsAlive())
            {
                _basicAttackCurrentState.ManagedStateTick();
            }
        }

        private void TickTimers()
        {
            float deltaTime = Time.deltaTime;
            // recent attack timer
            if (_recentAttackTimer > 0.0f)
            {
                _recentAttackTimer = Mathf.Clamp(_recentAttackTimer - deltaTime, 0.0f, 100.0f);
            }

            //combo mode timers
            if (_comboModeTimer > 0.0f)
            {
                _comboModeTimer = Mathf.Clamp(_comboModeTimer - deltaTime, 0.0f, 100.0f);
                if (_comboModeTimer <= 0.0f)
                {
                    _comboModeEndedGameEvent.TriggerEvent();
                }
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

        public bool HasAttackedRecently()
        {
            return _recentAttackTimer > 0.0f || _basicAttackBuffered;
        }

        public void RestartComboMode()
        {
            _comboModeTimer = _comboModeDuration;
            _comboModeStartedGameEvent.TriggerEvent();
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
