using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace BulletHell.UI
{
    public class TutorialDialogue : MonoBehaviour
    {
        public RectTransform rootTransform;

        public float expandDuration = 0.5f;
        public float stayDuration = 4f;
        public float collapseDuration = 0.5f;
        public float postCollapsePause = 0.5f;
        public UnityEvent afterCollapsePause;

        [ContextMenu("Do the thing")]
        public void DisplayDialogue()
        {
            StartCoroutine(DoDialogue());
        }

        private void Awake()
        {
            rootTransform.localScale = new Vector3(1, 0, 1);
        }

        private IEnumerator DoDialogue()
        {
            // Open
            float startTime = Time.time;
            float endTime = startTime + expandDuration;
            while (Time.time < endTime)
            {
                float t = (Time.time - startTime) / expandDuration;
                float y = Mathf.Lerp(0, 1, t);
                rootTransform.localScale = new Vector3(1, y, 1);
                yield return null;
            }
            rootTransform.localScale = Vector3.one;

            // Wait
            yield return new WaitForSeconds(stayDuration);

            // Close
            startTime = Time.time;
            endTime = startTime + collapseDuration;
            while (Time.time < endTime)
            {
                float t = (Time.time - startTime) / collapseDuration;
                float y = Mathf.Lerp(1, 0, t);
                rootTransform.localScale = new Vector3(1, y, 1);
                yield return null;
            }
            rootTransform.localScale = new Vector3(1, 0, 1);

            // Wait
            yield return new WaitForSeconds(postCollapsePause);

            afterCollapsePause.Invoke();
        }
    }
}