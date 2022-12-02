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

        private void Awake()
        {
            _playerInput = new PlayerInput();
        }

        public void TryEnableActionMap(EInputSystem inputType)
        {
            switch (inputType)
            {
                case EInputSystem.Menu:
                    _playerInput.Player.Enable();
                    break;
                case EInputSystem.Player:
                    _playerInput.Menu.Enable();
                    break;
                default:
                    break;
            }
        }
    }
    
}
