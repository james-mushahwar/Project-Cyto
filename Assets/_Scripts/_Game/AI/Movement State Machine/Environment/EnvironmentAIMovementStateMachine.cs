using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.AI.MovementStateMachine.Environment{
    
    public class EnvironmentAIMovementStateMachine : AIMovementStateMachineBase
    {
        public override bool DoesStateUseAIPathFinding(AIMovementState state)
        {
            return false;
        }
    }
    
}
