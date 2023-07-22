using _Scripts._Game.Dialogue;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts._Game.General.Managers{

    public enum EInputSystem
    {
        Menu,
        Player,
        BondedPlayer,
    }

    public class InputManager : Singleton<InputManager>
    {
        private PlayerInput _playerInput;

        public PlayerInput PlayerInput => _playerInput;

        private Vector2 _globalMovementInput;
        private bool _globalSouthButtonDown;

        public Vector2 GlobalMovementInput => _globalMovementInput;
        public bool GlobalSouthButtonDown => _globalSouthButtonDown;

        protected override void Awake()
        {
            base.Awake();
            _playerInput = new PlayerInput();

            //movement
            _playerInput.Global.Movement.performed += ctx => _globalMovementInput = ctx.ReadValue<Vector2>();
            //south button
            _playerInput.Global.SouthButton.started += ctx => _globalSouthButtonDown = true;
            _playerInput.Global.SouthButton.canceled += ctx => _globalSouthButtonDown = false;
        }

        private void Update()
        {
            if (_globalSouthButtonDown)
            {
                //Debug.Log("Interact input");
                InteractableManager.Instance.InteractInput();
                _globalSouthButtonDown = false;
            }

            // safe check what should be enabled
            // enable player input
            if (!DialogueManager.Instance.IsDialogueActive() && (!UIManager.Instance.IsAnyMenuActive()) &&
                _playerInput.Menu.enabled)
            {
                TryEnableActionMap(EInputSystem.Player);
            }
            // enable menu input
            if (DialogueManager.Instance.IsDialogueActive() || (UIManager.Instance.IsAnyMenuActive()) &&
                !_playerInput.Menu.enabled)
            {
                TryEnableActionMap(EInputSystem.Menu);
            }

        }

        public void TryEnableActionMap(EInputSystem inputType)
        {
            _playerInput.Disable();
            _playerInput.Global.Enable();
            switch (inputType)
            {
                case EInputSystem.Menu:
                    _playerInput.Menu.Enable();
                    break;
                case EInputSystem.Player:
                    _playerInput.Player.Enable();
                    break;
                case EInputSystem.BondedPlayer:
                    _playerInput.BondedPlayer.Enable();
                    break;
                default:
                    break;
            }
        }

        // this method may not be needed since all action maps are disabled in TryEnableActionMap first
        public void TryDisableActionMap(EInputSystem inputType)
        {
            switch (inputType)
            {
                case EInputSystem.Menu:
                    _playerInput.Menu.Disable();
                    break;
                case EInputSystem.Player:
                    _playerInput.Player.Disable();
                    break;
                case EInputSystem.BondedPlayer:
                    _playerInput.BondedPlayer.Disable();
                    break;
                default:
                    break;
            }
        }

        public void NullifyInput(EPlayerInput input)
        {
            if (input == EPlayerInput.SButton)
            {
                _globalSouthButtonDown = false;
            }
        }
    }
}
