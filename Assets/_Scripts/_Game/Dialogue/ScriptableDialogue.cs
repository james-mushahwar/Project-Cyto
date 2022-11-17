using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.Dialogue{
    
    public enum EDialogueType
    {
        Overview,
        CharacterOverview,
        CharacterWorld,
        World
    }

    [Serializable]
    public class Phrase
    {
        [SerializeField] private string _phrase;
        [SerializeField] private bool _receiveInput; // can this phrase receive input
        [SerializeField] private bool _isSkipable; // can we skip to end of phrase with input
        [SerializeField] private bool _isAuto;      // does text move onto next automatically?
        [Header("Text speed properties")]
        [SerializeField] private float _textSpeed = 1.0f;
        [SerializeField] private float _fasterTextSpeed = 1.5f;
        [Header("Wait properties")]
        [SerializeField] private float _startOfPhraseWait = 0.25f;
        [SerializeField] private float _endOfPhraseWait = 1.0f;
    }

    [CreateAssetMenu(menuName ="Dialogue/Dialogue")]
    public class ScriptableDialogue : ScriptableObject
    {
        [SerializeField] private EDialogueType _dialogueType;

        [SerializeField] private Phrase[] _phrases;
    }
    
}
