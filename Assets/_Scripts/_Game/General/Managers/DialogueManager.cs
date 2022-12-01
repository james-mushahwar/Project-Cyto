using _Scripts._Game.Dialogue;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using _Scripts._Game.UI.Dialogue;

namespace _Scripts._Game.General.Managers{
    
    public class DialogueManager : Singleton<DialogueManager>
    {
        private Dictionary<EDialogueType, TMP_Text> _textBoxDictionary = new Dictionary<EDialogueType, TMP_Text>();

        private BaseWriterEffect[] _writerEffects;

        private Dictionary<EDialogueType, Coroutine> _dialogueCoroutines = new Dictionary<EDialogueType, Coroutine>();

        void Start()
        {
            TMP_Text overviewTextBox = GameObject.FindGameObjectWithTag("UI_Overview").GetComponent<TMP_Text>();
            if (overviewTextBox)
            {
                _textBoxDictionary.Add(EDialogueType.Overview, overviewTextBox);
            }

            TMP_Text characterOverviewTextBox = GameObject.FindGameObjectWithTag("UI_CharacterOverview").GetComponent<TMP_Text>();
            if (characterOverviewTextBox)
            {
                _textBoxDictionary.Add(EDialogueType.CharacterOverview, characterOverviewTextBox);
            }

            TMP_Text characterWorldTextBox = GameObject.FindGameObjectWithTag("UI_CharacterWorld").GetComponent<TMP_Text>();
            if (characterWorldTextBox)
            {
                _textBoxDictionary.Add(EDialogueType.CharacterWorld, characterWorldTextBox);
            }

            TMP_Text worldTextBox = GameObject.FindGameObjectWithTag("UI_World").GetComponent<TMP_Text>();
            if (worldTextBox)
            {
                _textBoxDictionary.Add(EDialogueType.World, worldTextBox);
            }

            // writer effects
            _writerEffects = GetComponents<BaseWriterEffect>();
        }

        public void PostText<T>(T text, EDialogueType dialogueType)
        {
            // recieve a post request 
            TMP_Text textBox = GetDialogueTextBox(dialogueType);
            if (textBox == null)
            {
                return;
            }

            // decide what ui it's going to occupy and what writer effect to use
            BaseWriterEffect writerEffect = _writerEffects[0]; // for now get first writer effect

            // check if dictionary already has coroutine running
            if (_dialogueCoroutines[dialogueType] != null)
            {
                StopCoroutine(_dialogueCoroutines[dialogueType]);
                _dialogueCoroutines[dialogueType] = null;
            }

            // start new coroutine
            string TString = (string)(object)text;
            if (TString != null)
            {
                Coroutine writerRun = writerEffect.Run(TString, textBox);
                if (writerRun != null)
                {
                    _dialogueCoroutines[dialogueType] = writerRun;
                }
            }
            else
            {
                Phrase TPhrase = (Phrase)(object)text;
                if (TPhrase != null)
                {
                    Coroutine writerRun = writerEffect.Run(TPhrase, textBox);
                    if (writerRun != null)
                    {
                        _dialogueCoroutines[dialogueType] = writerRun;
                    }
                }
            } 

            TMP_Text GetDialogueTextBox(EDialogueType dialogueType)
            {
                _textBoxDictionary.TryGetValue(dialogueType, out TMP_Text textBox);
                return textBox;
            }
        }
    }
    
}
