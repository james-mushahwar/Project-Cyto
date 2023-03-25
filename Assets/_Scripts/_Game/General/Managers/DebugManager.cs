using _Scripts._Game.General.Settings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.General.Managers{
    
    public class DebugManager : Singleton<DebugManager>
    {
        [SerializeField]
        private DebugSettings _debugSettings;

        public DebugSettings DebugSettings { get => _debugSettings; }
    }
    
}
