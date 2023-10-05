using _Scripts._Game.General.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.General.SceneLoading{
    
    public class Doorway : MonoBehaviour
    {
        [SerializeField]
        private int _zoneIndex;
        [SerializeField]
        private string _areaName;
        private int _areaBuildIndex;

        // Start is called before the first frame update
        void Start()
        {
            _areaBuildIndex = AssetManager.Instance.AreaNameToBuildIndex(_areaName);
        }

        
    }
    
}
