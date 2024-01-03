using UnityEngine;
using _Scripts._Game.AI.MovementStateMachine;
using _Scripts._Game.General.Managers;
using _Scripts._Game.Player;
using _Scripts.CautionaryTalesScripts;
using _Scripts._Game.AI.Entity.Ground.MushroomArcher;

namespace _Scripts._Game.AI.MovementStateMachine.Ground.MushroomArcher{
    
    public class MushroomArcherPatrolAIMovementState : BaseAIMovementState
    {
        private MushroomArcherAIMovementStateMachine _maCtx;
        private MushroomArcherAIEntity _maEntity;

        public MushroomArcherPatrolAIMovementState(AIMovementStateMachineBase ctx, AIMovementStateMachineFactory factory) : base(ctx, factory)
        {
            _maCtx = ctx.GetStateMachine<MushroomArcherAIMovementStateMachine>();
            _maEntity = ctx.Entity as MushroomArcherAIEntity;
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
        }
    
        public override void ExitState()
        {
            
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
