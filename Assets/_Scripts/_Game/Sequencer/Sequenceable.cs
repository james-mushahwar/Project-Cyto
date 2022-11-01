using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.Sequencer {

    public abstract class Sequenceable : MonoBehaviour
    {
        public bool _acceptInput = false;
        public abstract void Begin();
        public abstract void Stop();
        public abstract void Tick();
    }
    
}
