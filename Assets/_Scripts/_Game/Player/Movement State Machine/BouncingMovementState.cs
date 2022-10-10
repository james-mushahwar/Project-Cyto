using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class BouncingMovementState : BaseMovementState
{
    private float _cachedHorizontaVelocity;
    private float _bounceTimer;
    private float _jumpBufferTimer;
    private float _bounceCharge;

    public BouncingMovementState(PlayerMovementStateMachine ctx, PlayerMovementStateMachineFactory factory) : base(ctx, factory)
    {
    }

    public override bool CheckSwitchStates()
    {
        if (_ctx.IsJumpInputValid == true && _ctx.IsJumpPressed == true)
        {
            _jumpBufferTimer = _ctx.JumpBufferLimit;
            if (_ctx.JumpCounter == 0 && _stateTimer >= _ctx.BouncingInputForceDelay)
            {
                _jumpBufferTimer = 0.0f;
                SwitchStates(_factory.GetState(MovementState.Jumping));
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

        if (_stateTimer >= _ctx.BouncingInputForceDelay)
        {
            if (_ctx.IsBounceInputValid == false)
            {
                if (_ctx.IsGrounded)
                {
                    SwitchStates(_factory.GetState(MovementState.Grounded));
                    return true;
                }
                else
                {
                    SwitchStates(_factory.GetState(MovementState.Falling));
                    return true;
                }
            }
            
            if (_ctx.IsFloatInputValid == true)
            {
                _ctx.NullifyInput(MovementState.Bouncing);
                SwitchStates(_factory.GetState(MovementState.Floating));
                return true;
            }
        }

        if (_ctx.IsBounceInputValid == true && _stateTimer < _ctx.BouncingInputForceDelay)
        {
            if (_ctx.IsGrounded)
            {
                SwitchStates(_factory.GetState(MovementState.Grounded));
                return true;
            }
        }

        return false;
    }

    public override void EnterState()
    {
        Debug.Log("Hello I'm Bouncing!!");
        if (_ctx.BouncingChargeTimer >= _ctx.BouncingFullChargeTime)
        {
            SuperBounce();
        }
        else
        {
            _cachedHorizontaVelocity = _ctx.Rb.velocity.x;
            _ctx.Rb.gravityScale = _ctx.BouncingDownwardGravityScale;
        }

        _jumpBufferTimer = 0.0f;
        _stateTimer = 0.0f;
        _bounceTimer = 0.0f;
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
        if (_ctx.BouncingChargeTimer >= _ctx.BouncingFullChargeTime)
        {
            // hits once on first tick after a super bounce
            _cachedHorizontaVelocity = _ctx.Rb.velocity.x;
            _ctx.BouncingChargeTimer = 0.0f;
            _ctx.Rb.gravityScale = _ctx.BouncingDownwardGravityScale;
        }

        _stateTimer += Time.deltaTime;
        _jumpBufferTimer -= Time.deltaTime;
        _bounceTimer += Time.deltaTime;

        if (CheckSwitchStates() == false)
        {
            if (_ctx.IsGrounded == true && _ctx.Rb.velocity.y < 0.0f)
            {
                Bounce();
            }

            float targetSpeed = _ctx.CurrentMovementInput.x * Mathf.Clamp(_cachedHorizontaVelocity * Mathf.Sign(_cachedHorizontaVelocity), _ctx.BouncingDefaultHorizontalVelocity, _ctx.BouncingMaximumHorizontalVelocity);
            float speedDif = targetSpeed - _ctx.Rb.velocity.x;
            float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? _ctx.BouncingHorizontalAcceleration : _ctx.BouncingHorizontalDeceleration;
            float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, _ctx.BouncingVelocityPower) * Mathf.Sign(speedDif);

            _ctx.Rb.AddForce(movement * Vector2.right);

            if (_ctx.Rb.velocity.y >= 0.0f || _ctx.IsGrounded == true)
            {
                _ctx.Rb.gravityScale = _ctx.BouncingUpwardGravityScale;
            }
            else
            {
                _ctx.Rb.gravityScale = _ctx.BouncingDownwardGravityScale;
            }
        }
    }

    private void Bounce()
    {
        //reset counters
        _ctx.BouncingCounter++;
        _ctx.JumpCounter = 0;
        _ctx.DashCounter = 0;
        _bounceTimer = 0.0f;

        // explosive force up
        float yVelocity = _ctx.Rb.velocity.y;
        float bouncingPowerMultiplier = _ctx.BouncingCounter >= 3 ? _ctx.BouncingPowerMultiplier.z : _ctx.BouncingCounter >= 2 ? _ctx.BouncingPowerMultiplier.y : _ctx.BouncingPowerMultiplier.x;

        yVelocity = Mathf.Clamp(yVelocity * bouncingPowerMultiplier, _ctx.BouncingMaximumDownwardsVelocity, _ctx.BouncingDefaultDownwardsVelocity);
        float yRatio = (yVelocity - _ctx.BouncingDefaultDownwardsVelocity) / (_ctx.BouncingMaximumDownwardsVelocity - _ctx.BouncingDefaultDownwardsVelocity);

        float upwardsForce = Mathf.Lerp(_ctx.BouncingMinMaxUpwardsBounceForce.x, _ctx.BouncingMinMaxUpwardsBounceForce.y, yRatio);

        Debug.Log("yVelocity is : " + _ctx.Rb.velocity.y + " and yRatio is: " + yRatio + " and upwardsForce is: " + upwardsForce + " and starting world Y coords is " + _ctx.transform.position.y);

        _ctx.Rb.velocity *= Vector2.right;
        _ctx.Rb.AddForce(Vector2.up * upwardsForce, ForceMode2D.Impulse);

        CameraShaker.Instance.ShakeOnce(1.5f, 0.1f, 0.1f, 0.1f);
    }

    private void SuperBounce()
    {
        //reset counters
        _ctx.BouncingCounter++;
        _ctx.JumpCounter = 0;
        _ctx.DashCounter = 0;
        _bounceTimer = 0.0f;

        // explosive force up
        float yVelocity = _ctx.Rb.velocity.y;
        float bouncingPowerMultiplier = _ctx.BouncingCounter >= 3 ? _ctx.BouncingPowerMultiplier.z : _ctx.BouncingCounter >= 2 ? _ctx.BouncingPowerMultiplier.y : _ctx.BouncingPowerMultiplier.x;

        yVelocity = Mathf.Clamp(yVelocity * bouncingPowerMultiplier, _ctx.BouncingMaximumDownwardsVelocity, _ctx.BouncingDefaultDownwardsVelocity);
        //float yRatio = (yVelocity - _ctx.BouncingDefaultDownwardsVelocity) / (_ctx.BouncingMaximumDownwardsVelocity - _ctx.BouncingDefaultDownwardsVelocity);

        //float upwardsForce = Mathf.Lerp(_ctx.BouncingMinMaxUpwardsBounceForce.x, _ctx.BouncingMinMaxUpwardsBounceForce.y, yRatio);
        float upwardsForce = _ctx.BouncingMinMaxUpwardsBounceForce.y;

        //Debug.Log("yVelocity is : " + _ctx.Rb.velocity.y + " and yRatio is: " + yRatio + " and upwardsForce is: " + upwardsForce + " and starting world Y coords is " + _ctx.transform.position.y);

        _ctx.Rb.velocity *= Vector2.right;
        _ctx.Rb.AddForce(Vector2.up * upwardsForce, ForceMode2D.Impulse);

        CameraShaker.Instance.ShakeOnce(1.5f, 0.1f, 0.1f, 0.1f);
    }
}
