using _Scripts._Game.General;
using UnityEngine;
using _Scripts._Game.AI.MovementStateMachine;
using _Scripts._Game.AI.MovementStateMachine.Ground;
using _Scripts._Game.AI.MovementStateMachine.Flying;

namespace _Scripts._Game.AI.MovementStateMachine.Bosses.GigaBombDroid{
    
    public class GigaBombDroidAIMovementStateMachine : BossAIMovementStateMachine
    {
        #region #TBD# Stats
        [Header("Patrol Properties")]
        [SerializeField]
        private float _patrolSpeed;
        [SerializeField]
        private Vector2 _patrolWaitTimeRange;
        public Vector2 PatrolWaitTimeRange { get => _patrolWaitTimeRange; }
        #endregion

        #region Bonded #TBD# Stats
        [Header("Bonded Flying Properties")]
        [SerializeField]
        private float _flyingMaximumHorizontalVelocity;
        [SerializeField]
        private float _flyingMaximumVerticalVelocity;
        [SerializeField]
        private float _flyingMovementDirectionThrust;

        public float FlyingMaximumVerticalVelocity { get => _flyingMaximumVerticalVelocity; }
        public float FlyingMaximumHorizontalVelocity { get => _flyingMaximumHorizontalVelocity; }
        public float FlyingMovementDirectionThrust { get => _flyingMovementDirectionThrust; }
        #endregion

        protected override void Awake()
        {
            base.Awake();

            States.AddState(AIMovementState.Sleep, new GigaBombDroidSleepAIMovementState(this, States));
            States.AddState(AIMovementState.Wake, new GigaBombDroidWakeAIMovementState(this, States));
            States.AddState(AIMovementState.Idle, new GigaBombDroidIdleAIMovementState(this, States));
            States.AddState(AIMovementState.Patrol, new GigaBombDroidPatrolAIMovementState(this, States));
            States.AddState(AIMovementState.Attack, new GigaBombDroidAttackAIMovementState(this, States));
            
            States.AddState(AIBondedMovementState.Flying, new GigaBombDroidFlyingAIBondedMovementState(this, States));
        }

        public override void Tick()
        {
            base.Tick();
        }

        public override bool DoesStateUseAIPathFinding(AIMovementState state)
        {
            return false;
        }
        
        // ISaveable
        [System.Serializable]
        private struct SaveData
        {
    
        }
    
        public override object SaveState()
        {
            return new SaveData();
        }
    
        public override void LoadState(object state)
        {
            SaveData saveData = (SaveData)state;
        }
    }
}