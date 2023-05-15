using _Scripts._Game.General.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedMovementState : BaseMovementState
{
    public GroundedMovementState(PlayerMovementStateMachine ctx, PlayerMovementStateMachineFactory factory) : base(ctx, factory)
    {
    }

    public override bool CheckSwitchStates()
    {
        if (_ctx.IsBondInputValid == true)
        {
            if (TargetManager.Instance.BondableTarget != null)
            {
                SwitchStates(_factory.GetState(MovementState.Bonding));
                return true;
            }
        }

        if (_ctx.BouncingChargeTimer >= _ctx.BouncingFullChargeTime)
        {
            SwitchStates(_factory.GetState(MovementState.Bouncing));
            return true;
        }

        if (_ctx.IsGrounded == false)
        {
            SwitchStates(_factory.GetState(MovementState.Falling));
            return true;
        }
        
        if (_ctx.IsJumpInputValid == true)
        {
            SwitchStates(_factory.GetState(MovementState.Jumping));
            return true;
        }

        if (_ctx.IsDashInputValid == true && _ctx.DashTimeElapsed <= 0.0f)
        {
            SwitchStates(_factory.GetState(MovementState.Dashing));
            return true;
        }

        return false;
    }

    public override void EnterState()
    {
        _ctx.JumpCounter = 0;
        _ctx.DashCounter = 0;
        _ctx.BouncingCounter = 0;
        _ctx.Rb.gravityScale = _ctx.GroundedGravityScale;
    }

    public override void ExitState()
    {
        return;
    }

    public override void InitialiseState()
    {
        return;
    }

    public override void ManagedStateTick()
    {
        _ctx.BouncingChargeTimer = (_ctx.IsBounceInputValid == true) ? _ctx.BouncingChargeTimer + Time.deltaTime : 0.0f;

        if (CheckSwitchStates() == false)
        {
            // update - not switched states this frame
            //float xInput = _ctx.CurrentMovementInput.x;
           //_ctx.Rb.velocity = Vector2.right * xInput * _ctx.GroundedHorizontalVelocity;

            #region Run

            float targetSpeed = _ctx.CurrentMovementInput.x * _ctx.GroundedHorizontalVelocity;
            float speedDif = targetSpeed - _ctx.Rb.velocity.x;
            float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? _ctx.GroundedAcceleration : _ctx.GroundedDeceleration;
            float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, _ctx.GroundedVelocityPower) * Mathf.Sign(speedDif);

            _ctx.Rb.AddForce(movement * Vector2.right);

            #endregion
        }
    }

    void HandleGravity()
    {
        
    }
}
