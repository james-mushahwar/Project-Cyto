using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackingState
{
    Idle,
    BasicAttack,
}

namespace _Scripts._Game.Player.AttackingStateMachine{
    
    public class PlayerAttackingStateMachineFactory
    {
        PlayerAttackingStateMachine _attackStateMachine;
        Dictionary<AttackingState, BaseAttackingState> _stateDict = new Dictionary<AttackingState, BaseAttackingState>();

        public PlayerAttackingStateMachineFactory(PlayerAttackingStateMachine sm)
        {
            _stateDict.Add(AttackingState.Idle, new IdleAttackingState(sm, this));
            _stateDict.Add(AttackingState.BasicAttack, new BasicAttackingState(sm, this));

            _attackStateMachine = sm;
        }

        public BaseAttackingState GetState(AttackingState state)
        {
            return _stateDict[state];
        }

        public AttackingState GetMovementStateEnum(BaseAttackingState state)
        {
            foreach (KeyValuePair<AttackingState, BaseAttackingState> entry in _stateDict)
            {
                if (entry.Value == state)
                {
                    return entry.Key;
                }
            }

            return AttackingState.Idle;
        }
    }
    
}
