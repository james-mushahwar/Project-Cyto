using UnityEngine;
using _Scripts._Game.AI.MovementStateMachine;

namespace _Scripts._Game.AI.MovementStateMachine.Bosses.GigaBombDroid{
    
    public class GigaBombDroidFlyingAIBondedMovementState : GigaBombDroidBaseAIBondedMovementState
    {
        public GigaBombDroidFlyingAIBondedMovementState(AIMovementStateMachineBase ctx, AIMovementStateMachineFactory factory) : base(ctx, factory)
        {
    
        }
    
        public override bool CheckSwitchStates()
        {
            return false;
        }
    
        public override void EnterState()
        {
            _stateTimer = 0.0f;
        }
    
        public override void ExitState()
        {
            
        }
    
        public override void InitialiseState()
        {
            
        }
    
        public override void ManagedStateTick()
        {
            _stateTimer += Time.deltaTime;

            if (CheckSwitchStates() == false)
            {
                #region Flying Movement
                Vector3 thrust = _ctx.CurrentMovementInput.normalized * _gbdCtx.FlyingMovementDirectionThrust * Time.deltaTime;
                _ctx.Rb.AddForce(thrust);

                float rbXVelocity = Mathf.Clamp(_ctx.Rb.velocity.x, -_gbdCtx.FlyingMaximumHorizontalVelocity, _gbdCtx.FlyingMaximumHorizontalVelocity);
                float rbYVelocity = Mathf.Clamp(_ctx.Rb.velocity.y, -_gbdCtx.FlyingMaximumVerticalVelocity, _gbdCtx.FlyingMaximumVerticalVelocity);

                _ctx.Rb.velocity = new Vector2(rbXVelocity, rbYVelocity);

                #endregion
            }
        }
        
    }
    
}
