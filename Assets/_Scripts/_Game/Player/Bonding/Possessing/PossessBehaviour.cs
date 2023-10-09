using System.Collections;
using System.Collections.Generic;
using _Scripts._Game.General;
using UnityEngine;

namespace _Scripts._Game.Player.Bonding.Possessing{
    
    public class PossessBehaviour : BondBehaviour
    {
        public override EBondBehaviourType BondBehaviourType
        {
            get => EBondBehaviourType.Possess; 
        }

        public override void OnBondStarted()
        {
             
        }

        public override void OnBondStopped()
        {
             
        }

        public override void OnBondCompleted()
        {
             
        }

        public override bool IsBeingBondedWith()
        {
            return false;
        }

        public override bool CanBeBonded()
        {
            return false;
        }
    }
    
}
