using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

namespace BulletHell.UI
{
    public class SelectionManager : MonoBehaviour
    {
        public PlayerData pData;

        public AudioClip selectClip;
        public AudioClip beginClip;

        [Space]
        public SelectablePanel[] selectablePanels;

        public GameObject startButton;
        public UnityEvent onBegin;
        [Header("Transition")]
        public AudioSource src;
        public float preAudioPause = 2f;
        public float postAudioPause = 6f;
        public string nextScene;

        private bool started = false;

        public void Awake()
        {
            startButton.SetActive(false);
        }

        public void DeselectAllPanels()
        {
            foreach (SelectablePanel p in selectablePanels)
            {
                p.DeselectPanel();
            }
        }

        public void SelectPanel(SelectablePanel panel)
        {
            if (started) return;

            src.PlayOneShot(selectClip);
            DeselectAllPanels();
            panel.SelectPanel();
            pData.selectedCharacter = panel.associatedCharacter;
            startButton.SetActive(true);
        }

        public void BeginBattle()
        {
            if (started) return;
            started = true;
            onBegin.Invoke();
            src.PlayOneShot(beginClip);
            StartCoroutine(DoTransition());

            Analytics.CustomEvent("CharacterSelected", new Dictionary<string, object> { { "Character", pData.selectedCharacter.characterName } });
        }

        private IEnumerator DoTransition()
        {
            yield return new WaitForSeconds(preAudioPause);
            src.Play();
            yield return new WaitForSeconds(postAudioPause);
            SceneManager.LoadScene(nextScene);
        }
    }
}