using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.Player.AttackingStateMachine{

    public abstract class BaseAttackingState
    {
        protected PlayerAttackingStateMachine _ctx;
        protected PlayerAttackingStateMachineFactory _factory;

        protected float _stateTimer = 0.0f;

        public BaseAttackingState(PlayerAttackingStateMachine ctx, PlayerAttackingStateMachineFactory factory)
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

        protected void SwitchStates(BaseAttackingState newState)
        {
            ExitState();

            newState.EnterState();

            _ctx.CurrentState = newState;
        }

        void SetSuperState() { }

        void SetSubState() { }

    }

}
