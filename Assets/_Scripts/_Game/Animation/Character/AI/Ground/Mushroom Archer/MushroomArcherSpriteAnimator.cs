using UnityEngine;
using System;
using _Scripts._Game.AI.MovementStateMachine;

namespace _Scripts._Game.Animation.Character.AI.Ground.MushroomArcher{
    
    public class MushroomArcherSpriteAnimator : SpriteAnimator
    {
        //MushroomArcherAIMovementStateMachine _moveCtx;
        //MushroomArcherAIAttackStateMachine _attackCtx;
    
        #region Hashed States
        //public static readonly int Sleep = Animator.StringToHash("MushroomArcherAnim_Sleep");
        //public static readonly int Wake = Animator.StringToHash("MushroomArcherAnim_Wake");
        //public static readonly int Idle = Animator.StringToHash("MushroomArcherAnim_Idle");
        //public static readonly int Patrol = Animator.StringToHash("MushroomArcherAnim_Patrol");
        //public static readonly int Chase = Animator.StringToHash("MushroomArcherAnim_Chase");
        //public static readonly int Attack = Animator.StringToHash("MushroomArcherAnim_Attack");
        #endregion
        
    
        protected override void Awake()
        {
            base.Awake();
    
            //_moveCtx = GetComponentInParent<MushroomArcherAIMovementStateMachine>();
            //_attackCtx = GetComponentInParent<MushroomArcherAIAttackStateMachine>();
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
            if (Entity.IsPossessed())
            {
                //Renderer.flipX = _moveCtx.Rb.velocity.x < 0.0f;
            }
            else
            {
                //if (_moveCtx.AIPath.velocity.x != 0)
                //{
                //    Renderer.flipX = _moveCtx.AIPath.velocity.x < 0.0f;
                //}
            }
        }
    }
    
}
