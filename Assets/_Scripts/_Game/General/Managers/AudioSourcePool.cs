using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.General.Managers{
    
    public class AudioSourcePool : PoolComponentManager<AudioSource>
    {
        protected override void Awake()
        {
            base.Awake();

            Debug.Log("Audio source pool is created");
            for (int i = 0; i < m_PoolCount; ++i)
            {
                GameObject newGO = new GameObject(gameObject.name + i);
                newGO.transform.parent = this.gameObject.transform;

                AudioSource comp = newGO.AddComponent(typeof(AudioSource)) as AudioSource;
                comp.volume = 1.0f;
                //comp.outputAudioMixerGroup = AudioManager.Instance.SFXMixerGroup;
                m_Pool.Push(comp);
            }

            foreach (AudioSource aSource in m_Pool)
            {
                aSource.playOnAwake = false;
            }
        }
        protected override bool IsActive(AudioSource component)
        {
             return component.isPlaying;
        }

        public AudioSource GetAudioSource()
        {
            return GetPooledComponent();
        }
    }
    
}
