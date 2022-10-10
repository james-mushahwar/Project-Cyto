using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;
using UnityEditor;
using UnityEngine;

namespace _Scripts.Debugger{
    
    public class AudioDebugger : EditorWindow
    {
        [MenuItem("Debugger/Audio")]
        private static void OpenWindow()
        {
            var window = GetWindow<AudioDebugger>();
            //window.m_TrackedAudioSources = Extensions.GetAllInstances<AudioSource>().ToList();
        }

        public List<AudioSource> m_TrackedAudioSources;
    }
    
}
