using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts._Game.AI;
using _Scripts._Game.UI.UIStateMachine;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace _Scripts._Game.General.Managers
{
    public class UIManager : Singleton<UIManager>
    {
        [Header("UI references")]
        #region General
        [SerializeField] 
        private GameObject _pauseBackgroundGO;
        private Dictionary<EPlayerInput, Action<InputAction.CallbackContext>> _inputsDict = new Dictionary<EPlayerInput, Action<InputAction.CallbackContext>>();

        public Dictionary<EPlayerInput, Action<InputAction.CallbackContext>> InputsDict { get => _inputsDict; }
        #endregion

        #region State Machine
        private BaseUIState _currentState;
        private PlayerMovementStateMachineFactory _states;

        public BaseUIState CurrentState { get => _currentState; set => _currentState = value; }
        #endregion

        #region Player Inputs
        private Vector2 _currentMovementInput = Vector2.zero;
        private Vector2 _currentDirectionInput = Vector2.zero;
        private bool _isMovementPressed = false;
        private bool _isDirectionPressed = false;
        private bool _isNorthButtonPressed = false;
        private bool _isSouthButtonPressed = false;
        private bool _isEastButtonPressed = false;
        private bool _isWestButtonPressed = false;
        private bool _isLeftBumperPressed = false;
        private bool _isRightBumperPressed = false;
        private bool _isLeftTriggerPressed = false;
        private bool _isRightTriggerPressed = false;

        private bool _isNorthInputValid = false;
        private bool _isSouthInputValid = false;
        private bool _isEastInputValid = false;
        private bool _isWestInputValid = false;
        private bool _isLeftBumperInputValid = false;
        private bool _isRightBumperInputValid = false;
        private bool _isLeftTriggerInputValid = false;
        private bool _isRightTriggerInputValid = false;

        public bool IsSouthButtonPressed { get => _isSouthButtonPressed; }

        #endregion

        #region Pause menu
        [SerializeField]
        private GameObject _pauseMenuGO;
        [SerializeField]
        private GameObject _pauseMenuFirstButton;
        private Tweener _pauseMenuTweener;
        #endregion

        #region Dialogue 
        [SerializeField]
        private GameObject _dialogueBoxGO;
        #endregion

        public void Awake()
        {
            
        }

        public void Start()
        {
            // assign bond inputs
            PlayerInput playerInput = InputManager.Instance.PlayerInput;

            // assign player inputs
            for (int i = 0; i < (int)EPlayerInput.COUNT - 1; i++)
            {
                EPlayerInput inputType = (EPlayerInput)i;

                switch (inputType)
                {
                    case EPlayerInput.Movement:
                        playerInput.Menu.Movement.started += OnMovementInput;
                        playerInput.Menu.Movement.canceled += OnMovementInput;
                        break;
                    case EPlayerInput.EButton:
                        playerInput.Menu.Back.started += OnEastButtonInput;
                        playerInput.Menu.Back.canceled += OnEastButtonInput;
                        break;
                    case EPlayerInput.SButton:
                        playerInput.Menu.Enter.started += OnSouthButtonInput;
                        playerInput.Menu.Enter.canceled += OnSouthButtonInput;
                        break;
                }
            }
        }

        public void ShowPauseMenu(bool show)
        {
            float targetOpactity = show ? 1.0f : 0.0f;

            _pauseMenuGO.SetActive(show);
            _pauseBackgroundGO.SetActive(show);

            EventSystem.current.SetSelectedGameObject(show ? _pauseMenuFirstButton : null);
        }

        // Player Input 
        public void OnMovementInput(InputAction.CallbackContext context)
        {
            _currentMovementInput = context.ReadValue<Vector2>();
            _isMovementPressed = _currentMovementInput.sqrMagnitude != 0.0f;
        }
        public void OnDirectionInput(InputAction.CallbackContext context)
        {
            _currentDirectionInput = context.ReadValue<Vector2>();
            _isDirectionPressed = _currentDirectionInput.sqrMagnitude != 0.0f;
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
    }
    
}
