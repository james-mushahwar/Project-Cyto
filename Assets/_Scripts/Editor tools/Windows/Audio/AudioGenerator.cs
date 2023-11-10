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

namespace _Scripts.Editortools.Windows.Audio{
    
    public class AudioGenerator : EditorWindow
    {
        #region General
        private const string _audioTypeRootPath = "Assets/Resources/Audio/Audio Type/";
        private const string _audioTypePlayerPath = "Assets/Resources/Audio/Audio Type/Player";
        private const string _audioTypeEnemyPath = "Assets/Resources/Audio/Audio Type/Enemy";
        private const string _audioTypeEnvironmentPath = "Assets/Resources/Audio/Audio Type/Environment";

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
                Generate();
            }
            EditorGUILayout.EndScrollView();

            this.Repaint();
        }

        private void Generate()
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
            //foreach (var o in Resources.LoadAll(_audioTypeEnemyPath)) scripts.Append<UnityEngine.Object>(o);
            //foreach (var o in Resources.LoadAll(_audioTypeEnvironmentPath)) scripts.Append<UnityEngine.Object>(o);


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
                        }
                    }
                }
            }
            //return;

            // generate remaining audio type scriptable objects
            for (int i = 0; i < audioTypes.Count; i++)
            {
                EAudioType newAudioType = audioTypes[i];
                AudioTypeScriptableObject newAudioTypeSO = ScriptableObject.CreateInstance<AudioTypeScriptableObject>();

                newAudioTypeSO.name = Enum.GetName(typeof(EAudioType), newAudioType);

                newAudioTypeSO.AudioType = newAudioType;

                string path = _audioTypePlayerPath;
                if ((int)newAudioType >= 2000 && (int)newAudioType < 3000)
                {
                    // enemy
                    path = _audioTypeEnemyPath;
                }
                else if ((int)newAudioType >= 3000 && (int)newAudioType < 4000)
                {
                    // environment
                    path = _audioTypeEnvironmentPath;
                }

                path = path + "/" + Enum.GetName(typeof(EAudioType), newAudioType) + "SO.asset";

                AssetDatabase.CreateAsset(newAudioTypeSO, path);

                Debug.Log("Created AudioTypeSO script: " + path);
            }
        }
    }

}

#endif
