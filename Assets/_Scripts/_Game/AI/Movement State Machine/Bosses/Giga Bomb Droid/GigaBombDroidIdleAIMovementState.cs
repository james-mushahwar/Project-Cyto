using UnityEngine;
using _Scripts._Game.AI.MovementStateMachine;
using _Scripts._Game.AI.Entity.Flying.Bombdroid;
using _Scripts._Game.AI.MovementStateMachine.Flying.Bombdroid;
using _Scripts._Game.AI.Entity.Bosses.GigaBombDroid;
using _Scripts._Game.General.Managers;

namespace _Scripts._Game.AI.MovementStateMachine.Bosses.GigaBombDroid{
    
    public class GigaBombDroidIdleAIMovementState : GigaBombDroidBaseAIMovementState
    {
        public GigaBombDroidIdleAIMovementState(AIMovementStateMachineBase ctx, AIMovementStateMachineFactory factory) : base(ctx, factory)
        {
            
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

            SwitchStates(_factory.GetState(AIMovementState.Patrol));
            return true;
        }
    
        public override void EnterState()
        {
            _stateTimer = 0.0f;

            _gbdCtx.Seeker.enabled = false;
            _gbdCtx.DestinationSetter.enabled = false;
            _gbdCtx.AIPath.enabled = false;
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
                
            }
        }
        
    }
    
}
