using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

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

        private IEnumerator[] _asyncSceneOperation = new IEnumerator[2];
        // Start is called before the first frame update

        void OnDrawGizmos()
        {
            
        }

        void Start()
        {
            for (int i = 0; i < 2; ++i)
            {
                SceneSwitcherCollider ssc = _colliders[i].gameObject.GetComponent<SceneSwitcherCollider>();
                if (ssc)
                {
                    ssc.Init(this, i);
                }
            }
        }

        // used to unload scene on player exited collider event
        public void OnPlayerEntered(int index)
        {
            Debug.Log("Scene asset name is " + _sceneAssets[index].name);
            Scene targetScene = SceneManager.GetSceneByName(_sceneAssets[index].name);

            if (targetScene != null)
            {
                if (!targetScene.isLoaded || _asyncSceneOperation[index] != null)
                {
                    Debug.LogWarning(SceneManager.GetSceneByName(_sceneAssets[index].name) + " is already unloaded");
                    return;
                }
            }

            _asyncSceneOperation[index] = UnloadSceneAsync(index, _sceneAssets[index].name);
            StartCoroutine(_asyncSceneOperation[index]);
        }

        // used to load scene on on player exited collider event
        public void OnPlayerExited(int index)
        {
            Debug.Log("Scene asset name is " + _sceneAssets[index].name);
            Scene targetScene = SceneManager.GetSceneByName(_sceneAssets[index].name);

            if (targetScene != null)
            {
                if (targetScene.isLoaded || _asyncSceneOperation[index] != null)
                {
                    Debug.LogWarning(SceneManager.GetSceneByName(_sceneAssets[index].name) + " is already loaded");
                    return;
                }
            }

            _asyncSceneOperation[index] = LoadSceneAsync(index, _sceneAssets[index].name);
            StartCoroutine(_asyncSceneOperation[index]);
        }

        private IEnumerator LoadSceneAsync(int index, string sceneName)
        {
            Debug.Log("Trying to load scene: " + sceneName);
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            while (!operation.isDone)
            {
                yield return null;
            }

            _asyncSceneOperation[index] = null;
        }

        private IEnumerator UnloadSceneAsync(int index, string sceneName)
        {
            Debug.Log("Trying to unload scene: " + sceneName);
            AsyncOperation operation = SceneManager.UnloadSceneAsync(sceneName);

            while (!operation.isDone)
            {
                yield return null;
            }

            _asyncSceneOperation[index] = null;
        }
    }
    
}
