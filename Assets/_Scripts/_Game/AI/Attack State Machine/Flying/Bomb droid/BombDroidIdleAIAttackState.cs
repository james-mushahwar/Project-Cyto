using _Scripts._Game.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.AI.AttackStateMachine.Flying.Bombdroid{

    public class BombDroidIdleAIAttackState : BaseAIAttackState
    {
        private BombDroidAIAttackStateMachine _bdCtx;

        public BombDroidIdleAIAttackState(AIAttackStateMachineBase ctx, AIAttackStateMachineFactory factory) : base(ctx, factory)
        {
            _bdCtx = ctx as BombDroidAIAttackStateMachine;
        }

        public override bool CheckSwitchStates()
        {
            if (_stateTimer >= _bdCtx.BombDropCooldown)
            {
                GameObject playerGO = PlayerEntity.Instance.GetControlledGameObject();
                bool isAbovePlayer = playerGO.transform.position.y < _bdCtx.transform.position.y + _bdCtx.BombDropMinimumYDistance;

                if (isAbovePlayer)
                {
                    bool isWithinXLimits = Mathf.Abs(playerGO.transform.position.x - _bdCtx.transform.position.x) <= _bdCtx.BombDropMaximumXLimit;
                    if (isWithinXLimits)
                    {
                        SwitchStates(_factory.GetState(AIAttackState.Attack1));
                        return true;
                    }

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
