using _Scripts._Game.General.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor.ShaderGraph.Internal;
#endif

namespace _Scripts._Game.Player.AttackingStateMachine{

    public class BasicAttackAttackingState : BaseAttackingState
    {
        private float _comboWaitDuration;
        private float _comboElapseTime;
        private float _comboBufferStartTime;

        public BasicAttackAttackingState(PlayerAttackingStateMachine ctx, PlayerAttackingStateMachineFactory factory) : base(ctx, factory)
        {
        }

        public override bool CheckSwitchStates()
        {
            if (_stateTimer >= _comboBufferStartTime && _ctx.BasicAttackBuffered == false)
            {
                if (_ctx.IsAttackInputValid == true)
                {
                    _ctx.BasicAttackBuffered = true;
                }
            }

            if (_stateTimer >= _comboWaitDuration)
            {
                if (_ctx.BasicAttackBuffered)
                {
                    if (_ctx.CurrentBasicAttackCombo < _ctx.BasicComboLimit && TargetManager.Instance.DamageableTarget != null)
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
            _ctx.BasicAttackBuffered = false;

            _ctx.CurrentBasicAttackCombo++;

            _ctx.NullifyInput(AttackingState.Basic_Attack);

            //Debug.Log("Basic attack combo stats- Combo Index = " + comboIndex);
            //Debug.Log("Basic attack combo: " + _ctx.CurrentBasicAttackCombo);
            bool success = false;

            if (TargetManager.Instance.DamageableTarget != null)
            {
                success = ProjectileManager.Instance.TryBasicAttackProjectile(TargetManager.Instance.DamageableTarget, PlayerEntity.Instance.transform.position, comboIndex);
                FollowCamera.Instance.OnAttack();
                Vector3 direction = (TargetManager.Instance.DamageableTarget.Transform.position - PlayerEntity.Instance.transform.position).normalized;
                _ctx.OnBasicAttackStart.Invoke(direction);
            }

            if (!success)
            {
                Debug.LogWarning("Basic Attack was unsuccessful");
                SwitchStates(_factory.GetState(AttackingState.Basic_Idle));
            }
            else
            {
                _ctx.RecentAttackTimer = _ctx.BasicAttackRecentTimer;
            }
        }

        public override void ExitState()
        {
            _ctx.BasicAttackBuffered = false;
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
