﻿using _Scripts._Game.General.Managers;
using _Scripts._Game.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.AI.AttackStateMachine.Flying.Bombdroid{
    
    public class BombDroidBombDropAIBondedAttackState : BaseAIBondedAttackState
    {
        private BombDroidAIAttackStateMachine _bdCtx;
        private bool _hasDroppedBomb;

        private bool _inputHeld;

        public BombDroidBombDropAIBondedAttackState(AIAttackStateMachineBase ctx, AIAttackStateMachineFactory factory) : base(ctx, factory)
        {
            _bdCtx = ctx.GetStateMachine<BombDroidAIAttackStateMachine>();
        }

        public override bool CheckSwitchStates()
        {
            if (StatsManager.Instance.IsAbilityUsable(General.EAbility.BombDroid_SuperBombDrop))
            {
                if (_inputHeld)
                {
                    _inputHeld = _ctx.IsWestButtonPressed;

                    if (_stateTimer >= _bdCtx.BondedSuperBombDropTransitionDelay)
                    {
                        SwitchStates(_factory.GetBondedState(AIAttackState.Attack2));
                        return true;
                    }
                }
            }

            if (_stateTimer >= _bdCtx.BondedBombDropBuildUpDuration)
            {
                if (!_hasDroppedBomb)
                {
                    DropBomb();
                }
                SwitchStates(_factory.GetBondedState(AIAttackState.Idle));
                return true;
            }
            return false;
        }

        public override void EnterState()
        {
            _stateTimer = 0.0f;
            _hasDroppedBomb = false;
            _inputHeld = _ctx.IsWestButtonPressed;
            AudioManager.Instance.TryPlayAudioSourceAttached(EAudioType.SFX_Enemy_BombDroid_ChargeBombAttack, _ctx.transform);
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
                if (_stateTimer >= _bdCtx.BondedBombDropBuildUpDuration && !_hasDroppedBomb)
                {
                    DropBomb();
                }
            }
        }

        private void DropBomb()
        {
            // drop bomb
            _hasDroppedBomb = true;
            ProjectileManager.Instance.TryBombDroidBombDropProjectile(General.EEntityType.BondedEnemy, _ctx.transform.position);
        }
    }
    
}
