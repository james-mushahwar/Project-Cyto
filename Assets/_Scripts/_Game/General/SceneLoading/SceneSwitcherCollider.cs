using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace _Scripts._Game.General.SceneLoading{

    [RequireComponent(typeof(Collider2D))]
    public class SceneSwitcherCollider : MonoBehaviour
    {
        private SceneSwitcher _sceneSwitcherRef;
        private Collider2D _collider;
        private int _colliderIndex;

        [SerializeField]
        private UnityEvent<int> _collisionEnterEvent;
        [SerializeField]
        private UnityEvent<int> _collisionExitEvent;

        private void OnDrawGizmos()
        {
            // shows right direction of scene switcher collider
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + (5.0f * transform.right));

            //draw collider bounds as cube
            BoxCollider2D box2D = GetComponent<Collider2D>() as BoxCollider2D;
            if (box2D)
            {
                Gizmos.color = new Color(0, 1, 0, 1.0f);
                Gizmos.DrawCube(transform.position + new Vector3(box2D.offset.x, box2D.offset.y, 0), new Vector3(box2D.size.x, box2D.offset.y, 10.0f));
            } 
        }

        public void Init(SceneSwitcher ss, int index)
        {
            _sceneSwitcherRef = ss;
            _collider = GetComponent<Collider2D>();
            _colliderIndex = index;
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            PlayerMovementStateMachine playerMovement = collision.gameObject.GetComponent<PlayerMovementStateMachine>();
            if (playerMovement)
            {
                float dotProduct = Vector3.Dot(playerMovement.Rb.velocity.normalized, _collider.transform.right);
                if (dotProduct > 0)
                {
                    _collisionEnterEvent.Invoke(_colliderIndex);
                }
                else
                {
                    _collisionExitEvent.Invoke(_colliderIndex);
                }
                
            }
        }
    }
    
}
