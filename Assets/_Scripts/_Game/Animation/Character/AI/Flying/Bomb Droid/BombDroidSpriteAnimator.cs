using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _Scripts._Game.AI.MovementStateMachine.Flying.Bombdroid;
using _Scripts._Game.AI.MovementStateMachine;
using System;

namespace _Scripts._Game.Animation.Character.AI.Flying.BombDroid{
    
    public class BombDroidSpriteAnimator : SpriteAnimator
    {
        BombDroidAIMovementStateMachine _ctx;

        #region Hashed States
        public static readonly int Idle = Animator.StringToHash("BombDroidAnim_Idle");
        public static readonly int Patrol = Animator.StringToHash("BombDroidAnim_Patrol");
        public static readonly int Chase = Animator.StringToHash("BombDroidAnim_Chase");
        #endregion

        #region Bonded animation properties
        [SerializeField]
        private Vector2 _bondedMovementAnimSpeedRange;
        #endregion

        protected override void Awake()
        {
            base.Awake();

            _ctx = GetComponentInParent<BombDroidAIMovementStateMachine>();
        }

        protected override int GetState()
        {
            if (Entity.IsPossessed())
            {
                BaseAIBondedMovementState currentBondedMovementState = _ctx.CurrentBondedState;
                if (currentBondedMovementState is BombDroidFlyingAIBondedMovementState)
                {
                    return Patrol;
                }
                else
                {
                    return Patrol;
                }
            }
            else
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
            }
            
            return Idle;

            int LockState(int s, float t) 
            {
                //_lockedTill = Time.time + t;
                return s;
            }
        }

        protected override float GetSpeed(int state)
        {
            if (Entity.IsPossessed())
            {
                if (state == Patrol)
                {
                    float newSpeed = Mathf.Lerp(_bondedMovementAnimSpeedRange.x, _bondedMovementAnimSpeedRange.y, Convert.ToInt16(Mathf.Abs(_ctx.Rb.velocity.x) > 1.0f || Mathf.Abs(_ctx.Rb.velocity.y) > 1.0f));
                    return newSpeed;
                }
                else
                {
                    return 1.0f;
                }
            }
            else
            {
                return 1.0f;
            }
        }

        protected override void SpriteDirection()
        {
            if (Entity.IsPossessed())
            {
                Renderer.flipX = _ctx.Rb.velocity.x < 0.0f;
            }
            else
            {
                if (_ctx.AIPath.velocity.x != 0)
                {
                    Renderer.flipX = _ctx.AIPath.velocity.x < 0.0f;
                }
            }
            
        }
    }
    
}
