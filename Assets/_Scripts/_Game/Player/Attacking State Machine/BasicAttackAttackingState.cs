using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

namespace _Scripts._Game.Player.AttackingStateMachine{

    public class BasicAttackAttackingState : BaseAttackingState
    {
        private float _comboWaitDuration;
        private float _comboElapseTime;
        private float _comboBufferStartTime;
        private bool _isAttackBuffered;

        public BasicAttackAttackingState(PlayerAttackingStateMachine ctx, PlayerAttackingStateMachineFactory factory) : base(ctx, factory)
        {
        }

        public override bool CheckSwitchStates()
        {
            if (_stateTimer >= _comboBufferStartTime && _isAttackBuffered == false)
            {
                if (_ctx.IsAttackInputValid == true)
                {
                    _isAttackBuffered = true;
                }
            }

            if (_stateTimer >= _comboWaitDuration)
            {
                if (_isAttackBuffered)
                {
                    if (_ctx.CurrentBasicAttackCombo < _ctx.BasicComboLimit)
                    {
                        SwitchStates(_factory.GetState(AttackingState.Basic_Attack));
                        return true;
                    }
                }
                
                if (_stateTimer >= _comboElapseTime)
                {
                    SwitchStates(_factory.GetState(AttackingState.Basic_Idle));
                    return true;
                }
            }

            return false;
        }

        public override void EnterState()
        {
            _stateTimer = 0.0f;

            int comboIndex = _ctx.CurrentBasicAttackCombo;
            _comboWaitDuration = _ctx.BasicComboWaitTimes[comboIndex];
            _comboElapseTime = _ctx.BasicComboElapseTimes[comboIndex];
            _comboBufferStartTime = _comboWaitDuration - _ctx.BasicComboBufferTimes[comboIndex];
            _isAttackBuffered = false;

            _ctx.CurrentBasicAttackCombo++;

            _ctx.NullifyInput(AttackingState.Basic_Attack);

            Debug.Log("Basic attack combo stats- Combo Index = " + comboIndex);
            Debug.Log("Basic attack combo: " + _ctx.CurrentBasicAttackCombo);
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

            }
        }
    }

}
