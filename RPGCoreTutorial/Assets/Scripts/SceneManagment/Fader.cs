using UnityEngine;
using System.Collections;


namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        //  Cached Variables
        CanvasGroup canvasGroup;
        Coroutine currentFade = null;


        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void FadeOutImmediate()
        {
            canvasGroup.alpha = 1;
        }

        public Coroutine FadeOut(float time){
            return Fade(1f, time);
        }

        public Coroutine FadeIn(float time)
        {
            return Fade(0f, time);
        }

        private Coroutine Fade(float target, float time)
        {
            if (currentFade != null) { StopCoroutine(currentFade); }
            currentFade = StartCoroutine(FadeRoutine(target, time));
            return currentFade;
        }

        private IEnumerator FadeRoutine(float target, float time)
        {
            while (!Mathf.Approximately(canvasGroup.alpha, target))
            {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, target, Time.deltaTime / time);
                yield return null;
            }
        }
    }
}