using UnityEngine;
using _Scripts._Game.AI.MovementStateMachine;
using _Scripts._Game.General;
using _Scripts._Game.AI.Entity.Bosses.GigaBombDroid;

namespace _Scripts._Game.AI.AttackStateMachine.Bosses.GigaBombDroid{
    
    public class GigaBombDroidIdleAIBondedAttackState : BossBaseAIBondedAttackState
    {
        protected GigaBombDroidAIAttackStateMachine _gbdCtx;
        protected GigaBombDroidAIEntity _gbdEntity;

        protected float _attack1CooldownTimer;
        protected float _attack2CooldownTimer;

        public GigaBombDroidIdleAIBondedAttackState(AIAttackStateMachineBase ctx, AIAttackStateMachineFactory factory) : base(ctx, factory)
        {
            _gbdCtx = ctx.GetStateMachine<GigaBombDroidAIAttackStateMachine>();
            _gbdEntity = ctx.Entity as GigaBombDroidAIEntity;
        }

        public override bool CheckSwitchStates()
        {
            if (_attack1CooldownTimer > 0.0f)
            {
                _attack1CooldownTimer -= Time.deltaTime;
                _attack1CooldownTimer = Mathf.Max(0.0f, _attack1CooldownTimer);
            }

            bool attack1CooldownComplete = _attack1CooldownTimer == 0.0f;

            if (attack1CooldownComplete)
            {
                if (_ctx.IsWestButtonPressed == true)
                {
                    _ctx.NullifyInput(PossessInput.WButton);
                    SwitchStates(_factory.GetBondedState(AIAttackState.Attack1));
                    return true;
                }
            }

            return false;
        }

        public override void EnterState()
        {
            bool fromBombDropAttackState = AIAttackState.Attack1 == _factory.GetBondedAttackStateEnum(_ctx.PreviousBondedState);
            _attack1CooldownTimer = fromBombDropAttackState ? _gbdCtx.PostBombFireCooldownDuration : 0.0f;
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
                // do nothing
            }
        }

    }
    
}
