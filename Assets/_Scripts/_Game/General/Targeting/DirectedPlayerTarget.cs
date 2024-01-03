using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _Scripts._Game.General.Managers;
using _Scripts._Game.Player;

namespace _Scripts._Game.General.Targeting
{
    public class DirectedPlayerTarget : MonoBehaviour, ITarget
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
            return _centralTransform;
        }

        public void ManagedTargetTick()
        {
            //PlayerMovementStateMachine playerMovementStateMachine = PlayerMovementStateMachine.Instance;
            GameObject playerTarget = PlayerEntity.Instance?.GetControlledGameObject();

            Vector3 playerPosition = playerTarget.gameObject.transform.position;
            Vector3 newPosition = playerPosition + (Vector3)(_raycastDirection * _raycastDistance);

            RaycastHit2D hit = Physics2D.Raycast(playerPosition, _raycastDirection, _raycastDistance, _groundedLayer);
            if (hit.collider != null)
            {
                newPosition = hit.point;
            }

            _centralTransform.position = newPosition;
        }
    }

}
