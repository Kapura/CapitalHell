using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BulletHell.UI
{
    public class MoneyLayer : MonoBehaviour
    {
        public float minScale, maxScale;

        [Header("Appear animation")]
        public float animationDuration = 0.5f;
        [Space]
        public float animationStartScale = 2f;
        public AnimationCurve scaleCurve = AnimationCurve.Linear(0, 0, 1, 1);
        public Color initialColor = new Color(1, 1, 1, 0.2f);
        public AnimationCurve colorCurve = AnimationCurve.Linear(0, 0, 1, 1);

        [Header("Audio")]
        public AudioSource source;
        public AudioClip[] moneyClips;

        [Header("BGM")]
        public AudioSource bgmSource;
        public int countdownStart = 20;
        public AnimationCurve volumeCurve;
        [Header("Green Screen")]
        public Image greenScreen;
        public Color emptyGreen;
        public Color fullGreen;
        public AnimationCurve greenCurve = AnimationCurve.Linear(0, 0, 1, 1);

        private List<SpriteRenderer> _moneyRenderers = new List<SpriteRenderer>();
        private int _sortingOrder = 0;

        public void Awake()
        {
            foreach (Transform child in this.transform)
            {
                child.localScale = Vector3.one * Random.Range(minScale, maxScale);
                child.localRotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
                SpriteRenderer renderer = child.GetComponent<SpriteRenderer>();
                renderer.enabled = false;
                _moneyRenderers.Add(renderer);
            }

            bgmSource.volume = 0f;
        }

        public bool GetMoney()
        {
            if (_moneyRenderers.Count == 0)
                return true;

            int index = Random.Range(0, _moneyRenderers.Count);
            StartCoroutine(MoneyAnimation(_moneyRenderers[index]));
            _moneyRenderers.RemoveAt(index);

            if (_moneyRenderers.Count <= countdownStart)
            {
                float t = (countdownStart - _moneyRenderers.Count) / (float)countdownStart;
                
                bgmSource.volume = volumeCurve.Evaluate(t);

                if (!bgmSource.isPlaying)
                    bgmSource.Play();

                float colorVal = greenCurve.Evaluate(t);

                greenScreen.color = Color.Lerp(emptyGreen, fullGreen, colorVal);

                if (!greenScreen.enabled)
                    greenScreen.enabled = true;
            }

            return _moneyRenderers.Count == 0;
        }

        private IEnumerator MoneyAnimation(SpriteRenderer rend)
        {
            source.PlayOneShot(moneyClips[Random.Range(0, moneyClips.Length)]);

            Transform tf = rend.transform;

            Vector3 finalScale = tf.localScale;
            Vector3 initialScale = Vector3.one * animationStartScale;

            rend.enabled = true;
            rend.sortingOrder = _sortingOrder;
            _sortingOrder++;

            float startTime = Time.time;
            float endTime = Time.time + animationDuration;

            while (Time.time < endTime)
            {
                float t = (Time.time - startTime) / animationDuration;
                tf.localScale = Vector3.LerpUnclamped(initialScale, finalScale, scaleCurve.Evaluate(t));
                rend.color = Color.Lerp(initialColor, Color.white, colorCurve.Evaluate(t));
                yield return null;
            }

            tf.localScale = finalScale;
            rend.color = Color.white;
        }
    }
}