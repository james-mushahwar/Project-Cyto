using _Scripts._Game.General;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.AI.MovementStateMachine.Flying.Bombdroid{
    
    public class BombDroidAIMovementStateMachine : FlyingAIMovementStateMachine
    {
        #region Components
        private Seeker _seeker;
        private Rigidbody2D _rb;
        private Path _path;
        private AIDestinationSetter _destinationSetter;

        public Seeker Seeker { get => _seeker; }
        public Rigidbody2D Rb { get => _rb; }
        public Path Path { get => _path; }
        public AIDestinationSetter DestinationSetter { get => _destinationSetter; }
        #endregion

        #region Enemy Stats

        [Header("Patrol Properties")]
        [SerializeField]
        private float _patrolSpeed;
        [SerializeField]
        private Waypoints _waypoints;

        public Waypoints Waypoints { get => _waypoints; }
        #endregion

        protected override void Awake()
        {
            base.Awake();

            CurrentState = _states.GetState(AIMovementState.Idle);
        }

        protected override void Start()
        {
            base.Start();

            _seeker = GetComponent<Seeker>();
            _destinationSetter = GetComponent<AIDestinationSetter>();

            CurrentState.EnterState();
        }
    
        void Update()
        {
            
        }

        void FixedUpdate()
        {
            CurrentState.ManagedStateTick();
        }
    }
    
}
