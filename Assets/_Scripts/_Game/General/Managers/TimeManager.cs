using System.Collections;
using UnityEngine;
using UnityEngine.Android;

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
        private float prePauseTimeScale = 1.0f;
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
            else
            {
                if (!PauseManager.Instance.IsPaused && (Time.timeScale > 1.0f || Time.timeScale < 1.0f))
                {
                    LogWarning("Time scale is messed up! Should be 1.0f but is " + Time.timeScale + " instead. Fixing now...");
                    Time.timeScale = 1.0f;
                }
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
                    if (PauseManager.Instance.IsPaused)
                    {
                        yield return null;
                    }
                    else
                    {
                        Time.timeScale = Mathf.Lerp(initialTimeScale, targetTimeScale, timer / easeIn);
                        timer += Time.unscaledDeltaTime;
                        yield return null;
                    }
                }
            }

            if (PauseManager.Instance.IsPaused)
            {
                yield return null;
            }
            Time.timeScale = targetTimeScale;

            if (delay > 0.0f)
            {
                float timer = delay;
                while (timer > 0.0f)
                {
                    if (PauseManager.Instance.IsPaused)
                    {
                        yield return null;
                    }
                    else
                    {
                        timer -= Time.unscaledDeltaTime;
                        yield return null;
                    }
                }
            }

            if (easeOut > 0.0f)
            {
                float initialTimeScale = Time.timeScale;
                float timer = 0.0f;
                while (Time.timeScale < 1.0f)
                {
                    if (PauseManager.Instance.IsPaused)
                    {
                        yield return null;
                    }
                    else
                    {
                        Time.timeScale = Mathf.Lerp(initialTimeScale, 1.0f, timer / easeOut);
                        timer += Time.unscaledDeltaTime;
                        yield return null;
                    }
                }
            }

            if (PauseManager.Instance.IsPaused)
            {
                yield return null;
            }
            else
            {
                Time.timeScale = 1.0f;
            }
            
            _timeImportance = ETimeImportance.Low;
            _timeScaleEnumerator = null;
        }

        public void TryRequestPauseGame(bool pause)
        {
            if (pause && _timeScaleEnumerator != null)
            {
                prePauseTimeScale = Time.timeScale;
            }
            else if (!pause && _timeScaleEnumerator == null)
            {
                prePauseTimeScale = 1.0f;
            }

            Time.timeScale = pause ? 0.0f : Mathf.Clamp(prePauseTimeScale, 0.0f, 1.0f);
        }


        #region Debug
        private void Log(string log)
        {
            Debug.Log("TimeManager: " + log);
        }

        private void LogWarning(string log)
        {
            Debug.LogWarning("TimeManager: " + log);
        }
        #endregion
    }

}
