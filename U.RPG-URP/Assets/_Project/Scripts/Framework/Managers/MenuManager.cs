/*
 * MenuManager - Handles interactions with the Menu Ui
 * Created by : Allan N. Murillo
 * Last Edited : 3/17/2022
 */

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using ANM.Input;
using ANM.Framework.Options;
using ANM.Scriptables.Manager;
using ANM.Framework.Extensions;
using UnityEngine.EventSystems;

namespace ANM.Framework.Managers
{
    public class MenuManager : MonoBehaviour
    {
        [Header("Menu Panels")]
        [SerializeField] private AudioOptionsPanel audioOptions = null;
        [SerializeField] private VideoOptionsPanel videoOptions = null;
        [SerializeField] private QuitOptionsPanel quitOptionsPanel = null;
        [SerializeField] private Button mainPanelSelectedObj = null;
        [SerializeField] private Button pausePanelSelectedObj = null;
        [SerializeField] private Button quitPanelSelectedObj = null;
        [Space] [Header("Local Game Info")]
        [SerializeField] private bool isSceneTransitioning = false;
        [SerializeField] private bool isMainMenuActive = false;
        [SerializeField] private int lastSceneBuildIndex = 0;

        private bool _isPaused;
        private Camera _menuUiCamera;
        private EventSystem _eventSystem;
        private InputController _inputController;
        private MenuPageTransitionHandler _transitionHandler;
        private GameManager _gameManager;
        private const float DesignWidth = 1920;
        private const float DesignHeight = 1080;


        #region Unity Funcs

        private void Awake()
        {
            isSceneTransitioning = true;
            _gameManager = GameManager.Instance;
            _eventSystem = FindObjectOfType<EventSystem>();
            _transitionHandler = FindObjectOfType<MenuPageTransitionHandler>();
            if (!SaveSettings.SettingsLoadedIni) SaveSettings.DefaultSettings();
        }

        private void Start()
        {
            _menuUiCamera = FindObjectOfType<Camera>();
            isMainMenuActive = SceneExtension.IsThisSceneActive(SceneExtension.MenuUiSceneName);
            SceneExtension.FinishSceneLoadEvent += OnFinishLoadScene;
            SceneExtension.StartSceneLoadEvent += OnStartLoadScene;
            ApplyIniSettings();
            ControllerSetup();
            _isPaused = false;
        }

        private void OnGUI()
        {
            if (isMainMenuActive) return;
            
            //  Dynamic Scaling
            int w = Screen.width, h = Screen.height;
            var resX = w / DesignWidth;
            var resY = h / DesignHeight;
            GUI.matrix = Matrix4x4.TRS(new Vector3(0, 0, 0), Quaternion.identity, new Vector3(resX, resY, 1));
            
            //  Custom Style
            var style = new GUIStyle();
            var rect = new Rect(16, 120, 80, 32);
            style.alignment = TextAnchor.UpperLeft;
            style.normal.textColor = Color.white;
            style.fontSize = 24;
            
            //  Pause Menu Gui
            var text = _isPaused ? "Press TAB to Resume" : "Press TAB to Pause";
            GUI.Label(rect, text, style);
        }

        private void OnDestroy()
        {
            if (_inputController != null)
            {
                _inputController.OnCancelEvent -= OnCancelEventCalled;
                _inputController.OnPauseToggleEvent -= OnTogglePauseEventCalled;
            }

            SceneExtension.StartSceneLoadEvent -= OnStartLoadScene;
            SceneExtension.FinishSceneLoadEvent -= OnFinishLoadScene;
        }

        #endregion

        #region Public Funcs

        public void OnPause()
        {
            lastSceneBuildIndex = SceneExtension.GetCurrentSceneBuildIndex();
            if (!SceneExtension.TrySwitchToScene(SceneExtension.MenuUiSceneName)) return;
            _inputController.OnCancelEvent += OnCancelEventCalled;
            BackToPreviousPage();
            _isPaused = true;
        }

        public void OnResume()
        {
            if (!SceneExtension.TrySwitchToScene(lastSceneBuildIndex)) return;
            _transitionHandler.TurnMenuPageOff(_transitionHandler.GetCurrentMenuPageType());
            _inputController.OnCancelEvent -= OnCancelEventCalled;
            _isPaused = false;
        }

        public void Reset()
        {
            isMainMenuActive = true;
            _menuUiCamera.gameObject.SetActive(isMainMenuActive);
        }

        public void OnTogglePauseEventCalled()
        {
            if (isMainMenuActive || isSceneTransitioning) return;
            _gameManager.TogglePause();
        }

        public void AvatarSelection()
        {
            _transitionHandler.TurnMenuPageOff(_transitionHandler.GetCurrentMenuPageType(), MenuPageType.AvatarSelection);
        }
        
        public void Leaderboard()
        {
            _transitionHandler.TurnMenuPageOff(_transitionHandler.GetCurrentMenuPageType(), MenuPageType.Leaderboard);
        }
        
        public void SelectMaleAvatar()
        {
            _gameManager.userChoiceAvatar = PlayerAvatar.Male;
        }
        
        public void SelectFemaleAvatar()
        {
            _gameManager.userChoiceAvatar = PlayerAvatar.Female;
        }

        public void Audio()
        {
            _transitionHandler.TurnMenuPageOff(_transitionHandler.GetCurrentMenuPageType(), MenuPageType.AudioSettings);
            audioOptions.AudioPanelIn(_eventSystem);
        }

        public void Video()
        {
            _transitionHandler.TurnMenuPageOff(_transitionHandler.GetCurrentMenuPageType(), MenuPageType.VideoSettings);
            videoOptions.VideoPanelIn(_eventSystem);
        }

        public void UpdateAudio() => ApplySettings(audioOptions.SaveOptions());

        public void UpdateVideo() => ApplySettings(videoOptions.SaveOptions());

        public void CancelAudio() => ApplySettings(audioOptions.RevertOptions());

        public void CancelVideo() => ApplySettings(videoOptions.RevertOptions());

        public void PlayAgain()
        {
            GameManager.HardReset();
            _transitionHandler.TurnMenuPageOff(_transitionHandler.GetCurrentMenuPageType(), MenuPageType.MainMenu);
        }

        public void ReturnToMenu()
        {
            if (isMainMenuActive) return;
            GameManager.HardReset();
            StartCoroutine(SceneExtension.ForceMenuSceneSequence(
                true, true, true, lastSceneBuildIndex));
            Reset();
        }

        public void QuitOptions()
        {
            if (quitOptionsPanel == null || quitPanelSelectedObj == null) return;
            quitOptionsPanel.TurnOnPanel();
            if (_eventSystem == null) _eventSystem = EventSystem.current;
            _eventSystem.SetSelectedGameObject(quitPanelSelectedObj.gameObject);
            quitPanelSelectedObj.OnSelect(null);
        }

        public void QuitCancel()
        {
            if (quitOptionsPanel == null) return;
            quitOptionsPanel.TurnOffPanel();
            BackToPreviousPage();
        }

        public void Quit()
        {
            GameManager.ReturnToMenu(MenuPageType.Credits);
            if (Time.timeScale <= 0) _gameManager.TogglePause();
            GameManager.HardReset();
        }

        #endregion

        #region Private Funcs

        private void ControllerSetup()
        {
            if (_inputController == null)
                _inputController = ResourcesManager.FindResource<InputController>("PlayerControls");

            if (_inputController == null)
            {
                //Debug.Log("[MenuManager]: Input from Resources not found - invoking controller setup after delay");
                Invoke(nameof(ControllerSetup), 1f);
                return;
            }
            _inputController.OnCancelEvent += OnCancelEventCalled;
            _inputController.OnPauseToggleEvent += OnTogglePauseEventCalled;
        }

        private void OnStartLoadScene(bool b1, bool b2)
        {
            //Debug.Log("[MenuManager]: OnStartLoadScene");
            _transitionHandler.TurnMenuPageOff(_transitionHandler.GetCurrentMenuPageType());
            if (_inputController != null) _inputController.OnCancelEvent -= OnCancelEventCalled;
            isSceneTransitioning = true;
            if (Time.timeScale <= 0.1f) _gameManager.SoftReset();
            isMainMenuActive = false;
        }

        private void OnFinishLoadScene(bool b1, bool b2)
        {
            //Debug.Log("[MenuManager]: OnFinishLoadScene");
            isMainMenuActive = SceneExtension.IsThisSceneActive(SceneExtension.MenuUiSceneName);
            if (isMainMenuActive)
            {
                BackToPreviousPage();
                _eventSystem.SetSelectedGameObject(mainPanelSelectedObj.gameObject);
                mainPanelSelectedObj.OnSelect(null);
            }
            else
            {
                _eventSystem.SetSelectedGameObject(pausePanelSelectedObj.gameObject);
                pausePanelSelectedObj.OnSelect(null);
            }

            _menuUiCamera.gameObject.SetActive(isMainMenuActive);
            isSceneTransitioning = false;
        }

        private void ApplyIniSettings()
        {
            audioOptions.ApplyAudioSettings();
            videoOptions.ApplyVideoSettings();
        }

        private void ApplySettings(IEnumerator ie)
        {
            StartCoroutine(ie);
            BackToPreviousPage();
        }

        public void BackToPreviousPage()
        {
            //Debug.Log("[MenuManager]: BackToPreviousPage");
            var target = isMainMenuActive ? MenuPageType.MainMenu : MenuPageType.Pause;
            _transitionHandler.TurnMenuPageOff(_transitionHandler.GetCurrentMenuPageType(), target);
            //avatarSelectionPanel.AvatarSelectionPanelOut();
        }

        private void OnCancelEventCalled() => BackToPreviousPage();

        #endregion
    }
}
