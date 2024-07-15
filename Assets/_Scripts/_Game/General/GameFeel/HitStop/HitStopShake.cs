using _Scripts._Game.General.Managers;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
        private Transform _transformToShake;

        [Header("Shake Properties")]
        [SerializeField]
        private float _shakeDuration;
        [SerializeField]
        private float _shakePreDelay;
        [SerializeField]
        private ShakeParameters _horizontalShake = new ShakeParameters(false);
        [SerializeField]
        private ShakeParameters _verticalShake = new ShakeParameters(false);

        [Header("Jolt properties")]
        [SerializeField]
        private Vector2 _attackJoltXYMagnitude;
        [SerializeField] 
        private float _attackJoltDuration;
        [SerializeField] 
        private Ease _attackJoltEase;

        private Vector3 _defaultSpriteLocalPosition;

        [Header("General")]
        private bool _displacementPlaying;
        private Tweener _offsetTweener;

        private void Awake()
        {
            _defaultSpriteLocalPosition = _transformToShake.localPosition;
        }

        private void OnDisable()
        {
            StopAllCoroutines();
            KillActiveTween(ref _offsetTweener);
        }

        public void PlayShake(GameObject instigator)
        {
            if (_displacementPlaying)
            {
                StopAllCoroutines();
                KillActiveTween(ref _offsetTweener);
            }
            
            StartCoroutine(Shake(instigator.transform.position));
        }

        private IEnumerator Shake(Vector3 direction)
        {
            _displacementPlaying = true;

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

                _transformToShake.localPosition = new Vector3(newXPosition, newYPosition, _transformToShake.position.z);
                yield return null;
            }

            _transformToShake.localPosition = Vector3.zero;
            _displacementPlaying = false;
        }

        public void PlayJolt(Vector3 direction)
        {
            if (_displacementPlaying)
            {
                StopAllCoroutines();
                KillActiveTween(ref _offsetTweener);
            }

            _displacementPlaying = true;
            Vector3 targetPosition = _defaultSpriteLocalPosition + new Vector3(direction.x * _attackJoltXYMagnitude.x, direction.y * _attackJoltXYMagnitude.y, 0.0f);
            _offsetTweener = DOVirtual.Vector3(_defaultSpriteLocalPosition, targetPosition, _attackJoltDuration,
                value =>
                {
                    _transformToShake.localPosition = value;
                }).SetEase(_attackJoltEase).OnComplete(() => _transformToShake.localPosition = _defaultSpriteLocalPosition);
        }

        private void KillActiveTween(ref Tweener tweener)
        {
            if (tweener != null)
            {
                if (tweener.IsActive())
                {
                    DOTween.Kill(tweener);
                    tweener = null;
                }
            }
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
