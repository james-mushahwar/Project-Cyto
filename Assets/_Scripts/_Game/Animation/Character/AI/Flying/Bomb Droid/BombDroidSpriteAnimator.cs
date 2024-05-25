using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using _Scripts._Game.AI.MovementStateMachine.Flying.Bombdroid;
using _Scripts._Game.AI.MovementStateMachine;
using _Scripts._Game.AI.AttackStateMachine.Flying.Bombdroid;

namespace _Scripts._Game.Animation.Character.AI.Flying.BombDroid{
    
    public class BombDroidSpriteAnimator : SpriteAnimator
    {
        BombDroidAIMovementStateMachine _moveCtx;
        BombDroidAIAttackStateMachine _attackCtx;

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

            _moveCtx = GetComponentInParent<BombDroidAIMovementStateMachine>();
            _attackCtx = GetComponentInParent<BombDroidAIAttackStateMachine>();
        }

        protected override int GetState()
        {
            if (Entity.IsPossessed())
            {
                BaseAIBondedMovementState currentBondedMovementState = _moveCtx.CurrentBondedState;
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
                BaseAIMovementState currentMovementState = _moveCtx.CurrentState;
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
                    float newSpeed = Mathf.Lerp(_bondedMovementAnimSpeedRange.x, _bondedMovementAnimSpeedRange.y, Convert.ToInt16(Mathf.Abs(_moveCtx.Rb.velocity.x) > 1.0f || Mathf.Abs(_moveCtx.Rb.velocity.y) > 1.0f));
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
                if (Mathf.Abs(_moveCtx.Rb.velocity.x) >= 5.0f)
                {
                    Renderer.flipX = _moveCtx.Rb.velocity.x < 0.0f;
                }
            }
            else
            {
                if (Mathf.Abs(_moveCtx.AIPath.velocity.x) >= 1.0f)
                {
                    Renderer.flipX = _moveCtx.AIPath.velocity.x < 0.0f;
                }
            }
            
        }
    }
    
}
