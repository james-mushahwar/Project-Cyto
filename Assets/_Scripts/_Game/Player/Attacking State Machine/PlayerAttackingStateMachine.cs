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
        private bool _isAttackPressed = false;

        public bool IsAttackPressed { get => _isAttackPressed; }

        private bool _isAttackInputValid = false;

        public bool IsAttackInputValid { get => _isAttackInputValid; }
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
        private float[] _basicComboBufferTimes = new float[5]; // how long can the buffer be open for the next combo
        private int _currentBasicAttackCombo = 0;

        public int BasicComboLimit { get => _basicComboLimit; }
        public float[] BasicComboWaitTimes { get => _basicComboWaitTimes; }
        public float[] BasicComboBufferTimes { get => _basicComboBufferTimes; }
        public int CurrentBasicAttackCombo { get => _currentBasicAttackCombo; set => _currentBasicAttackCombo = value; }
        #endregion

        protected override void Awake()
        {
            base.Awake();

            PlayerInput playerInput = InputManager.Instance.PlayerInput;
            //setup player input callbacks
            playerInput.Player.Attack.started += OnAttackInput;
            playerInput.Player.Attack.canceled += OnAttackInput;

            _states = new PlayerAttackingStateMachineFactory(this);
            _basicAttackCurrentState = _states.GetState(AttackingState.Basic_Idle);
            //_abilityAttackCurrentState = _states.GetState(AttackingState.Ability_Idle);

            PlayerEntity.Instance.AttackingSM = this;
        }

        void FixedUpdate()
        {
            _basicAttackCurrentState.ManagedStateTick();
            //_abilityAttackCurrentState.ManagedStateTick();
        }

        void OnAttackInput(InputAction.CallbackContext context)
        {
            _isAttackPressed = context.ReadValueAsButton();
            _isAttackInputValid = _isAttackPressed;
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
        public void LoadState(object state)
        {
            throw new System.NotImplementedException();
        }

        public object SaveState()
        {
            throw new System.NotImplementedException();
        }
    }
    
}
