using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.General.Managers{
    
    public static class Bootstrapper
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Execute()
        {
            Object.DontDestroyOnLoad(Object.Instantiate(Resources.Load("Prefabs/Managers")));
            Object.DontDestroyOnLoad(Object.Instantiate(Resources.Load("Prefabs/Systems")));
            //Debug.LogWarning("Bootstrapper Execute");
        }
    }
    
}
