using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.Player.AttackingStateMachine{
    
    public class IdleAttackingState : BaseAttackingState
    {
        public IdleAttackingState(PlayerAttackingStateMachine ctx, PlayerAttackingStateMachineFactory factory) : base(ctx, factory)
        {
        }

        public override bool CheckSwitchStates()
        {
            throw new System.NotImplementedException();
        }

        public override void EnterState()
        {
            throw new System.NotImplementedException();
        }

        public override void ExitState()
        {
            throw new System.NotImplementedException();
        }

        public override void InitialiseState()
        {
            throw new System.NotImplementedException();
        }

        public override void ManagedStateTick()
        {
            throw new System.NotImplementedException();
        }
    }
    
}
