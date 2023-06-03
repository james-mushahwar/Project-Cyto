using _Scripts._Game.Player.MovementStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementState
{
    NONE,
    Grounded,
    Jumping,
    Falling,
    Dashing,
    Bouncing,
    Floating,
    Bonding,
    Phasing,
}

public class PlayerMovementStateMachineFactory
{
    PlayerMovementStateMachine _moveStateMachine;
    Dictionary<MovementState, BaseMovementState> _stateDict = new Dictionary<MovementState, BaseMovementState>();

    public PlayerMovementStateMachineFactory(PlayerMovementStateMachine sm)
    {
        _stateDict.Add(MovementState.Grounded, new GroundedMovementState(sm, this));
        _stateDict.Add(MovementState.Jumping, new JumpingMovementState(sm, this));
        _stateDict.Add(MovementState.Falling, new FallingMovementState(sm, this));
        _stateDict.Add(MovementState.Dashing, new DashingMovementState(sm, this));
        _stateDict.Add(MovementState.Bouncing, new BouncingMovementState(sm, this));
        _stateDict.Add(MovementState.Floating, new FloatingMovementState(sm, this));
        _stateDict.Add(MovementState.Bonding, new BondingMovementState(sm, this));
        _stateDict.Add(MovementState.Phasing, new PhasingMovementState(sm, this));

        _moveStateMachine = sm;
    }

    public BaseMovementState GetState(MovementState state)
    {
        return _stateDict[state];
    }

    public MovementState GetMovementStateEnum(BaseMovementState state)
    {
        foreach (KeyValuePair<MovementState, BaseMovementState> entry in _stateDict)
        {
            if (entry.Value == state)
            {
                return entry.Key;
            }    
        }

        return MovementState.Grounded;
    }
}
