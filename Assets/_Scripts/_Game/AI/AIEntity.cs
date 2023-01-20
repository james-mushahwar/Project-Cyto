﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _Scripts._Game.Player;
using _Scripts._Game.AI.MovementStateMachine;
using _Scripts._Game.Animation;
using _Scripts._Game.General;
using _Scripts._Game.General.Managers;
using _Scripts._Game.AI.AttackStateMachine;

namespace _Scripts._Game.AI{
    
    public class AIEntity : MonoBehaviour, IPossessable
    {
        private bool _isPossessed;

        #region State Machines
        private AIMovementStateMachineBase _movementSM;
        private AIAttackStateMachineBase _attackSM;
        private SpriteAnimator _spriteAnimator;

        public AIMovementStateMachineBase MovementSM { get => _movementSM; set => _movementSM = value; }
        public AIAttackStateMachineBase AttackSM { get => _attackSM; set => _attackSM = value; }
        public SpriteAnimator SpriteAnimator { get => _spriteAnimator; set => _spriteAnimator = value; }
        #endregion

        #region AI Components
        private EnemyHealthStats _enemyHealthStats;

        public EnemyHealthStats EnemyHealthStats { get => _enemyHealthStats; }
        #endregion

        protected void Awake()
        {
            _enemyHealthStats = new EnemyHealthStats(3.0f, 3.0f);
        }

        // IPossessable
        public bool IsPossessed()
        {
            return _isPossessed;
        }

        public bool CanBePossessed()
        {
            return !_isPossessed && GetHealthStats().IsAlive();
        }

        public void OnPossess()
        {
            // possess control of this AI
            _movementSM.OnBonded();
            _movementSM.CurrentState.ExitState();
            _movementSM.CurrentBondedState.EnterState();
            _movementSM.Collider.isTrigger = false;

            _attackSM.OnBonded();
            //InputManager.Instance.TryEnableActionMap(EInputSystem.BondedPlayer);
            _isPossessed = true;
        }

        public void OnDispossess()
        {
            // dispossess this AI
            _movementSM.OnUnbonded();
            _movementSM.CurrentBondedState.ExitState();
            _movementSM.CurrentState.EnterState();
            _movementSM.Collider.isTrigger = true;

            _attackSM.OnUnbonded();
            //InputManager.Instance.TryDisableActionMap(EInputSystem.BondedPlayer);
            _isPossessed = false;

            PlayerEntity.Instance.OnPossess();
        }

        public HealthStats GetHealthStats()
        {
            return _enemyHealthStats;
        }

        public Transform PossessableTransform { get => transform; }

        public Vector2 GetMovementInput()
        {
            if (_isPossessed)
            {
                return _movementSM.CurrentMovementInput;
            }
            else
            {
                return new Vector2(0,0);
            }
        }
    }
    
}
