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

        // Start is called before the first frame update
        void Start()
        {
            
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

        private void Update()
        {
            //if (_justLeft)
            //{
            //    _debugTimer -= Time.deltaTime;

            //    if (_debugTimer < 0)
            //    {
            //        _justLeft = false;
            //    }

            //    ContactPoint2D[] contacts = new ContactPoint2D[1];
            //    _cachedCollider2D.GetContacts(contacts);
            //    DrawArrow.ForPointsDebug(new Vector3(transform.position.x, transform.position.y, 0.0f), new Vector3(transform.position.x, transform.position.y + _cameraHeight, 0.0f));
            //}
        }
    }
    
}
