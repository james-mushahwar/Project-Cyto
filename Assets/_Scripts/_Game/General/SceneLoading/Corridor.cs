using System.Collections;
using System.Collections.Generic;
using _Scripts._Game.General.Managers;
using Assets._Scripts._Game.General.SceneLoading;
using UnityEngine;

namespace _Scripts._Game.General.SceneLoading{
    
    public class Corridor : MonoBehaviour
    {
        [SerializeField]
        private SceneField _leftAreaName;
        private int _leftAreaBuildIndex;
        [SerializeField]
        private SceneField _rightAreaName;
        private int _rightAreaBuildIndex;

        public void Start()
        {
            _leftAreaBuildIndex = AssetManager.Instance.AreaNameToBuildIndex(_leftAreaName);
            _rightAreaBuildIndex = AssetManager.Instance.AreaNameToBuildIndex(_rightAreaName);
        }

        public void EnterArea(bool left)
        {
            int areaIndex = left ? _leftAreaBuildIndex : _rightAreaBuildIndex;
            GameStateManager.Instance.EnterZoneAndArea(-1, areaIndex);
        }
    }
    
}
