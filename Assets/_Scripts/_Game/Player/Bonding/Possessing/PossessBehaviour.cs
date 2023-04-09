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
            throw new System.NotImplementedException();
        }

        public override void OnBondStopped()
        {
            throw new System.NotImplementedException();
        }

        public override void OnBondCompleted()
        {
            throw new System.NotImplementedException();
        }

        public override bool IsBeingBondedWith()
        {
            throw new System.NotImplementedException();
        }

        public override bool CanBeBonded()
        {
            throw new System.NotImplementedException();
        }
    }
    
}
