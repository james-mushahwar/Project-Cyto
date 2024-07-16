using _Scripts._Game.General;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.AI.MovementStateMachine.Bosses{
    
    public abstract class BossAIMovementStateMachine : AIMovementStateMachineBase
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

            _seeker = GetComponent<Seeker>();
            _destinationSetter = GetComponent<AIDestinationSetter>();
            _aiPath = GetComponent<AIPath>();

            BondInputsDict.Add(PossessInput.Movement, OnMovementInput);
        }
        //public override bool DoesStateUseAIPathFinding(AIMovementState state)
        //{
        //    throw new System.NotImplementedException();
        //}

        public override void StartKnockbackEffect()
        {
            if (DoesStateUseAIPathFinding(CurrentStateEnum))
            {
                Seeker.enabled = false;
                DestinationSetter.enabled = false;
                AIPath.enabled = false;
            }
        }

        public override void EndKnockbackEffect()
        {
            if (DoesStateUseAIPathFinding(CurrentStateEnum) && !Entity.IsExposed())
            {
                Seeker.enabled = true;
                DestinationSetter.enabled = true;
                AIPath.enabled = true;
            }
        }
    }

}
