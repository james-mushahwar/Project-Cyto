using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _Scripts._Game.General.Managers;

namespace _Scripts._Game.General{
    
    public class Target : MonoBehaviour, ITarget
    {
        public virtual Transform GetTargetTransform()
        {
            return transform;
        }
    }
    
}
