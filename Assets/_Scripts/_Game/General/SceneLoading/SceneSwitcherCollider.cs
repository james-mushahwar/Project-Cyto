using _Scripts._Game.AI;
using _Scripts._Game.General.Managers;
using _Scripts._Game.Player;
using Assets._Scripts._Game.General.SceneLoading;
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
        private SceneField _customEnterScene;
        [SerializeField]
        private SceneField _customExitScene;

        [SerializeField]
        private UnityEvent<int, string> _collisionEnterEvent;
        [SerializeField]
        private UnityEvent<int, string> _collisionExitEvent;
        private void OnDrawGizmos()
        {
            // shows right direction of scene switcher collider
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + (5.0f * transform.right));
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + (-5.0f * transform.right));

            //draw collider bounds as cube
            BoxCollider2D box2D = GetComponent<Collider2D>() as BoxCollider2D;
            if (box2D)
            {
                Gizmos.color = new Color(0, 1, 0, 1.0f);
                Gizmos.DrawCube(transform.position + new Vector3(box2D.offset.x, box2D.offset.y, 0), new Vector3(box2D.size.x, box2D.offset.y, 10.0f));
            } 
        }

        private void Awake()
        {
            if (_collider == null)
            {
                _collider = GetComponent<Collider2D>();
            }
        }

        public void Init(SceneSwitcher ss, int index)
        {
            _sceneSwitcherRef = ss;
            if (_collider == null)
            {
                _collider = GetComponent<Collider2D>();
            }
            _colliderIndex = index;
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (GameStateManager.Instance.IsGameRunning == false)
            {
                return;
            }

            IPossessable possessable = collision.gameObject.GetComponent<IPossessable>();
            Rigidbody2D rb = null;
            if (possessable != null)
            {
                if (possessable is AIEntity)
                {
                    AIEntity aiEntity = (AIEntity)possessable;
                    if (aiEntity.IsPossessed() == false)
                    {
                        return;
                    }
                    rb = aiEntity.MovementSM.Rb;
                }
            }
            else
            {
                if (PlayerEntity.Instance.IsPossessed() == false)
                {
                    return;
                }
                rb = PlayerEntity.Instance.MovementSM.Rb;
            }

            if (rb != null)
            {
                float dotProduct = Vector3.Dot(rb.velocity.normalized, _collider.transform.right);

                string newSceneName = "";
                if (dotProduct > 0)
                {
                    if (_customEnterScene != null)
                    {
                        newSceneName = _customEnterScene.SceneName;
                    }
                    _collisionEnterEvent.Invoke(_colliderIndex, newSceneName);
                }
                else
                {
                    if (_customExitScene != null)
                    {
                        newSceneName = _customExitScene.SceneName;
                    }
                    _collisionExitEvent.Invoke(_colliderIndex, newSceneName);
                }
            }
        }
    }
    
}
