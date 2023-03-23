using System.Collections;
using System.Collections.Generic;
using _Scripts._Game.General.Managers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace _Scripts._Game.AI{
    
    public class KnockbackFeedback : MonoBehaviour
    {
        #region General
        private Rigidbody2D _rb;
        private bool _feedbackPlaying;
        #endregion

        #region Knockback Properties
        [Header("Properties")]
        [SerializeField] 
        private float _knockbackPreDelay;
        [SerializeField]
        private float _knockbackPostDelay;
        [SerializeField] 
        private float _knockbackForce;
        #endregion

        #region Knockback Events
        [Header("Events")] 
        [SerializeField] 
        private UnityEvent _beginKnockbackEvent;

        [SerializeField]
        private UnityEvent _endKnockbackEvent;
        #endregion

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();

        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        public void PlayFeedback(GameObject instigator)
        {
            if (_feedbackPlaying)
            {
                return;
            }
            StopAllCoroutines();
            
            StartCoroutine(Feedback(instigator.transform.position));
        }

        private IEnumerator Feedback(Vector3 position)
        {
            _feedbackPlaying = true;
            _beginKnockbackEvent.Invoke();

            if (_knockbackPreDelay > 0.0f)
            {
                yield return TaskManager.Instance.WaitForSecondsPool.Get(_knockbackPreDelay);
            }

            //Vector2 direction = (transform.position - position).normalized;
            Vector2 direction = Vector2.right;
            _rb.velocity = Vector2.zero;
            _rb.AddForce(direction * _knockbackForce, ForceMode2D.Impulse);

            if (_knockbackPostDelay > 0.0f)
            {
                yield return TaskManager.Instance.WaitForSecondsPool.Get(_knockbackPostDelay);
            }

            _endKnockbackEvent.Invoke();
            _feedbackPlaying = false;
        }

        #region Debug
        private void Log(string log)
        {
            Debug.Log("Knockback Effect - " + gameObject.name + ": " + log);
        }

        private void LogWarning(string log)
        {
            Debug.LogWarning("Knockback Effect - " + gameObject.name + ": " + log);
        }
        #endregion
    }

}
