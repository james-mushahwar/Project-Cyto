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
        private Dictionary<AIAttackState, BaseAIBondedAttackState> _bondedStateDict = new Dictionary<AIAttackState, BaseAIBondedAttackState>();

        public AIAttackStateMachineFactory(AIAttackStateMachineBase sm)
        {
            _attackStateMachine = sm;
            _stateDict.Add(AIAttackState.NOTHING, new BaseAINothingAttackState(sm, this));

            //BombDroidAIAttackStateMachine bombDroid = _attackStateMachine as BombDroidAIAttackStateMachine;

            //if (bombDroid)
            //{
            //    _stateDict.Add(AIAttackState.Idle, new BombDroidIdleAIAttackState(sm, this));
            //    _stateDict.Add(AIAttackState.Attack1, new BombDroidBombDropAIAttackState(sm, this));
   
            //    _bondedStateDict.Add(AIAttackState.Idle, new BombDroidIdleAIBondedAttackState(sm, this));
            //    _bondedStateDict.Add(AIAttackState.Attack1, new BombDroidBombDropAIBondedAttackState(sm, this));
            //}
        }

        public void AddState(AIAttackState state, BaseAIAttackState attackState)
        {
            _stateDict.Add(state, attackState);
        }

        public void AddState(AIAttackState state, BaseAIBondedAttackState bondedAttackState)
        {
            _bondedStateDict.Add(state, bondedAttackState);
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

        public BaseAIBondedAttackState GetBondedState(AIAttackState state)
        {
            return _bondedStateDict[state];
        }

        public AIAttackState GetBondedAttackStateEnum(BaseAIBondedAttackState state)
        {
            foreach (KeyValuePair<AIAttackState, BaseAIBondedAttackState> entry in _bondedStateDict)
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
