public abstract class BaseMovementState
{
    protected PlayerMovementStateMachine _ctx;
    protected PlayerMovementStateMachineFactory _factory;

    protected float _stateTimer = 0.0f;

    public BaseMovementState(PlayerMovementStateMachine ctx, PlayerMovementStateMachineFactory factory)
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

    protected void SwitchStates(BaseMovementState newState) 
    {
        ExitState();

        newState.EnterState();

        _ctx.CurrentState = newState;
    }

    void SetSuperState() { }

    void SetSubState() { }

}
