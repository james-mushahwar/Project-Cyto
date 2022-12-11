using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.Sequencer {

//CMERA ANIMATION
//https://blog.ashwanik.in/2015/01/animate-or-focus-camera-on-game-object.html

//MAIN MENU AND MENU SYSTEM
//https://www.youtube.com/watch?v=QxRAIjXdfFU&list=PLt_Y3Hw1v3QTEbh8fQV1DUOUIh9nF0k6c&index=10&ab_channel=3DBuzz
    public abstract class Sequenceable : MonoBehaviour
    {
        public bool _acceptInput = false;
        public abstract void Begin();
        public abstract void Stop();
        public abstract void Tick();
        public abstract bool IsComplete();
    }

}
