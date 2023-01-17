using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.Player.MovementStateMachine {
    
    public class BondingMovementState : BaseMovementState
    {
        private bool _isInBondTransition;
        private float _bondTransitionTimer;
        private float _unbondTransitionTimer;
        private IPossessable _localBondingTarget;
    
        public BondingMovementState(PlayerMovementStateMachine ctx, PlayerMovementStateMachineFactory factory) : base(ctx, factory)
        {
        }
    
        public override bool CheckSwitchStates()
        {
            if (_isInBondTransition)
            {
                if (_localBondingTarget != _ctx.BondableTarget)
                {
                    // target is lost, abort 

                }
            }
            return false;
        }
    
        public override void EnterState()
        {
            _isInBondTransition = true;
            
        }
    
        public override void ExitState()
        {
            _isInBondTransition = false;
            _localBondingTarget = null;
        }
    
        public override void InitialiseState()
        {
            
        }
    
        public override void ManagedStateTick()
        {
            if (CheckSwitchStates() == false)
            {
                if (_isInBondTransition)
                {
                    _bondTransitionTimer -= Time.deltaTime;

                    if (_bondTransitionTimer <= 0.0f)
                    {
                        // dispossess and possess
                    }
                }
                else 
                {

                }
            }
        }
    }
    
}
