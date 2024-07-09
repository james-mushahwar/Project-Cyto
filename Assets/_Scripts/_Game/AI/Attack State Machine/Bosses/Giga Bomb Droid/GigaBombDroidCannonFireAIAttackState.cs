using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.AI.AttackStateMachine.Bosses.GigaBombDroid{
    
    public class GigaBombDroidCannonFireAIAttackState : GigaBombDroidBaseAIAttackState
    {
        private bool _fired;
        private float _cooldownTimer;

        public GigaBombDroidCannonFireAIAttackState(AIAttackStateMachineBase ctx, AIAttackStateMachineFactory factory) : base(ctx, factory)
        {
        }

        public override bool CheckSwitchStates()
        {
            if (_fired)
            {
                if (_cooldownTimer >= _gbdCtx.ShootCooldown)
                {
                    SwitchStates(_factory.GetState(AIAttackState.Idle));
                    return true;
                }
            }
            return false;
        }

        public override void EnterState()
        {
            _stateTimer = 0.0f;
            _cooldownTimer = 0.0f;
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
            if (_fired)
            {
                _cooldownTimer += Time.deltaTime;
            }

            if (CheckSwitchStates() == false)
            {
                if (_fired == false)
                {
                    _fired = true;
                    _gbdCtx.FireCannons();
                    return;
                }
            }
        }
    }
    
}
