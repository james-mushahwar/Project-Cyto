using _Scripts._Game.General.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.General.SceneLoading{
    
    public class AudioTrackSwitcher : MonoBehaviour
    {
        [Header("Left side audio track")]
        [SerializeField]
        private EAudioTrackTypes _leftType;
        [SerializeField]
        private float _leftDelay = 0.0f;
        [SerializeField]
        private bool _leftFade = false;

        [Header("Right side audio track")]
        [SerializeField]
        private EAudioTrackTypes _rightType;
        [SerializeField]
        private float _rightDelay = 0.0f;
        [SerializeField]
        private bool _rightFade = false;

        private AudioManager _audioManager;

        private void Start()
        {
            _audioManager = AudioManager.Instance as AudioManager;
        }

        public void PlayAudioTrack(bool left)
        {
            if (left)
            {
                _audioManager.PlayAudio(_leftType, _leftFade, _leftDelay);
            }
            else
            {
                _audioManager.PlayAudio(_rightType, _rightFade, _rightDelay);
            }
            
        }

        public void StopAudioTrack(bool left)
        {
            if (left)
            {
                _audioManager.StopAudio(_leftType, _leftFade, _leftDelay);
            }
            else
            {
                _audioManager.StopAudio(_rightType, _rightFade, _rightDelay);
            }
        }
    }
    
}
