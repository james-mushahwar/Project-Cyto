using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace _Scripts._Game.General.Managers
{
    public class UIManager : Singleton<UIManager>
    {
        [Header("UI references")]
        #region Pause menu
        [SerializeField]
        private GameObject _pauseMenuGO;
        private Tweener _pauseMenuTweener;
        #endregion

        #region Dialogue 
        [SerializeField]
        private GameObject _dialogueBoxGO;
        #endregion

        public void ShowPauseMenu(bool show)
        {
            float targetOpactity = show ? 1.0f : 0.0f;

            _pauseMenuGO.SetActive(show);
        }
    }
    
}
