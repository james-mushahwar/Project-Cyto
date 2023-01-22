namespace _Scripts._Game.AI.MovementStateMachine{
    
    public abstract class BaseAIMovementState
    {
        protected AIMovementStateMachineBase _ctx;
        protected AIMovementStateMachineFactory _factory;

        protected float _stateTimer = 0.0f;

        public BaseAIMovementState(AIMovementStateMachineBase ctx, AIMovementStateMachineFactory factory)
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

        protected void SwitchStates(BaseAIMovementState newState)
        {
            _ctx.PreviousState = this;

            ExitState();

            newState.EnterState();

            _ctx.CurrentState = newState;
        }
    }
}
