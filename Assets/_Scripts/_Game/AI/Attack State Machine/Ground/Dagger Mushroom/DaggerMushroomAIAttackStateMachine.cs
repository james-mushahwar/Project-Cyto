using UnityEngine;
using _Scripts._Game.AI.Bonding;
using _Scripts._Game.AI.AttackStateMachine;
using _Scripts._Game.General;

namespace _Scripts._Game.AI.AttackStateMachine.Ground.DaggerMushroom{
    
    public class DaggerMushroomAIAttackStateMachine : AIAttackStateMachineBase
    {
        #region #TBD# Attack Stats
    
        #endregion
    
        #region #TBD#Bonded Attack Stats
    
        #endregion
    
        protected override void Awake()
        {
            base.Awake();
    
            CurrentState = _states.GetState(AIAttackState.Idle);
    
            BondInputsDict.Add(PossessInput.Movement, OnMovementInput);
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
    
            if (IsAttackInterrupted)
            {
                IsAttackInterrupted = false;
            }
        }
    }
    
}
