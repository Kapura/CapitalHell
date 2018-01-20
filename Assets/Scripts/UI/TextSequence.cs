using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace BulletHell.UI
{
    public class TextSequence : MonoBehaviour
    {
        public PlayerData pData;

        [Serializable]
        public struct DialoguePage
        {
            [Multiline]
            public string text;
        }

        public string firstNameSubstitutionString = "$fName";
        public DialoguePage[] pages;

        public Text dialogueBox;
        public float fadeDuration = 0.5f;

        public UnityEvent onSequenceComplete;

        private int _currentPage = -1;

        private bool advancing = false;

        private void OnEnable()
        {
            AdvancePage();
        }

        private void Update()
        {
            if (Input.GetButtonDown("Fire") || Input.GetButtonDown("Fire1"))
            {
                AdvancePage();
            }
        }
        
        public void AdvancePage()
        {
            if (advancing)
                return;  // TODO: fastforward the fade

            StartCoroutine(DoPageAdvance());
        }

        private IEnumerator DoPageAdvance()
        {
            advancing = true;
            Color fullColor = dialogueBox.color;
            Color emptyColor = fullColor;
            emptyColor.a = 0f;

            // Fade out
            float startTime = Time.time;
            float endTime = startTime + fadeDuration;
            while (Time.time < endTime)
            {
                float t = (Time.time - startTime) / fadeDuration;
                dialogueBox.color = Color.Lerp(fullColor, emptyColor, t);
                yield return null;
            }
            dialogueBox.color = emptyColor;

            // Replace page
            _currentPage++;
            if (_currentPage >= pages.Length)
            {
                onSequenceComplete.Invoke();
                this.enabled = false;
                yield break;
            }

            dialogueBox.text = pages[_currentPage].text.Replace(firstNameSubstitutionString, pData.selectedCharacter.FirstName);

            // Fade in
            startTime = Time.time;
            endTime = startTime + fadeDuration;
            while (Time.time < endTime)
            {
                float t = (Time.time - startTime) / fadeDuration;
                dialogueBox.color = Color.Lerp(emptyColor, fullColor, t);
                yield return null;
            }
            dialogueBox.color = fullColor;
            advancing = false;
        }
    }
}