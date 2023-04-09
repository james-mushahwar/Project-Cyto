using _Scripts._Game.General.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.General.SceneLoading{
    
    public class AudioTrackSwitcher : MonoBehaviour
    {
        [SerializeField]
        private EAudioTrackTypes _type;
        [SerializeField]
        private float _delay = 0.0f;
        [SerializeField]
        private bool _fade = false;

        private AudioManager _audioManager;

        private void Start()
        {
            _audioManager = AudioManager.Instance as AudioManager;
        }

        public void PlayAudioTrack()
        {
            _audioManager.PlayAudio(_type, _fade, _delay);
        }

        public void StopAudioTrack()
        {
            _audioManager.StopAudio(_type, _fade, _delay);
        }
    }
    
}
