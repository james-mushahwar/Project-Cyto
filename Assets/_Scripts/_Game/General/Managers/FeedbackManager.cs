using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts._Game.General.Managers{

    public enum EFeedbackPattern
    {
        UI_Touch,
        UI_Click,
        UI_Enter,
        UI_Exit,
        //Game - damage
        Game_BasicAttackLight,
        Game_BasicAttackHeavy,
        Game_TakeDamageLight,
        Game_TakeDamageHeavy,
        // Game - interaction
        Game_BondEnter,
        Game_BondExit,
        //Overrides
        Override_Freeze,
        Override_UnFreeze,
        None
    }

    [System.Serializable]
    public struct FFeedbackPattern
    {
        public AnimationCurve _lowFrequencyPattern; // from 0 to 1 scale
        public AnimationCurve _highFrequencyPattern; // from 0 to 1 scale
    }

    public class FeedbackManager : Singleton<FeedbackManager>
    {
        #region General
        private Gamepad _gamepad;
        private EFeedbackPattern _feedbackType;
        private FFeedbackPattern _feedbackPattern;
        private Coroutine _stopGamepadFeedback;
        private float _feedbackTimer;
        private float _feedbackDuration;
        #endregion

        [Header("Feedback patterns")]
        [SerializeField]
        private FFeedbackPattern _UITouchFeedback;
        [SerializeField]
        private FFeedbackPattern _BasicAttackLightFeedback;
        [SerializeField]
        private FFeedbackPattern _BasicAttackHeavyFeedback;

        protected override void Awake() 
        {
            base.Awake();
        }

        private void FixedUpdate()
        {
            _gamepad = Gamepad.current;

            if (_gamepad == null)
            {
                return;
            }

            TickFeedbackPattern();

        }

        private void TickFeedbackPattern()
        {
            if (_feedbackType == EFeedbackPattern.None)
            {
                return;
            }

            if (_feedbackTimer >= _feedbackDuration)
            {
                _feedbackType = EFeedbackPattern.None;
                _feedbackTimer = 0.0f;
                _gamepad.SetMotorSpeeds(0.0f, 0.0f);
            }
            else
            {
                float lowFreq = _feedbackPattern._lowFrequencyPattern.Evaluate(_feedbackTimer);
                float highFreq = _feedbackPattern._highFrequencyPattern.Evaluate(_feedbackTimer);
                _gamepad.SetMotorSpeeds(lowFreq, highFreq);
            }

            _feedbackTimer += Time.deltaTime;
        }

        public void TryFeedbackPattern(EDamageType damageType)
        {
            // convert dmaage type to pattern first
            EFeedbackPattern newPatternType = EFeedbackPattern.None;
            if (damageType == EDamageType.Player_BasicAttack)
            {
                //tbd
            }

            TryFeedbackPattern(newPatternType);
        }

        public void TryFeedbackPattern(EFeedbackPattern pattern)
        {
            _gamepad = Gamepad.current;

            if (_gamepad == null)
            {
                return;
            }

            if (!IsFeedbackValid(pattern))
            {
                return;
            }

            FFeedbackPattern newFeedback = GetFeedbackPattern(pattern);

            _feedbackPattern = newFeedback;
            _feedbackType = pattern;
            _feedbackTimer = 0.0f;
            float lowFreqLength = _feedbackPattern._lowFrequencyPattern.length > 0 ? _feedbackPattern._lowFrequencyPattern.keys[_feedbackPattern._lowFrequencyPattern.length - 1].time : 0.0f;
            float highFreqLength = _feedbackPattern._highFrequencyPattern.length > 0 ? _feedbackPattern._highFrequencyPattern.keys[_feedbackPattern._highFrequencyPattern.length - 1].time : 0.0f;
            _feedbackDuration = Mathf.Max(lowFreqLength, highFreqLength);

            _gamepad.SetMotorSpeeds(0.0f, 0.0f);
            //_stopGamepadFeedback = StartCoroutine(StopRumbleFeedback(duration, _gamepad));
        }

        private bool IsFeedbackValid(EFeedbackPattern pattern)
        {
            return (pattern != EFeedbackPattern.None && pattern != _feedbackType);
        }

        private FFeedbackPattern GetFeedbackPattern(EFeedbackPattern pattern)
        {
            if (pattern == _feedbackType)
            {
                return _feedbackPattern;
            }
            switch (pattern)
            {
                case EFeedbackPattern.Game_BasicAttackLight:
                    return _BasicAttackLightFeedback;
                case EFeedbackPattern.Game_BasicAttackHeavy:
                    return _BasicAttackHeavyFeedback;
                default:
                    return _feedbackPattern;
            }
        }

        public IEnumerator StopRumbleFeedback(float delay, Gamepad gamepad)
        {
            yield return TaskManager.Instance.WaitForSecondsPool.Get(delay);

            if (gamepad != null)
            {
                gamepad.SetMotorSpeeds(0.0f, 0.0f);
            }
        }
      
    }
    
}
