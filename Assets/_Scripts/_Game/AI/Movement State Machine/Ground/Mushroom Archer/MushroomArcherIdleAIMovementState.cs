using UnityEngine;
using _Scripts._Game.AI.MovementStateMachine;
using _Scripts._Game.AI.Entity.Ground.MushroomArcher;
using _Scripts._Game.General.Managers;

namespace _Scripts._Game.AI.MovementStateMachine.Ground.MushroomArcher{
    
    public class MushroomArcherIdleAIMovementState : BaseAIMovementState
    {
        private MushroomArcherAIMovementStateMachine _maCtx;
        private MushroomArcherAIEntity _maEntity;

        public MushroomArcherIdleAIMovementState(AIMovementStateMachineBase ctx, AIMovementStateMachineFactory factory) : base(ctx, factory)
        {
            _maCtx = ctx.GetStateMachine<MushroomArcherAIMovementStateMachine>();
            _maEntity = ctx.Entity as MushroomArcherAIEntity;
        }
    
        public override bool CheckSwitchStates()
        {
            if (DebugManager.Instance.DebugSettings.AIFreezeMovement)
            {
                return false;
            }

            if (_ctx.Entity.IsExposed())
            {
                return false;
            }

            // being bonded - freeze
            if ((AIEntity)TargetManager.Instance.LockedBondableTarget == _ctx.Entity)
            {
                return false;
            }

            if (_stateTimer >= 1.0f)
            {
                SwitchStates(_factory.GetState(AIMovementState.Patrol));
                return true;
            }

            return false;
        }
    
        public override void EnterState()
        {
            _stateTimer = 0.0f;

            _maCtx.Seeker.enabled = false;
            _maCtx.DestinationSetter.enabled = false;
            _maCtx.AIPath.enabled = false;
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
                // do nothing
            }
        }
        
    }
    
}
