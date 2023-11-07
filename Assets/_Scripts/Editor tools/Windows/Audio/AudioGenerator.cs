#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using _Scripts._Game.Audio;
using _Scripts._Game.General.Managers;
using Pathfinding;

using UnityEditor;
using System;

namespace _Scripts.Editortools.Windows.Audio{
    
    public class AudioGenerator : EditorWindow
    {
        #region General
        private const string _audioTypeRootPath = "Assets/_Scripts/_Game/Audio/Audio Type/";

        private Vector2 _scrollPos = Vector2.zero;
        #endregion

        private void OnGUI()
        {
            if (GUILayout.Button("Generate scripts!"))
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
                audioTypes.Add((EAudioType)i);
            }


            // remove types that already exist
            UnityEngine.Object[] scripts = Resources.LoadAll(_audioTypeRootPath);

            foreach (UnityEngine.Object script in scripts)
            {
                if (script.GetType() == typeof(AudioTypeScriptableObject))
                {
                    AudioTypeScriptableObject audioTypeSO = (AudioTypeScriptableObject)script;
                    if (audioTypeSO)
                    {
                        Debug.Log("Found audioTypeSO: " + audioTypeSO.name);
                        if (audioTypes.Contains(audioTypeSO.AudioType))
                        {
                            audioTypes.Remove(audioTypeSO.AudioType);
                        }
                    }
                    
                }
            }

            // generate remaining audio type scriptable objects
            for (int i = 0; i < audioTypes.Count; i++)
            {
                EAudioType newAudioType = audioTypes[i];
                AudioTypeScriptableObject newAudioTypeSO = ScriptableObject.CreateInstance<AudioTypeScriptableObject>();

                newAudioTypeSO.name = Enum.GetName(typeof(EAudioType), newAudioType);

                newAudioTypeSO.AudioType = newAudioType;
            }
        }
    }

}

#endif
