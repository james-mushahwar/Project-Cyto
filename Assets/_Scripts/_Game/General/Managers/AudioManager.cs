using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.General.Managers {

    public class AudioManager : PoolComponentManager<AudioSource>
    {
        protected new void Awake()
        {
            base.Awake();

            foreach (AudioSource aSource in m_Pool)
            {
                aSource.playOnAwake = false;
            }
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        protected override bool IsActive(AudioSource component)
        {
            return component.isActiveAndEnabled || component.isPlaying;
        }

        AudioSource TryPlayAudioSourceAtLocation(AudioClip audioClip, Vector3 worldLoc)
        {
            AudioSource pooledComp = GetPooledComponent();

            if (pooledComp)
            {
                pooledComp.gameObject.transform.position = worldLoc;
                pooledComp.clip = audioClip;
                pooledComp.Play();
            }
            
            return pooledComp;
        }
    }
}
