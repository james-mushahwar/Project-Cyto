using UnityEngine;
using _Scripts._Game.AI.MovementStateMachine;
using _Scripts._Game.General.Managers;
using _Scripts._Game.Player;
using _Scripts.CautionaryTalesScripts;
using _Scripts._Game.AI.Entity.Ground.MushroomArcher;
using Pathfinding;

namespace _Scripts._Game.AI.MovementStateMachine.Ground.MushroomArcher{
    
    public class MushroomArcherPatrolAIMovementState : BaseAIMovementState
    {
        private MushroomArcherAIMovementStateMachine _maCtx;
        private MushroomArcherAIEntity _maEntity;

        private float _waitTimer;

        public MushroomArcherPatrolAIMovementState(AIMovementStateMachineBase ctx, AIMovementStateMachineFactory factory) : base(ctx, factory)
        {
            _maCtx = ctx.GetStateMachine<MushroomArcherAIMovementStateMachine>();
            _maEntity = ctx.Entity.GetEntity<MushroomArcherAIEntity>();
        }
    
        public override bool CheckSwitchStates()
        {
            // debug settings
            if (DebugManager.Instance.DebugSettings.AIFreezeMovement)
            {
                SwitchStates(_factory.GetState(AIMovementState.Idle));
                return true;
            }

            GameObject target = PlayerEntity.Instance?.GetControlledGameObject();
            if (CTGlobal.IsInSqDistanceRange(target, _maCtx.gameObject, _maCtx.ChaseDetectionSqRange))
            {
                SwitchStates(_factory.GetState(AIMovementState.Chase));
                return true;
            }

            return false;
        }
    
        public override void EnterState()
        {
            _stateTimer = 0.0f;

            _maCtx.Seeker.enabled = true;
            _maCtx.DestinationSetter.enabled = true;
            _maCtx.AIPath.enabled = true;

            _maCtx.AIPath.autoRepath.mode = AutoRepathPolicy.Mode.Never;

            Transform waypoint = _maCtx.Waypoints!.GetWaypoint();
            _maCtx.Seeker.StartPath(_maCtx.Rb.position, waypoint.position);
            _maCtx.DestinationSetter.target = waypoint;
        }
    
        public override void ExitState()
        {
            _maCtx.Seeker.enabled = false;
            _maCtx.DestinationSetter.enabled = false;
            _maCtx.AIPath.enabled = false;
        }
    
        public override void InitialiseState()
        {
            
        }
    
        public override void ManagedStateTick()
        {
            _stateTimer += Time.deltaTime;

            if (CheckSwitchStates() == false)
            {
                if (_maCtx.AIPath.reachedEndOfPath)
                {
                    _waitTimer -= Time.deltaTime;
                    if (_waitTimer <= 0.0f)
                    {
                        // set new random waypoint as target
                        Transform waypoint = _maCtx.Waypoints!.GetWaypoint();
                        _maCtx.Seeker.StartPath(_maCtx.Rb.position, waypoint.position);
                        _maCtx.DestinationSetter.target = waypoint;
                        // new wait time for next time
                        _waitTimer = Random.Range(_maCtx.PatrolWaitTimeRange.x, _maCtx.PatrolWaitTimeRange.y);
                    }
                }
            }
        }
        
    }
    
}
