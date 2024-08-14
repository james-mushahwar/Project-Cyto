using System.Collections;
using System.Collections.Generic;
using Assets._Scripts._Game.General.SceneLoading;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using _Scripts._Game.General.Managers;
using System;
using UnityEditor.SearchService;
using Scene = UnityEngine.SceneManagement.Scene;

namespace _Scripts._Game.General.SceneLoading{

    public class SceneSwitcher : MonoBehaviour
    {
        [SerializeField]
        private SceneField[] _sceneAssets = new SceneField[2];
        [SerializeField]
        private Collider2D[] _colliders = new Collider2D[2];

        private IEnumerator[] _asyncSceneOperation = new IEnumerator[2];

        private GameStateManager _gameStateManager;

        // Start is called before the first frame update
        void Start()
        {
            _gameStateManager = GameStateManager.Instance;

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
        public void OnPlayerEntered(int index, string sceneName = "")
        {
            //Debug.Log("Scene asset name is " + _sceneAssets[index]);
            string newSceneName = sceneName != "" ? sceneName : _sceneAssets[index];

            LoadAdditiveScene(newSceneName, index);
        }

        private void LoadAdditiveScene(string sceneName, int index)
        {
            Scene targetScene = SceneManager.GetSceneByName(sceneName);

            if (targetScene != null)
            {
                if (targetScene.isLoaded || _asyncSceneOperation[index] != null)
                {
                    //Debug.LogWarning(SceneManager.GetSceneByName(_sceneAssets[index]) + " is already loaded");
                    return;
                }
            }

            _asyncSceneOperation[index] = LoadSceneAsync(index, sceneName);
            StartCoroutine(_asyncSceneOperation[index]);
        }

        // used to load scene on on player exited collider event
        public void OnPlayerExited(int index, string sceneName = "")
        {
            //Debug.Log("Scene asset name is " + _sceneAssets[index]);
            string newSceneName = sceneName != "" ? sceneName : _sceneAssets[index];

            UnloadAdditiveScene(newSceneName, index);
        }

        private void UnloadAdditiveScene(string sceneName, int index)
        {
            Scene targetScene = SceneManager.GetSceneByName(sceneName);

            if (targetScene != null)
            {
                if (!targetScene.isLoaded || _asyncSceneOperation[index] != null)
                {
                    //Debug.LogWarning(SceneManager.GetSceneByName(_sceneAssets[index]) + " is already unloaded");
                    return;
                }
            }

            _asyncSceneOperation[index] = UnloadSceneAsync(index, sceneName);
            StartCoroutine(_asyncSceneOperation[index]);
        }

        private IEnumerator LoadSceneAsync(int index, string sceneName)
        {
            //Debug.Log("Trying to load scene: " + sceneName);
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            while (!operation.isDone)
            {
                yield return null;
            }

            _asyncSceneOperation[index] = null;
        }

        private IEnumerator UnloadSceneAsync(int index, string sceneName)
        {
            //Debug.Log("Trying to unload scene: " + sceneName);
            AsyncOperation operation = SceneManager.UnloadSceneAsync(sceneName);

            while (!operation.isDone)
            {
                yield return null;
            }

            _asyncSceneOperation[index] = null;
        }
    }
    
}
