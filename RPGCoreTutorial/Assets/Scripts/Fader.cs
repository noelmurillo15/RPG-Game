using UnityEngine;
using System.Collections;


namespace RPG.SceneManagement
{    
    public class Fader : MonoBehaviour {

        CanvasGroup canvasGroup;


        void Start() {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public IEnumerator FadeOut(float _time){
            while(canvasGroup.alpha < 1f){
                canvasGroup.alpha += Time.deltaTime / _time;
                yield return null;
            }        
            StartCoroutine(FadeIn(2f));    
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