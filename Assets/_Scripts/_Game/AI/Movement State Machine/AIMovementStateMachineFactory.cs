using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.AI.MovementStateMachine{

    public enum AIMovementState
    {
        Idle,
        Waking,
        Walking,
        Flying,
        Jumping,
        Falling,
        Landing
    }

    public class AIMovementStateMachineFactory : MonoBehaviour
    {
        AIMovementStateMachineBase _moveStateMachine;
        Dictionary<AIMovementState, BaseAIMovementState> _stateDict = new Dictionary<AIMovementState, BaseAIMovementState>();

        public AIMovementStateMachineFactory(AIMovementStateMachineBase sm)
        {
            _moveStateMachine = sm;
        }

        public BaseAIMovementState GetState(AIMovementState state)
        {
            return _stateDict[state];
        }

        public AIMovementState GetMovementStateEnum(BaseAIMovementState state)
        {
            foreach (KeyValuePair<AIMovementState, BaseAIMovementState> entry in _stateDict)
            {
                if (entry.Value == state)
                {
                    return entry.Key;
                }
            }

            return AIMovementState.Idle;
        }

        public void AddState(AIMovementState state)
        {
            
        }
    }
    
}
