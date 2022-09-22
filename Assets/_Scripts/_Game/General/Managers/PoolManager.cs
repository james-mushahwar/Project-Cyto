using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _Scripts._Game.General;

//https://www.sebaslab.com/zero-allocation-code-in-unity/
namespace _Scripts._Game.General.Managers{
    
    public abstract class PoolManager<T> : Singleton<PoolManager<T>> where T : MonoBehaviour
    {


        private new void Awake()
        {
            base.Awake();
        }
    }
    
}
