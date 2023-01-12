using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.Player.MovementStateMachine {
    
    public class BondingMovementState : BaseMovementState
    {
        private float _bondTransitionTimer;
    
        public BondingMovementState(PlayerMovementStateMachine ctx, PlayerMovementStateMachineFactory factory) : base(ctx, factory)
        {
        }
    
        public override bool CheckSwitchStates()
        {
            return false;
        }
    
        public override void EnterState()
        {
            return;
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
            
        }
    }
    
}
