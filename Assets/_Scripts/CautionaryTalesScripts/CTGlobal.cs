using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace _Scripts.CautionaryTalesScripts{
    
    public static class CTGlobal
    {
        public static bool IsInSqDistanceRange(GameObject obj1, GameObject obj2, float sqRange)
        {
            Vector3 differenceToTarget = obj1.transform.position - obj2.transform.position;
            float distance = differenceToTarget.sqrMagnitude;

            return distance <= sqRange;
        }
    }
    
}
