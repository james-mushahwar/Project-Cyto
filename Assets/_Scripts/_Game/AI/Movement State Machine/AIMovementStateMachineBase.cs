﻿using System;
using System.Collections.Generic;
using _Scripts._Game.General.SaveLoad;
using _Scripts._Game.AI.Bonding;
using UnityEngine;
using UnityEngine.InputSystem;
using _Scripts._Game.General.Managers;
using _Scripts._Game.Player;

namespace _Scripts._Game.AI.MovementStateMachine{
    
    public class AIMovementStateMachineBase : MonoBehaviour, ISaveable, IBondable
    {
        #region State Machine
        //Movement
        private BaseAIMovementState _currentState;
        private BaseAIBondedMovementState _currentBondedState;

        public BaseAIMovementState CurrentState { get => _currentState; set => _currentState = value; }
        public BaseAIBondedMovementState CurrentBondedState { get => _currentBondedState; set => _currentBondedState = value; }
        
        //Attacking

        protected AIMovementStateMachineFactory _states;
        #endregion

        #region AI Components
        private Rigidbody2D _rb;
        private CapsuleCollider2D _capsule;
        [SerializeField]
        private LayerMask _groundedLayer;

        public Rigidbody2D Rb { get => _rb; }
        public LayerMask GroundedLayer { get => _groundedLayer; }
        #endregion

        #region Bond Inputs
        private static Vector2 _currentMovementInput = Vector2.zero;
        private static Vector2 _currentDirectionInput = Vector2.zero;
        private static bool _isMovementPressed = false;
        private static bool _isDirectionPressed = false;
        private static bool _isNorthButtonPressed = false;
        private static bool _isSouthButtonPressed = false;
        private static bool _isEastButtonPressed = false;
        private static bool _isWestButtonPressed = false;
        private static bool _isLeftBumperPressed = false;
        private static bool _isRightBumperPressed = false;
        private static bool _isLeftTriggerPressed = false;
        private static bool _isRightTriggerPressed = false;

        public Vector2 CurrentMovementInput { get => _currentMovementInput; }
        public Vector2 CurrentDirectionInput { get => _currentDirectionInput; }
        public bool IsMovementPressed { get => _isMovementPressed; }
        public bool IsDirectionPressed { get => _isDirectionPressed; }
        public bool IsNorthButtonPressed { get => _isNorthButtonPressed; }
        public bool IsSouthButtonPressed { get => _isSouthButtonPressed; }
        public bool IsEastButtonPressed { get => _isEastButtonPressed; }
        public bool IsWestButtonPressed { get => _isWestButtonPressed; }
        public bool IsLeftBumperPressed { get => _isLeftBumperPressed; }
        public bool IsRightBumperPressed { get => _isRightBumperPressed; }
        public bool IsLeftTriggerPressed { get => _isLeftTriggerPressed; }
        public bool IsRightTriggerPressed { get => _isRightTriggerPressed; }

        private static bool _isNorthInputValid = false;
        private static bool _isSouthInputValid = false;
        private static bool _isEastInputValid = false;
        private static bool _isWestInputValid = false;
        private static bool _isLeftBumperInputValid = false;
        private static bool _isRightBumperInputValid = false;
        private static bool _isLeftTriggerInputValid = false;
        private static bool _isRightTriggerInputValid = false;

        private Dictionary<BondInput, Action<InputAction.CallbackContext>> _bondInputsDict = new Dictionary<BondInput, Action<InputAction.CallbackContext>> ();
        public Dictionary<BondInput, Action<InputAction.CallbackContext>> BondInputsDict { get => _bondInputsDict; }
        #endregion

        #region AI Entity
        private AIEntity _entity;

        public AIEntity Entity { get => _entity; }
        #endregion

        protected virtual void Awake()
        {
            _states = new AIMovementStateMachineFactory(this);

            _entity = GetComponent<AIEntity>();
            if (_entity)
            {
                _entity.MovementSM = this;
            }
        }

        // Start is called before the first frame update
        protected virtual void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _capsule = GetComponent<CapsuleCollider2D>();
        }
   

        // ISaveable
        [System.Serializable]
        private struct SaveData
        {

        }

        public virtual object SaveState()
        {
            return new SaveData();
        }

        public virtual void LoadState(object state)
        {
            SaveData saveData = (SaveData)state;
        }

        // IBondable
        public void OnMovementInput(InputAction.CallbackContext context)
        {
            _currentMovementInput = context.ReadValue<Vector2>();
            _isMovementPressed = _currentMovementInput.magnitude != 0.0f;
        }
        public void OnDirectionInput(InputAction.CallbackContext context)
        {
            _currentDirectionInput = context.ReadValue<Vector2>();
            _isDirectionPressed = _currentDirectionInput.magnitude != 0.0f;
        }
        public void OnNorthButtonInput(InputAction.CallbackContext context)
        {
            _isNorthButtonPressed = context.ReadValueAsButton();
            _isNorthInputValid = _isNorthButtonPressed;
        }
        public void OnSouthButtonInput(InputAction.CallbackContext context)
        {
            _isSouthButtonPressed = context.ReadValueAsButton();
            _isSouthInputValid = _isSouthButtonPressed;
        }
        public void OnEastButtonInput(InputAction.CallbackContext context)
        {
            _isEastButtonPressed = context.ReadValueAsButton();
            _isEastInputValid = _isEastButtonPressed;
        }
        public void OnWestButtonInput(InputAction.CallbackContext context)
        {
            _isWestButtonPressed = context.ReadValueAsButton();
            _isWestInputValid = _isWestButtonPressed;
        }
        public void OnLeftBumperInput(InputAction.CallbackContext context)
        {
            _isLeftBumperPressed = context.ReadValueAsButton();
            _isLeftBumperInputValid = _isLeftBumperPressed;
        }
        public void OnRightBumperInput(InputAction.CallbackContext context)
        {
            _isRightBumperPressed = context.ReadValueAsButton();
            _isRightBumperInputValid = _isRightBumperPressed;
        }
        public void OnLeftTriggerInput(InputAction.CallbackContext context)
        {
            _isLeftTriggerPressed = context.ReadValueAsButton();
            _isLeftTriggerInputValid = _isLeftTriggerPressed;
        }
        public void OnRightTriggerInput(InputAction.CallbackContext context)
        {
            _isRightTriggerPressed = context.ReadValueAsButton();
            _isRightTriggerInputValid = _isRightTriggerPressed;
        }

        public virtual void OnBonded()
        {
            _currentMovementInput = Vector2.zero;
            _currentDirectionInput = Vector2.zero;
            _isMovementPressed = false;
            _isDirectionPressed = false;
            _isNorthButtonPressed = false;
            _isSouthButtonPressed = false;
            _isEastButtonPressed = false;
            _isWestButtonPressed = false;
            _isLeftBumperPressed = false;
            _isRightBumperPressed = false;
            _isLeftTriggerPressed = false;
            _isRightTriggerPressed = false;

            _isNorthInputValid = false;
            _isSouthInputValid = false;
            _isEastInputValid = false;
            _isWestInputValid = false;
            _isLeftBumperInputValid = false;
            _isRightBumperInputValid = false;
            _isLeftTriggerInputValid = false;
            _isRightTriggerInputValid = false;

            // assign bond inputs
            PlayerInput playerInput = InputManager.Instance.PlayerInput;

            int bondedInputsCount = BondInputsDict.Count;
            int foundBondedInputs = 0;

            for (int i = 0; i < (int)BondInput.COUNT - 1 && foundBondedInputs < bondedInputsCount; i++)
            {
                BondInput bondInput = (BondInput)i;

                BondInputsDict.TryGetValue(bondInput, out Action<InputAction.CallbackContext> bondedAction);

                if (bondedAction != null)
                {
                    foundBondedInputs++;
                    switch (bondInput)
                    {
                        case BondInput.Movement:
                            playerInput.BondedPlayer.Movement.started += bondedAction;
                            playerInput.BondedPlayer.Movement.canceled += bondedAction;
                            break;

                        case BondInput.Direction:
                            playerInput.BondedPlayer.Direction.started += bondedAction;
                            playerInput.BondedPlayer.Direction.canceled += bondedAction;
                            break;
                        case BondInput.NButton:
                            playerInput.BondedPlayer.NorthButton.started += bondedAction;
                            playerInput.BondedPlayer.NorthButton.canceled += bondedAction;
                            break;
                        case BondInput.SButton:
                            playerInput.BondedPlayer.SouthButton.started += bondedAction;
                            playerInput.BondedPlayer.SouthButton.canceled += bondedAction;
                            break;
                        case BondInput.EButton:
                            playerInput.BondedPlayer.EastButton.started += bondedAction;
                            playerInput.BondedPlayer.EastButton.canceled += bondedAction;
                            break;
                        case BondInput.WButton:
                            playerInput.BondedPlayer.WestButton.started += bondedAction;
                            playerInput.BondedPlayer.WestButton.canceled += bondedAction;
                            break;
                        case BondInput.LBumper:
                            playerInput.BondedPlayer.LeftBumper.started += bondedAction;
                            playerInput.BondedPlayer.LeftBumper.canceled += bondedAction;
                            break;
                        case BondInput.RBumper:
                            playerInput.BondedPlayer.RightBumper.started += bondedAction;
                            playerInput.BondedPlayer.RightBumper.canceled += bondedAction;
                            break;
                        case BondInput.LTrigger:
                            playerInput.BondedPlayer.LeftTrigger.started += bondedAction;
                            playerInput.BondedPlayer.LeftTrigger.canceled += bondedAction;
                            playerInput.BondedPlayer.LeftTrigger.performed += bondedAction;
                            break;
                        case BondInput.RTrigger:
                            playerInput.BondedPlayer.RightTrigger.started += bondedAction;
                            playerInput.BondedPlayer.RightTrigger.canceled += bondedAction;
                            playerInput.BondedPlayer.RightTrigger.performed += bondedAction;
                            break;
                        default:
                            break;
                    }
                }
            }

            InputManager.Instance.TryEnableActionMap(EInputSystem.BondedPlayer);
        }

        public virtual void OnUnbonded()
        {
            // assign bond inputs
            PlayerInput playerInput = InputManager.Instance.PlayerInput;

            int bondedInputsCount = BondInputsDict.Count;
            int foundBondedInputs = 0;

            for (int i = 0; i < (int)BondInput.COUNT - 1 && foundBondedInputs < bondedInputsCount; i++)
            {
                BondInput bondInput = (BondInput)i;

                BondInputsDict.TryGetValue(bondInput, out Action<InputAction.CallbackContext> bondedAction);

                if (bondedAction != null)
                {
                    foundBondedInputs++;
                    switch (bondInput)
                    {
                        case BondInput.Movement:
                            playerInput.BondedPlayer.Movement.started -= bondedAction;
                            playerInput.BondedPlayer.Movement.canceled -= bondedAction;
                            break;

                        case BondInput.Direction:
                            playerInput.BondedPlayer.Direction.started -= bondedAction;
                            playerInput.BondedPlayer.Direction.canceled -= bondedAction;
                            break;
                        case BondInput.NButton:
                            playerInput.BondedPlayer.NorthButton.started -= bondedAction;
                            playerInput.BondedPlayer.NorthButton.canceled -= bondedAction;
                            break;
                        case BondInput.SButton:
                            playerInput.BondedPlayer.SouthButton.started -= bondedAction;
                            playerInput.BondedPlayer.SouthButton.canceled -= bondedAction;
                            break;
                        case BondInput.EButton:
                            playerInput.BondedPlayer.EastButton.started -= bondedAction;
                            playerInput.BondedPlayer.EastButton.canceled -= bondedAction;
                            break;
                        case BondInput.WButton:
                            playerInput.BondedPlayer.WestButton.started -= bondedAction;
                            playerInput.BondedPlayer.WestButton.canceled -= bondedAction;
                            break;
                        case BondInput.LBumper:
                            playerInput.BondedPlayer.LeftBumper.started -= bondedAction;
                            playerInput.BondedPlayer.LeftBumper.canceled -= bondedAction;
                            break;
                        case BondInput.RBumper:
                            playerInput.BondedPlayer.RightBumper.started -= bondedAction;
                            playerInput.BondedPlayer.RightBumper.canceled -= bondedAction;
                            break;
                        case BondInput.LTrigger:
                            playerInput.BondedPlayer.LeftTrigger.started -= bondedAction;
                            playerInput.BondedPlayer.LeftTrigger.canceled -= bondedAction;
                            playerInput.BondedPlayer.LeftTrigger.performed -= bondedAction;
                            break;
                        case BondInput.RTrigger:
                            playerInput.BondedPlayer.RightTrigger.started -= bondedAction;
                            playerInput.BondedPlayer.RightTrigger.canceled -= bondedAction;
                            playerInput.BondedPlayer.RightTrigger.performed -= bondedAction;
                            break;
                        default:
                            break;
                    }
                }
            }

            InputManager.Instance.TryDisableActionMap(EInputSystem.BondedPlayer);

            _currentMovementInput = Vector2.zero;
            _currentDirectionInput = Vector2.zero;
            _isMovementPressed = false;
            _isDirectionPressed = false;
            _isNorthButtonPressed = false;
            _isSouthButtonPressed = false;
            _isEastButtonPressed = false;
            _isWestButtonPressed = false;
            _isLeftBumperPressed = false;
            _isRightBumperPressed = false;
            _isLeftTriggerPressed = false;
            _isRightTriggerPressed = false;

            _isNorthInputValid = false;
            _isSouthInputValid = false;
            _isEastInputValid = false;
            _isWestInputValid = false;
            _isLeftBumperInputValid = false;
            _isRightBumperInputValid = false;
            _isLeftTriggerInputValid = false;
            _isRightTriggerInputValid = false;
        }
    }
}
