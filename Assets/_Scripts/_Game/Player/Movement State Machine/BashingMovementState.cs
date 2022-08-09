using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BashingMovementState : BaseMovementState
{
    public BashingMovementState(PlayerMovementStateMachine ctx, PlayerMovementStateMachineFactory factory) : base(ctx, factory)
    {
    }

    public override bool CheckSwitchStates()
    {
        return false;
    }

    public override void EnterState()
    {
        return;
    }

    public override void ExitState()
    {
        return;
    }

    public override void InitialiseState()
    {
        return;
    }

    public override void ManagedStateTick()
    {
        
    }
}
