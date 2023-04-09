using System.Collections;
using System.Collections.Generic;
using _Scripts._Game.General;
using _Scripts._Game.General.Managers;
using UnityEngine;

namespace _Scripts._Game.Player.MovementStateMachine {
    
    public class BondingMovementState : BaseMovementState
    {
        private bool _isInBondTransition;
        private float _bondTransitionTimer;
        private float _unbondTransitionTimer;
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
            _bondTransitionTimer = _ctx.BondTransitionDuration;
            _localBondingTarget = TargetManager.Instance.BondableTarget;
            TargetManager.Instance.LockedBondableTarget = _localBondingTarget;

            _ctx.Rb.velocity = Vector2.zero;
            _ctx.Rb.gravityScale = 0.0f;
            _ctx.Capsule.isTrigger = true;
        }
    
        public override void ExitState()
        {
            _isInBondTransition = false;
            _localBondingTarget = null;

            // player components
            Vector2 inputDirection = _ctx.CurrentMovementInput.normalized;
            _ctx.Rb.AddForce(inputDirection * _ctx.BondingExitForce, ForceMode2D.Impulse);
            _ctx.Capsule.isTrigger = false;
        }
    
        public override void InitialiseState()
        {
            
        }
    
        public override void ManagedStateTick()
        {
            if (CheckSwitchStates() == false)
            {
                if (_isInBondTransition && _localBondingTarget.CanBeBonded())
                {
                    _bondTransitionTimer -= Time.deltaTime;

                    Vector2 newPosition = Vector2.Lerp(_ctx.transform.position, _localBondingTarget.BondTargetTransform.position, (_ctx.BondTransitionDuration - _bondTransitionTimer) / _ctx.BondTransitionDuration);
                    _ctx.transform.position = newPosition;

                    if (_bondTransitionTimer <= 0.0f)
                    {
                        // dispossess and possess
                        _isInBondTransition = false;
                        PlayerEntity.Instance.OnDispossess();
                    }
                }
            }
        }
    }
    
}
