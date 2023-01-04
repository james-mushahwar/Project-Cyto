using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.AI.MovementStateMachine.Flying.Bombdroid{
    
    public class BombDroidIdleAIMovementState : BaseAIMovementState
    {
        public BombDroidIdleAIMovementState(AIMovementStateMachineBase ctx, AIMovementStateMachineFactory factory) : base(ctx, factory)
        {
        }

        public override bool CheckSwitchStates()
        {
            if (_stateTimer >= 2.0f)
            {
                SwitchStates(_factory.GetState(AIMovementState.Patrol));
                return true;
            }

            return false;
        }

        public override void EnterState()
        {
            Debug.Log("Hello I'm a bomb droid in idle");
        }

        public override void ExitState()
        {
            
        }

        public override void InitialiseState()
        {
            
        }

        public override void ManagedStateTick()
        {
            // do nothing :)
        }
    }

}
