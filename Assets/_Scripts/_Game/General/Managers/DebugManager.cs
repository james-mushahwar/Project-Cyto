using _Scripts._Game.General.Settings;
using _Scripts.Editortools.GameObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.General.Managers{
    
    public class DebugManager : Singleton<DebugManager>, IManager
    {
        [SerializeField]
        private DebugSettings _debugSettings;

        public DebugSettings DebugSettings { get => _debugSettings; }

        public void PostInGameLoad()
        {
        }

        public void PostMainMenuLoad()
        {
        }

        public void PreInGameLoad()
        {
        }

        public void PreMainMenuLoad()
        {
        }
    }
    
}
