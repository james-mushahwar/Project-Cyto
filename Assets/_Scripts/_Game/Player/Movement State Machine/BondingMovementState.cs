using System.Collections;
using System.Collections.Generic;
using _Scripts._Game.General;
using _Scripts._Game.General.Managers;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace _Scripts._Game.Player.MovementStateMachine {
    
    public class BondingMovementState : BaseMovementState
    {
        private bool _isInBondTransition;
        private float _bondTransitionTimer;
        private float _unbondTransitionTimer;
        private bool _isBondInputHeld;
        private IBondable _localBondingTarget;
    
        public BondingMovementState(PlayerMovementStateMachine ctx, PlayerMovementStateMachineFactory factory) : base(ctx, factory)
        {
        }
    
        public override bool CheckSwitchStates()
        {
            if (_isInBondTransition && (_localBondingTarget == null) || !_isInBondTransition)
            {
                SwitchStates(_factory.GetState(MovementState.Falling));
                return true;
            }

            return false;
        }
    
        public override void EnterState()
        {
            _isInBondTransition = true;
            _isBondInputHeld = true;
            _bondTransitionTimer = _ctx.BondTransitionDuration;
            _localBondingTarget = TargetManager.Instance.BondableTarget;
            TargetManager.Instance.LockedBondableTarget = _localBondingTarget;
            _localBondingTarget.OnStartBond();

            _ctx.Rb.velocity = Vector2.zero;
            _ctx.Rb.gravityScale = 0.0f;
            _ctx.Capsule.isTrigger = true;

            AudioSource pooledSource = AudioManager.Instance.TryPlayAudioSourceAtLocation(EAudioType.SFX_Player_BondStart, PlayerEntity.Instance.transform.position);

        }

        public override void ExitState()
        {
            _isInBondTransition = false;
            _localBondingTarget = null;

            //Vector2 inputDirection = _ctx.CurrentMovementInput.normalized;
            //_ctx.Rb.AddForce(inputDirection * _ctx.BondingExitForce, ForceMode2D.Impulse);
            _ctx.Capsule.isTrigger = false;
        }
    
        public override void InitialiseState()
        {
            
        }
    
        public override void ManagedStateTick()
        {
            if (_isBondInputHeld)
            {
                if (!_ctx.IsBondPressed)
                {
                    _isBondInputHeld = false;
                }
            }

            if (CheckSwitchStates() == false)
            {
                if (_isInBondTransition && _localBondingTarget.CanBeBonded())
                {
                    _bondTransitionTimer -= Time.deltaTime;
                    float timeStep = (_ctx.BondTransitionDuration - _bondTransitionTimer);
                    float timeStepNormalised = timeStep / _ctx.BondTransitionDuration;

                    float step = _ctx.BondingSpeedCurve.Evaluate(timeStepNormalised) * Time.deltaTime;

                    Vector2 straightPathPosition = Vector2.MoveTowards(_ctx.transform.position, _localBondingTarget.BondTargetTransform.position, step);

                    Vector2 direction = (_localBondingTarget.BondTargetTransform.position - _ctx.transform.position).normalized;
                    Vector2 perpendicularDirection = Vector2.Perpendicular(direction);
                    if (perpendicularDirection.y < 0.0f)
                    {
                        //perpendicularDirection *= -1.0f;
                    }

                    float displacement = _ctx.BondingDisplacementCurve.Evaluate(timeStepNormalised);
                    float magnitude = _ctx.BondingMagnitudeCurve.Evaluate(timeStepNormalised);

                    Vector2 resultingPostion = straightPathPosition + (perpendicularDirection * displacement * magnitude);

                    _ctx.transform.position= resultingPostion;


                    //Vector2 newPosition = Vector2.Lerp(_ctx.transform.position, _localBondingTarget.BondTargetTransform.position, (_ctx.BondTransitionDuration - _bondTransitionTimer) / _ctx.BondTransitionDuration);
                    //_ctx.transform.position = newPosition;

                    float sqDistance = (_ctx.transform.position - _localBondingTarget.BondTargetTransform.position).sqrMagnitude;
                    //if (_bondTransitionTimer <= 0.0f || sqDistance <= _localBondingTarget.SqDistanceToCompleteBond)
                    if (sqDistance <= _localBondingTarget.SqDistanceToCompleteBond)
                    {
                        _isInBondTransition = false;
                        if (_isBondInputHeld)
                        {
                            SwitchStates(_factory.GetState(MovementState.Phasing));
                        }
                        else
                        {
                            // dispossess and possess
                            PlayerEntity.Instance.OnDispossess();
                        }
                    }
                }
            }
        }
    }
    
}
