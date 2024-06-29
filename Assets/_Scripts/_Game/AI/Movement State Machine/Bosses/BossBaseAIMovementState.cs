using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.AI.MovementStateMachine.Bosses{

    public abstract class BossBaseAIMovementState : BaseAIMovementState
    {
        protected BossBaseAIMovementState(AIMovementStateMachineBase ctx, AIMovementStateMachineFactory factory) : base(ctx, factory)
        {
        }
    }

}
