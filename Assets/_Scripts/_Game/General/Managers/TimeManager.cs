using System.Collections;
using UnityEngine;

namespace _Scripts._Game.General.Managers {

    public enum ETimeImportance
    {
        Low, 
        High,
        Ultra
    }

    public class TimeManager : Singleton<TimeManager>
    {
        #region Current State
        private ETimeImportance _timeImportance;
        private IEnumerator _timeScaleEnumerator;
        #endregion

        private void FixedUpdate()
        {
            if (PauseManager.Instance.IsPaused)
            {
                return;
            }

            //if (_timeScaleEnumerator != null)
            //{
            //    _timeScaleEnumerator.MoveNext();
            //}
        }

        public void TryRequestTimeScale(ETimeImportance importance, float targetTimeScale, float easeIn = 0.0f, float easeOut = 0.0f, float delay = 0.0f)
        {
            int importanceInt = (int)importance;
            if (importanceInt < (int)_timeImportance)
            {
                // less relevant time importance
                return;
            }

            if (_timeScaleEnumerator != null)
            {
                StopCoroutine(_timeScaleEnumerator);
            }

            _timeImportance = importance;
            _timeScaleEnumerator = TickTimeScale(targetTimeScale, easeIn, easeOut, delay);
            StartCoroutine(_timeScaleEnumerator);
        }

        private IEnumerator TickTimeScale(float targetTimeScale, float easeIn = 0.0f, float easeOut = 0.0f, float delay = 0.0f)
        {
            if (easeIn > 0.0f)
            {
                float initialTimeScale = Time.timeScale;
                float timer = 0.0f;
                while (Time.timeScale > targetTimeScale)
                {
                    Time.timeScale = Mathf.Lerp(initialTimeScale, targetTimeScale, timer / easeIn);
                    timer += Time.unscaledDeltaTime;
                    yield return null;
                }
            }
            Time.timeScale = targetTimeScale;

            if (delay > 0.0f)
            {
                float timer = delay;
                while (timer > 0.0f)
                {
                    timer -= Time.unscaledDeltaTime;
                    yield return null;
                }
            }

            if (easeOut > 0.0f)
            {
                float initialTimeScale = Time.timeScale;
                float timer = 0.0f;
                while (Time.timeScale < 1.0f)
                {
                    Time.timeScale = Mathf.Lerp(initialTimeScale, 1.0f, timer / easeOut);
                    timer += Time.unscaledDeltaTime;
                    yield return null;
                }
            }
            Time.timeScale = 1.0f;

            _timeImportance = ETimeImportance.Low;
            _timeScaleEnumerator = null;
        }

        public void TryRequestPauseGame(bool pause)
        {
            if (pause && _timeScaleEnumerator != null)
            {
                StopCoroutine(_timeScaleEnumerator);
            }

            Time.timeScale = pause ? 0.0f : 1.0f;
        }

        public void OnExposedAIRequestTimeScale()
        {
            TryRequestTimeScale(ETimeImportance.Low, 0.25f, 0.0f, 0.1f, 0.1f);
        }

        public void OnPlayerTakeDamageRequestTimeScale()
        {
            TryRequestTimeScale(ETimeImportance.High, 0.0f, 0.0f, 0.025f, 0.2f);
        }
    }
    
}
