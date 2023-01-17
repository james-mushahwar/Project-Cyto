using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashingMovementState : BaseMovementState
{
    private float _jumpBufferTimer;

    public DashingMovementState(PlayerMovementStateMachine ctx, PlayerMovementStateMachineFactory factory) : base(ctx, factory)
    {
    }

    public override bool CheckSwitchStates()
    {
        if (_ctx.IsBondInputValid == true)
        {
            if (_ctx.BondableTarget != null)
            {
                SwitchStates(_factory.GetState(MovementState.Bonding));
                return true;
            }
        }

        if (_ctx.IsJumpInputValid == true)
        {
            _jumpBufferTimer = _ctx.DashingJumpBufferLimit;
        }

        if (_stateTimer >= _ctx.DashingStateDuration)
        {
            if (_ctx.IsGrounded)
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
            else
            {
                if (_jumpBufferTimer > 0.0f)
                {
                    if (_ctx.JumpCounter == 0)
                    {
                        SwitchStates(_factory.GetState(MovementState.Jumping));
                        return true;
                    }
                }
                else if (_ctx.IsBounceInputValid == true)
                {
                    SwitchStates(_factory.GetState(MovementState.Bouncing));
                    return true;
                }
                else if (_ctx.IsFloatInputValid == true)
                {
                    SwitchStates(_factory.GetState(MovementState.Floating));
                    return true;
                }
                else
                {
                    SwitchStates(_factory.GetState(MovementState.Falling));
                    return true;
                }
            }
        }
        return false;
    }

    public override void EnterState()
    {
        _ctx.NullifyInput(MovementState.Dashing);
        _stateTimer = 0.0f;
        _ctx.DashCounter++;

        Debug.Log("Dash time!");
        _ctx.Rb.gravityScale = _ctx.DashingGravityScale;
        _ctx.Rb.velocity = _ctx.Rb.velocity * Vector2.right;

        _ctx.Rb.AddForce(Vector2.right * _ctx.DashingForce * Mathf.Sign(_ctx.CurrentMovementInput.x));
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
        _stateTimer += Time.deltaTime;

        if (CheckSwitchStates() == false)
        {
            _jumpBufferTimer -= Time.deltaTime;
        }
    }
}
