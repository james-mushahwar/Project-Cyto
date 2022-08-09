using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingMovementState : BaseMovementState
{
    private float _jumpBufferTimer;

    public FallingMovementState(PlayerMovementStateMachine ctx, PlayerMovementStateMachineFactory factory) : base(ctx, factory)
    {
    }

    public override bool CheckSwitchStates()
    {
        if (_ctx.IsJumpInputValid == true && _ctx.IsJumpPressed == true)
        {
            _jumpBufferTimer = _ctx.JumpBufferLimit;
            if (_ctx.JumpCounter == 0)
            {
                _jumpBufferTimer = 0.0f;
                SwitchStates(_factory.GetState(MovementState.Jumping));
                return true;
            }
        }

        if (_ctx.IsFloatInputValid == true)
        {
            SwitchStates(_factory.GetState(MovementState.Floating));
            return true;
        }

        if (_ctx.IsGrounded == true)
        {
            if (_jumpBufferTimer > 0.0f)
            {
                SwitchStates(_factory.GetState(MovementState.Jumping));
                return true;
            }
            else
            {
                SwitchStates(_factory.GetState(MovementState.Grounded));
                return true;
            }
        }

        if (_ctx.IsDashInputValid == true)
        {
            if (_ctx.DashCounter == 0)
            {
                SwitchStates(_factory.GetState(MovementState.Dashing));
                return true;
            }
        }

        if (_ctx.IsBounceInputValid == true)
        {
            SwitchStates(_factory.GetState(MovementState.Bouncing));
            return true;
        }

        return false;
    }

    public override void EnterState()
    {
        Debug.Log("Hello I'm falling!!");
        _ctx.Rb.gravityScale = _ctx.FallingGravityScale;
        _jumpBufferTimer = -1.0f;
    }

    public override void ExitState()
    {
        
    }

    public override void InitialiseState()
    {

    }

    public override void ManagedStateTick()
    {
        if (CheckSwitchStates() == false)
        {
            // update 
            _jumpBufferTimer -= Time.deltaTime;

            #region Run

            float targetSpeed = _ctx.CurrentMovementInput.x * _ctx.FallingHorizontalVelocity;
            float speedDif = targetSpeed - _ctx.Rb.velocity.x;
            float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? _ctx.FallingAcceleration : _ctx.FallingDeceleration;
            float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, _ctx.FallingVelocityPower) * Mathf.Sign(speedDif);

            _ctx.Rb.AddForce(movement * Vector2.right);

            #endregion
        }
    }
}
