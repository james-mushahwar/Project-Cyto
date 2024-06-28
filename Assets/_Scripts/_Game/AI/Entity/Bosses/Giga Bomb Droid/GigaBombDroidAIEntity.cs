using _Scripts._Game.AI.MovementStateMachine.Bosses.GigaBombDroid;
using _Scripts._Game.AI.MovementStateMachine.Flying.Bombdroid;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.AI.Entity.Bosses.GigaBombDroid{
    
    public class GigaBombDroidAIEntity : BossAIEntity
    {
        public GigaBombDroidAIMovementStateMachine GigaBombDroidMovementSM
        {
            get { return _movementSM as GigaBombDroidAIMovementStateMachine; }
        }
        public GigaBombDroidAIEntity()
        {
            _entity = General.EEntity.GigaBombDroid;
        }
    
        protected override void Awake()
        {
             base.Awake();
           
        }

        public override void Tick()
        {
             base.Tick();
        }
    }
}
