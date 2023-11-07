using _Scripts._Game.General.Managers;
using _Scripts._Game.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.AI.AttackStateMachine.Flying.Bombdroid{
    
    public class BombDroidBombDropAIBondedAttackState : BaseAIBondedAttackState
    {
        private BombDroidAIAttackStateMachine _bdCtx;
        private bool _hasDroppedBomb;

        public BombDroidBombDropAIBondedAttackState(AIAttackStateMachineBase ctx, AIAttackStateMachineFactory factory) : base(ctx, factory)
        {
            _bdCtx = ctx as BombDroidAIAttackStateMachine;
        }

        public override bool CheckSwitchStates()
        {
            if (_stateTimer >= _bdCtx.BondedBombDropBuildUpDuration)
            {
                if (!_hasDroppedBomb)
                {
                    // drop bomb
                    _hasDroppedBomb = true;
                    ProjectileManager.Instance.TryBombDroidBombDropProjectile(General.EEntityType.BondedEnemy, _ctx.transform.position);
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
                    // drop bomb
                    _hasDroppedBomb = true;
                    ProjectileManager.Instance.TryBombDroidBombDropProjectile(General.EEntityType.BondedEnemy, _ctx.transform.position);
                }
            }
        }

    }
    
}
