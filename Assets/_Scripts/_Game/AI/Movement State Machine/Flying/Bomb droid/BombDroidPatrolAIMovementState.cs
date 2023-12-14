using _Scripts._Game.General;
using _Scripts._Game.General.Managers;
using _Scripts._Game.Player;
using _Scripts._Game.AI.Entity.Flying;
using Pathfinding;
using UnityEngine;

namespace _Scripts._Game.AI.MovementStateMachine.Flying.Bombdroid{
    
    public class BombDroidPatrolAIMovementState : BaseAIMovementState
    {
        private BombDroidAIMovementStateMachine _bdCtx;
        private BombDroidAIEntity _bdEntity;

        private float _waitTimer;

        public BombDroidPatrolAIMovementState(AIMovementStateMachineBase ctx, AIMovementStateMachineFactory factory) : base(ctx, factory)
        {
            _bdCtx = ctx.GetStateMachine<BombDroidAIMovementStateMachine>();
            _bdEntity = ctx.Entity as BombDroidAIEntity;
            UsesAIPathfinding = true;
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
            
            Vector3 differenceToTarget = target.transform.position - _bdCtx.transform.position;
            float distance = differenceToTarget.sqrMagnitude;

            if (distance <= _bdCtx.ChaseDetectionSqRange)
            {
                SwitchStates(_factory.GetState(AIMovementState.Chase));
                return true;
            }

            return false;
        }

        public override void EnterState()
        {
            //Debug.Log("Hello I'm a bomb droid in Patrol");
            _waitTimer = Random.Range(_bdCtx.PatrolWaitTimeRange.x, _bdCtx.PatrolWaitTimeRange.y);

            _bdCtx.Seeker.enabled = true;
            _bdCtx.DestinationSetter.enabled = true;
            _bdCtx.AIPath.enabled = true;
            
            _bdCtx.AIPath.autoRepath.mode = AutoRepathPolicy.Mode.Never;

            Transform waypoint = _bdCtx.Waypoints.GetWaypoint(Random.Range(0, 4));
            _bdCtx.Seeker.StartPath(_bdCtx.Rb.position, waypoint.position);
            _bdCtx.DestinationSetter.target = waypoint;

            //audio movement
            _bdEntity.BombDroidMovementAudioHandler.VolumeAlpha = 0.5f;
            AudioManager.Instance.TryPlayAudioSourceAttached(EAudioType.SFX_Enemy_BombDroid_Movement, _bdCtx.transform, 
                _bdEntity.BombDroidMovementAudioHandler, _bdEntity.BombDroidMovementAudioHandler._position);
        }

        public override void ExitState()
        {
            _bdCtx.Seeker.enabled = false;
            _bdCtx.DestinationSetter.enabled = false;
            _bdCtx.AIPath.enabled = false;
        }

        public override void InitialiseState()
        {
            
        }

        public override void ManagedStateTick()
        {
            _stateTimer += Time.deltaTime;

            if (CheckSwitchStates() == false)
            {
                // do nothing :) 
                if (_bdCtx.AIPath.reachedEndOfPath)
                {
                    _waitTimer -= Time.deltaTime;
                    if (_waitTimer <= 0.0f)
                    {
                        // set new random waypoint as target
                        Transform waypoint = _bdCtx.Waypoints.GetWaypoint(Random.Range(0, 4));
                        _bdCtx.Seeker.StartPath(_bdCtx.Rb.position, waypoint.position);
                        _bdCtx.DestinationSetter.target = waypoint;
                        // new wait time for next time
                        _waitTimer = Random.Range(_bdCtx.PatrolWaitTimeRange.x, _bdCtx.PatrolWaitTimeRange.y);
                    }
                }
            }
        }
    }
    
}
