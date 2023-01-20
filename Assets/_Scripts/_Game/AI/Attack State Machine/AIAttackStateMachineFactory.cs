using _Scripts._Game.AI.MovementStateMachine;
using _Scripts._Game.AI.MovementStateMachine.Flying.Bombdroid;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.AI.AttackStateMachine{

    public enum AIAttackState
    {
        Nothing,
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

            //BombDroidAIMovementStateMachine bombDroid = _attackStateMachine as BombDroidAIMovementStateMachine;

            //if (bombDroid)
            //{
            //    _stateDict.Add(AIMovementState.Idle, new BombDroidIdleAIMovementState(sm, this));
            //    _stateDict.Add(AIMovementState.Patrol, new BombDroidPatrolAIMovementState(sm, this));
            //    _stateDict.Add(AIMovementState.Chase, new BombDroidChaseAIMovementState(sm, this));

            //    _bondedStateDict.Add(AIBondedMovementState.Flying, new BombDroidFlyingAIBondedMovementState(sm, this));
            //}
        }

        public BaseAIAttackState GetState(AIAttackState state)
        {
            return _stateDict[state];
        }

        public AIAttackState GetMovementStateEnum(BaseAIAttackState state)
        {
            foreach (KeyValuePair<AIAttackState, BaseAIAttackState> entry in _stateDict)
            {
                if (entry.Value == state)
                {
                    return entry.Key;
                }
            }

            return AIAttackState.Nothing;
        }
    }
    
}
