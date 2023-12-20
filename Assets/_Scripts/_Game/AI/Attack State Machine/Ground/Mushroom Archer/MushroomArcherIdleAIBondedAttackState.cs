using UnityEngine;
using _Scripts._Game.AI.MovementStateMachine;

namespace _Scripts._Game.AI.AttackStateMachine.Ground.MushroomArcher {
    
    public class MushroomArcherIdleAIBondedAttackState : BaseAIBondedAttackState
    {
        public MushroomArcherIdleAIBondedAttackState(AIAttackStateMachineBase ctx, AIAttackStateMachineFactory factory) : base(ctx, factory)
        {
    
        }
    
        public override bool CheckSwitchStates()
        {
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
