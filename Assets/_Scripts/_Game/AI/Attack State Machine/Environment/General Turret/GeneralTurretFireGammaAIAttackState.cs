using UnityEngine;
using _Scripts._Game.AI.AttackStateMachine;

namespace _Scripts._Game.AI.AttackStateMachine.Environment.GeneralTurret{
    
    public class GeneralTurretFireGammaAIAttackState : BaseAIAttackState
    {
        public GeneralTurretFireGammaAIAttackState(AIAttackStateMachineBase ctx, AIAttackStateMachineFactory factory) : base(ctx, factory)
        {
    
        }
    
        public override bool CheckSwitchStates()
        {
            return false;
        }
    
        public override void EnterState()
        {
            
        }
    
        public override void ExitState()
        {
            
        }
    
        public override void InitialiseState()
        {
            
        }
    
        public override void ManagedStateTick()
        {
            
        }
    }
    
}
