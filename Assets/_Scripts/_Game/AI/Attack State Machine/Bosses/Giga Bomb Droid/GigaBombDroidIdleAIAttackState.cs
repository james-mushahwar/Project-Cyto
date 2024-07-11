using UnityEngine;
using _Scripts._Game.AI.AttackStateMachine;
using _Scripts._Game.AI.MovementStateMachine;

namespace _Scripts._Game.AI.AttackStateMachine.Bosses.GigaBombDroid{
    
    public class GigaBombDroidIdleAIAttackState : GigaBombDroidBaseAIAttackState
    {
        public GigaBombDroidIdleAIAttackState(AIAttackStateMachineBase ctx, AIAttackStateMachineFactory factory) : base(ctx, factory)
        {
    
        }
    
        public override bool CheckSwitchStates()
        {
            float idleToAttackDelay = _gbdCtx.IdleToShootDelay;

            if (_stateTimer >= idleToAttackDelay)
            {
                SwitchStates(_factory.GetState(AIAttackState.Attack1));
                return true;
            }

            return false;
        }
    
        public override void EnterState()
        {
            _stateTimer = 0;
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
                // do nothing
            }
        }
    }
    
}
