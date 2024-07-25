using _Scripts._Game.AI.MovementStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.AI.AttackStateMachine{
    
    public abstract class BaseAIAttackState
    {
        protected AIAttackStateMachineBase _ctx;
        protected AIAttackStateMachineFactory _factory;

        protected float _stateTimer = 0.0f;

        public BaseAIAttackState(AIAttackStateMachineBase ctx, AIAttackStateMachineFactory factory)
        {
            _ctx = ctx;
            _factory = factory;
        }

        public abstract void InitialiseState();

        public abstract void EnterState();

        public abstract void ManagedStateTick();

        public abstract bool CheckSwitchStates();

        public abstract void ExitState();

        void UpdateStates() { }

        protected void SwitchStates(BaseAIAttackState newState)
        {
            _ctx.PreviousState = this;

            ExitState();

            newState.EnterState();

            _ctx.CurrentState = newState;
        }
    }
    
}
