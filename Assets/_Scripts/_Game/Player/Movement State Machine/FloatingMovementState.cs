using _Scripts._Game.General.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingMovementState : BaseMovementState
{
    private float _jumpBufferTimer;

    public FloatingMovementState(PlayerMovementStateMachine ctx, PlayerMovementStateMachineFactory factory) : base(ctx, factory)
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

        if (_ctx.IsDashInputValid == true && _ctx.DashTimeElapsed <= 0.0f)
        {
            if (_ctx.DashCounter == 0)
            {
                SwitchStates(_factory.GetState(MovementState.Dashing));
                return true;
            }
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
        else if(_ctx.IsFloatInputValid == false)
        {
            SwitchStates(_factory.GetState(MovementState.Falling));
            return true;
        }

        if (_stateTimer >= _ctx.FloatingInputForceDelay)
        {
            if (_ctx.IsBounceInputValid == true)
            {
                _ctx.NullifyInput(MovementState.Floating);
                SwitchStates(_factory.GetState(MovementState.Bouncing));
                return true;
            }
        }

        return false;
    }

    public override void EnterState()
    {
        //_ctx.Rb.velocity += (Vector2.up * _ctx.JumpForce);
        _ctx.Rb.gravityScale = _ctx.FloatingGravityScale;
        _ctx.Rb.velocity *= Vector2.right;
        _ctx.Rb.AddForce(Vector2.up * _ctx.FloatingUpwardsEnterForce, ForceMode2D.Impulse);
        _stateTimer = 0.0f;
        _jumpBufferTimer = 0.0f;
    }

    public override void ExitState()
    {
        
    }

    public override void InitialiseState()
    {
        return;
    }

    public override void ManagedStateTick()
    {
        _stateTimer += Time.deltaTime;
        _jumpBufferTimer -= Time.deltaTime;

        if (CheckSwitchStates() == false)
        {
            #region Float Movement


            float targetSpeed = _ctx.CurrentMovementInput.x * _ctx.FloatingHorizontalVelocity;
            float speedDif = targetSpeed - _ctx.Rb.velocity.x;
            float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? _ctx.FloatingHorizontalAcceleration : _ctx.FloatingHorizontalDeceleration;
            float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, _ctx.FallingVelocityPower) * Mathf.Sign(speedDif);

            _ctx.Rb.AddForce(movement * Vector2.right);
            float rbYVelocity = Mathf.Clamp(_ctx.Rb.velocity.y, _ctx.FloatingMaximumDownwardsVelocity, Mathf.Infinity );
            _ctx.Rb.velocity = new Vector2(_ctx.Rb.velocity.x, rbYVelocity);

            #endregion
        }
    }
}
