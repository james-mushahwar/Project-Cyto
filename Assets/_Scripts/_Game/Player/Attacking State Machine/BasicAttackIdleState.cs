using _Scripts._Game.General.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.Player.AttackingStateMachine{
    
    public class BasicAttackIdleState : BaseAttackingState
    {
        public BasicAttackIdleState(PlayerAttackingStateMachine ctx, PlayerAttackingStateMachineFactory factory) : base(ctx, factory)
        {
        }

        public override bool CheckSwitchStates()
        {
            if (_ctx.IsAttackInputValid == true && TargetManager.Instance.DamageableTarget != null)
            {
                SwitchStates(_factory.GetState(AttackingState.Basic_Attack));
                return true;
            }

            return false;
        }

        public override void EnterState()
        {
            Debug.Log("Basic attack idle!");
            _stateTimer = 0.0f;
            _ctx.CurrentBasicAttackCombo = 0;
        }

        public override void ExitState()
        {
            return;
        }

        public override void InitialiseState()
        {
            return;
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
