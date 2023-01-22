using _Scripts._Game.AI.Bonding;
using _Scripts._Game.AI.MovementStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.AI.AttackStateMachine.Flying.Bombdroid{
    
    public class BombDroidAIAttackStateMachine : AIAttackStateMachineBase
    {
        #region Bomb Droid Attack Stats
        [Header("Idle Properties")]
        [SerializeField]
        private float _attackCooldown;

        public float AttackCooldown { get => _attackCooldown; set => _attackCooldown = value; }

        [Header("Bomb drop attack properties")]
        [SerializeField]
        private float _bombDropMinimumYDistance;
        [SerializeField]
        private float _bombDropMaximumXLimit;
        [SerializeField]
        private float _bombDropStateDuration;
        [SerializeField]
        private float _bombDropUpwardsForce;
        [SerializeField]
        private float _bombDropBuildUpDuration;

        public float BombDropStateDuration { get => _bombDropStateDuration; }
        public float BombDropUpwardsForce { get => _bombDropUpwardsForce; }
        public float BombDropBuildUpDuration { get => _bombDropBuildUpDuration; }
        public float BombDropMinimumYDistance { get => _bombDropMinimumYDistance; }
        public float BombDropMaximumXLimit { get => _bombDropMaximumXLimit; }
        #endregion

        #region Bonded Bomb Droid Attack Stats
        #endregion

        private bool _isAttackInterrupted = false;

        public bool IsAttackInterrupted { get => _isAttackInterrupted; set => _isAttackInterrupted = value; }

        protected override void Awake()
        {
            base.Awake();

            CurrentState = _states.GetState(AIAttackState.Idle);

            BondInputsDict.Add(BondInput.Movement, OnMovementInput);
        }

        protected void FixedUpdate()
        {
            if (!Entity.IsPossessed())
            {
                CurrentState.ManagedStateTick();
            }
            else
            {
                //CurrentBondedState.ManagedStateTick();
            }

            if (_isAttackInterrupted)
            {
                _isAttackInterrupted = false;
            }
        }
    }

}
