using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Scripts._Game.General{
    
    public class Waypoints : MonoBehaviour
    {
        private Transform[] _waypointTransforms;

        void Start()
        {
            _waypointTransforms = new Transform[GetComponentsInChildren<Transform>().Length - 1];

            int i = 0;
            foreach (Transform childTransform in transform)
            {
                _waypointTransforms[i] = childTransform;
                i++;
            }
        }
    }
    
}
