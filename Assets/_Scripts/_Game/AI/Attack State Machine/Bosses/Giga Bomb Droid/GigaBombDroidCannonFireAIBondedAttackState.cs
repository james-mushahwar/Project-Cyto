using UnityEngine;
using _Scripts._Game.AI.MovementStateMachine;
using _Scripts._Game.General;
using _Scripts._Game.AI.Entity.Bosses.GigaBombDroid;

namespace _Scripts._Game.AI.AttackStateMachine.Bosses.GigaBombDroid{
    
    public class GigaBombDroidCannonFireAIBondedAttackState : BossBaseAIBondedAttackState
    {
        protected GigaBombDroidAIAttackStateMachine _gbdCtx;
        protected GigaBombDroidAIEntity _gbdEntity;

        private bool _fired;

        public GigaBombDroidCannonFireAIBondedAttackState(AIAttackStateMachineBase ctx, AIAttackStateMachineFactory factory) : base(ctx, factory)
        {
            _gbdCtx = ctx.GetStateMachine<GigaBombDroidAIAttackStateMachine>();
            _gbdEntity = ctx.Entity as GigaBombDroidAIEntity;
        }

        public override bool CheckSwitchStates()
        {
            if (_fired)
            {
                SwitchStates(_factory.GetBondedState(AIAttackState.Idle));
                return true;
            }

            return false;
        }

        public override void EnterState()
        {
            _stateTimer = 0.0f;
            _fired = false;
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
                if (!_fired)
                {
                    _fired = true;
                    _gbdCtx.FireCannons();
                    return;
                }
            }
        }

    }
    
}
