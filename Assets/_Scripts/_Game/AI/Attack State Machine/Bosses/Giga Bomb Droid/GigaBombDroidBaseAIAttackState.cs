using _Scripts._Game.AI.Entity.Bosses.GigaBombDroid;
using _Scripts._Game.AI.MovementStateMachine.Bosses.GigaBombDroid;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.AI.AttackStateMachine.Bosses.GigaBombDroid{
    
    public abstract class GigaBombDroidBaseAIAttackState : BossBaseAIAttackState
    {
        protected GigaBombDroidAIAttackStateMachine _gbdCtx;
        protected GigaBombDroidAIEntity _gbdEntity;

        protected GigaBombDroidBaseAIAttackState(AIAttackStateMachineBase ctx, AIAttackStateMachineFactory factory) : base(ctx, factory)
        {
            _gbdCtx = ctx.GetStateMachine<GigaBombDroidAIAttackStateMachine>();
            _gbdEntity = ctx.Entity as GigaBombDroidAIEntity;
        }
    }
    
}
