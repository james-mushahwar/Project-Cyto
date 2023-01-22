using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.AI.AttackStateMachine.Flying.Bombdroid{
    
    public class BombDroidBombDropAIAttackState : BaseAIAttackState
    {
        public BombDroidBombDropAIAttackState(AIAttackStateMachineBase ctx, AIAttackStateMachineFactory factory) : base(ctx, factory)
        {
        }

        public override bool CheckSwitchStates()
        {
            return false;
        }

        public override void EnterState()
        {
            Debug.Log("BOOOOM Bomb drop!");
        }

        public override void ExitState()
        {
           
        }

        public override void InitialiseState()
        {
            
        }

        public override void ManagedStateTick()
        {
            
        }
    }
    
}
