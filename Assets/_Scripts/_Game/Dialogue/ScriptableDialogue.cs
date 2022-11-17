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

    [CreateAssetMenu(menuName ="Dialogue/Dialogue")]
    public class ScriptableDialogue : ScriptableObject
    {
        [SerializeField] private EDialogueType _dialogueType;

        [SerializeField] private ScriptablePhrase[] _dialogue;
    
    }
    
}
