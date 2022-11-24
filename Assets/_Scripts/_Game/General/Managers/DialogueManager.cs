using _Scripts._Game.Dialogue;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.General.Managers{
    
    public class DialogueManager : Singleton<DialogueManager>
    {
        public void PostText(string text, EDialogueType dialogueType)
        {
            // recieve a post request 
            // decide what ui it's going to occupy and what writer effect to use
        }
    }
    
}
