using UnityEngine;
using _Scripts._Game.AI.Bonding;
using _Scripts._Game.AI.AttackStateMachine;
using _Scripts._Game.General;
using _Scripts._Game.AI.AttackStateMachine.Flying.Bombdroid;

namespace _Scripts._Game.AI.AttackStateMachine.Bosses.GigaBombDroid{
    
    public class GigaBombDroidAIAttackStateMachine : BossAIAttackStateMachine
    {
        #region #TBD# Attack Stats
    
        #endregion
    
        #region #TBD#Bonded Attack Stats
    
        #endregion
    
        protected override void Awake()
        {
            base.Awake();

            // ai attacks
            States.AddState(AIAttackState.Idle, new GigaBombDroidIdleAIAttackState(this, States));
            //bonded attacks
            States.AddState(AIAttackState.Idle, new GigaBombDroidIdleAIBondedAttackState(this, States));

            BondInputsDict.Add(PossessInput.Movement, OnMovementInput);
            BondInputsDict.Add(PossessInput.WButton, OnWestButtonInput);

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
