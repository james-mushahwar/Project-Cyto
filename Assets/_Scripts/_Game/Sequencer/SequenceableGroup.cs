﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.Sequencer{
    
    public class SequenceableGroup : MonoBehaviour
    {
        [SerializeField]
        private bool _isSimultaneousGroup;
        [SerializeField]
        public List<Sequenceable> sequenceablesInGroup = new List<Sequenceable>();
    }

}
