using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.Player.AttackingStateMachine{

    public class BasicAttackAttackingState : BaseAttackingState
    {
        private float _comboWaitDuration;
        private float _comboBufferDuration;

        public BasicAttackAttackingState(PlayerAttackingStateMachine ctx, PlayerAttackingStateMachineFactory factory) : base(ctx, factory)
        {
        }

        public override bool CheckSwitchStates()
        {
            return false;
        }

        public override void EnterState()
        {
            _stateTimer = 0.0f;
            int comboIndex = _ctx.CurrentBasicAttackCombo;
            _comboWaitDuration = _ctx.BasicComboWaitTimes[comboIndex];
            _comboBufferDuration = _ctx.BasicComboBufferTimes[comboIndex];
            _ctx.CurrentBasicAttackCombo++;
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
            return;
        }
    }

}
