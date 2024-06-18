using _Scripts._Game.General.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.AI.AttackStateMachine.Flying.Bombdroid{
    
    public class BombDroidSuperBombDropAIBondedAttackState : BaseAIBondedAttackState
    {
        private BombDroidAIAttackStateMachine _bdCtx;
        private bool _hasDroppedBomb;
        private bool _bombDropCharged;

        public BombDroidSuperBombDropAIBondedAttackState(AIAttackStateMachineBase ctx, AIAttackStateMachineFactory factory) : base(ctx, factory)
        {
            _bdCtx = ctx.GetStateMachine<BombDroidAIAttackStateMachine>();
        }

        public override bool CheckSwitchStates()
        {
            bool chargeElapsed = _stateTimer >= _bdCtx.BondedSuperBombDropHoldInputDuration;
            
            bool inputDown = _bdCtx.IsWestButtonPressed;

            bool fireSuperBomb = !inputDown;
            if (fireSuperBomb)
            {
                DropSuperBomb();
                SwitchStates(_factory.GetBondedState(AIAttackState.Idle));
                return true;
            }

            if (chargeElapsed)
            {
                FullyCharged();
            }
            else
            {
                if (!inputDown)
                {
                    DropBomb();
                    SwitchStates(_factory.GetBondedState(AIAttackState.Idle));
                    return true;
                }
            }

            return false;
        }

        public override void EnterState()
        {
            _stateTimer = _bdCtx.BondedSuperBombDropHoldInputDuration - _bdCtx.BondedSuperBombDropTransitionDelay;
            _hasDroppedBomb = false;
            _bombDropCharged = false;
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
                if (_bombDropCharged)
                {
                    
                }
            }
        }

        void FullyCharged()
        {
            _bombDropCharged = true;
        }

        void DropBomb()
        {
            // drop super bomb
            _hasDroppedBomb = true;
            ProjectileManager.Instance.TryBombDroidBombDropProjectile(General.EEntityType.BondedEnemy, _ctx.transform.position);
        }

        private void DropSuperBomb()
        {
            // drop super bomb
            _hasDroppedBomb = true;
            ProjectileManager.Instance.TryBombDroidSuperBombDropProjectile(General.EEntityType.BondedEnemy, _ctx.transform.position);
        }
    }
    
}
