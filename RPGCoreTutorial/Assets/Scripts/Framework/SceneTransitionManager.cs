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
        [SerializeField] private float fadeOutDelay = 1.5f;
        [SerializeField] private float fadeInDelay = 0.5f;

        public GameEvent onStartLoadScene;
        public GameEvent onFinishLoadScene;
        
        private const string MenuUiSceneName = "Menu Ui";
        private const string CreditsSceneName = "Credits";
        private const string GameplaySceneName = "Level 1";
        
        private string[] _sceneNames;
        private Coroutine _currentFade;
        private Scene _oldScene;
        

        private void Start()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            FadeInImmediate();
            
            var sceneNumber = SceneManager.sceneCountInBuildSettings;
            _sceneNames = new string[sceneNumber];
            for (var i = 0; i < sceneNumber; i++)
                _sceneNames[i] = Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
            
            if (IsThisSceneActive(MenuUiSceneName)) return;
            LoadMenuUi();
        }
        
        public void LoadStartingLevel()
        {
            StartCoroutine(LoadMultiScene(GameplaySceneName));
        }

        public void LoadCredits()
        {
            StartCoroutine(LoadSingleScene(CreditsSceneName));
        }

        public void ReloadCurrentScene()
        {    //    TODO : Not working properly atm
            Debug.Log("SceneTransitionManager::ReloadCurrentScene()");
            string sceneToBeReloaded = GetCurrentScene().name;
            if (SceneManager.SetActiveScene(SceneManager.GetSceneByName(MenuUiSceneName)))
            {
                SceneManager.UnloadSceneAsync(sceneToBeReloaded).completed += operation =>
                {
                    StartCoroutine(LoadMultiScene(sceneToBeReloaded));
                };
            }
        }
        
        public IEnumerator BeginLoadScene(int buildIndex)
        {
            onStartLoadScene.Raise();
            yield return FadeOut();
            yield return SceneManager.LoadSceneAsync(buildIndex, LoadSceneMode.Additive);
            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(buildIndex));
        }
        
        public IEnumerator EndLoadScene()
        {
            onFinishLoadScene.Raise();
            yield return FadeIn();
        }
        
        public static IEnumerator BeginLoadNextLevel(int buildIndex)
        {
            for (int x = 0; x < SceneManager.sceneCount; x++)
            {
                Scene sceneName = SceneManager.GetSceneAt(x);
                if (sceneName.name.Contains(MenuUiSceneName)) continue;
                SceneManager.UnloadSceneAsync(sceneName);
            }
            
            yield return SceneManager.LoadSceneAsync(buildIndex, LoadSceneMode.Additive);
            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(buildIndex));
        }
        
        public void SwitchBackToLevel()
        {
            if (!_oldScene.isLoaded) return;
            SceneManager.SetActiveScene(_oldScene);
        }
        
        public void SwitchToMenuUi()
        {
            _oldScene = GetCurrentScene();
            if (!SceneManager.GetSceneByName(MenuUiSceneName).isLoaded) return;
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(MenuUiSceneName));
        }

        public static void UnloadAllSceneExceptMenu()
        {
            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                if (SceneManager.GetSceneAt(i).name.Contains(MenuUiSceneName))
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
        
        public static bool IsMainMenuActive()
        {
            return SceneManager.GetActiveScene() == SceneManager.GetSceneByName(MenuUiSceneName);
        }

        public static int GetCurrentSceneBuildIndex()
        {
            return SceneManager.GetActiveScene().buildIndex;
        }
        
        public Scene GetCurrentScene()
        {
            return SceneManager.GetActiveScene();
        }
        
        private bool IsThisSceneActive(string sceneName)
        {
            return GetCurrentScene().name.Contains(sceneName);
        }
        
        private void LoadMenuUi()
        {
            if (SceneManager.GetSceneByName(MenuUiSceneName).isLoaded) return;
            SceneManager.LoadSceneAsync(MenuUiSceneName, LoadSceneMode.Additive).completed += operation =>
            {
                if (!SceneManager.GetSceneByName(_sceneNames[0]).isLoaded) return;
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(MenuUiSceneName));
                SceneManager.UnloadSceneAsync(_sceneNames[0]);
            };
        }
        
        private IEnumerator LoadMultiScene(string sceneName)
        {
            onStartLoadScene.Raise();
            yield return FadeOut();
            yield return LoadAdditiveScene(sceneName);
            yield return EndLoadScene();
        }
        
        private IEnumerator LoadFastMultiScene(string sceneName)
        {
            FadeOutImmediate();
            yield return LoadAdditiveScene(sceneName);
            yield return FadeIn();
        }

        private static IEnumerator LoadAdditiveScene(string sceneName)
        {
            yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
        }

        private static IEnumerator LoadSingleScene(string sceneName)
        {
            yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
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
