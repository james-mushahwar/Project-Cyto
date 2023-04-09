using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.General{

    public abstract class BondBehaviour : MonoBehaviour
    {
        public Transform Transform { get; }
        public abstract EBondBehaviourType BondBehaviourType { get; }

        public abstract void OnBondStarted();
        public abstract void OnBondStopped();
        public abstract void OnBondCompleted();

        public abstract bool IsBeingBondedWith(); // is the player currently in a bond transition with this?
        public abstract bool CanBeBonded();
    }

}
