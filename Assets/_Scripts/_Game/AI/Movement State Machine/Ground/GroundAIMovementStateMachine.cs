using _Scripts._Game.AI.Bonding;
using _Scripts._Game.General;
using Pathfinding;
using UnityEngine;

namespace _Scripts._Game.AI.MovementStateMachine.Ground
{

    public abstract class GroundAIMovementStateMachine : AIMovementStateMachineBase
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

            CurrentState = _states.GetState(AIMovementState.Idle);
            CurrentBondedState = _states.GetState(AIBondedMovementState.Grounded);

            BondInputsDict.Add(PossessInput.Movement, OnMovementInput);

            _seeker = GetComponent<Seeker>();
            _destinationSetter = GetComponent<AIDestinationSetter>();
            _aiPath = GetComponent<AIPath>();
        }

    }

}
