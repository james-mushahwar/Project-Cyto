using UnityEngine;
using _Scripts._Game.AI.MovementStateMachine;
using _Scripts._Game.Player;
using _Scripts.CautionaryTalesScripts;
using _Scripts._Game.General.Managers;

namespace _Scripts._Game.AI.MovementStateMachine.Bosses.GigaBombDroid{
    
    public class GigaBombDroidPatrolAIMovementState : GigaBombDroidBaseAIMovementState
    {
        private float _waitTimer;

        public GigaBombDroidPatrolAIMovementState(AIMovementStateMachineBase ctx, AIMovementStateMachineFactory factory) : base(ctx, factory)
        {
            
        }
    
        public override bool CheckSwitchStates()
        {
            // debug settings
            if (DebugManager.Instance.DebugSettings.AIFreezeMovement)
            {
                SwitchStates(_factory.GetState(AIMovementState.Idle));
                return true;
            }

            //GameObject target = PlayerEntity.Instance?.GetControlledGameObject();
            //if (CTGlobal.IsInSqDistanceRange(target, _gbdCtx.gameObject, _gbdCtx.ChaseDetectionSqRange))
            //{
            //    SwitchStates(_factory.GetState(AIMovementState.Chase));
            //    return true;
            //}

            return false;
        }
    
        public override void EnterState()
        {
            _stateTimer = 0.0f;

            _gbdCtx.Seeker.enabled = true;
            _gbdCtx.DestinationSetter.enabled = true;
            _gbdCtx.AIPath.enabled = true;
        }
    
        public override void ExitState()
        {
            _gbdCtx.Seeker.enabled = false;
            _gbdCtx.DestinationSetter.enabled = false;
            _gbdCtx.AIPath.enabled = false;
        }
    
        public override void InitialiseState()
        {
            
        }
    
        public override void ManagedStateTick()
        {
            _stateTimer += Time.deltaTime;
    
            if (CheckSwitchStates() == false)
            {
                if (_gbdCtx.AIPath.reachedEndOfPath || _gbdCtx.AIPath.hasPath == false)
                {
                    _waitTimer -= Time.deltaTime;
                    if (_waitTimer <= 0.0f)
                    {
                        // set new random waypoint as target
                        Transform waypoint = _gbdCtx.Waypoints!.GetWaypoint();
                        _gbdCtx.Seeker.StartPath(_gbdCtx.Rb.position, waypoint.position);
                        _gbdCtx.DestinationSetter.target = waypoint;
                        // new wait time for next time
                        _waitTimer = Random.Range(_gbdCtx.PatrolWaitTimeRange.x, _gbdCtx.PatrolWaitTimeRange.y);
                    }
                }
            }
        }
        
    }
    
}
