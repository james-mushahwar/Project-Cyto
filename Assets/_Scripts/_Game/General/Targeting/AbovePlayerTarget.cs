using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _Scripts._Game.General.Managers;

namespace _Scripts._Game.General.Targeting{
    
    public class AbovePlayerTarget : MonoBehaviour, ITarget
    {
        [SerializeField]
        private ETargetType _targetType;

        public ETargetType TargetType { get => _targetType; }

        [SerializeField]
        private Transform _leftTransform;
        [SerializeField]
        private Transform _centralTransform;
        [SerializeField]
        private Transform _rightTransform;

        [Header("Raycast Properties")]
        [SerializeField]
        private Vector2 _raycastDirection;
        [SerializeField]
        private float _raycastDistance;
        [SerializeField]
        private LayerMask _groundedLayer;

        public Transform GetTargetTransform()
        {
            PlayerMovementStateMachine playerMovementStateMachine = PlayerMovementStateMachine.Instance;

            Vector3 playerPosition = playerMovementStateMachine.gameObject.transform.position;
            Vector3 newPosition = transform.position + (Vector3)(_raycastDirection * _raycastDistance);

            RaycastHit2D hit = Physics2D.Raycast(transform.position, _raycastDirection, _raycastDistance, _groundedLayer);
            if (hit.collider != null)
            {
                newPosition = hit.collider.transform.position;
            }

            _centralTransform.position = newPosition;

            return _centralTransform;
        }
    }
    
}
