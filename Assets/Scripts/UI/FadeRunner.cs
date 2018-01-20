using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BulletHell.UI
{
    public class FadeRunner : MonoBehaviour
    {
        public float prefadeDelay = 3f;
        [Space]
        public float fadeDuration;
        public AnimationCurve fadeCurve;

        public Color fullColor;
        public Color emptyColor = Color.clear;

        public Text[] textPanels;

        public UnityEvent onPostFade;

        private void Awake()
        {
            foreach (Text t in textPanels)
            {
                t.color = emptyColor;
                t.enabled = false;
            }
        }

        public void StartFades()
        {
            StartCoroutine(RunTheFades());
        }

        private IEnumerator RunTheFades()
        {
            yield return new WaitForSeconds(prefadeDelay);

            int currentPanel = 0;

            while (currentPanel < textPanels.Length)
            {
                Text currentText = textPanels[currentPanel];
                currentText.enabled = true;
                float startTime = Time.time;
                float endTime = startTime + fadeDuration;

                while (Time.time <= endTime)
                {
                    float t = (Time.time - startTime) / fadeDuration;
                    currentText.color = Color.Lerp(emptyColor, fullColor, fadeCurve.Evaluate(t));
                    yield return null;
                }
                currentText.enabled = false;
                currentPanel++;
            }

            if (onPostFade != null)
                onPostFade.Invoke();
        }

    }
}