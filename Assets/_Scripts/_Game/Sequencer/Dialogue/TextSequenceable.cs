using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _Scripts._Game.Dialogue;
using Unity.Mathematics;

namespace _Scripts._Game.Sequencer.Dialogue{
    
    public class TextSequenceable : Sequenceable
    {
        [SerializeField]
        private ScriptableDialogue _scriptableDialogue;
        [SerializeField]
        private EDialogueType _dialogueType;

        public override void Begin()
        {
            
        }

        public override void Stop()
        {
            
        }

        public override void Tick()
        {
            
        }

        public override bool IsComplete()
        {
            return false;
        }

    }
    
}
