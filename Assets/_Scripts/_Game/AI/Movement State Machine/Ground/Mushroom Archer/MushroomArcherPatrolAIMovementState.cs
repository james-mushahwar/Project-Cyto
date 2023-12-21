using UnityEngine;
using _Scripts._Game.AI.MovementStateMachine;
using _Scripts._Game.General.Managers;

namespace _Scripts._Game.AI.MovementStateMachine.Ground.MushroomArcher{
    
    public class MushroomArcherPatrolAIMovementState : BaseAIMovementState
    {
        public MushroomArcherPatrolAIMovementState(AIMovementStateMachineBase ctx, AIMovementStateMachineFactory factory) : base(ctx, factory)
        {
    
        }
    
        public override bool CheckSwitchStates()
        {
            // debug settings
            if (DebugManager.Instance.DebugSettings.AIFreezeMovement)
            {
                SwitchStates(_factory.GetState(AIMovementState.Idle));
                return true;
            }

            return false;
        }
    
        public override void EnterState()
        {
            _stateTimer = 0.0f;
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
                
            }
        }
        
    }
    
}
