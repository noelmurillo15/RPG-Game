using UnityEngine;
using System.Collections;


namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        //  Cached Variables
        private CanvasGroup _canvasGroup;
        private Coroutine _currentFade = null;


        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void FadeOutImmediate()
        {
            _canvasGroup.alpha = 1;
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
            if (_currentFade != null) { StopCoroutine(_currentFade); }
            _currentFade = StartCoroutine(FadeRoutine(target, time));
            return _currentFade;
        }

        private IEnumerator FadeRoutine(float target, float time)
        {
            while (!Mathf.Approximately(_canvasGroup.alpha, target))
            {
                _canvasGroup.alpha = Mathf.MoveTowards(_canvasGroup.alpha, target, Time.deltaTime / time);
                yield return null;
            }
        }
    }
}