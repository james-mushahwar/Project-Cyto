﻿using _Scripts._Game.General.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.AI.AttackStateMachine.Flying.Bombdroid{
    
    public class BombDroidBombDropAIAttackState : BaseAIAttackState
    {
        private BombDroidAIAttackStateMachine _bdCtx;
        private bool _hasDroppedBomb;

        public BombDroidBombDropAIAttackState(AIAttackStateMachineBase ctx, AIAttackStateMachineFactory factory) : base(ctx, factory)
        {
            _bdCtx = ctx as BombDroidAIAttackStateMachine;
        }

        public override bool CheckSwitchStates()
        {
            if (_stateTimer >= _bdCtx.BombDropStateDuration)
            {
                SwitchStates(_factory.GetState(AIAttackState.Idle));
                return true;
            }
            return false;
        }

        public override void EnterState()
        {
            _stateTimer = 0.0f;
            _hasDroppedBomb = false;
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
                if (_stateTimer >= _bdCtx.BombDropBuildUpDuration && !_hasDroppedBomb)
                {
                    // drop bomb
                    _hasDroppedBomb = true;
                    ProjectileManager.Instance.TryBombDroidBombDropProjectile(General.EEntityType.Enemy, _ctx.transform.position);
                }
            }
        }
    }
    
}
