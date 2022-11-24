using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.UI.Dialogue{
    
    public abstract class BaseWriterEffect : MonoBehaviour
    {
        public abstract Coroutine Run();
        public abstract IEnumerator TypeText();
    }
    
}
