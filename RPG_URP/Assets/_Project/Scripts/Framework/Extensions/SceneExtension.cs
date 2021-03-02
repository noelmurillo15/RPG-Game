/*
 * SceneExtension - Static Class used for async scene transitions via Co-routines from anywhere
 * Classes can subscribe to SceneLoadEvents and be notified before and after a scene transition occured
 * Created by : Allan N. Murillo
 * Last Edited : 3/1/2021
 */

using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace ANM.Framework.Extensions
{
    public static class SceneExtension
    {
        public static event Action<bool, bool> StartSceneLoadEvent = (fade, save) => { };
        public static event Action<bool, bool> FinishSceneLoadEvent = (fade, save) => { };
        public const string MenuUiSceneName = "Menu Ui";


        #region Public Funcs

        public static int GetCurrentSceneBuildIndex() => GetCurrentScene().buildIndex;

        public static string GetCurrentSceneName() => GetCurrentScene().name;

        public static bool IsThisSceneActive(int buildIndex) => GetCurrentSceneBuildIndex() == buildIndex;
        public static bool IsThisSceneActive(string sceneName) => GetCurrentSceneName().Contains(sceneName);

        public static bool TrySwitchToScene(string sceneName)
        {
            var tryScene = GetLoadedScene(sceneName);
            if (!IsSceneLoaded(tryScene)) return false;
            SetThisSceneActive(tryScene);
            return true;
        }

        public static bool TrySwitchToScene(int buildIndex)
        {
            var tryScene = GetSceneFromBuildIndex(buildIndex);
            if (!IsSceneLoaded(tryScene)) return false;
            SetThisSceneActive(tryScene);
            return true;
        }

        public static void UnloadAllScenesExcept(string sceneName)
        {
            for (var sceneIndex = 0; sceneIndex < SceneManager.sceneCount; sceneIndex++)
            {
                var loadedScene = GetLoadedScene(sceneIndex);
                if (loadedScene.name.Contains(sceneName)) continue;
                UnloadThisActiveScene(loadedScene);
            }
        }

        #endregion

        #region Public Coroutines

        public static IEnumerator LoadSingleSceneSequence(string sceneName, bool fade = false)
        {
            yield return StartLoadWithFade(fade);
            LoadSingleSceneWithOnFinish(sceneName);
        }

        public static IEnumerator LoadMultiSceneSequence(string sceneName, bool fade = false)
        {
            yield return StartLoadWithFade(fade);
            LoadMultiSceneWithOnFinish(sceneName);
        }

        public static IEnumerator ReloadCurrentSceneSequence(bool fade = false, bool save = false)
        {
            var sceneToReload = GetCurrentSceneName();
            yield return StartLoadWithFade(fade, save);
            yield return ForceMainMenu();
            LoadMultiSceneWithOnFinish(sceneToReload);
        }

        public static IEnumerator LoadMultiSceneWithBuildIndexSequence(int index, bool fade = false, bool save = false)
        {
            if (index == -1) yield break;
            yield return StartLoadWithFade(fade);

            if (TrySwitchToScene(MenuUiSceneName)) UnloadAllScenesExcept(MenuUiSceneName);
            else yield return ForceMainMenu(true);

            yield return SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);
            SetThisSceneActive(GetSceneFromBuildIndex(index));
            yield return FinishLoadWithFade(fade);
        }

        public static IEnumerator LoadMenuInBackground()
        {
            if(IsSceneLoaded(MenuUiSceneName)) yield return null;
            var currentScene = GetCurrentScene();
            yield return SceneManager.LoadSceneAsync(MenuUiSceneName, LoadSceneMode.Additive);
            SetThisSceneActive(currentScene);
        }

        public static IEnumerator ForceMainMenu(bool unload = false, bool fade = false,
            bool save = false, int lastBuildIndex = -1)
        {
            if (lastBuildIndex != -1)
            {
                SetThisSceneActive(GetSceneFromBuildIndex(lastBuildIndex));
                InvokeStartScene(fade, save);
            }

            var menu = GetLoadedScene(MenuUiSceneName);
            if (IsSceneLoaded(menu))
            {
                SetThisSceneActive(menu);
                if (!unload) yield break;
                UnloadAllScenesExcept(MenuUiSceneName);
                InvokeFinishScene(fade, false);
                yield break;
            }

            LoadSingleSceneWithOnFinish(MenuUiSceneName);
        }

        public static IEnumerator StartLoadWithFade(bool fade = false, bool save = false)
        {
            InvokeStartScene(fade, save);
            if (fade) yield return new WaitForSeconds(1.1f);
        }

        public static IEnumerator FinishLoadWithFade(bool fade = false, bool load = false)
        {
            InvokeFinishScene(fade, load);
            if (fade) yield return new WaitForSeconds(1.1f);
        }

        #endregion

        #region Private Funcs

        private static bool IsSceneLoaded(Scene scene) => scene.isLoaded;

        private static bool IsSceneLoaded(string sceneName) => IsSceneLoaded(GetLoadedScene(sceneName));

        private static Scene GetCurrentScene() => SceneManager.GetActiveScene();

        private static Scene GetLoadedScene(int sceneIndex) => SceneManager.GetSceneAt(sceneIndex);

        private static Scene GetLoadedScene(string sceneName) => SceneManager.GetSceneByName(sceneName);

        private static Scene GetSceneFromBuildIndex(int buildIndex) => SceneManager.GetSceneByBuildIndex(buildIndex);

        private static void SetThisSceneActive(Scene scene) => SceneManager.SetActiveScene(scene);

        private static void UnloadThisActiveScene(Scene scene) => SceneManager.UnloadSceneAsync(scene);

        private static void LoadSingleSceneWithOnFinish(string sceneName)
        {
            //Debug.Log("[SceneExtension]: LoadSingleSceneWithOnFinish - "+sceneName);
            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single).completed +=
                callback => InvokeFinishScene(true, false);
        }

        private static void LoadMultiSceneWithOnFinish(string sceneName)
        {
            if (IsThisSceneActive(sceneName)) return;
            //Debug.Log("[SceneExtension]: LoadMultiSceneWithOnFinish - "+sceneName);
            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive).completed +=
                callback =>
                {
                    SetThisSceneActive(GetLoadedScene(sceneName));
                    InvokeFinishScene(true, true);
                };
        }

        private static void InvokeStartScene(bool fade = false, bool save = false)
        {
            StartSceneLoadEvent?.Invoke(fade, save);
        }

        private static void InvokeFinishScene(bool fade = false, bool load = false)
        {
            FinishSceneLoadEvent?.Invoke(fade, load);
        }

        #endregion
    }
}
