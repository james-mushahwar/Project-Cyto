using _Scripts._Game.General.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.General.GameFeel.HitStop{
    
    public class HitStopShake : MonoBehaviour
    {
        [System.Serializable]
        private struct ShakeParameters
        {
            public float _rate;
            public float _displacement;
            public Vector2 _dampenOverTime;

            public ShakeParameters(bool b)
            {
                _rate = 0.0f;
                _displacement = 0.0f;
                _dampenOverTime = Vector2.one; 
            }
        }

        [Header("Components")]
        [SerializeField]
        private SpriteRenderer _spriteRenderer;

        [Header("Properties")]
        [SerializeField]
        private float _shakeDuration;
        [SerializeField]
        private float _shakePreDelay;
        [SerializeField]
        private ShakeParameters _horizontalShake = new ShakeParameters(false);
        [SerializeField]
        private ShakeParameters _verticalShake = new ShakeParameters(false);

        [Header("General")]
        private bool _shakePlaying;

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        public void PlayShake(GameObject instigator)
        {
            if (_shakePlaying)
            {
                StopAllCoroutines();
            }
            
            StartCoroutine(Shake(instigator.transform.position));
        }

        private IEnumerator Shake(Vector3 direction)
        {
            _shakePlaying = true;

            if (_shakePreDelay > 0.0f)
            {
                yield return TaskManager.Instance.WaitForSecondsPool.Get(_shakePreDelay);
            }

            //Vector2 direction = (transform.position - position).normalized;
            float shakeTimer = 0.0f;

            bool displaceHorz = (_horizontalShake._displacement != 0.0f && _horizontalShake._rate != 0.0f);
            bool displaceVert = (_verticalShake._displacement != 0.0f && _verticalShake._rate != 0.0f);

            while (shakeTimer < _shakeDuration)
            {
                shakeTimer += Time.deltaTime;
                float alphaXDampen = Mathf.SmoothStep(_horizontalShake._dampenOverTime.x, _horizontalShake._dampenOverTime.y, shakeTimer / _shakeDuration);
                float newXPosition = displaceHorz ? Mathf.Sin(shakeTimer * _horizontalShake._rate) * _horizontalShake._displacement * alphaXDampen : 0.0f;
                
                float alphaYDampen = Mathf.SmoothStep(_verticalShake._dampenOverTime.x, _verticalShake._dampenOverTime.y, shakeTimer / _shakeDuration);
                float newYPosition = displaceVert ? Mathf.Sin(shakeTimer * _verticalShake._rate) * _verticalShake._displacement * alphaYDampen : 0.0f;

                _spriteRenderer.gameObject.transform.localPosition = new Vector3(newXPosition, newYPosition, _spriteRenderer.gameObject.transform.position.z);
                yield return null;
            }

            _spriteRenderer.gameObject.transform.localPosition = Vector3.zero;
            _shakePlaying = true;
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
