using _Scripts._Game.Animation;
using _Scripts._Game.Animation.Character.Player;
using _Scripts._Game.General;
using _Scripts._Game.Player.AttackingStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.Player{
    
    public class PlayerEntity : Singleton<PlayerEntity>
    {
        #region State Machines
        private PlayerMovementStateMachine _movementSM;
        private PlayerAttackingStateMachine _attackingSM;
        private PlayerSpriteAnimator _spriteAnimator;

        public PlayerMovementStateMachine MovementSM { get => _movementSM; set => _movementSM = value; }
        public PlayerAttackingStateMachine AttackingSM { get => _attackingSM; set => _attackingSM = value; }
        public PlayerSpriteAnimator SpriteAnimator { get => _spriteAnimator; set => _spriteAnimator = value; }
        #endregion

        protected override void Awake()
        {
            base.Awake();
        }
    }
    
}
