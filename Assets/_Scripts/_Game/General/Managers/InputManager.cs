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
    }

    public class InputManager : Singleton<InputManager>
    {
        private PlayerInput _playerInput;

        public PlayerInput PlayerInput => _playerInput;

        private new void Awake()
        {
            base.Awake();
            _playerInput = new PlayerInput();
        }

        public void TryEnableActionMap(EInputSystem inputType)
        {
            _playerInput.Disable();
            switch (inputType)
            {
                case EInputSystem.Menu:
                    _playerInput.Menu.Enable();
                    break;
                case EInputSystem.Player:
                    _playerInput.Player.Enable();
                    break;
                default:
                    break;
            }
        }

        public void TryDisableActionMap(EInputSystem inputType)
        {
            _playerInput.Disable();
            switch (inputType)
            {
                case EInputSystem.Menu:
                    _playerInput.Menu.Enable();
                    break;
                case EInputSystem.Player:
                    _playerInput.Player.Enable();
                    break;
                default:
                    break;
            }
        }
    }
    
}
