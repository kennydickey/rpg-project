using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SceneManageMent
{
    public class Fader : MonoBehaviour
    {

    }
}

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        CanvasGroup canvasGroup; // located in fader canvas inspector
        Coroutine currentlyActiveFade;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        private void Start()
        {
            //StartCoroutine(FadeOut(3f));
            StartCoroutine(FadeOutIn());
        }

        //Nested IEnum ex
        IEnumerator FadeOutIn()
        {
            yield return FadeOut(1f);
            print("faded out");
            yield return FadeIn(1f);
            print("faded in");
        }

        public void FadeOutImmediate()
        {
            canvasGroup.alpha = 1; // opaque
        }

        public IEnumerator FadeIn(float time)
        {
            return Fade(0, time);
        }

        public IEnumerator FadeOut(float time)
        {
            return Fade(1, time);
        }

        public IEnumerator Fade(float target, float time)
        {
            if (currentlyActiveFade != null)
            {
                StopCoroutine(currentlyActiveFade);
            }
            currentlyActiveFade = StartCoroutine(FadeRoutine(target, time));
            yield return currentlyActiveFade;
        }

        private IEnumerator FadeRoutine(float target, float time)
        {
            while (!Mathf.Approximately(canvasGroup.alpha, target)) // alpha is not 1
            {
                // sort of the same as += I think, set to move toward a 0 target or a 1 target
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, target, Time.deltaTime / time);
                yield return null; //with null, coroutine will run again on next possible frame
            }
        }
    }

}

