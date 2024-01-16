using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _Scripts._Game.General.Managers;

public class JumpingMovementState : BaseMovementState
{
    public JumpingMovementState(PlayerMovementStateMachine ctx, PlayerMovementStateMachineFactory factory) : base(ctx, factory)
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

        if (_ctx.Rb.velocity.y < 0.0f && _ctx.IsGrounded == true && _stateTimer > 0.33f)
        {
            SwitchStates(_factory.GetState(MovementState.Grounded));
            return true;
        }
        
        if ((_ctx.IsJumpPressed == false && _stateTimer >= _ctx.JumpInputForceDelay) || _stateTimer >= _ctx.JumpInputDuration)
        {
            SwitchStates(_factory.GetState(MovementState.Falling));
            return true;
        }

        if ( _stateTimer >= _ctx.JumpInputForceDelay && _ctx.IsDashInputValid == true && _ctx.DashTimeElapsed <= 0.0f)
        {
            SwitchStates(_factory.GetState(MovementState.Dashing));
            return true;
        }

        if (_stateTimer >= _ctx.JumpInputForceDelay && _ctx.IsBounceInputValid == true)
        {
            SwitchStates(_factory.GetState(MovementState.Bouncing));
            return true;
        }

        if (_stateTimer >= _ctx.JumpInputForceDelay && _ctx.IsFloatInputValid == true)
        {
            SwitchStates(_factory.GetState(MovementState.Floating));
            return true;
        }

        return false;
    }

    public override void EnterState()
    {
        //_ctx.Rb.velocity += (Vector2.up * _ctx.JumpForce);
        _ctx.Rb.velocity *= Vector2.right;
        _ctx.Rb.AddForce(Vector2.up * _ctx.JumpForce, ForceMode2D.Impulse);
        _ctx.Rb.gravityScale = _ctx.JumpingGravityScale;

        _stateTimer = 0.0f;
        _ctx.JumpCounter++;
        _ctx.NullifyInput(MovementState.Jumping);

        AudioManager.Instance.TryPlayAudioSourceAtLocation(EAudioType.SFX_Player_Jump, _ctx.transform.position);
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
            //_ctx.Rb.velocity = Vector2.up * _ctx.JumpForce;
            if (_stateTimer >= _ctx.JumpInputForceDelay)
            {
                float forceTimer = _stateTimer / _ctx.JumpInputDuration;
                float forceAlpha = Mathf.Lerp(_ctx.JumpForce, _ctx.JumpForce * 0.5f, forceTimer);
                _ctx.Rb.AddForce(Vector2.up * forceAlpha * Time.deltaTime, ForceMode2D.Force);
            }

            #region Movement

            float targetSpeed = _ctx.CurrentMovementInput.x * _ctx.FallingHorizontalVelocity;
            float speedDif = targetSpeed - _ctx.Rb.velocity.x;
            float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? _ctx.FallingAcceleration : _ctx.FallingDeceleration;
            float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, _ctx.FallingVelocityPower) * Mathf.Sign(speedDif);

            _ctx.Rb.AddForce(movement * Vector2.right * Time.deltaTime);

            #endregion
        }
    }

    
}
