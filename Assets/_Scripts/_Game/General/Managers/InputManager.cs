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

        public Vector2 GlobalMovementInput => _globalMovementInput;

        protected override void Awake()
        {
            base.Awake();
            _playerInput = new PlayerInput();

            _playerInput.Global.Movement.performed += ctx => _globalMovementInput = ctx.ReadValue<Vector2>();
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
    }
    
}
