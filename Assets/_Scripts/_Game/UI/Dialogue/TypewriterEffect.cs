using _Scripts._Game.Dialogue;
using _Scripts._Game.General.Managers;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;

namespace _Scripts._Game.UI.Dialogue{
    
    public class TypewriterEffect : BaseWriterEffect
    {

        public override IEnumerator TypeText(string textToType, TMP_Text textLabel)
        {
            textLabel.text = "";
            float t = 0;
            int charIndex = 0;

            while (charIndex < textToType.Length)
            {
                t += Time.deltaTime;
                int newcharIndex = Mathf.FloorToInt(t);

                if (newcharIndex > charIndex)
                {
                    charIndex = Mathf.Clamp(newcharIndex, 0, textToType.Length);

                    textLabel.text = textToType.Substring(0, charIndex);
                }

                yield return null;
            }
        }

        public override IEnumerator TypeText(Phrase phrase, TMP_Text textLabel)
        {
            textLabel.text = "";
            float t = 0;
            int charIndex = 0;
            string textToType = phrase.Text;

            if (phrase.StartOfPhraseWait > 0.0f)
            {
                yield return TaskManager.Instance.WaitForSecondsPool.Get(phrase.StartOfPhraseWait);
            }

            while (charIndex < textToType.Length)
            {
                t += Time.deltaTime * phrase.TextSpeed;

                int newcharIndex = Mathf.FloorToInt(t);

                if (newcharIndex > charIndex)
                {
                    charIndex = Mathf.Clamp(newcharIndex, 0, textToType.Length);

                    textLabel.text = textToType.Substring(0, charIndex);
                }

                yield return null;
            }

            if (phrase.EndOfPhraseWait > 0.0f)
            {
                yield return TaskManager.Instance.WaitForSecondsPool.Get(phrase.EndOfPhraseWait);
            }
            
            textLabel.text = "";
        }
    }
}
