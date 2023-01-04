using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.AI.MovementStateMachine.Flying.Bombdroid{
    
    public class BombDroidPatrolAIMovementState : BaseAIMovementState
    {
        private BombDroidAIMovementStateMachine _BombDroidAIMovementSM;

        public BombDroidPatrolAIMovementState(AIMovementStateMachineBase ctx, AIMovementStateMachineFactory factory) : base(ctx, factory)
        {
            _BombDroidAIMovementSM = ctx as BombDroidAIMovementStateMachine;
        }

        public override bool CheckSwitchStates()
        {
            return false;
        }

        public override void EnterState()
        {
            _BombDroidAIMovementSM.Seeker.StartPath(_BombDroidAIMovementSM.Rb.position, _BombDroidAIMovementSM.Waypoints.GetWaypoint(Random.Range(0, 3)).position);
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
