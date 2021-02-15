using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        CanvasGroup canvasGroup; // located in fader canvas inspector

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

        public IEnumerator FadeOut(float time)
        {
            while (canvasGroup.alpha < 1) // alpha is not 1
            {
                canvasGroup.alpha += Time.deltaTime / time;
                yield return null; //with null, coroutine will run again on next possible frame
            }
        }

        public IEnumerator FadeIn(float time)
        {
            while (canvasGroup.alpha > 0) // alpha is not 1
            {
                canvasGroup.alpha -= Time.deltaTime / time;
                yield return null; //with null, coroutine will run again on next possible frame
            }
        }
    }

}

