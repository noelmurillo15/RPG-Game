/*
 * MenuManager - Handles interactions with the Menu Ui
 * Created by : Allan N. Murillo
 * Last Edited : 2/22/2020
 */

using ANM.Saving;
using UnityEngine;
using UnityEngine.UI;
using ANM.Framework.Settings;
using ANM.Framework.Extensions;
using UnityEngine.EventSystems;

namespace ANM.Framework.Managers
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] private GameObject mainPanel = null;
        [SerializeField] private GameObject pausePanel = null;
        
        [SerializeField] private AudioSettingsPanel audioSettingsPanel = null;
        [SerializeField] private VideoSettingsPanel videoSettingsPanel = null;
        
        [SerializeField] private Animator quitPanelAnimator = null;
        [SerializeField] private Toggle fpsDisplayToggle = null;
        [SerializeField] private Camera menuUiCamera = null;
        
        [SerializeField] private Button mainPanelSelectedObj = null;
        [SerializeField] private Button pausePanelSelectedObj = null;
        [SerializeField] private Button quitPanelSelectedObj = null;
        [SerializeField] private Button loadButton = null;

        [SerializeField] private bool isMainMenuActive = false;
        [SerializeField] private int lastSceneBuildIndex = 0;
        
        private EventSystem _eventSystem;
        private GameManager _gameManager;
        

        private void Awake()
        {
            TurnOffAllPanels();
            _gameManager = GameManager.Instance;
            _eventSystem = FindObjectOfType<EventSystem>();
            if (!SaveSettings.SettingsLoadedIni) SaveSettings.DefaultSettings();
            ApplyIniSettings();
        }

        private void Start()
        {
            isMainMenuActive = SceneExtension.IsThisSceneActive(SceneExtension.MenuUiSceneName);
            mainPanel.SetActive(isMainMenuActive);
            menuUiCamera.gameObject.SetActive(isMainMenuActive);
            SceneExtension.StartSceneLoadEvent += OnStartLoadScene;
            SceneExtension.FinishSceneLoadEvent += OnFinishLoadScene;
        }

        private void OnGUI()
        {
            if (isMainMenuActive) return;
            var style = new GUIStyle();
            int w = Screen.width, h = Screen.height;
            h *= 2 / 100;
            var rect = new Rect(16, 16, w * 0.5f, 32);
            style.alignment = TextAnchor.UpperLeft;
            style.fontSize = h * 2 / 100;
            style.normal.textColor = Color.white;
            var text = _gameManager.GetIsGamePaused() ? "Press TAB to Resume" : "Press TAB to Pause";
            GUI.Label(rect, text, style);
        }

        private void OnDestroy()
        {
            SceneExtension.StartSceneLoadEvent -= OnStartLoadScene;
            SceneExtension.FinishSceneLoadEvent -= OnFinishLoadScene;
        }

        private void OnStartLoadScene(bool b)
        {
            if (quitPanelAnimator.transform.GetChild(0).gameObject.activeSelf)
                quitPanelAnimator.Play("QuitPanelOut");
        }
        
        private void OnFinishLoadScene(bool b)
        {
            isMainMenuActive = SceneExtension.IsThisSceneActive(SceneExtension.MenuUiSceneName);
            if (isMainMenuActive) TurnOnMainPanel();
            else TurnOffAllPanels();
            menuUiCamera.gameObject.SetActive(isMainMenuActive);
        }
        
        private void OnPause()
        {
            lastSceneBuildIndex = SceneExtension.GetCurrentSceneBuildIndex();
            if (SceneExtension.TrySwitchToScene(SceneExtension.MenuUiSceneName))
            {
                TurnOnMainPanel();
            }
        }
        
        private void OnResume()
        {
            if (!SceneExtension.TrySwitchToScene(lastSceneBuildIndex)) return;
            menuUiCamera.gameObject.SetActive(false);
            TurnOffAllPanels();
        }
        
        private void TurnOffAllPanels()
        {
            if (quitPanelAnimator.transform.GetChild(0).gameObject.activeSelf)
                quitPanelAnimator.Play("QuitPanelOut");
            
            videoSettingsPanel.TurnOffPanel();
            mainPanel.SetActive(false);
            pausePanel.SetActive(false);
            audioSettingsPanel.TurnOffPanel();
        }
        
        private void TurnOnMainPanel()
        {
            menuUiCamera.gameObject.SetActive(true);
            if (isMainMenuActive)
            {
                loadButton.interactable = true;
                mainPanel.SetActive(true);
                _eventSystem.SetSelectedGameObject(mainPanelSelectedObj.gameObject);
                mainPanelSelectedObj.OnSelect(null);
            }
            else
            {
                pausePanel.SetActive(true);
                _eventSystem.SetSelectedGameObject(pausePanelSelectedObj.gameObject);
                pausePanelSelectedObj.OnSelect(null);
            }
            videoSettingsPanel.TurnOffPanel();
            audioSettingsPanel.TurnOffPanel();
        }

        private void ApplyIniSettings()
        {
            audioSettingsPanel.ApplyAudioSettings();
            videoSettingsPanel.ApplyVideoSettings();
            ToggleFpsDisplay(SaveSettings.DisplayFpsIni);
        }

        public void StartGame()
        {
            StartCoroutine(SceneExtension.LoadMultiSceneSequence(
                SceneExtension.GameplaySceneName, true));
        }
        
        public void LoadGame()
        {
            if (SavingWrapper.CanLoadSaveFile())
            {
                StartCoroutine(SavingWrapper.LoadLastGameState());
            }
            else loadButton.interactable = false;
        }

        public void TogglePause()
        {
            _gameManager.TogglePause();
        }

        public void QuitOptions()
        {
            videoSettingsPanel.TurnOffPanel();
            audioSettingsPanel.TurnOffPanel();
            if (quitPanelAnimator != null)
            {
                quitPanelAnimator.enabled = true;
                quitPanelAnimator.Play("QuitPanelIn");
            }
            _eventSystem.SetSelectedGameObject(quitPanelSelectedObj.gameObject);
            quitPanelSelectedObj.OnSelect(null);
        }

        public void QuitCancel()
        {
            if (quitPanelAnimator != null)
                quitPanelAnimator.Play("QuitPanelOut");
            TurnOnMainPanel();
        }

        public void ReturnToMenu()
        {
            TurnOffAllPanels();
            _gameManager.HardReset();
            StartCoroutine(SceneExtension.ForceMenuSceneSequence(true));
        }
        
        public void QuitGame()
        {
            _gameManager.HardReset();
            Invoke(nameof(LoadCredits), 0.15f);
        }

        private void LoadCredits()
        {
            StartCoroutine(SceneExtension.LoadSingleSceneSequence(
                SceneExtension.CreditsSceneName, true));
        }

        public void Audio()
        {
            TurnOffAllPanels();
            audioSettingsPanel.AudioPanelIn(_eventSystem);
        }
        public void UpdateAudioSettings()
        {
            StartCoroutine(audioSettingsPanel.SaveAudioSettings());
            TurnOnMainPanel();
        }

        public void CancelAudioSettings()
        {
            StartCoroutine(audioSettingsPanel.RevertAudioSettings());
            TurnOnMainPanel();
        }

        public void Video()
        {
            TurnOffAllPanels();
            videoSettingsPanel.VideoPanelIn(_eventSystem);
        }
        
        public void UpdateVideoSettings()
        {
            StartCoroutine(videoSettingsPanel.SaveVideoSettings());
            TurnOnMainPanel();
        }

        public void CancelVideoSettings()
        {
            StartCoroutine(videoSettingsPanel.RevertVideoSettings());
            TurnOnMainPanel();
        }

        public void ToggleFpsDisplay(bool b)
        {
            EventExtension.MuteEventListener(fpsDisplayToggle.onValueChanged);
            GameManager.Instance.SetDisplayFps(b);
            fpsDisplayToggle.isOn = b;
            SaveSettings.DisplayFpsIni = b;
            EventExtension.UnMuteEventListener(fpsDisplayToggle.onValueChanged);
        }
    }
}
