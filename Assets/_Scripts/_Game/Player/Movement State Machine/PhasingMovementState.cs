﻿using _Scripts._Game.Animation;
using _Scripts._Game.General.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.Player.MovementStateMachine{
    
    public class PhasingMovementState : BaseMovementState
    {
        private float _phaseStateMaxDuration;

        public PhasingMovementState(PlayerMovementStateMachine ctx, PlayerMovementStateMachineFactory factory) : base(ctx, factory)
        {
        }

        public override bool CheckSwitchStates()
        {
            if (!_ctx.IsBondPressed || _stateTimer >= _phaseStateMaxDuration)
            {
                SwitchStates(_factory.GetState(MovementState.Falling));
                return true;
            }

            return false;
        }

        public override void EnterState()
        {
            _stateTimer = 0.0f;
            _phaseStateMaxDuration = _ctx.PhasingMaxDuration;

            _ctx.Rb.velocity = Vector2.zero;
            _ctx.Rb.gravityScale = 0.0f;
            _ctx.Capsule.isTrigger = true;

            _ctx.transform.SetParent(TargetManager.Instance.LockedBondableTarget.BondTargetTransform);
            PlayerEntity.Instance.SpriteAnimator.enabled = false;
        }

        public override void ExitState()
        {
            _ctx.PostBondTimeElapsed = _ctx.PostBondCooldownTime;

            // do phase movement!
            Vector2 inputDirection = InputManager.Instance.GlobalMovementInput.normalized;
            //Debug.Log("Input direction = " + inputDirection);
            _ctx.Rb.AddForce(inputDirection * _ctx.PhasingExitForce, ForceMode2D.Impulse);
            _ctx.PhasingExitDirection = inputDirection;
            _ctx.Capsule.isTrigger = false;

            AudioSource pooledSource = (AudioManager.Instance as AudioManager).TryPlayAudioSourceAtLocation(EAudioType.SFX_Player_BondExit, PlayerEntity.Instance.transform.position);

            _ctx.transform.SetParent(null);
            PlayerEntity.Instance.SpriteAnimator.enabled = true;
        }

        public override void InitialiseState()
        {
            
        }

        public override void ManagedStateTick()
        {
            _stateTimer += Time.deltaTime;

            if (CheckSwitchStates() == false)
            {
                // do something
            }
        }
    }
    
}
