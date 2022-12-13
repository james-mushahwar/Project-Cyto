﻿using _Scripts._Game.Dialogue;
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

            enabled = false;
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
                default:
                    break;
            }
        }
    }
    
}
