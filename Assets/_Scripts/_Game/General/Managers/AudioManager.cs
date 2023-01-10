using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.General.Managers {

    public enum EAudioType
    {
        AC_TorchWoosh,
        COUNT
    }

    public class AudioManager : PoolComponentManager<AudioSource>
    {
        private static EAudioType[] _AudioTypes =
        {
            EAudioType.AC_TorchWoosh,
        };

        private string[] _AudioLocations;

        private Dictionary<EAudioType, string> _AudioTypeLocationsDict = new Dictionary<EAudioType, string>();

        private AudioClip[] _AudioClips = new AudioClip[(int) EAudioType.COUNT];

        protected override void Awake()
        {
            base.Awake();

            foreach (AudioSource aSource in m_Pool)
            {
                aSource.playOnAwake = false;
            }

            //_AudioLocations = GetValues(typeof(EAudioType));

            for (int i = 0; i < (int)EAudioType.COUNT; ++i)
            {
                _AudioTypeLocationsDict.Add((EAudioType)i, Enum.GetName(typeof(EAudioType), (EAudioType)i));
            }
        }

        protected override bool IsActive(AudioSource component)
        {
            return component.isPlaying;
        }

        public AudioSource TryPlayAudioSourceAtLocation(EAudioType audioType, Vector3 worldLoc)
        {
            AudioSource pooledComp = GetPooledComponent();

            if (pooledComp)
            {
                pooledComp.gameObject.transform.position = worldLoc;
                pooledComp.clip = (AudioClip)Resources.Load("Audio/SFX/" + _AudioTypeLocationsDict[audioType]);
                pooledComp.Play();
            }
            else
            {
                Debug.Log("No more pooled audio components");
            }
            
            return pooledComp;
        }
    }
}
