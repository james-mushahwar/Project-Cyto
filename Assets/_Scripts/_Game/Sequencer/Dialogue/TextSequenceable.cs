using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _Scripts._Game.Dialogue;
using Unity.Mathematics;
using _Scripts._Game.General.Managers;

namespace _Scripts._Game.Sequencer.Dialogue{
    
    public class TextSequenceable : Sequenceable
    {
        [SerializeField]
        private ScriptableDialogue _scriptableDialogue;
        [SerializeField]
        private EDialogueType _dialogueType;

        private int _phrasesIndex = 0;
        private Task _taskRef;

        public override void Begin()
        {
            _taskRef = DialogueManager.Instance.PostText<Phrase>(_scriptableDialogue.GetPhrase(_phrasesIndex), _dialogueType);
            _phrasesIndex++;
        }

        public override void Stop()
        {
            _phrasesIndex = 0;
            if (_taskRef != null)
            {
                _taskRef.Stop();
                _taskRef = null;
            }
        }

        public override void Tick()
        {
            if (_taskRef == null)
            {
                return;
            }

            if (_taskRef.Running == false)
            {
                _taskRef = DialogueManager.Instance.PostText<Phrase>(_scriptableDialogue.GetPhrase(_phrasesIndex), _dialogueType);
                _phrasesIndex++;
            }
        }

        public override bool IsComplete()
        {
            if (_taskRef == null)
            {
                _taskRef = DialogueManager.Instance.PostText<Phrase>(_scriptableDialogue.GetPhrase(_phrasesIndex), _dialogueType);
                _phrasesIndex++;

                if (_taskRef == null)
                {
                    return true;
                }
            }

            if (_taskRef.Running || _taskRef.Paused)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

    }
    
}
