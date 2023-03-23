using _Scripts._Game.General.Managers;
using _Scripts._Game.Player;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.AI.MovementStateMachine.Flying.Bombdroid{
    
    public class BombDroidChaseAIMovementState : BaseAIMovementState
    {
        private BombDroidAIMovementStateMachine _bdCtx;
        private Transform _targetTransform;

        public BombDroidChaseAIMovementState(AIMovementStateMachineBase ctx, AIMovementStateMachineFactory factory) : base(ctx, factory)
        {
            _bdCtx = ctx as BombDroidAIMovementStateMachine;
            UsesAIPathfinding = true;
        }

        public override bool CheckSwitchStates()
        {
            GameObject target = PlayerEntity.Instance.GetControlledGameObject();

            Vector3 differenceToTarget = target.transform.position - _bdCtx.transform.position;
            float distance = differenceToTarget.sqrMagnitude;

            if (distance > _bdCtx.ChaseLostDetectionSqRange)
            {
                SwitchStates(_factory.GetState(AIMovementState.Patrol));
                return true;
            }

            return false;
        }

        public override void EnterState()
        {
            Debug.Log("Hello I'm a bomb droid in Chase!");
            _bdCtx.Seeker.enabled = true;
            _bdCtx.DestinationSetter.enabled = true;
            _bdCtx.AIPath.enabled = true;

            _bdCtx.AIPath.autoRepath.mode = AutoRepathPolicy.Mode.Dynamic; // this should auto update path 

            _targetTransform = TargetManager.Instance.GetTargetTypeTransform(ETargetType.AbovePlayer);
            if (_targetTransform != null)
            {
                _bdCtx.Seeker.StartPath(_bdCtx.Rb.position, _targetTransform.position);
                _bdCtx.DestinationSetter.target = _targetTransform;
            }
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
                //if (_bdCtx.AIPath.reachedEndOfPath)
                //{
                //    // set new random waypoint as target
                //    Transform waypoint = TargetManager.Instance.GetTargetTypeTransform(ETargetType.AbovePlayer);
                //    if (waypoint != null)
                //    {
                //        _bdCtx.Seeker.StartPath(_bdCtx.Rb.position, waypoint.position);
                //        _bdCtx.DestinationSetter.target = waypoint;
                //    }
                //}
            }
        }
    }
    
}
