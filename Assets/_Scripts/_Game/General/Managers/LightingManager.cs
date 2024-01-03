using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace _Scripts._Game.General.Managers{
    
    public class LightingManager : Singleton<LightingManager>, IManager
    {
        #region Lights
        private Light2D _globalLight2D;

        public Light2D GlobalLight2D
        {
            get { return _globalLight2D; }
        }
        #endregion

        public void ManagedTick()
        {

        }

        public void DisableGlobalLight()
        {
            if (_globalLight2D)
            {
                _globalLight2D.gameObject.SetActive(false);
            }
        }

        public void FindGlobalLight()
        {
            GameObject lightGO = GameObject.FindGameObjectWithTag("GlobalLight");
            if (lightGO)
            {
                Light2D light = lightGO.GetComponent<Light2D>();
                if (light)
                {
                    if (light.lightType == Light2D.LightType.Global &&
                        (_globalLight2D == null || _globalLight2D != light))
                    {
                        _globalLight2D = light;
                    }
                }
            }
        }

        public void PreInGameLoad()
        {
            
        }

        public void PostInGameLoad()
        {
            
        }

        public void PreMainMenuLoad()
        {
            
        }

        public void PostMainMenuLoad()
        {
            
        }
    }
    
}
