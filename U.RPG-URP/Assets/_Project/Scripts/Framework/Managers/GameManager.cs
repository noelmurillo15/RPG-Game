/*
 * GameManager - Backbone of the game application
 * Contains data that needs to persist and be accessed from anywhere
 * Created by : Allan N. Murillo
 * Last Edited : 3/11/2022
 */

using System;
using ANM.Utils;
using UnityEngine;
using ANM.Framework.Events;
using ANM.Framework.Options;
using ANM.Scriptables.Manager;
using ANM.Framework.Extensions;

namespace ANM.Framework.Managers
{
    public sealed class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("Game Events")]
        [SerializeField] private GameEvent onGameResume = null;
        [SerializeField] private GameEvent onGamePause = null;

        [Space] [Header("Local Game Info")]
        [SerializeField] private int score = 0;
        [SerializeField] private bool displayFps = false;
        [SerializeField] private bool isGamePaused = false;
        [SerializeField] private bool debug = false;

        [Space] [Header("Cursor")]
        [SerializeField] private Texture2D customCursor = null;

        private SaveSettings _save;
        private FpsDisplay _fpsDisplay;
        public PlayerAvatar userChoiceAvatar;


        #region Unity Funcs

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

#if UNITY_EDITOR
            Debug.unityLogger.logEnabled = true;
#else
            Debug.unityLogger.logEnabled = debug;
#endif

            /*if (SystemInfo.operatingSystem.Contains("Mac OS"))
                macOS = true;*/

            Log("Awake");
            //userChoiceAvatar = PlayerAvatar.None;
            SaveSettings.SettingsLoadedIni = false;
            Application.targetFrameRate = -1;
            DontDestroyOnLoad(gameObject);
            _save = new SaveSettings();
            _save.Initialize();
            Instance = this;
        }

        private void Start()
        {
            //Log("Start");
            Cursor.visible = true;
            ResourcesManager.Initialize();
            Cursor.lockState = CursorLockMode.None;
            Cursor.SetCursor(customCursor, Vector2.zero, CursorMode.Auto);
            
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (!SceneExtension.IsThisSceneActive("Level 1")) return;
            //Log("OnApplicationHasFocus - " + hasFocus);
            WindowLostFocus(hasFocus);
        }

        private void OnDestroy()
        {
            if (Instance != this) return;
            //Log("OnDestroy");
            Resources.UnloadUnusedAssets();
            GC.Collect();
            QuitWebGL();
        }

        #endregion

        #region Public Funcs
        
        public void AttachFpsDisplay(FpsDisplay fps = null)
        {
            _fpsDisplay = fps;
            _fpsDisplay?.ToggleFpsDisplay(displayFps);
        }

        public void SetDisplayFps(bool b)
        {
            displayFps = b;
            _fpsDisplay?.ToggleFpsDisplay(b);
        }

        public void SaveGameSettings()
        {
            _save.SaveGameSettings();
            SaveToDatabase();
        }

        public void ReloadScene() => StartCoroutine(SceneExtension.ReloadCurrentSceneSequence());

        public void IncreaseScore(int amount) => score += amount;

        public void DecreaseScore(int amount) => Mathf.Clamp(score - amount, 0f, 9999f);

        public int GetScore() => score;
        
        public void TogglePause() => SetPause(!isGamePaused);

        public void SoftReset()
        {
            //Log("SoftReset");
            SetScore(0);
            SetPause(false);
            Cursor.SetCursor(customCursor, Vector2.zero, CursorMode.Auto);
        }

        public static void HardReset()
        {
            //Debug.Log("[GM]: HardReset");
        }

        public static void ReturnToMenu(MenuPageType pageToLoad)
        {
            //Debug.Log("[GM]: ReturnToMenu");
            //SceneExtension.TrySwitchToScene(SceneExtension.MenuUiSceneName);
            SceneExtension.UnloadAllScenesExcept(SceneExtension.MenuUiSceneName);
            FindObjectOfType<MenuManager>().Reset();
            var controller = FindObjectOfType<MenuPageTransitionHandler>();
            controller.TurnMenuPageOff(controller.GetCurrentMenuPageType(), pageToLoad);
        }

        #endregion

        #region Private Funcs

        private void SetPause(bool b)
        {
            //Log("Setting Pause - " + b);
            if (isGamePaused == b) return;
            if (b) RaisePause();
            else RaiseResume();
        }

        private void RaisePause()
        {
            if (isGamePaused) return;
            Cursor.SetCursor(customCursor, Vector2.zero, CursorMode.Auto);
            isGamePaused = true;
            onGamePause.Raise();
            Time.timeScale = 0;
        }

        private void RaiseResume()
        {
            if (!isGamePaused) return;
            //Log("RaiseResume");
            Time.timeScale = 1;
            isGamePaused = false;
            onGameResume.Raise();
        }

        //  Called from WebBrowserInteraction.jslib
        private void RaiseLimitedPause()
        {
            if (isGamePaused) return;
            //Log("RaiseLimitedPause");
            Cursor.SetCursor(customCursor, Vector2.zero, CursorMode.Auto);
            isGamePaused = true;
            onGamePause.Raise();
        }

        private void SetScore(int amount)
        {
            //Log("Setting score - " + amount);
            score = amount;
        }

        //  Called from WebBrowserInteraction.jslib
        private void LoadSettingsFromIndexedDb()
        {
            //Log("LoadSettingsFromIndexedDb");
            SaveSettings.SettingsLoadedIni = _save.LoadGameSettings();
        }

        private void Log(string log)
        {
            if (!debug) return;
            Debug.Log("[GM]: " + log);
        }

        #endregion

        #region External JS Lib

#if UNITY_WEBGL && !UNITY_EDITOR
        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void Save();
        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void LostFocus(bool hasFocus);
        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void QuitCleanup();

        private static void WindowLostFocus(bool hasFocus) => LostFocus(hasFocus);
        private static void QuitWebGL() => QuitCleanup();
        private static void SaveToDatabase() => Save();
#else
        private static void WindowLostFocus(bool hasFocus) { }
        private static void SaveToDatabase() { }
        private static void QuitWebGL() { }
#endif

        #endregion
    }

    public enum PlayerAvatar
    {
        None,
        Male,
        Female
    }
}
