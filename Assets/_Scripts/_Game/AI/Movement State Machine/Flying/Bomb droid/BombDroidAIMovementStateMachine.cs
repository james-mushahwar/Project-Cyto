﻿using _Scripts._Game.AI.Bonding;
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

        #region Bonded Bomb Droid Stats
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
