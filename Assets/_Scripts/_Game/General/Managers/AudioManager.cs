using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

namespace _Scripts._Game.General.Managers {

    public enum EAudioType
    {
        AC_TorchWoosh,
        COUNT
    }

    public enum EAudioAction
    {
        START,
        STOP,
        RESTART,
        PAUSE,
    }

    public class AudioManager : PoolComponentManager<AudioSource>
    {
        private static EAudioType[] _AudioTypes =
        {
            EAudioType.AC_TorchWoosh,
        };

        private Dictionary<EAudioType, string> _AudioTypeLocationsDict = new Dictionary<EAudioType, string>();

        private AudioClip[] _AudioClips = new AudioClip[(int) EAudioType.COUNT];

        #region AudioTracks
        public AudioTrack[] _tracks;

        private Dictionary<EAudioType, AudioTrack> _audioTable; // relationship between audio types (key) and audio tracks (value)

        [System.Serializable]
        public class AudioObject
        {
            public EAudioType _type;
            public AudioClip _clip;
        }

        [System.Serializable]
        public class AudioTrack
        {
            public AudioSource _source;
            public AudioObject[] _audio;
        }
        #endregion

        #region AudioJobs
        private Dictionary<EAudioType, IEnumerator> _jobTable; // relationship between audio types (key) and jobs (value) (Coroutine, Ienumerator)

        private class AudioJob
        {
            public EAudioAction _action;
            public EAudioType _type;
            public bool _fade;
            public float _delay;

            public AudioJob(EAudioAction action, EAudioType type, bool fade = false, float delay = 0.0f)
            {
                _action = action;
                _type = type;
                _fade = fade;
                _delay = delay;
            }
        }

        #endregion

        protected override void Awake()
        {
            base.Awake();

            Log("PoolComponentManager<T> awake check");
            for (int i = 0; i < m_PoolCount; ++i)
            {
                GameObject newGO = new GameObject(gameObject.name + i);
                newGO.transform.parent = this.gameObject.transform;

                AudioSource comp = newGO.AddComponent(typeof(AudioSource)) as AudioSource;
                m_Pool.Push(comp);
            }

            foreach (AudioSource aSource in m_Pool)
            {
                aSource.playOnAwake = false;
            }

            for (int i = 0; i < (int)EAudioType.COUNT; ++i)
            {
                _AudioTypeLocationsDict.Add((EAudioType)i, Enum.GetName(typeof(EAudioType), (EAudioType)i));
            }

            _audioTable = new Dictionary<EAudioType, AudioTrack>();
            _jobTable = new Dictionary<EAudioType, IEnumerator>();
            GenerateAudioTable();
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
                Log("No more pooled audio components");
            }

            return pooledComp;
        }

        private void GenerateAudioTable()
        {
            foreach(AudioTrack audioTrack in _tracks)
            {
                foreach(AudioObject audioObj in audioTrack._audio)
                {
                    if (_audioTable.ContainsKey(audioObj._type))
                    {
                        LogWarning("Trying to register audio [" + audioObj._type + "] which is already registered.");
                    }
                    else
                    {
                        _audioTable.Add(audioObj._type, audioTrack);
                        Log("Registered audio [" + audioObj._type + "].");
                    }
                }
            }
        }

        private void AddJob(AudioJob job)
        {
            // remove conflicting jobs if any
            RemoveConflictingJob(job._type);

            // start new job
            IEnumerator _jobRunner = RunAudioJob(job);
            _jobTable.Add(job._type, _jobRunner);
            StartCoroutine(_jobRunner);
            Log("Starting job on [" + job._type + "] with operation: " + job._action);
        }

        private void RemoveConflictingJob(EAudioType type)
        {
            if (_jobTable.ContainsKey(type))
            {
                // same audio type is playing
                RemoveJob(type);
            }

            EAudioType conflictAudio = EAudioType.COUNT;
            foreach(KeyValuePair<EAudioType, IEnumerator> entry in _jobTable)
            {
                EAudioType audioType = entry.Key;
                AudioTrack audioTrackInUse = _audioTable[audioType];
                AudioTrack audioTrackNeeded = _audioTable[type];

                if (audioTrackNeeded._source == audioTrackInUse._source)
                {
                    //conflict found
                    conflictAudio = audioType;
                }
            }

            if (conflictAudio != EAudioType.COUNT)
            {
                RemoveJob(conflictAudio);
            }
        }

        private void RemoveJob(EAudioType type)
        {
            if (!_jobTable.ContainsKey(type))
            {
                LogWarning("Trying to stop a job [" + type + "] that is not running.");
                return;
            }

            IEnumerator runningJob = (IEnumerator)_jobTable[type];
            StopCoroutine(runningJob);
            _jobTable.Remove(type);
        }

        private IEnumerator RunAudioJob(AudioJob job)
        {
            yield return TaskManager.Instance.WaitForSecondsPool.Get(job._delay);

            AudioTrack track = _audioTable[job._type];
            track._source.clip = GetAudioClipFromAudioTrack(job._type, track);

            switch (job._action)
            {
                case EAudioAction.START:
                    track._source.Play();
                    break;
                case EAudioAction.STOP:
                    if (!job._fade)
                    {
                        track._source.Stop();
                    }
                    break;
                case EAudioAction.RESTART:
                    track._source.Stop();
                    track._source.Play();
                    break;
                case EAudioAction.PAUSE:
                    track._source.Pause();
                    break;

                default:
                    break;
            }

            if (job._fade)
            {
                float initial = job._action == EAudioAction.START || job._action == EAudioAction.RESTART ? 0.0f : 1.0f;
                float target = initial == 0.0f ? 1.0f : 0.0f;
                float duration = 1.0f;
                float timer = 0.0f;

                while (timer <= duration)
                {
                    track._source.volume = Mathf.Lerp(initial, target, timer / duration);
                    timer += Time.deltaTime;
                    yield return null;
                }
            }

            _jobTable.Remove(job._type);
            yield return null;
        }

        private AudioClip GetAudioClipFromAudioTrack(EAudioType type, AudioTrack track)
        {
            foreach(AudioObject audioObj in track._audio)
            {
                if (audioObj._type == type)
                {
                    return audioObj._clip;
                }
            }

            return null;
        }

        #region Debug
        private void Log(string log)
        {
            Debug.Log("AudioManager: " + log);
        }

        private void LogWarning(string log)
        {
            Debug.LogWarning("AudioManager: " + log);
        }
        #endregion
    }
}
