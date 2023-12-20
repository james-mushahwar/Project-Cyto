﻿using UnityEngine;
using _Scripts._Game.AI.Bonding;
using _Scripts._Game.AI.AttackStateMachine;
using _Scripts._Game.General;

namespace _Scripts._Game.AI.AttackStateMachine.Ground.MushroomArcher{
    
    public class MushroomArcherAIAttackStateMachine : AIAttackStateMachineBase
    {
        #region Mushroom Archer Attack Stats
    
        #endregion
    
        #region Mushroom Archer Bonded Attack Stats
    
        #endregion
    
        protected override void Awake()
        {
            base.Awake();
    
            CurrentState = _states.GetState(AIAttackState.Idle);
    
            BondInputsDict.Add(PossessInput.Movement, OnMovementInput);
        }
    
        protected override void FixedUpdate()
        {
            if (!Entity.IsPossessed())
            {
                CurrentState.ManagedStateTick();
            }
            else
            {
                //CurrentBondedState.ManagedStateTick();
            }
    
            if (IsAttackInterrupted)
            {
                IsAttackInterrupted = false;
            }
        }
    }
    
}
