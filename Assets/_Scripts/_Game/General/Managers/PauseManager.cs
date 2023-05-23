using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.General.Managers{
    
    public class PauseManager : Singleton<PauseManager>
    {
        private bool _isPaused = false;

        public bool IsPaused {get => _isPaused;}

        public void TogglePause()
        {
            _isPaused = !_isPaused;
            TimeManager.Instance.TryRequestPauseGame(_isPaused);
            UIManager.Instance.ShowPauseMenu(_isPaused);
        }
    }
}
