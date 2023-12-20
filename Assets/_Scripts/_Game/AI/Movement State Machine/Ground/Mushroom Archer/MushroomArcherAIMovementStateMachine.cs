using _Scripts._Game.General;
using UnityEngine;
using _Scripts._Game.AI.MovementStateMachine;

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
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
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