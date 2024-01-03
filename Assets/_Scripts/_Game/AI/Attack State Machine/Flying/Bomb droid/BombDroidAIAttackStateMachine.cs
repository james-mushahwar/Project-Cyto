using _Scripts._Game.AI.Bonding;
using _Scripts._Game.AI.MovementStateMachine;
using System.Collections;
using System.Collections.Generic;
using _Scripts._Game.General;
using UnityEngine;
using _Scripts._Game.General.Managers;

namespace _Scripts._Game.AI.AttackStateMachine.Flying.Bombdroid{
    
    public class BombDroidAIAttackStateMachine : AIAttackStateMachineBase
    {
        #region Bomb Droid Attack Stats
        [Header("Idle Properties")]
        [SerializeField]
        private float _bombDropCooldown;

        public float BombDropCooldown { get => _bombDropCooldown; }

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
        [Header("Bonded Idle Properties")]
        [SerializeField]
        private float _bondedBombDropCooldown;

        public float BondedBombDropCooldown { get => _bondedBombDropCooldown;  }

        [Header("Bonded Bomb drop attack properties")]
        [SerializeField]
        private float _bondedBombDropBuildUpDuration;
        [SerializeField]
        private float _bondedBombDropStateDuration;

        public float BondedBombDropBuildUpDuration { get => _bondedBombDropBuildUpDuration; }
        public float BondedBombDropStateDuration { get => _bondedBombDropStateDuration; }
        #endregion

        protected override void Awake()
        {
            base.Awake();

            //CurrentState = _states.GetState(AIAttackState.Idle);
            //CurrentBondedState = _states.GetBondedState(AIAttackState.Idle);
            // ai attacks
            States.AddState(AIAttackState.Idle, new BombDroidIdleAIAttackState(this, States));
            States.AddState(AIAttackState.Attack1, new BombDroidBombDropAIAttackState(this, States));
            //bonded attacks
            States.AddState(AIAttackState.Idle, new BombDroidIdleAIBondedAttackState(this, States));
            States.AddState(AIAttackState.Attack1, new BombDroidBombDropAIBondedAttackState(this, States));

            BondInputsDict.Add(PossessInput.Movement, OnMovementInput);
            BondInputsDict.Add(PossessInput.WButton, OnWestButtonInput);
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            if (GameStateManager.Instance?.IsGameRunning == false)
            {
                return;
            }

            if (!Entity.IsPossessed())
            {
                CurrentState.ManagedStateTick();
            }
            else
            {
                CurrentBondedState.ManagedStateTick();
            }

            if (IsAttackInterrupted)
            {
                IsAttackInterrupted = false;
            }
        }

        
    }

}
