using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.General.Managers{
    
    public class CameraManager : Singleton<CameraManager>, IManager
    {
        private Camera _mainCamera;

        public Camera MainCamera { get { return _mainCamera; } }

        public void OnCreated()
        {
            if (MainCamera == null)
            {
                _mainCamera = Camera.main;
            }
        }

        public void ManagedPostMainMenuLoad()
        {
            if (MainCamera == null)
            {
                _mainCamera = Camera.main;
            }
        }

        public void ManagedPrePlayGame()
        {
            Camera followCamera = FollowCamera.Instance.Camera;

            _mainCamera = followCamera;
        }
    }
    
}
