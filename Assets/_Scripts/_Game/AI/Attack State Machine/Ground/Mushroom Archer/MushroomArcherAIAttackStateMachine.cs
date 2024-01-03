using UnityEngine;
using _Scripts._Game.AI.Bonding;
using _Scripts._Game.AI.AttackStateMachine;
using _Scripts._Game.General;
using _Scripts._Game.AI.AttackStateMachine.Flying.Bombdroid;

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

            States.AddState(AIAttackState.Idle, new MushroomArcherIdleAIAttackState(this, States));
            States.AddState(AIAttackState.Attack1, new MushroomArcherGhostFireAIAttackState(this, States));
            States.AddState(AIAttackState.Attack2, new MushroomArcherChargeFireAIAttackState(this, States));

            States.AddState(AIAttackState.Idle, new MushroomArcherIdleAIBondedAttackState(this, States));
            States.AddState(AIAttackState.Attack1, new MushroomArcherGhostFireAIBondedAttackState(this, States));
            States.AddState(AIAttackState.Attack2, new MushroomArcherChargeFireAIBondedAttackState(this, States));

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
