using UnityEngine;
using System;
using _Scripts._Game.AI.MovementStateMachine;

namespace _Scripts._Game.Animation.Character.AI.Bosses.GigaBombDroid{
    
    public class GigaBombDroidSpriteAnimator : SpriteAnimator
    {
        //#TBD#AIMovementStateMachine _moveCtx;
        //#TBD#AIAttackStateMachine _attackCtx;
    
        #region Hashed States
        //public static readonly int Sleep = Animator.StringToHash("#TBD#Anim_Sleep");
        //public static readonly int Wake = Animator.StringToHash("#TBD#Anim_Wake");
        //public static readonly int Idle = Animator.StringToHash("#TBD#Anim_Idle");
        //public static readonly int Patrol = Animator.StringToHash("#TBD#Anim_Patrol");
        //public static readonly int Chase = Animator.StringToHash("#TBD#Anim_Chase");
        //public static readonly int Attack = Animator.StringToHash("#TBD#Anim_Attack");
        #endregion
        
    
        protected override void Awake()
        {
            base.Awake();
    
            //_moveCtx = GetComponentInParent<#TBD#AIMovementStateMachine>();
            //_attackCtx = GetComponentInParent<#TBD#AIAttackStateMachine>();
        }
    
        protected override int GetState()
        {
            return 0;
        }
    
        protected override float GetSpeed(int state)
        {
            return 1.0f;        
        }
    
        protected override void SpriteDirection()
        {
            //if (Entity.IsPossessed())
            //{
            //    Renderer.flipX = _moveCtx.Rb.velocity.x < 0.0f;
            //}
            //else
            //{
            //    if (_moveCtx.AIPath.velocity.x != 0)
            //    {
            //        Renderer.flipX = _moveCtx.AIPath.velocity.x < 0.0f;
            //    }
            //}
        }
    }
    
}
