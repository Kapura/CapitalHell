using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace BulletHell.UI
{
    public class ScrimFader : MonoBehaviour
    {
        public Image scrimImage;

        public Color fullColor = Color.black;
        public Color emptyColor = Color.clear;

        public bool startWithFade = false;
        public float startFadeDuration = 2f;

        public UnityEvent OnPostFadeOut;
        public UnityEvent OnPostFadeIn;

        private void Awake()
        {
            if (startWithFade)
                FadeOut(startFadeDuration);
        }

        public void FadeOut(float duration)
        {
            StartCoroutine(DoFade(fullColor, emptyColor, duration));
        }

        public void FadeIn(float duration)
        {
            StartCoroutine(DoFade(emptyColor, fullColor, duration));
        }

        private IEnumerator DoFade(Color startColor, Color endColor, float duration)
        {
            scrimImage.enabled = true;

            float startTime = Time.time;
            float endTime = Time.time + duration;

            while (Time.time < endTime)
            {
                float t = (Time.time - startTime) / duration;

                scrimImage.color = Color.Lerp(startColor, endColor, t);
                yield return null;
            }
            scrimImage.color = endColor;

            bool fadedIn = startColor.a < endColor.a;
            scrimImage.enabled = fadedIn;
            (fadedIn ? OnPostFadeIn : OnPostFadeOut).Invoke();
        }
    }
}