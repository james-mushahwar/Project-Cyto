using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts._Game.General.Managers;
using UnityEngine;

namespace _Scripts._Game.Audio{
    [Serializable]
    [CreateAssetMenu(menuName = "Audio/Audio Type")]
    public class AudioTypeScriptableObject : ScriptableObject
    {
        [SerializeField, HideInInspector]
        private EAudioType _audioType;
        [SerializeField] 
        private General.Managers.AudioConcurrency _audioConcurrency;

        [SerializeField]
        private AudioPlayback _audioPlayback;
        //[SerializeField] 
        //private ScriptableAudioPlayback _audioPlayback;

        public EAudioType AudioType
        {
            get { return _audioType; }
            set { _audioType = value; }
        }

        public General.Managers.AudioConcurrency Concurrency
        {
            get { return _audioConcurrency; }
        }

        public AudioPlayback AudioPlayback
        {
            get { return _audioPlayback; }
        }
    }
    
}
