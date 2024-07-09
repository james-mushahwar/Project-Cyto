using _Scripts._Game.AI.AttackStateMachine.Bosses.GigaBombDroid;
using _Scripts._Game.AI.MovementStateMachine.Bosses.GigaBombDroid;
using _Scripts._Game.AI.MovementStateMachine.Flying.Bombdroid;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.AI.Entity.Bosses.GigaBombDroid{
    
    public class GigaBombDroidAIEntity : BossAIEntity
    {
        private int _damageState = 0;
        public int DamageState
        {
            get { return _damageState;}
        }

        public GigaBombDroidAIEntity()
        {
            _entity = General.EEntity.GigaBombDroid;
        }

        public GigaBombDroidAIMovementStateMachine GigaBombDroidMovementSM
        {
            get { return _movementSM as GigaBombDroidAIMovementStateMachine; }
        }

        public GigaBombDroidAIAttackStateMachine GigaBombDroidAIAttackSM
        {
            get { return _attackSM as GigaBombDroidAIAttackStateMachine; }
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
