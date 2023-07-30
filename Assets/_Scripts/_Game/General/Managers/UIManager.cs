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
        [Header("General")]
        #region General
        [SerializeField] 
        private GameObject _pauseBackgroundGO;
        private Dictionary<EPlayerInput, Action<InputAction.CallbackContext>> _inputsDict = new Dictionary<EPlayerInput, Action<InputAction.CallbackContext>>();

        private Dictionary<UIInputState, GameObject> _menuGameObjectDict = new Dictionary<UIInputState, GameObject>();

        public Dictionary<EPlayerInput, Action<InputAction.CallbackContext>> InputsDict { get => _inputsDict; }
        #endregion

        #region State Machine
        private BaseUIState _currentState;

        public BaseUIState CurrentState { get => _currentState; set => _currentState = value; }
        #endregion

        [Header("World")]
        #region World
        [SerializeField] 
        private Canvas _promptCanvas;
        [SerializeField] 
        private PlayerInputPromptDictionary _playerInputPromptDict = new PlayerInputPromptDictionary();
        private Dictionary<EPlayerInput, Transform> _promptWorldTransformDict = new Dictionary<EPlayerInput, Transform>();
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

        #region Main menu
        [SerializeField]
        private GameObject _mainMenuGO;
        [SerializeField]
        private GameObject _mainMenuFirstButton;
        #endregion

        #region Save files
        [SerializeField]
        private GameObject _saveFilesGO;
        [SerializeField]
        private GameObject _saveFilesFirstButton;
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
            //prompts
            foreach (EPlayerInput inputType in _playerInputPromptDict.Keys)
            {
                _promptWorldTransformDict.Add(inputType, transform);
            }

            // Gameobject dict
            _menuGameObjectDict.Add(UIInputState.MainMenu, _mainMenuGO);
            _menuGameObjectDict.Add(UIInputState.PauseMenu, _pauseMenuGO);

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

        private void Update()
        {
            foreach (EPlayerInput inputType in _playerInputPromptDict.Keys)
            {
                _playerInputPromptDict.TryGetValue(inputType, out RectTransform promptTransform);
                _promptWorldTransformDict.TryGetValue(inputType, out Transform targetTransform);

                if (promptTransform == null || targetTransform == null || targetTransform == transform)
                {
                    continue;
                }

                if (promptTransform.gameObject.activeSelf == false)
                {
                    continue;
                }

                //first you need the RectTransform component of your canvas
                RectTransform CanvasRect = _promptCanvas.GetComponent<RectTransform>();

                Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(targetTransform.position);
                Vector2 WorldObject_ScreenPosition = new Vector2(
                    ((ViewportPosition.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f)),
                    ((ViewportPosition.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)));

                //now you can set the position of the ui element
                promptTransform.anchoredPosition = WorldObject_ScreenPosition;
            }
        }

        public bool IsAnyMenuActive()
        {
            bool anyActive = false;

            foreach (GameObject menuGO in _menuGameObjectDict.Values)
            {
                if (menuGO.activeSelf)
                {
                    anyActive = true;
                    break;
                }
            }

            return anyActive;
        }

        // main menu screens
        public void ShowMainMenu(bool show)
        {
            float targetOpactity = show ? 1.0f : 0.0f;

            _mainMenuGO.SetActive(show);

            EventSystem.current.SetSelectedGameObject(show ? _mainMenuFirstButton : null);
        }
        public void ShowSaveFilesPage(bool show)
        {
            _saveFilesGO.SetActive(show);

            EventSystem.current.SetSelectedGameObject(show ? _saveFilesFirstButton : null);
        }

        // pause menu screens
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

        public void NullifyInput(EPlayerInput input)
        {
            if (input == EPlayerInput.SButton)
            {
                _isSouthButtonPressed = false;
            }
        }

        // prompts
        public void TogglePlayerInputPrompt(EPlayerInput inputType, bool show, Transform attachTransform)
        {
            _playerInputPromptDict.TryGetValue(inputType, out RectTransform promptGO);
            _promptWorldTransformDict.TryGetValue(inputType, out Transform targetTransform);
            if (promptGO)
            {
                if ((!show && targetTransform != attachTransform) ||
                    (show && targetTransform != transform))
                {
                    return;
                }

                if (promptGO.gameObject.activeSelf != show)
                {
                    promptGO.gameObject.SetActive(show);
                }

                _promptWorldTransformDict[inputType] = show ? attachTransform : this.transform;
            }
        }
    }
    
}
