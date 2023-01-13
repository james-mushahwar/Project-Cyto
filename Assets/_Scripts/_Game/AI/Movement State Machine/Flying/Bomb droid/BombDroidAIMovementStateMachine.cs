using _Scripts._Game.AI.Bonding;
using _Scripts._Game.General;
using _Scripts._Game.General.Managers;
using Pathfinding;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts._Game.AI.MovementStateMachine.Flying.Bombdroid{
    
    public class BombDroidAIMovementStateMachine : FlyingAIMovementStateMachine
    {
        #region Components
        private Seeker _seeker;
        private AIPath _aiPath;
        private AIDestinationSetter _destinationSetter;

        public Seeker Seeker { get => _seeker; }
        public AIPath AIPath { get => _aiPath; }
        public AIDestinationSetter DestinationSetter { get => _destinationSetter; }
        #endregion

        #region Enemy Stats
        [Header("Patrol Properties")]
        [SerializeField]
        private float _patrolSpeed;
        [SerializeField]
        private Vector2 _patrolWaitTimeRange;
        [SerializeField]
        private Waypoints _waypoints;

        public Vector2 PatrolWaitTimeRange { get => _patrolWaitTimeRange; }
        public Waypoints Waypoints { get => _waypoints; }

        [Header("Chase Properties")]
        [SerializeField]
        private float _chaseDetectionRange;
        [SerializeField]
        private float _chaseLostDetectionRange;
        [SerializeField]
        private float _chaseSpeed;
        [SerializeField]
        private Vector2 _startStopChaseDistances;

        public float ChaseDetectionRange { get => _chaseDetectionRange; }
        public float ChaseLostDetectionRange { get => _chaseLostDetectionRange; }
        #endregion

        protected override void Awake()
        {
            base.Awake();

            CurrentState = _states.GetState(AIMovementState.Idle);

            BondInputsDict.Add(BondInput.Movement, OnMovementInput);
        }

        protected override void Start()
        {
            base.Start();

            _seeker = GetComponent<Seeker>();
            _destinationSetter = GetComponent<AIDestinationSetter>();
            _aiPath = GetComponent<AIPath>();

            CurrentState.EnterState();
        }

        void FixedUpdate()
        {
            CurrentState.ManagedStateTick();
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

        // IBondable
        public override void OnMovementInput(InputAction.CallbackContext context)
        {

        }

    }
    
}
