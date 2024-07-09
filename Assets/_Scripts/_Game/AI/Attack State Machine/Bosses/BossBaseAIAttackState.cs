using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.AI.AttackStateMachine.Bosses{

    public abstract class BossBaseAIAttackState : BaseAIAttackState
    {
        protected BossBaseAIAttackState(AIAttackStateMachineBase ctx, AIAttackStateMachineFactory factory) : base(ctx, factory)
        {
        }
    }

}
