using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackingState
{
    // BasicAttack
    Basic_Idle,
    Basic_Attack,
    // Abilities
    Ability_Idle,
}

namespace _Scripts._Game.Player.AttackingStateMachine{
    
    public class PlayerAttackingStateMachineFactory
    {
        PlayerAttackingStateMachine _attackStateMachine;
        Dictionary<AttackingState, BaseAttackingState> _basicAttackStateDict = new Dictionary<AttackingState, BaseAttackingState>();
        Dictionary<AttackingState, BaseAttackingState> _abilityStateDict = new Dictionary<AttackingState, BaseAttackingState>();

        public PlayerAttackingStateMachineFactory(PlayerAttackingStateMachine sm)
        {
            _basicAttackStateDict.Add(AttackingState.Basic_Idle, new BasicAttackIdleState(sm, this));
            _basicAttackStateDict.Add(AttackingState.Basic_Attack, new BasicAttackAttackingState(sm, this));

            _attackStateMachine = sm;
        }

        public BaseAttackingState GetState(AttackingState state)
        {
            if ((int)state > (int)AttackingState.Basic_Attack)
            {
                return _abilityStateDict[state];
            }
            else
            {
                return _basicAttackStateDict[state];
            }
        }

        public AttackingState GetMovementStateEnum(BaseAttackingState state)
        {
            if (state is BasicAttackIdleState || state is BasicAttackAttackingState)
            {
                foreach (KeyValuePair<AttackingState, BaseAttackingState> entry in _basicAttackStateDict)
                {
                    if (entry.Value == state)
                    {
                        return entry.Key;
                    }
                }

                return AttackingState.Basic_Idle;
            }
            else
            {
                foreach (KeyValuePair<AttackingState, BaseAttackingState> entry in _abilityStateDict)
                {
                    if (entry.Value == state)
                    {
                        return entry.Key;
                    }
                }

                return AttackingState.Ability_Idle;
            }
        }
    }
    
}
