using _Scripts._Game.Dialogue;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using _Scripts._Game.UI.Dialogue;
using Newtonsoft.Json.Bson;

namespace _Scripts._Game.General.Managers{
    
    public class DialogueManager : Singleton<DialogueManager>
    {
        private Dictionary<EDialogueType, GameObject> _textGameObjectDictionary = new Dictionary<EDialogueType, GameObject>();
        private Dictionary<EDialogueType, TMP_Text> _textBoxDictionary = new Dictionary<EDialogueType, TMP_Text>();

        private BaseWriterEffect[] _writerEffects;

        private Dictionary<EDialogueType, Task> _dialogueTasks = new Dictionary<EDialogueType, Task>();

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

        private void FixedUpdate()
        {
            if (_dialogueTasks[EDialogueType.Overview] != null)
            {
                _dialogueTasks.TryGetValue(EDialogueType.Overview, out Task textTask);
                if (textTask == null)
                {
                    _textGameObjectDictionary[EDialogueType.Overview].SetActive(false);
                }
            }
        }

        public Task PostText<T>(T text, EDialogueType dialogueType)
        {
            // recieve a post request 
            TMP_Text textBox = GetDialogueTextBox(dialogueType);
            if (textBox == null)
            {
                return null;
            }

            if (text == null)
            {
                return null;
            }

            // decide what ui it's going to occupy and what writer effect to use
            BaseWriterEffect writerEffect = _writerEffects[0]; // for now get first writer effect

            // check if dictionary already has coroutine running
            if (IsTextTaskRunning(dialogueType))
            {
                _dialogueTasks[dialogueType].Stop();
                _dialogueTasks[dialogueType] = null;
            }

            // start new Task coroutine
            string TString = (object)text as string;
            Task returnTask = null;

            if (TString != null)
            {
                returnTask = new Task(writerEffect.TypeText(TString, textBox), true);
                if (returnTask != null)
                {
                    _dialogueTasks[dialogueType] = returnTask;
                }
            }
            else
            {
                Phrase TPhrase = (object)text as Phrase;
                if (TPhrase != null)
                {
                    returnTask = new Task(writerEffect.TypeText(TPhrase, textBox), true);
                    if (returnTask != null)
                    {
                        _dialogueTasks[dialogueType] = returnTask;
                    }
                }
            } 

            _textGameObjectDictionary[dialogueType].SetActive(true);

            return returnTask;


            bool IsTextTaskRunning(EDialogueType dialogueType)
            {
                _dialogueTasks.TryGetValue(dialogueType, out Task textTask);
                if (textTask != null)
                {
                    return textTask.Running;
                }

                return false;
            }
        }

        private TMP_Text GetDialogueTextBox(EDialogueType dialogueType)
        {
            _textBoxDictionary.TryGetValue(dialogueType, out TMP_Text textBox);
            return textBox;
        }


        public void OnDialogueFinished()
        {
            Debug.Log("DIALOGUE TASK FIN");
        }
    }
    
}
