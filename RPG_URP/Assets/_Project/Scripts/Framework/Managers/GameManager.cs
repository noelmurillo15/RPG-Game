/*
 * GameManager - Backbone of the game application
 * Contains data that needs to persist and be accessed from anywhere
 * Created by : Allan N. Murillo
 * Last Edited : 3/1/2021
 */

using System;
using ANM.Utils;
using UnityEngine;
using ANM.Scriptables;
using ANM.Framework.Audio;
using ANM.Framework.Events;
using ANM.Framework.Options;
using ANM.Framework.Extensions;
using AudioType = ANM.Framework.Audio.AudioType;

namespace ANM.Framework.Managers
{
    public sealed class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("Game Events")]
        [SerializeField] private GameEvent onGameResume = null;
        [SerializeField] private GameEvent onGamePause = null;
        [Space] [Header("Local Game Info")]
        [SerializeField] private bool displayFps = false;
        [SerializeField] private bool isGamePaused = false;
        [SerializeField] private bool debug = false;
        [Space] [Header("Cursor")]
        [SerializeField] private Texture2D customCursor = null;

        private static ResourcesManager _resources;
        private FpsDisplay _fpsDisplay;
        private SaveSettings _save;


        #region Unity Funcs

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Log("Awake");
            _resources = ResourcesManager.FindResource("ResourcesManager") as ResourcesManager;
            SaveSettings.settingsLoadedIni = false;
            Application.targetFrameRate = -1;
            DontDestroyOnLoad(gameObject);
            _save = new SaveSettings();
            _save.Initialize();
            Instance = this;
        }

        private void Start()
        {
            Log("Start");
            Cursor.visible = true;
            _resources?.Initialize();
            Invoke(nameof(Initialize), 2f);
            Cursor.lockState = CursorLockMode.None;
            Cursor.SetCursor(customCursor, Vector2.zero, CursorMode.Auto);
        }

        private void OnDestroy()
        {
            if (Instance != this) return;
            Resources.UnloadUnusedAssets();
            Log("OnDestroy");
            GC.Collect();
        }

        #endregion

        #region Public Funcs

        public void SaveGameSettings() => _save.SaveGameSettings();

        public void TogglePause() => SetPause(!isGamePaused);

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

        public void SoftReset()
        {
            Log("SoftReset");
            SetPause(false);
        }

        public static void HardReset()
        {
            Debug.Log("[GM]: Hard Reset");
        }

        public static void ReturnToMenu(MenuPageType pageToLoad)
        {
            Debug.Log("[GM]: ReturnToMenu");
            SceneExtension.TrySwitchToScene(SceneExtension.MenuUiSceneName);
            SceneExtension.UnloadAllScenesExcept(SceneExtension.MenuUiSceneName);
            FindObjectOfType<MenuManager>().Reset();
            var controller = FindObjectOfType<MenuPageController>();
            controller.TurnMenuPageOff(controller.GetCurrentMenuPageType(), pageToLoad);
        }

        public static ResourcesManager GetResources() => _resources;

        #endregion

        #region Private Funcs

        private void Initialize()
        {
            Log("Initialize");
#if UNITY_EDITOR
            Debug.unityLogger.logEnabled = true;
            AudioController.instance.PlayAudio(AudioType.St01, true, 2f);
            if (SceneExtension.IsThisSceneActive(SceneExtension.MenuUiSceneName)) return;
            StartCoroutine(SceneExtension.ForceMainMenu(true));
#else
            Debug.unityLogger.logEnabled = debug;
            AudioController.instance.PlayAudio(AudioType.St01);
            StartCoroutine(SceneExtension.ForceMainMenu(true));
#endif
        }

        private void SetPause(bool b)
        {
            if (isGamePaused == b) return;
            if (b) RaisePause();
            else RaiseResume();
        }

        private void RaisePause()
        {
            if (isGamePaused) return;
            Cursor.SetCursor(customCursor, Vector2.zero, CursorMode.Auto);
            Time.timeScale = 0;
            isGamePaused = true;
            onGamePause.Raise();
        }

        private void RaiseResume()
        {
            if (!isGamePaused) return;
            Time.timeScale = 1;
            isGamePaused = false;
            onGameResume.Raise();
        }

        private void Log(string log)
        {
            if (!debug) return;
            Debug.Log("[GM]: " + log);
        }

        #endregion
    }
}
