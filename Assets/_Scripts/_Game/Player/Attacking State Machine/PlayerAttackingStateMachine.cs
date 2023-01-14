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
        private BaseAttackingState _currentState; // states that determine player movement
        private BaseAttackingState _basicAttackCurrentState; // just for X attack - doesn't affect movement

        public BaseAttackingState CurrentState { get => _currentState; set => _currentState = value; }
        public BaseAttackingState BasicAttackCurrentState { get => _basicAttackCurrentState; set => _basicAttackCurrentState = value; }

        private PlayerAttackingStateMachineFactory _states;
        #endregion

        #region Basic Attack Properties
        [SerializeField]
        private int _basicComboLimit;
        [SerializeField]
        private int[] _basicComboWaitTimes = new int[5]; // wait for next combo to be ready
        private int[] _basicComboBufferTimes = new int[5]; // how long can the buffer be open for the next combo
        private int _currentBasicAttackCombo = 0;

        public int BasicComboLimit { get => _basicComboLimit; }
        public int[] BasicComboWaitTimes { get => _basicComboWaitTimes; }
        public int[] BasicComboBufferTimes { get => _basicComboBufferTimes; }
        public int CurrentBasicAttackCombo { get => _currentBasicAttackCombo; set => _currentBasicAttackCombo = value; }
        #endregion

        protected override void Awake()
        {
            base.Awake();

            PlayerInput playerInput = InputManager.Instance.PlayerInput;
            //setup player input callbacks
            playerInput.Player.Attack.started += OnAttackInput;
            playerInput.Player.Attack.canceled += OnAttackInput;

            PlayerEntity.Instance.AttackingSM = this;
        }

        void FixedUpdate()
        {
            _basicAttackCurrentState.ManagedStateTick();
            _currentState.ManagedStateTick();
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
