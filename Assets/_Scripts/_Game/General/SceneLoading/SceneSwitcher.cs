using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace _Scripts._Game.General.SceneLoading{
    
    public struct SceneLoadHandshake
    {
        int arrival;
    }

    public class SceneSwitcher : MonoBehaviour
    {
        [SerializeField]
        private SceneAsset[] _sceneAssets = new SceneAsset[2];
        [SerializeField]
        private Collider2D[] _colliders = new Collider2D[2];
        private SceneLoadHandshake _handshake;
        // Start is called before the first frame update
        void Start()
        {
            
        }
    
        // Update is called once per frame
        void Update()
        {
            
        }
    }
    
}
