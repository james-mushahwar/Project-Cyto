using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.AI.MovementStateMachine.Flying.Bombdroid{
    
    public class BombDroidChaseAIMovementState : BaseAIMovementState
    {
        private BombDroidAIMovementStateMachine _bombDroidAIMovementSM;

        public BombDroidChaseAIMovementState(AIMovementStateMachineBase ctx, AIMovementStateMachineFactory factory) : base(ctx, factory)
        {
            _bombDroidAIMovementSM = ctx as BombDroidAIMovementStateMachine;
        }

        public override bool CheckSwitchStates()
        {
            throw new System.NotImplementedException();
        }

        public override void EnterState()
        {
            Debug.Log("Hello I'm a bomb droid in Chase!");
        }

        public override void ExitState()
        {
            throw new System.NotImplementedException();
        }

        public override void InitialiseState()
        {
            throw new System.NotImplementedException();
        }

        public override void ManagedStateTick()
        {
            throw new System.NotImplementedException();
        }
    }
    
}
