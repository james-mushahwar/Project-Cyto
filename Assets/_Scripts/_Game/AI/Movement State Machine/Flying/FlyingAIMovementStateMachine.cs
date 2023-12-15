using _Scripts._Game.AI.Bonding;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using _Scripts._Game.General;
using UnityEngine;

namespace _Scripts._Game.AI.MovementStateMachine.Flying{
    
    public class FlyingAIMovementStateMachine : AIMovementStateMachineBase
    {
        #region Components
        private Seeker _seeker;
        private AIPath _aiPath;
        private AIDestinationSetter _destinationSetter;

        public Seeker Seeker { get => _seeker; }
        public AIPath AIPath { get => _aiPath; }
        public AIDestinationSetter DestinationSetter { get => _destinationSetter; }
        #endregion

        protected override void Awake()
        {
            base.Awake();

            //CurrentState = _states.GetState(AIMovementState.Idle);
            //CurrentBondedState = _states.GetState(AIBondedMovementState.Flying);

            BondInputsDict.Add(PossessInput.Movement, OnMovementInput);

            _seeker = GetComponent<Seeker>();
            _destinationSetter = GetComponent<AIDestinationSetter>();
            _aiPath = GetComponent<AIPath>();
        }

        // knockback reaction and reset
        public void StartKnockbackEffect()
        {
            if (CurrentState.UsesAIPathfinding)
            {
                Seeker.enabled = false;
                DestinationSetter.enabled = false;
                AIPath.enabled = false;
            }
        }

        public void EndKnockbackEffect()
        {
            if (CurrentState.UsesAIPathfinding && !Entity.IsExposed())
            {
                Seeker.enabled = true;
                DestinationSetter.enabled = true;
                AIPath.enabled = true;
            }
        }

    }
    
}
