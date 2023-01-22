using _Scripts._Game.AI.AttackStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.AI.MovementStateMachine{

    // This class can be used as a basic attack movement class for every AI
    public class AttackAIMovementState : BaseAIMovementState
    {
        private AIAttackStateMachineBase _attackCtx;

        public AttackAIMovementState(AIMovementStateMachineBase ctx, AIMovementStateMachineFactory factory) : base(ctx, factory)
        {
            _attackCtx = ctx.Entity.AttackSM;
        }

        public override bool CheckSwitchStates()
        {
            AIAttackState attackState = _attackCtx.States.GetAttackStateEnum(_attackCtx.CurrentState);
            if (attackState != AIAttackState.Attack1 && attackState != AIAttackState.Attack2 && attackState != AIAttackState.Attack3)
            {
                AIMovementState previousState = _factory.GetMovementStateEnum(_ctx.PreviousState);
                SwitchStates(_factory.GetState(previousState));
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
                // do nothing
            }
        }
    }

}
