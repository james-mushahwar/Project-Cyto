using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.General.Managers{
    
    public class PauseManager : Singleton<PauseManager>
    {
        private bool _isPaused = false;

        public bool IsPaused {get => _isPaused;}
        // Start is called before the first frame update
        void Start()
        {
            
        }
    
        // Update is called once per frame
        void Update()
        {
            
        }

        public void TogglePause()
        {
            _isPaused = !_isPaused;
            Time.timescale = _isPaused ? 0.0f : 1.0f;
        }
    }
}
