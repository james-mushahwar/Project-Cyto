using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.Audio{
    [CreateAssetMenu(fileName = "AudioPlayback_SFX_", menuName = "Audio/Audio Playback")]
    public class ScriptableAudioPlayback : ScriptableObject
    {
        [SerializeField] 
        private float _volume;

        public float Volume
        {
            get { return _volume; }
        }
    }
    
}
