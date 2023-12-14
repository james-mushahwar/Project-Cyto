using _Scripts._Game.Player;
using System.Collections;
using System.Collections.Generic;
using _Scripts._Game.General;
using UnityEngine;

namespace _Scripts._Game.AI.AttackStateMachine.Flying.Bombdroid{
    
    public class BombDroidIdleAIBondedAttackState : BaseAIBondedAttackState
    {
        private BombDroidAIAttackStateMachine _bdCtx;

        public BombDroidIdleAIBondedAttackState(AIAttackStateMachineBase ctx, AIAttackStateMachineFactory factory) : base(ctx, factory)
        {
            _bdCtx = ctx.GetStateMachine<BombDroidAIAttackStateMachine>();
        }

        public override bool CheckSwitchStates()
        {
            if (_stateTimer >= _bdCtx.BondedBombDropCooldown)
            {
                if (_ctx.IsWestButtonPressed == true && _ctx.IsWestInputValid == true)
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
