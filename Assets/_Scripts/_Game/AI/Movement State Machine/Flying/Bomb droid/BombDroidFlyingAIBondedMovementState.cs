using UnityEngine;

namespace _Scripts._Game.AI.MovementStateMachine.Flying.Bombdroid{
    
    public class BombDroidFlyingAIBondedMovementState : BaseAIBondedMovementState
    {
        private BombDroidAIMovementStateMachine _bdCtx;

        public BombDroidFlyingAIBondedMovementState(AIMovementStateMachineBase ctx, AIMovementStateMachineFactory factory) : base(ctx, factory)
        {
            _bdCtx = ctx.GetStateMachine<BombDroidAIMovementStateMachine>();
        }

        public override bool CheckSwitchStates()
        {
            return false;
        }

        public override void EnterState()
        {
            _bdCtx.Seeker.enabled = false;
            _bdCtx.DestinationSetter.enabled = false;
            _bdCtx.AIPath.enabled = false;
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


                //float targetSpeed = _bdCtx.CurrentMovementInput.x * _bdCtx.FlyingHorizontalVelocity;
                //float speedDif = targetSpeed - _bdCtx.Rb.velocity.x;
                //float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? _bdCtx.FlyingHorizontalAcceleration : _bdCtx.FlyingHorizontalDeceleration;
                //float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, 1) * Mathf.Sign(speedDif);

                //_ctx.Rb.AddForce(movement * Vector2.right);
                //float rbYVelocity = Mathf.Clamp(_ctx.Rb.velocity.y, _bdCtx.FlyingMaximumDownwardsVelocity, Mathf.Infinity);
                //_ctx.Rb.velocity = new Vector2(_ctx.Rb.velocity.x, rbYVelocity);

                _ctx.Rb.AddForce(_ctx.CurrentMovementInput.normalized * _bdCtx.FlyingMovementDirectionThrust);

                float rbXVelocity = Mathf.Clamp(_ctx.Rb.velocity.x, -_bdCtx.FlyingMaximumHorizontalVelocity, _bdCtx.FlyingMaximumHorizontalVelocity);
                float rbYVelocity = Mathf.Clamp(_ctx.Rb.velocity.y, -_bdCtx.FlyingMaximumVerticalVelocity, _bdCtx.FlyingMaximumVerticalVelocity);

                _ctx.Rb.velocity = new Vector2(rbXVelocity, rbYVelocity);

                #endregion
            }
        }
    }

}
