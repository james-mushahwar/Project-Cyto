using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.AI.AttackStateMachine.Bosses{

    public abstract class BossBaseAIBondedAttackState : BaseAIBondedAttackState
    {
        protected BossBaseAIBondedAttackState(AIAttackStateMachineBase ctx, AIAttackStateMachineFactory factory) : base(ctx, factory)
        {
        }
    }

}
