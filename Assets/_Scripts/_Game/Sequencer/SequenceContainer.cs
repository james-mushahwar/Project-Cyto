using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.Sequencer{
    
    public class SequenceContainer : MonoBehaviour
    {
        [SerializeField]
        private bool _canBeQueued;
        [SerializeField]
        private bool _isSimultaneous;

        [SerializeField]
        private List<SequenceableGroup> _sequenceableGroups = new List<SequenceableGroup>();
        // Start is called before the first frame update
        void Start()
        {
            
        }
    
        // Update is called once per frame
        void Update()
        {
            
        }
    }
    
}
