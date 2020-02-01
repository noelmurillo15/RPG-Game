/*
 * SceneTransitionManager - Used for async scene transitions
 * Fades the screen in-between loading scenes
 * Created by : Allan N. Murillo
 */

using System.IO;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace ANM.Framework
{
    [RequireComponent(typeof(CanvasGroup))]
    public class SceneTransitionManager : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private float fadeOutDelay = 2f;
        [SerializeField] private float fadeInDelay = 0.5f;

        private const string MenuUiSceneName = "Menu Ui";
        private const string CreditsSceneName = "ExitScreen";
        private const string GameplaySceneName = "Level 1";
        
        public GameEvent onLoadScene;
        public GameEvent onFinishLoadScene;
        
        [SerializeField] private string[] _sceneNames = null;
        private Coroutine _currentFade = null;
        

        private void Start()
        {
            Debug.Log("SceneTransitionManager::Start()");
            FadeInImmediate();
            canvasGroup = GetComponent<CanvasGroup>();
            
            var sceneNumber = SceneManager.sceneCountInBuildSettings;
            _sceneNames = new string[sceneNumber];
            for (var i = 0; i < sceneNumber; i++)
                _sceneNames[i] = Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
            
            if (IsThisSceneActive(MenuUiSceneName)) return;
            LoadMenuUi();
        }
        
        public static bool IsMainMenuActive()
        {
            return IsThisSceneActive(MenuUiSceneName);
        }
        
        private static string GetCurrentSceneName()
        {
            return SceneManager.GetActiveScene().name;
        }
        
        private static bool IsThisSceneActive(string sceneName)
        {
            return GetCurrentSceneName().Contains(sceneName);
        }
        
        private void LoadMenuUi()
        {
            Debug.Log("SceneTransitionManager::LoadMenuUi()");
            if (SceneManager.GetSceneByName(MenuUiSceneName).isLoaded) return;
            SceneManager.LoadSceneAsync(MenuUiSceneName, LoadSceneMode.Additive).completed += operation =>
            {
                if (!SceneManager.GetSceneByName(_sceneNames[0]).isLoaded) return;
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(MenuUiSceneName));
                SceneManager.UnloadSceneAsync(_sceneNames[0]);
            };
        }
        
        public void LoadCredits()
        {
            Debug.Log("SceneTransitionManager::LoadCredits()");
            StartCoroutine(LoadNewScene(CreditsSceneName));
        }

        public void ReloadCurrentScene()
        {
            Debug.Log("SceneTransitionManager::ReloadCurrentScene()");
            string sceneToBeReloaded = GetCurrentSceneName();
            if (SceneManager.SetActiveScene(SceneManager.GetSceneByName(MenuUiSceneName)))
            {
                SceneManager.UnloadSceneAsync(sceneToBeReloaded).completed += operation =>
                {
                    StartCoroutine(LoadNewScene(sceneToBeReloaded));
                };
            }
        }

        public void LoadStartingLevel()
        {
            Debug.Log("SceneTransitionManager::LoadStartingLevel()");
            StartCoroutine(LoadNewScene(GameplaySceneName));
        }
        
        public void SwitchToLoadedScene(string sceneName)
        {
            Debug.Log("SceneTransitionManager::SwitchToLoadedScene() : " + sceneName);
            if (!SceneManager.GetSceneByName(sceneName).isLoaded) return;
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
        }

        public void UnloadAllSceneExcept(string sceneName)
        {
            Debug.Log("SceneTransitionManager::UnloadAllSceneExcept() : " + sceneName);
            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                if (SceneManager.GetSceneAt(i).name ==  sceneName)
                {
                    SceneManager.SetActiveScene(SceneManager.GetSceneAt(i));
                    continue;
                }
                
                if (SceneManager.GetSceneAt(i).isLoaded)
                {
                    SceneManager.UnloadSceneAsync(
                        SceneManager.GetSceneByName(SceneManager.GetSceneAt(i).name));
                }
            }
        }

        private IEnumerator LoadNewScene(string sceneName)
        {
            Debug.Log("SceneTransitionManager::LoadNewScene() : " + sceneName);
            onLoadScene.Raise();
            yield return FadeOut();
            AsyncOperation async = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            while (!async.isDone) { yield return null; }
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
            onFinishLoadScene.Raise();
            yield return FadeIn();
        }
        
        public void LoadSceneEvent()
        {    //    Handled by onStartSceneTransition ScriptableObject
            GameManager.Instance.IsSceneTransitioning = true;
        }

        public void FinishLoadSceneEvent()
        {    //    Handled by onFinishSceneTransition ScriptableObject
            GameManager.Instance.IsSceneTransitioning = false;
        }
        
        #region Screen Fade
        public void FadeOutImmediate()
        {
            canvasGroup.alpha = 1f;
        }
        
        public Coroutine FadeOut()
        {
            return Fade(1f, fadeOutDelay);
        }
        
        private void FadeInImmediate()
        {
            canvasGroup.alpha = 0f;
        }
        
        public Coroutine FadeIn()
        {
            return Fade(0f, fadeInDelay);
        }

        private Coroutine Fade(float target, float time)
        {
            if (_currentFade != null) { StopCoroutine(_currentFade); }
            _currentFade = StartCoroutine(FadeRoutine(target, time));
            return _currentFade;
        }

        private IEnumerator FadeRoutine(float target, float time)
        {
            while (!Mathf.Approximately(canvasGroup.alpha, target))
            {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, target, Time.deltaTime / time);
                yield return null;
            }
        }
        #endregion
    }
}
