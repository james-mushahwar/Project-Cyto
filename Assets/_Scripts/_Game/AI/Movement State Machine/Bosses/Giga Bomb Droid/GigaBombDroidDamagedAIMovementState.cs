using UnityEngine;
using _Scripts._Game.AI.MovementStateMachine;
using _Scripts._Game.Player;
using _Scripts.CautionaryTalesScripts;
using _Scripts._Game.General.Managers;

namespace _Scripts._Game.AI.MovementStateMachine.Bosses.GigaBombDroid{
    
    public class GigaBombDroidDamagedAIMovementState : GigaBombDroidBaseAIMovementState
    {
        public GigaBombDroidDamagedAIMovementState(AIMovementStateMachineBase ctx, AIMovementStateMachineFactory factory) : base(ctx, factory)
        {
            
        }
    
        public override bool CheckSwitchStates()
        {
            if (_stateTimer >= _gbdCtx.DamagedStateDuration)
            {
                SwitchStates(_factory.GetState(AIMovementState.Patrol));
                return true;
            }

            return false;
        }
    
        public override void EnterState()
        {
            _stateTimer = 0.0f;

            _gbdCtx.Seeker.enabled = true;
            _gbdCtx.DestinationSetter.enabled = true;
            _gbdCtx.AIPath.enabled = true;
        }
    
        public override void ExitState()
        {
            _gbdCtx.Seeker.enabled = false;
            _gbdCtx.DestinationSetter.enabled = false;
            _gbdCtx.AIPath.enabled = false;
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
