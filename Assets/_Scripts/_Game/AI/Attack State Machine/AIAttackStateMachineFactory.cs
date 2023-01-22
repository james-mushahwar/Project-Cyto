using _Scripts._Game.AI.AttackStateMachine.Flying.Bombdroid;
using _Scripts._Game.AI.MovementStateMachine;
using _Scripts._Game.AI.MovementStateMachine.Flying.Bombdroid;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.AI.AttackStateMachine{

    public enum AIAttackState
    {
        NOTHING, // unconnected state
        Idle,
        Attack1,
        Attack2,
        Attack3,
    }

    public class AIAttackStateMachineFactory
    {
        private AIAttackStateMachineBase _attackStateMachine;
        private Dictionary<AIAttackState, BaseAIAttackState> _stateDict = new Dictionary<AIAttackState, BaseAIAttackState>();

        public AIAttackStateMachineFactory(AIAttackStateMachineBase sm)
        {
            _attackStateMachine = sm;
            _stateDict.Add(AIAttackState.NOTHING, new BaseAINothingAttackState(sm, this));

            BombDroidAIAttackStateMachine bombDroid = _attackStateMachine as BombDroidAIAttackStateMachine;

            if (bombDroid)
            {
                _stateDict.Add(AIAttackState.Idle, new BomdDroidIdleAIAttackState(sm, this));
                _stateDict.Add(AIAttackState.Attack1, new BombDroidBombDropAIAttackState(sm, this));
   
                //_bondedStateDict.Add(AIBondedMovementState.Flying, new BombDroidFlyingAIBondedMovementState(sm, this));
            }
        }

        public BaseAIAttackState GetState(AIAttackState state)
        {
            return _stateDict[state];
        }

        public AIAttackState GetAttackStateEnum(BaseAIAttackState state)
        {
            foreach (KeyValuePair<AIAttackState, BaseAIAttackState> entry in _stateDict)
            {
                if (entry.Value == state)
                {
                    return entry.Key;
                }
            }

            return AIAttackState.NOTHING;
        }
    }
    
}
