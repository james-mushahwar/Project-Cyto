#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using _Scripts._Game.Audio;
using _Scripts._Game.General.Managers;
using Pathfinding;

using UnityEditor;
using System;
using System.Linq;
using Editor.Windows.AI;
using UnityEditor.VersionControl;

namespace _Scripts.Editortools.Windows.Audio{
    
    public class AudioGenerator : EditorWindow
    {
        #region General
        private const string _audioTypeRootPath = "Assets/Resources/Audio/Audio Type/";
        private const string _audioTypeGeneralPath = "Assets/Resources/Audio/Audio Type/General";
        private const string _audioTypePlayerPath = "Assets/Resources/Audio/Audio Type/Player";
        private const string _audioTypeEnemyPath = "Assets/Resources/Audio/Audio Type/Enemy";
        private const string _audioTypeEnvironmentPath = "Assets/Resources/Audio/Audio Type/Environment";

        private const string _audioPlaybackRootPath = "Assets/Resources/Audio/Audio Playback/";
        private const string _audioPlaybackGeneralPath = "Assets/Resources/Audio/Audio Playback/General";
        private const string _audioPlaybackPlayerPath = "Assets/Resources/Audio/Audio Playback/Player";
        private const string _audioPlaybackEnemyPath = "Assets/Resources/Audio/Audio Playback/Enemy";
        private const string _audioPlaybackEnvironmentPath = "Assets/Resources/Audio/Audio Playback/Environment";

        private Vector2 _scrollPos = Vector2.zero;
        #endregion

        [MenuItem("Window/Audio Generator")]
        public static void ShowWindow()
        {
            GetWindow<AudioGenerator>("Audio Generator");
        }

        private void OnGUI()
        {
            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);
            if (GUILayout.Button("Generate Audio type scripts!"))
            {
                GenerateAudioTypes();
            }

            EditorGUILayout.EndScrollView();

            this.Repaint();
        }

        private void GenerateAudioTypes()
        {
            // find audio types that exist
            List<EAudioType> audioTypes = new List<EAudioType>();
            
            for (int i = 0; i < (int)EAudioType.COUNT; i++)
            {
                if (Enum.IsDefined(typeof(EAudioType), i))
                {
                    audioTypes.Add((EAudioType)i);
                }
            }

            // remove types that already exist
            UnityEngine.Object[] scripts = Resources.LoadAll("Audio/Audio Type/");
            
            foreach (UnityEngine.Object script in scripts)
            {
                if (script.GetType() == typeof(AudioTypeScriptableObject))
                {
                    AudioTypeScriptableObject audioTypeSO = (AudioTypeScriptableObject)script;
                    if (audioTypeSO)
                    {
                        //Debug.Log("Found audioTypeSO: " + audioTypeSO.name);
                        if (audioTypes.Contains(audioTypeSO.AudioType))
                        {
                            audioTypes.Remove(audioTypeSO.AudioType);

                            string fileName = Enum.GetName(typeof(EAudioType), audioTypeSO.AudioType) + "SO";
                            if (audioTypeSO.name != fileName)
                            {
                                Debug.LogWarning("Found a name mismatched file and audiotype: " + audioTypeSO.name);
                                //audioTypeSO.name = fileName;
                            }
                        }
                    }
                }
            }

            AssetDatabase.Refresh();
            //return;

            // generate remaining audio type scriptable objects
            for (int i = 0; i < audioTypes.Count; i++)
            {
                EAudioType newAudioType = audioTypes[i];
                AudioTypeScriptableObject newAudioTypeSO = ScriptableObject.CreateInstance<AudioTypeScriptableObject>();

                newAudioTypeSO.name = Enum.GetName(typeof(EAudioType), newAudioType);

                newAudioTypeSO.AudioType = newAudioType;

                string typePath = _audioTypeGeneralPath;
                if ((int)newAudioType >= 1000 && (int)newAudioType < 2000)
                {
                    typePath = _audioTypePlayerPath;
                }
                else if ((int)newAudioType >= 2000 && (int)newAudioType < 3000)
                {
                    // enemy
                    typePath = _audioTypeEnemyPath;
                }
                else if ((int)newAudioType >= 3000 && (int)newAudioType < 4000)
                {
                    // environment
                    typePath = _audioTypeEnvironmentPath;
                }

                typePath = typePath + "/" + Enum.GetName(typeof(EAudioType), newAudioType) + "SO.asset";

                AssetDatabase.CreateAsset(newAudioTypeSO, typePath);

                Debug.Log("Created AudioTypeSO script: " + typePath);
            }
        }

        private void GenerateAudioPlayback()
        {
            // find audio types that exist
            List<EAudioType> audioTypes = new List<EAudioType>();
            
            for (int i = 0; i < (int)EAudioType.COUNT; i++)
            {
                if (Enum.IsDefined(typeof(EAudioType), i))
                {
                    audioTypes.Add((EAudioType)i);
                }
            }

            // remove types that already exist
            UnityEngine.Object[] scripts = Resources.LoadAll("Audio/Audio Playback/");
            
            foreach (UnityEngine.Object script in scripts)
            {
                if (script.GetType() == typeof(AudioTypeScriptableObject))
                {
                    AudioTypeScriptableObject audioTypeSO = (AudioTypeScriptableObject)script;
                    if (audioTypeSO)
                    {
                        //Debug.Log("Found audioTypeSO: " + audioTypeSO.name);
                        if (audioTypes.Contains(audioTypeSO.AudioType))
                        {
                            audioTypes.Remove(audioTypeSO.AudioType);

                            string fileName = Enum.GetName(typeof(EAudioType), audioTypeSO.AudioType) + "SO";
                            if (audioTypeSO.name != fileName)
                            {
                                Debug.LogWarning("Found a name mismatched file and audiotype: " + audioTypeSO.name);
                                //audioTypeSO.name = fileName;
                            }
                        }
                    }
                }
            }

            AssetDatabase.Refresh();
            //return;

            // generate remaining audio type scriptable objects
            for (int i = 0; i < audioTypes.Count; i++)
            {
                EAudioType newAudioType = audioTypes[i];
                ScriptableAudioPlayback newAudioPlaybackSO = ScriptableObject.CreateInstance<ScriptableAudioPlayback>();

                newAudioPlaybackSO.name = Enum.GetName(typeof(EAudioType), newAudioType);

                string playbackPath = _audioPlaybackGeneralPath;
                if ((int)newAudioType >= 1000 && (int)newAudioType < 2000)
                {
                    playbackPath = _audioPlaybackPlayerPath;
                }
                else if ((int)newAudioType >= 2000 && (int)newAudioType < 3000)
                {
                    // enemy
                    playbackPath = _audioPlaybackEnemyPath;
                }
                else if ((int)newAudioType >= 3000 && (int)newAudioType < 4000)
                {
                    // environment
                    playbackPath = _audioPlaybackEnvironmentPath;
                }

                playbackPath = playbackPath + "/" + Enum.GetName(typeof(EAudioType), newAudioType) + "_PlaybackSO.asset";

                AssetDatabase.CreateAsset(newAudioPlaybackSO, playbackPath);

                Debug.Log("Created AudioPlaybackSO script: " + playbackPath);
            }
        }
    }

}

#endif
