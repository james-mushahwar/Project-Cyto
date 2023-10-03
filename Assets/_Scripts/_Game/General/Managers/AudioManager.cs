using System.Collections;
using System;
using System.Collections.Generic;
using _Scripts._Game.Audio;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.Audio;

namespace _Scripts._Game.General.Managers {

    public enum EAudioType
    {
        //SFX
        SFX_BasicAttack1,
        SFX_BasicAttack2,
        SFX_BasicAttack3,
        SFX_BasicAttack4,
        SFX_BasicAttackImpact1,
        SFX_BasicAttackImpact2,
        SFX_BasicAttackImpact3,
        SFX_BasicAttackImpact4,
        SFX_BasicAttackImpact5,
        SFX_BasicAttackImpactWithLiquid1,
        SFX_BasicAttackImpactWithLiquid2,
        SFX_BasicAttackImpactWithLiquid3,
        SFX_BasicAttackImpactWithLiquid4,
        SFX_BasicAttackImpactWithLiquid5,
        SFX_Exposed,
        SFX_Player_BondStart,
        SFX_Player_BondExit,
        SFX_Player_PossessStart,
        SFX_Player_Jump,
        SFX_Player_Dash,
        SFX_SpaceDoor_Open,
        // 
        COUNT
    }

    public enum EAudioTrackTypes
    {
        //Music
        Music_before,
        Music_clarity,
        Music_connecting,
        Music_Rest,
        Music_Signal,
        Music_wonders,
        //Ambience
        Ambience_Magic,
        Ambience_DarkMagic,
        Ambience_HolyMagic,
        Ambience_Cave,
        Ambience_Dungeon,
        Ambience_Spaceship1,
        Ambience_Spaceship2,
        COUNT
    }

    public enum EAudioAction
    {
        START,
        STOP,
        RESTART,
        PAUSE,
    }

    public class AudioManager : Singleton<AudioManager>
    {
        private static EAudioType[] _AudioTypes =
        {
            EAudioType.SFX_BasicAttack1,
            EAudioType.SFX_BasicAttack2,
            EAudioType.SFX_BasicAttack3,
            EAudioType.SFX_BasicAttack4,
            EAudioType.SFX_BasicAttackImpact1,
            EAudioType.SFX_BasicAttackImpact2,
            EAudioType.SFX_BasicAttackImpact3,
            EAudioType.SFX_BasicAttackImpact4,
            EAudioType.SFX_BasicAttackImpact5,
            EAudioType.SFX_BasicAttackImpactWithLiquid1,
            EAudioType.SFX_BasicAttackImpactWithLiquid2,
            EAudioType.SFX_BasicAttackImpactWithLiquid3,
            EAudioType.SFX_BasicAttackImpactWithLiquid4,
            EAudioType.SFX_BasicAttackImpactWithLiquid5,
            EAudioType.SFX_Exposed,
            EAudioType.SFX_Player_BondStart,
            EAudioType.SFX_Player_BondExit,
            EAudioType.SFX_Player_PossessStart,
            EAudioType.SFX_Player_Jump,
            EAudioType.SFX_Player_Dash,
            EAudioType.SFX_SpaceDoor_Open,
        };

        [SerializeField]
        private AudioSourcePool _audioSourcePool;

        private Dictionary<EAudioType, string> _audioTypeLocationsDict = new Dictionary<EAudioType, string>();

        [SerializeField]
        private AudioPlaybackDictionary _audioPlaybackDict = new AudioPlaybackDictionary();

        private AudioClip[] _audioClips = new AudioClip[(int) EAudioType.COUNT];

        [Header("SFX")]
        [SerializeField]
        private AudioMixerGroup _sfxMixerGroup;

        public AudioMixerGroup SFXMixerGroup { get => _sfxMixerGroup; }

        #region AudioTracks
        [SerializeField]
        private AudioTrack[] _tracks;

        private Dictionary<EAudioTrackTypes, AudioTrack> _audioTable; // relationship between audio types (key) and audio tracks (value)

        [System.Serializable]
        public class AudioObject
        {
            public EAudioTrackTypes _type;
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
        private Dictionary<EAudioTrackTypes, IEnumerator> _jobTable; // relationship between audio types (key) and jobs (value) (Coroutine, Ienumerator)

        private class AudioJob
        {
            public EAudioAction _action;
            public EAudioTrackTypes _type;
            public bool _fade;
            public float _delay;

            public AudioJob(EAudioAction action, EAudioTrackTypes type, bool fade = false, float delay = 0.0f)
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

            for (int i = 0; i < (int)EAudioType.COUNT; ++i)
            {
                _audioTypeLocationsDict.Add((EAudioType)i, Enum.GetName(typeof(EAudioType), (EAudioType)i));
            }

            _audioTable = new Dictionary<EAudioTrackTypes, AudioTrack>();
            _jobTable = new Dictionary<EAudioTrackTypes, IEnumerator>();
            GenerateAudioTable();
        }

        public AudioSource TryPlayAudioSourceAtLocation(EAudioType audioType, Vector3 worldLoc)
        {
            AudioSource pooledComp = _audioSourcePool.GetAudioSource();

            if (pooledComp)
            {
                // audio playback
                AdjustAudioPlayback(audioType, pooledComp);

                pooledComp.gameObject.transform.position = worldLoc;
                pooledComp.clip = (AudioClip)Resources.Load("Audio/SFX/" + _audioTypeLocationsDict[audioType]);
                pooledComp.Play();
            }
            else
            {
                LogWarning("No more pooled audio source components");
            }

            return pooledComp;
        }

        public AudioSource TryPlayAudioSourceAttached(EAudioType audioType, Transform attachTransform, Vector3 localPosition = default)
        {
            AudioSource pooledComp = _audioSourcePool.GetAudioSource();

            if (pooledComp)
            {
                // audio playback
                AdjustAudioPlayback(audioType, pooledComp);

                if (attachTransform != null)
                {
                    pooledComp.transform.parent = attachTransform;
                }
                pooledComp.gameObject.transform.position = localPosition;
                pooledComp.clip = (AudioClip)Resources.Load("Audio/SFX/" + _audioTypeLocationsDict[audioType]);
                pooledComp.Play();
            }
            else
            {
                LogWarning("No more pooled audio source components");
            }

            return pooledComp;
        }

        private void AdjustAudioPlayback(EAudioType audioType, AudioSource audioSource)
        {
            ScriptableAudioPlayback audioPlayback = null;

            _audioPlaybackDict.TryGetValue(audioType, out audioPlayback);

            if (audioPlayback != null)
            {
                audioSource.volume = audioPlayback.Volume;
            }
            else
            {
                audioSource.volume = 1.0f;
            }
        }

        public void PlayAudio(EAudioTrackTypes type, bool fade = false, float delay = 0.0f)
        {
            AddJob(new AudioJob(EAudioAction.START, type, fade, delay));
        }
        public void StopAudio(EAudioTrackTypes type, bool fade = false, float delay = 0.0f)
        {
            AddJob(new AudioJob(EAudioAction.STOP, type, fade, delay));
        }
        public void RestartAudio(EAudioTrackTypes type, bool fade = false, float delay = 0.0f)
        {
            AddJob(new AudioJob(EAudioAction.RESTART, type, fade, delay));
        }
        public void StopAllAudioTracks(bool fade = false, float delay = 0.0f)
        {
            //Debug.Log("Stop all audio tracks");
            foreach (AudioTrack track in _tracks)
            {
                foreach (AudioObject audio in track._audio)
                {
                    EAudioTrackTypes audioType = audio._type;
                    AddJob(new AudioJob(EAudioAction.STOP, audioType, fade, delay));
                }
                
            }
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
                        //Log("Registered audio [" + audioObj._type + "].");
                    }
                }
            }
        }

        private void AddJob(AudioJob job)
        {
            if (!IsNewJobValid(job))
            {
                // exit early - don't start job
                return;
            }
            // remove conflicting jobs if any
            RemoveConflictingJob(job._type);

            // start new job
            IEnumerator _jobRunner = RunAudioJob(job);
            _jobTable.Add(job._type, _jobRunner);
            StartCoroutine(_jobRunner);
            //Log("Starting job on [" + job._type + "] with operation: " + job._action);
        }

        private bool IsNewJobValid(AudioJob job)
        {
            bool isValid = true;

            if (job._action == EAudioAction.START)
            {
                if (_jobTable.ContainsKey(job._type))
                {
                }
            }
            

            return isValid;
        }

        private void RemoveConflictingJob(EAudioTrackTypes type)
        {
            if (_jobTable.ContainsKey(type))
            {
                // same audio type is playing
                RemoveJob(type);
            }

            EAudioTrackTypes conflictAudio = EAudioTrackTypes.COUNT;
            foreach(KeyValuePair<EAudioTrackTypes, IEnumerator> entry in _jobTable)
            {
                EAudioTrackTypes audioType = entry.Key;
                AudioTrack audioTrackInUse = _audioTable[audioType];
                AudioTrack audioTrackNeeded = _audioTable[type];

                if (audioTrackNeeded._source == audioTrackInUse._source)
                {
                    //conflict found
                    conflictAudio = audioType;
                }
            }

            if (conflictAudio != EAudioTrackTypes.COUNT)
            {
                RemoveJob(conflictAudio);
            }
        }

        private void RemoveJob(EAudioTrackTypes type)
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

        private AudioClip GetAudioClipFromAudioTrack(EAudioTrackTypes type, AudioTrack track)
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
