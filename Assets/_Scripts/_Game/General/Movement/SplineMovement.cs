using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

namespace _Scripts._Game.General.Movement{
    
    public class SplineMovement : MonoBehaviour
    {
        [SerializeField] 
        private GameObject _moveGO;
        [SerializeField] 
        private SplineContainer _spline; 

        private void Awake()
        {
            if (_moveGO == null)
            {
                _moveGO = gameObject;
            }
        }
    }
    
}
