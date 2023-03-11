using _Scripts._Game.General.Identification;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Scripts._Game.General{
    
    public class Waypoints : MonoBehaviour, IRuntimeId
    {
        private Transform[] _waypointTransforms;

        #region ID
        private RuntimeID _runtimeID;

        public RuntimeID RuntimeID { get => _runtimeID; }
        #endregion

        void Awake()
        {
            _runtimeID = GetComponent<RuntimeID>();
        }

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

        public Transform GetWaypoint(int index)
        {
            if (index >= _waypointTransforms.Length || index < 0)
            {
                return _waypointTransforms[0];
            }
            else 
            {
                return _waypointTransforms[index];
            }
        }
    }
    
}
