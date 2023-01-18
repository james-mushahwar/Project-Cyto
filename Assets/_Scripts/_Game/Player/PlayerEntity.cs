using _Scripts._Game.Animation;
using _Scripts._Game.Animation.Character.Player;
using _Scripts._Game.General;
using _Scripts._Game.General.Managers;
using _Scripts._Game.Player.AttackingStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.Player{
    
    public class PlayerEntity : Singleton<PlayerEntity>, IPossessable
    {
        private bool _isPossessed;
        #region State Machines
        private PlayerMovementStateMachine _movementSM;
        private PlayerAttackingStateMachine _attackingSM;
        private PlayerSpriteAnimator _spriteAnimator;

        public PlayerMovementStateMachine MovementSM { get => _movementSM; set => _movementSM = value; }
        public PlayerAttackingStateMachine AttackingSM { get => _attackingSM; set => _attackingSM = value; }
        public PlayerSpriteAnimator SpriteAnimator { get => _spriteAnimator; set => _spriteAnimator = value; }
        #endregion

        #region Player Components
        private PlayerHealthStats _playerHealthStats;

        public PlayerHealthStats PlayerHealthStats { get => _playerHealthStats; }
        #endregion

        protected override void Awake()
        {
            base.Awake();

            _playerHealthStats = new PlayerHealthStats(10.0f, 10.0f);
        }

        void OnEnable()
        {
            OnPossess();
        }

        void OnDisable()
        {
            //InputManager.Instance.TryDisableActionMap(EInputSystem.Player);
        }

        // IPossessable
        public bool IsPossessed()
        {
            return _isPossessed;
        }

        public bool CanBePossessed()
        {
            return !_isPossessed && GetHealthStats().IsAlive();
        }

        public void OnPossess()
        {
            InputManager.Instance.TryEnableActionMap(EInputSystem.Player);
            _isPossessed = true;

            _movementSM.enabled = true;
            _attackingSM.enabled = true;
            _spriteAnimator.enabled = true;
        }

        public void OnDispossess()
        {
            // dispossess this player for something else
            InputManager.Instance.TryDisableActionMap(EInputSystem.Player);
            _isPossessed = false;

            if (_movementSM.BondableTarget != null)
            {
                _movementSM.BondableTarget.OnPossess();
            }

            _movementSM.enabled = false;
            _attackingSM.enabled = false;
            _spriteAnimator.enabled = false;
        }

        public HealthStats GetHealthStats()
        {
            return _playerHealthStats;
        }

        public Transform PossessableTransform { get => transform; }

        public Vector2 GetMovementInput()
        {
            return _movementSM.CurrentMovementInput;
        }
    }
    
}
