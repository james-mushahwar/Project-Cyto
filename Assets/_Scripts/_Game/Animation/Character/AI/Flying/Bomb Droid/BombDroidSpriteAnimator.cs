using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _Scripts._Game.AI.MovementStateMachine.Flying.Bombdroid;
using _Scripts._Game.AI.MovementStateMachine;

namespace _Scripts._Game.Animation.Character.AI.Flying.BombDroid{
    
    public class BombDroidSpriteAnimator : SpriteAnimator
    {
        BombDroidAIMovementStateMachine _ctx;

        #region Hashed States
        public static readonly int Idle = Animator.StringToHash("BombDroidAnim_Idle");
        public static readonly int Patrol = Animator.StringToHash("BombDroidAnim_Patrol");
        public static readonly int Chase = Animator.StringToHash("BombDroidAnim_Chase");
        #endregion

        protected override void Awake()
        {
            base.Awake();

            _ctx = GetComponentInParent<BombDroidAIMovementStateMachine>();
        }

        private void FixedUpdate()
        {            
            if (_ctx.AIPath.velocity.x != 0)
            {
                Renderer.flipX = _ctx.AIPath.velocity.x < 0;
            } 

            int state = GetState();
            if (state == CurrentState)
            {
                return;
            }

            Anim.CrossFade(state, 0, 0);
        }

        protected override int GetState()
        {
            BaseAIMovementState currentMovementState = _ctx.CurrentState;
            if (currentMovementState is BombDroidIdleAIMovementState)
            {
                return Idle;
            }
            else if (currentMovementState is BombDroidPatrolAIMovementState)
            {
                return Patrol;
            }
            else if (currentMovementState is BombDroidChaseAIMovementState)
            {
                return Chase;
            }
            return Idle;

            int LockState(int s, float t) 
            {
                //_lockedTill = Time.time + t;
                return s;
            }
        }
    }
    
}
