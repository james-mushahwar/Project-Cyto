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
        #region Bomb Droid Stats
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
        private float _chaseDetectionSqRange;
        [SerializeField]
        private float _chaseLostDetectionSqRange;
        [SerializeField]
        private float _chaseSpeed;
        [SerializeField]
        private Vector2 _startStopChaseDistances;

        public float ChaseDetectionSqRange { get => _chaseDetectionSqRange; }
        public float ChaseLostDetectionSqRange { get => _chaseLostDetectionSqRange; }
        #endregion

        #region Bonded Bomb Droid Stats
        [Header("Bonded Flying Properties")]
        [SerializeField]
        private float _flyingHorizontalVelocity;
        [SerializeField]
        private float _flyingVerticalVelocity;
        [SerializeField]
        private float _flyingMaximumHorizontalVelocity;
        [SerializeField]
        private float _flyingMaximumVerticalVelocity;
        [SerializeField]
        private float _flyingHorizontalAcceleration;
        [SerializeField]
        private float _flyingHorizontalDeceleration;
        [SerializeField]
        private float _flyingVerticalAcceleration;
        [SerializeField]
        private float _flyingVerticalDeceleration;

        [SerializeField]
        private float _flyingMovementDirectionThrust;

        public float FlyingHorizontalVelocity { get => _flyingHorizontalVelocity; }
        public float FlyingVerticalVelocity { get => _flyingVerticalVelocity; }
        public float FlyingMaximumVerticalVelocity { get => _flyingMaximumVerticalVelocity; }
        public float FlyingMaximumHorizontalVelocity { get => _flyingMaximumHorizontalVelocity; }
        public float FlyingHorizontalAcceleration { get => _flyingHorizontalAcceleration; }
        public float FlyingHorizontalDeceleration { get => _flyingHorizontalDeceleration; }
        public float FlyingVerticalAcceleration { get => _flyingVerticalAcceleration; }
        public float FlyingVerticalDeceleration { get => _flyingVerticalDeceleration; }
        public float FlyingMovementDirectionThrust { get => _flyingMovementDirectionThrust; }

        #endregion

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
