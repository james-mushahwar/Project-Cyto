using _Scripts._Game.Animation;
using _Scripts._Game.Animation.Character.Player;
using _Scripts._Game.General;
using _Scripts._Game.Player.AttackingStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.Player{
    
    public class PlayerEntity : Singleton<PlayerEntity>, IPossessable
    {
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

        // IPossessable
        public void OnPossess()
        {
            // return possesssion of player
        }

        public void OnDispossess()
        {
            // dispossess this player for something else
        }

        public HealthStats GetHealthStats()
        {
            return _playerHealthStats;
        }
    }
    
}
