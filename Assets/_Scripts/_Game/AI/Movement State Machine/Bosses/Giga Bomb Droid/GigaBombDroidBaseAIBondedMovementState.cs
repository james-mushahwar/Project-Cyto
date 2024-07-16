using _Scripts._Game.AI.Entity.Bosses.GigaBombDroid;
using _Scripts._Game.AI.Entity.Flying.Bombdroid;
using _Scripts._Game.AI.MovementStateMachine.Flying.Bombdroid;
using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using UnityEngine;

namespace _Scripts._Game.AI.MovementStateMachine.Bosses.GigaBombDroid{

    public abstract class GigaBombDroidBaseAIBondedMovementState : BossBaseAIBondedMovementState
    {
        protected GigaBombDroidAIMovementStateMachine _gbdCtx;
        protected GigaBombDroidAIEntity _gbdEntity;

        protected GigaBombDroidBaseAIBondedMovementState(AIMovementStateMachineBase ctx, AIMovementStateMachineFactory factory) : base(ctx, factory)
        {
            _gbdCtx = ctx.GetStateMachine<GigaBombDroidAIMovementStateMachine>();
            _gbdEntity = ctx.Entity as GigaBombDroidAIEntity;
        }
    }

}
