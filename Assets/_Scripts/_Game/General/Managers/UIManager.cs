using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts._Game.AI;
using _Scripts._Game.General.SaveLoad;
using _Scripts._Game.UI.MainMenu;
using _Scripts._Game.UI.UIStateMachine;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace _Scripts._Game.General.Managers
{
    public class UIManager : Singleton<UIManager>, IManager
    {
        [Header("General")]
        #region General
        [SerializeField] 
        private GameObject _pauseBackgroundGO;
        private Dictionary<EPlayerInput, Action<InputAction.CallbackContext>> _inputsDict = new Dictionary<EPlayerInput, Action<InputAction.CallbackContext>>();

        private Dictionary<UIInputState, GameObject> _menuGameObjectDict = new Dictionary<UIInputState, GameObject>();

        private Stack<UIInputState> _currentInputStateStack = new Stack<UIInputState>();
        private UIInputState CurrentInputState { get => _currentInputStateStack.Peek(); }

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
        [SerializeField]
        private SaveSlotsMenu _saveSlotsMenu;
        #endregion

        #region Pause menu
        [SerializeField]
        private GameObject _optionsMenuGO;
        [SerializeField]
        private GameObject _optionsMenuFirstButton;
        private Tweener _optionsMenuTweener;
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

        #region Loading Screen
        [SerializeField] 
        private GameObject _loadingScreenGO;
        #endregion

        public void Start()
        {
            //prompts
            foreach (EPlayerInput inputType in _playerInputPromptDict.Keys)
            {
                _promptWorldTransformDict.Add(inputType, transform);
            }

            _currentInputStateStack.Push(UIInputState.None);

            // Gameobject dict
            _menuGameObjectDict.Add(UIInputState.MainMenu, _mainMenuGO);
            _menuGameObjectDict.Add(UIInputState.SaveFiles, _saveFilesGO);
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
                    case EPlayerInput.WButton:
                        playerInput.Menu.Action.started += OnWestButtonInput;
                        playerInput.Menu.Action.canceled += OnWestButtonInput;
                        break;
                }
            }
        }

        public void ManagedTick()
        {
            //current input state
            if (CurrentInputState == UIInputState.SaveFiles)
            {
                GameObject selectedGameobject = EventSystem.current.currentSelectedGameObject;

                int slotIndex = _saveSlotsMenu.GetSelectedSlotIndex(selectedGameobject);
                if (slotIndex != -1)
                {
                    if (_isWestButtonPressed)
                    {
                        // delete save index
                        SaveLoadSystem.Instance.Delete(slotIndex);
                        _saveSlotsMenu.RefreshSaveSlotsView();
                        NullifyInput(EPlayerInput.WButton);
                    }
                }
            }


            //input prompts
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
                RectTransform canvasRect = _promptCanvas.GetComponent<RectTransform>();

                Vector2 viewportPosition = Camera.main.WorldToViewportPoint(targetTransform.position);
                Vector2 worldObject_ScreenPosition = new Vector2(
                    ((viewportPosition.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f)),
                    ((viewportPosition.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f)));

                //now you can set the position of the ui element
                promptTransform.anchoredPosition = worldObject_ScreenPosition;
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

        private void UpdateInputStack(bool add, UIInputState state)
        {
            if (add)
            {
                _currentInputStateStack.Push(state);
            }
            else
            {
                UIInputState lastState;
                _currentInputStateStack.TryPeek(out lastState);
                if (lastState != UIInputState.None)
                {
                    _currentInputStateStack.Pop();
                }
            }
        }

        // main menu screens
        public void ShowMainMenu(bool show)
        {
            if (_mainMenuGO.activeSelf == show)
            {
                return;
            }
            float targetOpactity = show ? 1.0f : 0.0f;

            _mainMenuGO.SetActive(show);

            UpdateInputStack(show, UIInputState.MainMenu);

            EventSystem.current.SetSelectedGameObject(show ? _mainMenuFirstButton : null);
        }
        public void ShowSaveFilesPage(bool show)
        {
            if (_saveFilesGO.activeSelf == show)
            {
                return;
            }
            _saveFilesGO.SetActive(show);

            UpdateInputStack(show, UIInputState.SaveFiles);

            EventSystem.current.SetSelectedGameObject(show ? _saveFilesFirstButton : null);
        }
        public void ShowOptions(bool show)
        {
            if (_optionsMenuGO.activeSelf == show)
            {
                return;
            }
            _optionsMenuGO.SetActive(show);

            UpdateInputStack(show, UIInputState.Options);

            EventSystem.current.SetSelectedGameObject(show ? _optionsMenuFirstButton : null);
        }
        // pause menu screens
        public void ShowPauseMenu(bool show)
        {
            if (_pauseMenuGO.activeSelf == show)
            {
                return;
            }
            float targetOpactity = show ? 1.0f : 0.0f;

            _pauseMenuGO.SetActive(show);
            _pauseBackgroundGO.SetActive(show);

            UpdateInputStack(show, UIInputState.PauseMenu);

            EventSystem.current.SetSelectedGameObject(show ? _pauseMenuFirstButton : null);
        }

        public void ShowLoadingScreen(bool show)
        {
            if (_loadingScreenGO.activeSelf == show)
            {
                return;
            }

            float targetOpactity = show ? 1.0f : 0.0f;

            _loadingScreenGO.SetActive(show);

            UpdateInputStack(show, UIInputState.LoadingScreen);
        }

        //UIButtons clicked
        //MainMenu
        public void StartGameButtonClicked(bool newGame = false)
        {
            GameStateManager.Instance.StartGame(newGame);
        }
        public void LoadSaveSlotButtonClicked(int index = 0)
        {
            GameStateManager.Instance.LoadGame(index);
        }
        public void DeleteAllSavesButtonClicked()
        {
            SaveLoadSystem.Instance.DeleteAllSaves();
            SaveLoadSystem.Instance.DeleteAllGamePrefs();
        }

        //Pause Menu
        public void OnSelect_Resume()
        {
            PauseManager.Instance.TogglePause();
        }
        public void OnSelect_Quit()
        {
            GameStateManager.Instance.QuitToMainMenu();
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
            Debug.Log("West input is " + _isWestButtonPressed);
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
                _isSouthInputValid = false;
            }
            else if (input == EPlayerInput.WButton)
            {
                _isWestButtonPressed = false;
                _isWestInputValid = false;
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

        public void PreInGameLoad()
        {
             
        }

        public void PostInGameLoad()
        {
             
        }

        public void PreMainMenuLoad()
        {
             
        }

        public void PostMainMenuLoad()
        {
             
        }
    }
    
}
