using _Scripts._Game.Dialogue;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using _Scripts._Game.UI.Dialogue;

namespace _Scripts._Game.General.Managers{
    
    public class DialogueManager : Singleton<DialogueManager>
    {
        private Dictionary<EDialogueType, GameObject> _textGameObjectDictionary = new Dictionary<EDialogueType, GameObject>();
        private Dictionary<EDialogueType, TMP_Text> _textBoxDictionary = new Dictionary<EDialogueType, TMP_Text>();

        private BaseWriterEffect[] _writerEffects;

        private Dictionary<EDialogueType, Coroutine> _dialogueCoroutines = new Dictionary<EDialogueType, Coroutine>();

        private new void Awake()
        {
            base.Awake();
        }

        void Start()
        {
            GameObject overviewGameObject = GameObject.FindGameObjectWithTag("UI_Overview");
            if (overviewGameObject)
            {
                TMP_Text overviewTextBox = overviewGameObject.GetComponentInChildren<TMP_Text>();
                overviewGameObject.SetActive(false);
                _textGameObjectDictionary.Add(EDialogueType.Overview, overviewGameObject);
                if (overviewTextBox)
                {
                    _textBoxDictionary.Add(EDialogueType.Overview, overviewTextBox);
                }
            }

            GameObject characterOverviewGameObject = GameObject.FindGameObjectWithTag("UI_CharacterOverview");
            if (characterOverviewGameObject)
            {
                TMP_Text characterOverviewTextBox = characterOverviewGameObject.GetComponentInChildren<TMP_Text>();
                characterOverviewGameObject.SetActive(false);
                _textGameObjectDictionary.Add(EDialogueType.CharacterOverview, characterOverviewGameObject);
                if (characterOverviewTextBox)
                {
                    _textBoxDictionary.Add(EDialogueType.CharacterOverview, characterOverviewTextBox);
                }
            }

            GameObject characterWorldGameObject = GameObject.FindGameObjectWithTag("UI_CharacterWorld");
            if (characterWorldGameObject)
            {
                TMP_Text characterWorldTextBox = characterWorldGameObject.GetComponentInChildren<TMP_Text>();
                characterWorldGameObject.SetActive(false);
                _textGameObjectDictionary.Add(EDialogueType.CharacterWorld, characterWorldGameObject);
                if (characterWorldTextBox)
                {
                    _textBoxDictionary.Add(EDialogueType.CharacterWorld, characterWorldTextBox);
                }
            }

            GameObject worldGameObject = GameObject.FindGameObjectWithTag("UI_World");
            if (worldGameObject)
            {
                TMP_Text worldTextBox = worldGameObject.GetComponentInChildren<TMP_Text>();
                worldGameObject.SetActive(false);
                _textGameObjectDictionary.Add(EDialogueType.World, worldGameObject);
                if (worldTextBox)
                {
                    _textBoxDictionary.Add(EDialogueType.World, worldTextBox);
                }
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
