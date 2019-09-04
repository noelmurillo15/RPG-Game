using UnityEngine;
using System.Collections;


namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        //  Cached Variables
        CanvasGroup canvasGroup;


        void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void FadeOutImmediate()
        {
            canvasGroup.alpha = 1;
        }

        public IEnumerator FadeOut(float _time)
        {
            while (canvasGroup.alpha < 1f)
            {
                canvasGroup.alpha += Time.deltaTime / _time;
                yield return null;
            }
        }

        public IEnumerator FadeIn(float _time)
        {
            while (canvasGroup.alpha > 0f)
            {
                canvasGroup.alpha -= Time.deltaTime / _time;
                yield return null;
            }
        }
    }
}