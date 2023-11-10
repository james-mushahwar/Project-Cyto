using System.Collections;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Numerics;
using _Scripts._Game.Audio;
using _Scripts._Game.Audio.AudioConcurrency;
using _Scripts._Game.Player;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.Audio;
using Vector3 = UnityEngine.Vector3;

namespace _Scripts._Game.General.Managers {

    public enum EAudioType
    {
        //SFX

        //general code = 0000

        //player code = 1000
        SFX_Player_BasicAttack1                            = 1001,
        SFX_Player_BasicAttack2                            ,
        SFX_Player_BasicAttack3                            ,
        SFX_Player_BasicAttack4                            ,
        SFX_Player_BasicAttackImpact1                      ,
        SFX_Player_BasicAttackImpact2                      ,
        SFX_Player_BasicAttackImpact3                      ,
        SFX_Player_BasicAttackImpact4                      ,
        SFX_Player_BasicAttackImpact5                      ,
        SFX_Player_BasicAttackImpactWithLiquid1            ,
        SFX_Player_BasicAttackImpactWithLiquid2            ,
        SFX_Player_BasicAttackImpactWithLiquid3            ,
        SFX_Player_BasicAttackImpactWithLiquid4            ,
        SFX_Player_BasicAttackImpactWithLiquid5            ,
        SFX_Player_Exposed                                 ,
        SFX_Player_BondStart                               ,
        SFX_Player_BondExit                                ,
        SFX_Player_PossessStart                            ,
        SFX_Player_Jump                                    ,
        SFX_Player_Dash                                    ,
                                                   
        // enemies code = 2000                              
        SFX_Enemy_BombDroid_ChargeBombAttack               = 2001,

        //environment = 3000                          
        SFX_SpaceDoor_Open                                 = 3001,
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

    public enum EAudioPriority
    {
        None = 0,
        Low = 1,
        Medium = 2,
        High = 3,
    }

    public enum EAudioConcurrencyRule
    {
        PreventNew,                         // don't play if > max concurrency
        StopOldest,                         // stop oldest, then play new
        StopFarthestThenPreventNew,         // stop farthest, if all same distance then preventnew
        StopFarthestThenOldest,             // stop farthest, or oldest
        //StopQuietest,                       // stop quietest to listener, then play new
        //StopLowestPriority,                 // stop loewest priority, then play new
        StopLowestPriorityThenOldest,       // stop loewest priority, if all same then stop oldest, then play new
        StopLowestPriorityThenPreventNew    // stop lowest priority and play new, if all same priority preventnew
    }

    [Serializable]
    public class AudioTypeConcurrency
    {
        [HideInInspector]
        public EAudioType _audioType;
        [SerializeField]
        public int _maxConcurrentAudioType = 2; // maximum audio clips of one type played at once
        [SerializeField]
        public EAudioConcurrencyRule _maxAudioTypeConcurrencyRule = EAudioConcurrencyRule.StopOldest; // rule when max concurrent audio type is exceeded
        [SerializeField]
        public EAudioPriority _priority = EAudioPriority.Low;
    }

    [Serializable]
    public class AudioConcurrency
    {
        [SerializeField] 
        public AudioTypeConcurrency _audioTypeConcurrency;
        [SerializeField] 
        public AudioConcurrencyGroupSO _audioConcurrencyGroup;
    }

    [Serializable]
    public class AudioHandler
    {
        [HideInInspector]
        public bool _active;                                 // is handle active with or without an audiosource
        [HideInInspector]
        public bool _release;                                // is this handle marked to be released = release Ausiosource and mark active = false
        [HideInInspector]
        public AudioSource _audioSource;

        private GameObject _owner;
        [SerializeField]
        private AudioHandleParameters _handleParametersSO;    // what parameters does this audiohandle share?
        private bool _loops;                                  // does this handle loop

        public AudioHandleParameters HandleParameters
        {
            get { return _handleParametersSO; }
        }

        public bool Loops
        {
            get { return _loops; }
        }

        public GameObject Owner
        {
            get { return _owner; }
            set { _owner = value; }
        }

        public AudioHandler(bool loops, GameObject owner)
        {
            _loops = loops;
            _owner = owner;
        }
    }

    [Serializable]
    [CreateAssetMenu(menuName = "Audio/Audio Handles/Range Group")]
    public class AudioHandleParameters : ScriptableObject
    {
        public float _startDistance;
        public float _stopDistance;
    }

    public class AudioManager : Singleton<AudioManager>, IManager
    {
        private static EAudioType[] _AudioTypes =
        {
            //player
            EAudioType.SFX_Player_BasicAttack1,
            EAudioType.SFX_Player_BasicAttack2,
            EAudioType.SFX_Player_BasicAttack3,
            EAudioType.SFX_Player_BasicAttack4,
            EAudioType.SFX_Player_BasicAttackImpact1,
            EAudioType.SFX_Player_BasicAttackImpact2,
            EAudioType.SFX_Player_BasicAttackImpact3,
            EAudioType.SFX_Player_BasicAttackImpact4,
            EAudioType.SFX_Player_BasicAttackImpact5,
            EAudioType.SFX_Player_BasicAttackImpactWithLiquid1,
            EAudioType.SFX_Player_BasicAttackImpactWithLiquid2,
            EAudioType.SFX_Player_BasicAttackImpactWithLiquid3,
            EAudioType.SFX_Player_BasicAttackImpactWithLiquid4,
            EAudioType.SFX_Player_BasicAttackImpactWithLiquid5,
            EAudioType.SFX_Player_Exposed,
            EAudioType.SFX_Player_BondStart,
            EAudioType.SFX_Player_BondExit,
            EAudioType.SFX_Player_PossessStart,
            EAudioType.SFX_Player_Jump,
            EAudioType.SFX_Player_Dash,
            //enemy
            EAudioType.SFX_Enemy_BombDroid_ChargeBombAttack,
            //environment
            EAudioType.SFX_SpaceDoor_Open,
        };

        [SerializeField]
        private AudioSourcePool _audioSourcePool;

        private Dictionary<EAudioType, string> _audioTypeLocationsDict = new Dictionary<EAudioType, string>();

        //[SerializeField]
        //private AudioPlaybackDictionary _audioPlaybackDict = new AudioPlaybackDictionary();

        private AudioClip[] _audioClips = new AudioClip[(int) EAudioType.COUNT];

        [Header("SFX")]
        [SerializeField]
        private AudioMixerGroup _sfxMixerGroup;

        public AudioMixerGroup SFXMixerGroup { get => _sfxMixerGroup; }

        [Header("Audio Concurrency")] 
        //[SerializeField]
        //private AudioConcurrencyDictionary _audioConcurrencyDictionary = new AudioConcurrencyDictionary();
        private Dictionary<EAudioType, AudioTypeScriptableObject> _audioTypeSODict = new Dictionary<EAudioType, AudioTypeScriptableObject>();

        private Dictionary<EAudioType, List<AudioSource>> _playingAudioTypeAudioSourcesDict = new Dictionary<EAudioType, List<AudioSource>>();

        private Dictionary<AudioConcurrencyGroupSO, List<AudioSource>> _playingConcurrencyGroupedAudioSourcesDict = new Dictionary<AudioConcurrencyGroupSO, List<AudioSource>>();

        private Dictionary<AudioSource, AudioConcurrency> _playingAudioSourceConcurrencyTypeDict = new Dictionary<AudioSource, AudioConcurrency>();
        
        public Dictionary<AudioSource, AudioConcurrency> PlayingAudioSourceConcurrencyTypeDict
        {
            get { return _playingAudioSourceConcurrencyTypeDict; }
        }

        [Header("Audio Handlers")] 
        private HashSet<AudioHandler> _activeAudioHandlers;
            //private Dictionary<AudioSource, AudioHandler> _audioSourceHandleDictionary = new Dictionary<AudioSource, AudioHandler>();
        //private Dictionary<AudioSource, AudioHandler> _allocatedAudioSourceHandlesDict = new Dictionary<AudioSource, AudioHandler>();

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

            // audio types
            List<EAudioType> audioTypes = new List<EAudioType>();

            for (int i = 0; i < (int)EAudioType.COUNT; i++)
            {
                if (Enum.IsDefined(typeof(EAudioType), i))
                {
                    audioTypes.Add((EAudioType)i);
                }
            }

            UnityEngine.Object[] audioTypeSOAssets = Resources.LoadAll("Audio/Audio Type/");

            for (int i = 0; i < audioTypes.Count; ++i)
            {
                EAudioType audioType = audioTypes[i];
                _audioTypeLocationsDict.Add(audioType, Enum.GetName(typeof(EAudioType), audioType));

                foreach (UnityEngine.Object script in audioTypeSOAssets)
                {
                    if (script.GetType() == typeof(AudioTypeScriptableObject))
                    {
                        AudioTypeScriptableObject audioTypeSO = (AudioTypeScriptableObject)script;
                        if (audioTypeSO)
                        {
                            if (audioTypeSO.AudioType == audioType)
                            {
                                _audioTypeSODict.Add(audioType, audioTypeSO);
                                break;
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }
                }

                List<AudioSource> sources = new List<AudioSource>();
                _playingAudioTypeAudioSourcesDict.Add(audioType, sources);

                AudioTypeScriptableObject _audioTypeSO = null;
                bool found = _audioTypeSODict.TryGetValue(audioType, out _audioTypeSO);
                if (found)
                {
                    AudioConcurrency concurrency = _audioTypeSO.Concurrency;

                    concurrency._audioTypeConcurrency._audioType = audioType;
                    List<AudioSource> groupSources = new List<AudioSource>();
                    _playingConcurrencyGroupedAudioSourcesDict.TryAdd(concurrency._audioConcurrencyGroup, groupSources);
                }
            }

            _audioTable = new Dictionary<EAudioTrackTypes, AudioTrack>();
            _jobTable = new Dictionary<EAudioTrackTypes, IEnumerator>();
            GenerateAudioTable();
        }

        public void ManagedTick()
        {
            // check handles
            foreach(AudioHandler audioHandler in _activeAudioHandlers)
            {
                if (audioHandler == null)
                {
                    _activeAudioHandlers.Remove(audioHandler);
                }

                if (audioHandler._release == false)
                {

                }
            }
        }

        public void PreInGameLoad()
        {
            _audioSourcePool.CleanAudioSourcePool();
        }

        public void PostInGameLoad()
        {
            
        }

        public void PreMainMenuLoad()
        {
            _audioSourcePool.CleanAudioSourcePool();
        }

        public void PostMainMenuLoad()
        {
        }

        public void RegisterPooledAudioSource(AudioSource audioSource)
        {
            // concurrency
            AudioConcurrency concurrency = null;

            bool added = _playingAudioSourceConcurrencyTypeDict.TryAdd(audioSource, concurrency);
            if (!added)
            {
                _playingAudioSourceConcurrencyTypeDict[audioSource] = concurrency;
            }

            // audio handler
            //AudioHandler handler = new AudioHandler();

            //added = _audioSourceHandleDictionary.TryAdd(audioSource, handler);
            //if (!added)
            //{
            //    _audioSourceHandleDictionary[audioSource] = handler;
            //}
        }

        public void CleanConcurrency(AudioSource audioSource)
        {
            // remove audio source from active playing dictionary lists.
            if (audioSource)
            {
                bool found = _playingAudioSourceConcurrencyTypeDict.TryGetValue(audioSource, out AudioConcurrency concurrency);

                if (found && concurrency != null)
                {
                    List<AudioSource> groupAudioSources;
                    _playingConcurrencyGroupedAudioSourcesDict.TryGetValue(concurrency._audioConcurrencyGroup, out groupAudioSources);
                    List<AudioSource> audioTypeAudioSources;
                    _playingAudioTypeAudioSourcesDict.TryGetValue(concurrency._audioTypeConcurrency._audioType, out audioTypeAudioSources);

                    groupAudioSources.Remove(audioSource);
                    audioTypeAudioSources.Remove(audioSource);
                }
            }
        }

        public AudioHandler TryPlayAudioSourceAtLocation(EAudioType audioType, Vector3 worldLoc, AudioHandler audioHandler = null)
        {
            AudioSource pooledComp = _audioSourcePool.GetAudioSource();

            //AudioHandler audioHandler = null;

            if (pooledComp)
            {
                // check concurrency rules
                bool canPlay = ResolveAudioConcurrency(audioType, pooledComp);
                if (!canPlay)
                {
                    return null;
                }

                if (RequestActivateHandle(audioHandler))
                {
                    audioHandler._active = true;
                    audioHandler._audioSource = pooledComp;
                    audioHandler._release = false;
                    _activeAudioHandlers.Add(audioHandler);
                }

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

            return audioHandler;
        }

        public AudioHandler TryPlayAudioSourceAttached(EAudioType audioType, Transform attachTransform, AudioHandler audioHandler = null, Vector3 localPosition = default)
        {
            AudioSource pooledComp = _audioSourcePool.GetAudioSource();

            //AudioHandler audioHandler = null;

            if (pooledComp)
            {
                // check concurrency rules
                bool canPlay = ResolveAudioConcurrency(audioType, pooledComp);
                if (!canPlay)
                {
                    return null;
                }

                if (RequestActivateHandle(audioHandler))
                {
                    audioHandler._active = true;
                    audioHandler._audioSource = pooledComp;
                    audioHandler._release = false;
                    _activeAudioHandlers.Add(audioHandler);
                }


                // audio playback
                AdjustAudioPlayback(audioType, pooledComp);

                if (attachTransform != null)
                {
                    pooledComp.transform.parent = attachTransform;
                }

                pooledComp.gameObject.transform.localPosition = localPosition;
                pooledComp.clip = (AudioClip)Resources.Load("Audio/SFX/" + _audioTypeLocationsDict[audioType]);
                pooledComp.Play();
            }
            else
            {
                LogWarning("No more pooled audio source components");
            }

            return audioHandler;
        }

        private bool RequestActivateHandle(AudioHandler audioHandler)
        {
            if (audioHandler == null)
            {
                return false;
            }

            if (audioHandler._active)
            {
                // audiohandler is already in use
                return false;
            }

            if (_activeAudioHandlers.Contains(audioHandler))
            {
                // audiohandler already in active use
                return false;
            }

            return true;
        }

        public bool RequestReleaseHandle(AudioHandler audioHandler)
        {
            if (audioHandler == null)
            {
                return false;
            }

            if (!_activeAudioHandlers.Contains(audioHandler))
            {
                return false;
            }

            audioHandler._release = true;

            return true;
        }

        private bool ResolveAudioConcurrency(EAudioType audioType, AudioSource audioSource)
        {
            bool play = true;
            AudioTypeScriptableObject _audioTypeSO = null;

            if (_audioTypeSODict.TryGetValue(audioType, out _audioTypeSO))
            {
                AudioConcurrency concurrency = _audioTypeSO.Concurrency;

                bool groupPassed = true;
                bool audioTypePassed = true;

                // check if concurrency group rule is needed
                int concGroupLimit = concurrency._audioConcurrencyGroup.GetMaxConcurrentAudioGroup();
                int activeGroupASCount = 0;
                _playingConcurrencyGroupedAudioSourcesDict.TryGetValue(concurrency._audioConcurrencyGroup, out List<AudioSource> groupAS);
                bool useGroupRule = false;
                if (groupAS != null)
                {
                    activeGroupASCount = groupAS.Count;
                }

                useGroupRule = activeGroupASCount >= concGroupLimit;

                groupPassed = !useGroupRule;

                //check if individual audio type is at a limit
                int concAudioTypeLimit = concurrency._audioTypeConcurrency._maxConcurrentAudioType;
                int activeAudioTypeASCount = 0;
                _playingAudioTypeAudioSourcesDict.TryGetValue(audioType, out List<AudioSource> typeAS);
                bool useAudioTypeRule = false;
                if (typeAS != null)
                {
                    activeAudioTypeASCount = typeAS.Count;
                }


                useAudioTypeRule = activeAudioTypeASCount >= concGroupLimit;

                audioTypePassed = !useAudioTypeRule;

                if (!audioTypePassed)
                {
                    // resolve audio type first if possible
                    bool audioTypeResolved = false;

                    audioTypeResolved = ResolveConcurrencyRule(audioType, concurrency, false);

                    audioTypePassed = audioTypeResolved;
                }

                if (!groupPassed && audioTypePassed)
                {
                    bool groupResolved = false;

                    groupResolved = ResolveConcurrencyRule(audioType, concurrency, true);

                    groupPassed = groupResolved;
                }

                play = groupPassed && audioTypePassed;

                if (play && audioSource != null)
                {
                    bool added = _playingAudioSourceConcurrencyTypeDict.TryAdd(audioSource, concurrency);
                    if (!added)
                    {
                        _playingAudioSourceConcurrencyTypeDict[audioSource] = concurrency;
                    }
                   
                    List<AudioSource> playingTypeSources = null;
                    _playingAudioTypeAudioSourcesDict.TryGetValue(audioType, out playingTypeSources);
                    if (playingTypeSources != null)
                    {
                        _playingAudioTypeAudioSourcesDict[audioType].Add(audioSource);
                    }

                    List<AudioSource> playingGroupSources = null;
                    _playingConcurrencyGroupedAudioSourcesDict.TryGetValue(concurrency._audioConcurrencyGroup, out playingGroupSources);
                    if (playingGroupSources != null)
                    {
                        _playingConcurrencyGroupedAudioSourcesDict[concurrency._audioConcurrencyGroup].Add(audioSource);
                    }
                }

                if (!play)
                {
                    LogWarning("Failed audio concurrency rule : GroupPassed = " + (groupPassed ? "Yes" : "No") + " and TypePassed = " + (audioTypePassed ? "Yes" : "No"));
                }
            }

            return play;
        }

        private bool ResolveConcurrencyRule(EAudioType audioType, AudioConcurrency concurrency, bool isGroup)
        {
            bool playNew = false;

            EAudioConcurrencyRule rule = isGroup ? concurrency._audioConcurrencyGroup.GetAudioConcurrencyRule() : concurrency._audioTypeConcurrency._maxAudioTypeConcurrencyRule;
            List<AudioSource> audioSources;
            AudioSource audioSourceToStop = null;

            if (isGroup)
            {
                _playingConcurrencyGroupedAudioSourcesDict.TryGetValue(concurrency._audioConcurrencyGroup, out audioSources);
            }
            else
            {
                _playingAudioTypeAudioSourcesDict.TryGetValue(audioType, out audioSources);
            }

            if (rule == EAudioConcurrencyRule.PreventNew)
            {
                playNew = false;
            }
            else if (rule == EAudioConcurrencyRule.StopOldest)
            {
                CheckOldest();

                playNew = true;
            }
            else if (rule == EAudioConcurrencyRule.StopFarthestThenPreventNew)
            {
                bool allEquidistant = CheckFurthestAudioSource();

                playNew = !allEquidistant;
            }
            else if (rule == EAudioConcurrencyRule.StopFarthestThenOldest)
            {
                bool allEquidistant = CheckFurthestAudioSource();

                if (audioSourceToStop == null)
                {
                    CheckOldest();
                }

                playNew = true;
            }
            //else if (rule == EAudioConcurrencyRule.StopQuietest)
            //{

            //}
            else if (rule == EAudioConcurrencyRule.StopLowestPriorityThenOldest)
            {
                bool stoppedLowestPriority = CheckLowestPriority();

                if (!stoppedLowestPriority)
                {
                    CheckOldest();
                }

                playNew = true;
            }
            else if (rule == EAudioConcurrencyRule.StopLowestPriorityThenPreventNew)
            {
                bool stoppedLowestPriority = CheckLowestPriority();

                playNew = false;
            }

            if (audioSourceToStop != null && playNew && isGroup)
            {
                audioSourceToStop.Stop();

                CleanConcurrency(audioSourceToStop);
            }

            return playNew;

            // local functions
            bool CheckFurthestAudioSource()
            {
                float furthestAudioSourceSqDist = 0.0f;
                float firstAudioSourceSqDist = -1.0f;
                bool allEquidistant = true;

                Transform audioListenerTransform;
                if (FollowCamera.Instance.GetAudioListener())
                {
                    audioListenerTransform = PlayerEntity.Instance.GetControlledGameObject().transform;
                }
                else
                {
                    audioListenerTransform = FollowCamera.Instance.GetAudioListener().transform;
                }

                Vector3 listenerLocation = audioListenerTransform.position;

                for (int i = 0; i < audioSources.Count; i++)
                {
                    AudioSource audioSource = audioSources[i];
                    Vector3 audioSourceLocation = audioSource.transform.position;

                    Vector3 diff = listenerLocation - audioSourceLocation;
                    float sqDiff = diff.sqrMagnitude;

                    if (firstAudioSourceSqDist < -1.0f)
                    {
                        firstAudioSourceSqDist = sqDiff;
                    }
                    else if (allEquidistant)
                    {
                        float absDistance = MathF.Abs(firstAudioSourceSqDist - sqDiff);
                        if (absDistance > 0.0f)
                        {
                            allEquidistant = false;
                        }
                    }

                    if (sqDiff > furthestAudioSourceSqDist)
                    {
                        furthestAudioSourceSqDist = diff.sqrMagnitude;
                        audioSourceToStop = audioSource;
                    }
                }

                return allEquidistant;
            }

            void CheckOldest()
            {
                float longestPlayTime = 0.0f;

                for (int i = 0; i < audioSources.Count; i++)
                {
                    AudioSource audioSource = audioSources[i];
                    if (audioSource.time > longestPlayTime)
                    {
                        longestPlayTime = audioSource.time;
                        audioSourceToStop = audioSource;
                    }
                }
            }

            bool CheckLowestPriority()
            {
                bool stoppedLowestPriority = false;
                EAudioPriority firstAudioSourcePriority = EAudioPriority.None;
                EAudioPriority lowestPriority = EAudioPriority.High;

                // do nothing if not group
                if (isGroup)
                {
                    bool allEqualPriority = true;
                    for (int i = 0; i < audioSources.Count; i++)
                    {
                        AudioSource audioSource = audioSources[i];
                        AudioConcurrency audioConcurrency;
                        _playingAudioSourceConcurrencyTypeDict.TryGetValue(audioSource, out audioConcurrency);

                        if (audioConcurrency != null)
                        {
                            AudioTypeConcurrency audioTypeConcurrency = audioConcurrency._audioTypeConcurrency;
                            if (firstAudioSourcePriority == EAudioPriority.None)
                            {
                                firstAudioSourcePriority = audioTypeConcurrency._priority;
                            }
                            else if (allEqualPriority)
                            {
                                if (firstAudioSourcePriority != audioTypeConcurrency._priority)
                                {
                                    allEqualPriority = false;
                                }
                            }

                            if (audioTypeConcurrency._priority < lowestPriority)
                            {
                                lowestPriority = audioTypeConcurrency._priority;
                                audioSourceToStop = audioSource;
                            }
                        }
                    }
                }

                return stoppedLowestPriority;
            }
        }

        private void AdjustAudioPlayback(EAudioType audioType, AudioSource audioSource)
        {
            AudioTypeScriptableObject audioTypeSO = null;
            ScriptableAudioPlayback audioPlayback = null;

            _audioTypeSODict.TryGetValue(audioType, out audioTypeSO);

            if (audioTypeSO)
            {
                audioPlayback = audioTypeSO.AudioPlayback;

                if (audioPlayback != null)
                {
                    audioSource.volume = audioPlayback.Volume;
                }
                else
                {
                    audioSource.volume = 1.0f;
                }
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
                    AudioMixer sourceMixer = track._source.outputAudioMixerGroup.audioMixer;
                    if (sourceMixer != null)
                    {
                        float alpha = Mathf.Lerp(initial, target, timer / duration);
                        sourceMixer.SetFloat("Volume", MathF.Log10(alpha) * 20.0f);
                        timer += Time.deltaTime;
                        yield return null;
                    }
                    else
                    {
                        track._source.volume = Mathf.Lerp(initial, target, timer / duration);
                        timer += Time.deltaTime;
                        yield return null;
                    }
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
