using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.AI.MovementStateMachine.Bosses{

    public abstract class BossBaseAIBondedMovementState : BaseAIBondedMovementState
    {
        protected BossBaseAIBondedMovementState(AIMovementStateMachineBase ctx, AIMovementStateMachineFactory factory) : base(ctx, factory)
        {
        }
    }

}
