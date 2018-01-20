using System.Collections;
using UnityEngine;

namespace BulletHell.UI
{
    public class AudioFade : MonoBehaviour
    {
        public AudioSource src;

        public void FadeOut(float duration)
        {
            StartCoroutine(DoFadeOut(duration));
        }

        private IEnumerator DoFadeOut(float duration)
        {
            float startTime = Time.time;
            float endTime = startTime + duration;

            while (Time.time < endTime)
            {
                float t = (Time.time - startTime) / duration;
                src.volume = 1 - t;
                yield return null;
            }

            src.Stop();
        }
    }
}