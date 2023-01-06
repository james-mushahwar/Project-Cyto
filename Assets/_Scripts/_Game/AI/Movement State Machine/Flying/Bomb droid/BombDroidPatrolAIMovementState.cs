using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.AI.MovementStateMachine.Flying.Bombdroid{
    
    public class BombDroidPatrolAIMovementState : BaseAIMovementState
    {
        private BombDroidAIMovementStateMachine _bombDroidAIMovementSM;

        private float _waitTimer;

        public BombDroidPatrolAIMovementState(AIMovementStateMachineBase ctx, AIMovementStateMachineFactory factory) : base(ctx, factory)
        {
            _bombDroidAIMovementSM = ctx as BombDroidAIMovementStateMachine;
        }

        public override bool CheckSwitchStates()
        {
            

            return false;
        }

        public override void EnterState()
        {
            Debug.Log("Hello I'm a bomb droid in Patrol");
            _waitTimer = Random.Range(_bombDroidAIMovementSM.PatrolWaitTimeRange.x, _bombDroidAIMovementSM.PatrolWaitTimeRange.y);

            _bombDroidAIMovementSM.Seeker.enabled = true;
            _bombDroidAIMovementSM.DestinationSetter.enabled = true;
            _bombDroidAIMovementSM.AIPath.enabled = true;

            Transform waypoint = _bombDroidAIMovementSM.Waypoints.GetWaypoint(Random.Range(0, 4));
            _bombDroidAIMovementSM.Seeker.StartPath(_bombDroidAIMovementSM.Rb.position, waypoint.position);
            _bombDroidAIMovementSM.DestinationSetter.target = waypoint;
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
            _stateTimer += Time.deltaTime;

            if (CheckSwitchStates() == false)
            {
                // do nothing :) 
                if (_bombDroidAIMovementSM.AIPath.reachedEndOfPath)
                {
                    _waitTimer -= Time.deltaTime;
                    if (_waitTimer <= 0.0f)
                    {
                        // set new random waypoint as target
                        Transform waypoint = _bombDroidAIMovementSM.Waypoints.GetWaypoint(Random.Range(0, 4));
                        _bombDroidAIMovementSM.Seeker.StartPath(_bombDroidAIMovementSM.Rb.position, waypoint.position);
                        _bombDroidAIMovementSM.DestinationSetter.target = waypoint;
                        // new wait time for next time
                        _waitTimer = Random.Range(_bombDroidAIMovementSM.PatrolWaitTimeRange.x, _bombDroidAIMovementSM.PatrolWaitTimeRange.y);
                    }
                }
            }
        }
    }
    
}
