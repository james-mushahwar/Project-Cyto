using _Scripts._Game.General.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.AI.MovementStateMachine.Flying.Bombdroid{
    
    public class BombDroidIdleAIMovementState : BaseAIMovementState
    {
        private BombDroidAIMovementStateMachine _bdCtx;

        public BombDroidIdleAIMovementState(AIMovementStateMachineBase ctx, AIMovementStateMachineFactory factory) : base(ctx, factory)
        {
            _bdCtx = ctx as BombDroidAIMovementStateMachine;
            UsesAIPathfinding = true;
        }

        public override bool CheckSwitchStates()
        {
            // debug settings
            if (DebugManager.Instance.DebugSettings.AIFreezeMovement)
            {
                return false;
            }

            // is exposed - freeze
            if (_ctx.Entity.IsExposed())
            {
                return false;
            }

            // being bonded - freeze
            if ((AIEntity)TargetManager.Instance.LockedBondableTarget == _ctx.Entity)
            {
                return false;
            }

            if (_stateTimer >= 2.0f)
            {
                SwitchStates(_factory.GetState(AIMovementState.Patrol));
                return true;
            }

            return false;
        }

        public override void EnterState()
        {
            //Debug.Log("Hello I'm a bomb droid in chase");

            _bdCtx.Seeker.enabled = false;
            _bdCtx.DestinationSetter.enabled = false;
            _bdCtx.AIPath.enabled = false;
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
            }
        }
    }

}
