using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.AI.AttackStateMachine{
    
    public abstract class BaseAIBondedAttackState
    {
        protected AIAttackStateMachineBase _ctx;
        protected AIAttackStateMachineFactory _factory;

        protected float _stateTimer = 0.0f;

        public BaseAIBondedAttackState(AIAttackStateMachineBase ctx, AIAttackStateMachineFactory factory)
        {
            _ctx = ctx;
            _factory = factory;
        }

        public abstract void InitialiseState();

        public abstract void EnterState();

        public void EnterState(float timer) { }

        public abstract void ManagedStateTick();

        public abstract bool CheckSwitchStates();

        public abstract void ExitState();

        void UpdateStates() { }

        protected void SwitchStates(BaseAIBondedAttackState newState)
        {
            _ctx.PreviousBondedState = this;

            ExitState();

            newState.EnterState();

            _ctx.CurrentBondedState = newState;
        }
    }
    
}
