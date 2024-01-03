using _Scripts._Game.General;
using UnityEngine;
using _Scripts._Game.AI.MovementStateMachine;
using _Scripts._Game.AI.MovementStateMachine.Flying.Bombdroid;

namespace _Scripts._Game.AI.MovementStateMachine.Ground.MushroomArcher{
    
    public class MushroomArcherAIMovementStateMachine : GroundAIMovementStateMachine
    {
        #region Mushroom Archer Stats
        [Header("Patrol Properties")]
        [SerializeField]
        private float _patrolSpeed;
        [SerializeField]
        private Vector2 _patrolWaitTimeRange;
        public Vector2 PatrolWaitTimeRange { get => _patrolWaitTimeRange; }

        [Header("Chase Properties")]
        [SerializeField]
        private float _chaseDetectionSqRange;
        [SerializeField]
        private float _chaseLostDetectionSqRange;
        [SerializeField]
        private float _chaseSpeed;

        public float ChaseDetectionSqRange { get => _chaseDetectionSqRange; }
        public float ChaseLostDetectionSqRange { get => _chaseLostDetectionSqRange; }
        #endregion

        #region Bonded Mushroom Archer Stats

        #endregion

        protected override void Awake()
        {
            base.Awake();

            States.AddState(AIMovementState.Idle, new MushroomArcherIdleAIMovementState(this, States));
            States.AddState(AIMovementState.Patrol, new MushroomArcherPatrolAIMovementState(this, States));
            States.AddState(AIMovementState.Chase, new MushroomArcherChaseAIMovementState(this, States));
            States.AddState(AIMovementState.Attack, new AttackAIMovementState(this, States));
            
            States.AddState(AIBondedMovementState.Grounded, new MushroomArcherGroundedAIBondedMovementState(this, States));
            States.AddState(AIBondedMovementState.Falling, new MushroomArcherFallingAIBondedMovementState(this, States));
            States.AddState(AIBondedMovementState.Jumping, new MushroomArcherJumpingAIBondedMovementState(this, States));
            States.AddState(AIBondedMovementState.Attacking, new MushroomArcherAttackingAIBondedMovementState(this, States));
        }

        public override void Tick()
        {
            base.Tick();
        }

        public override bool DoesStateUseAIPathFinding(AIMovementState state)
        {
            throw new System.NotImplementedException();
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