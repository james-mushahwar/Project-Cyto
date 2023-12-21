using _Scripts._Game.AI.MovementStateMachine.Flying;
using _Scripts._Game.AI.MovementStateMachine.Flying.Bombdroid;
using System.Collections;
using System.Collections.Generic;
using _Scripts._Game.AI.MovementStateMachine.Ground.MushroomArcher;
using UnityEngine;

namespace _Scripts._Game.AI.MovementStateMachine{

    public enum AIMovementState
    {
        Sleep,
        Wake,
        Idle,
        Patrol,
        Chase,
        Attack,
        Dead
    }

    public enum AIBondedMovementState
    {
        Grounded,
        Jumping,
        Falling, 
        Flying,
        Dashing,
        Attacking
    }

    public class AIMovementStateMachineFactory
    {
        private AIMovementStateMachineBase _moveStateMachine;
        private Dictionary<AIMovementState, BaseAIMovementState> _stateDict = new Dictionary<AIMovementState, BaseAIMovementState>();
        private Dictionary<AIBondedMovementState, BaseAIBondedMovementState> _bondedStateDict = new Dictionary<AIBondedMovementState, BaseAIBondedMovementState>();

        public AIMovementStateMachineFactory(AIMovementStateMachineBase sm)
        {
            _moveStateMachine = sm;

            //BombDroidAIMovementStateMachine bombDroid = _moveStateMachine as BombDroidAIMovementStateMachine;

            //if (bombDroid)
            //{
            //    _stateDict.Add(AIMovementState.Idle,    new BombDroidIdleAIMovementState(sm, this));
            //    _stateDict.Add(AIMovementState.Patrol,  new BombDroidPatrolAIMovementState(sm, this));
            //    _stateDict.Add(AIMovementState.Chase,   new BombDroidChaseAIMovementState(sm, this));
            //    _stateDict.Add(AIMovementState.Attack,  new AttackAIMovementState(sm, this));

            //    _bondedStateDict.Add(AIBondedMovementState.Flying, new BombDroidFlyingAIBondedMovementState(sm, this));
            //}

            //MushroomArcherAIMovementStateMachine mushroomArcher = _moveStateMachine as MushroomArcherAIMovementStateMachine;

        }

        public void AddState(AIMovementState state, BaseAIMovementState movementState)
        {
            _stateDict.Add(state, movementState);
        }

        public void AddState(AIBondedMovementState state, BaseAIBondedMovementState bondedMovementState)
        {
            _bondedStateDict.Add(state, bondedMovementState);
        }

        public BaseAIMovementState GetState(AIMovementState state)
        {
            return _stateDict[state];
        }

        public AIMovementState GetMovementStateEnum(BaseAIMovementState state)
        {
            foreach (KeyValuePair<AIMovementState, BaseAIMovementState> entry in _stateDict)
            {
                if (entry.Value == state)
                {
                    return entry.Key;
                }
            }

            return AIMovementState.Idle;
        }

        public BaseAIBondedMovementState GetState(AIBondedMovementState state)
        {
            return _bondedStateDict[state];
        }

        public AIBondedMovementState GetMovementStateEnum(BaseAIBondedMovementState state)
        {
            foreach (KeyValuePair<AIBondedMovementState, BaseAIBondedMovementState> entry in _bondedStateDict)
            {
                if (entry.Value == state)
                {
                    return entry.Key;
                }
            }

            return AIBondedMovementState.Grounded;
        }
    }
}
