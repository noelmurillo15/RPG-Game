/*
 * VideoOptionsPanel - Handles displaying / configuring graphics options
 * Created by : Allan N. Murillo
 * Last Edited : 7/10/2020
 */

using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using ANM.Framework.Utils;
using ANM.Framework.Managers;
using ANM.Framework.Extensions;
using UnityEngine.EventSystems;

namespace ANM.Framework.Options
{
    public class VideoOptionsPanel : MonoBehaviour
    {
        [SerializeField] private Toggle fpsDisplayToggle = null;
        [SerializeField] private Toggle fullScreenToggle = null;
        [SerializeField] private TMP_Dropdown msaaDropdown = null;
        [SerializeField] private TMP_Dropdown anisotropicDropdown = null;
        [SerializeField] private Slider renderDistSlider = null;
        [SerializeField] private Slider shadowDistSlider = null;
        [SerializeField] private Slider masterTexSlider = null;
        [SerializeField] private Slider shadowCascadesSlider = null;
        [SerializeField] private TMP_Text presetLabel = null;
        [SerializeField] private float[] shadowDist = null;
        [SerializeField] private Button videoPanelSelectedObj = null;

        private Camera _myCamera;
        private string[] _presets;
        private GameObject _panel;
        private Animator _videoPanelAnimator;


        private void Awake()
        {
            _myCamera = transform.root.GetComponentInChildren<Camera>();
            _presets = QualitySettings.names;
        }

        private void Start()
        {
            _videoPanelAnimator = GetComponent<Animator>();
            _panel = _videoPanelAnimator.transform.GetChild(0).gameObject;
            _panel.SetActive(false);
        }

        public void VideoPanelIn(EventSystem eventSystem)
        {
            _panel.SetActive(true);
            eventSystem.SetSelectedGameObject(GetSelectObject());
            videoPanelSelectedObj.OnSelect(null);
        }

        public IEnumerator SaveVideoSettings()
        {
            SaveSettings.currentQualityLevelIni = QualitySettings.GetQualityLevel();
            SaveSettings.msaaIni = msaaDropdown.value;
            SaveSettings.anisotropicFilteringLevelIni = anisotropicDropdown.value;
            SaveSettings.renderDistIni = renderDistSlider.value;
            SaveSettings.textureLimitIni = (int) masterTexSlider.value;
            SaveSettings.shadowDistIni = shadowDistSlider.value;
            SaveSettings.shadowCascadeIni = (int) shadowCascadesSlider.value;
            yield return StartCoroutine(CoroutineUtilities.WaitForRealTime(0.5f));
            GameManager.Instance.SaveGameSettings();
            _panel.SetActive(false);
        }

        public IEnumerator RevertVideoSettings()
        {
            QualitySettings.SetQualityLevel(SaveSettings.currentQualityLevelIni);
            msaaDropdown.value = SaveSettings.msaaIni;
            anisotropicDropdown.value = SaveSettings.anisotropicFilteringLevelIni;
            renderDistSlider.value = SaveSettings.renderDistIni;
            masterTexSlider.value = SaveSettings.textureLimitIni;
            shadowDistSlider.value = SaveSettings.shadowDistIni;
            shadowCascadesSlider.value = SaveSettings.shadowCascadeIni;
            yield return StartCoroutine(CoroutineUtilities.WaitForRealTime(0.5f));
            _panel.SetActive(false);
        }

        public void ApplyVideoSettings()
        {
            OverrideFpsDisplay(SaveSettings.displayFpsIni);
            OverrideFullscreen(SaveSettings.fullScreenIni);
            OverrideAnisotropicFiltering();
            OverrideMasterTextureQuality();
            OverrideGraphicsPreset();
            OverrideRenderDistance();
            OverrideShadowDistance();
            OverrideShadowCascade();
            OverrideMsaa();
        }

        public void ToggleFullScreen(bool b)
        {
            if (b) FullScreen();
            else ExitFullScreen();
        }

        public void ToggleFpsDisplay(bool b)
        {
            GameManager.Instance.SetDisplayFps(b);
        }

        public void UpdateRenderDistance(float renderDistance)
        {
            if (_myCamera == null) return;
            _myCamera.farClipPlane = renderDistance;
        }

        public void UpdateMasterTextureQuality(float textureQuality)
        {
            var f = Mathf.RoundToInt(textureQuality);
            QualitySettings.masterTextureLimit = f;
        }

        public void UpdateShadowDistance(float shadowDistance)
        {
            QualitySettings.shadowDistance = shadowDistance;
        }

        public void UpdateAnisotropicFiltering(int level)
        {
            switch (level)
            {
                case 0:
                    DisableAnisotropicFilter();
                    break;
                case 1:
                    PerTextureAnisotropicFilter();
                    break;
                case 2:
                    ForceOnAnisotropicFilter();
                    break;
            }
        }

        public void UpdateShadowCascades(float cascades)
        {
            var c = Mathf.RoundToInt(cascades);
            switch (c)
            {
                case 1:
                case 3:
                    c = 2;
                    break;
            }

            QualitySettings.shadowCascades = c;
        }

        public void UpdateMsaa(int level)
        {
            switch (level)
            {
                case 0:
                    DisableMsaa();
                    break;
                case 1:
                    TwoMsaa();
                    break;
                case 2:
                    FourMsaa();
                    break;
                case 3:
                    EightMsaa();
                    break;
            }
        }

        public void NextPreset()
        {
            QualitySettings.IncreaseLevel();
            var cur = QualitySettings.GetQualityLevel();
            PresetOverride(cur);
        }

        public void LastPreset()
        {
            QualitySettings.DecreaseLevel();
            var cur = QualitySettings.GetQualityLevel();
            PresetOverride(cur);
        }

        private static void ForceOnAnisotropicFilter()
        {
            QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable;
        }

        private static void PerTextureAnisotropicFilter()
        {
            QualitySettings.anisotropicFiltering = AnisotropicFiltering.Enable;
        }

        private static void DisableAnisotropicFilter()
        {
            QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
        }

        private static void DisableMsaa()
        {
            QualitySettings.antiAliasing = 0;
        }

        private static void TwoMsaa()
        {
            QualitySettings.antiAliasing = 2;
        }

        private static void FourMsaa()
        {
            QualitySettings.antiAliasing = 4;
        }

        private static void EightMsaa()
        {
            QualitySettings.antiAliasing = 8;
        }

        private void OverrideFpsDisplay(bool b)
        {
            EventExtension.MuteEventListener(fpsDisplayToggle.onValueChanged);
            GameManager.Instance.SetDisplayFps(b);
            fpsDisplayToggle.isOn = b;
            SaveSettings.displayFpsIni = b;
            EventExtension.UnMuteEventListener(fpsDisplayToggle.onValueChanged);
        }

        private void OverrideFullscreen(bool b)
        {
            EventExtension.MuteEventListener(fullScreenToggle.onValueChanged);
            fullScreenToggle.isOn = b;
            ToggleFullScreen(b);
            SaveSettings.fullScreenIni = b;
            EventExtension.UnMuteEventListener(fullScreenToggle.onValueChanged);
        }

        private void OverrideGraphicsPreset()
        {
            if (QualitySettings.GetQualityLevel() != SaveSettings.currentQualityLevelIni)
                QualitySettings.SetQualityLevel(SaveSettings.currentQualityLevelIni);

            if (!presetLabel.text.Contains(_presets[SaveSettings.currentQualityLevelIni]))
                presetLabel.text = _presets[SaveSettings.currentQualityLevelIni];
        }

        private void OverrideMsaa()
        {
            switch (SaveSettings.msaaIni)
            {
                case 0 when QualitySettings.antiAliasing != 0:
                    DisableMsaa();
                    break;
                case 1 when QualitySettings.antiAliasing != 2:
                    TwoMsaa();
                    break;
                case 2 when QualitySettings.antiAliasing != 4:
                    FourMsaa();
                    break;
                case 3 when QualitySettings.antiAliasing < 8:
                    EightMsaa();
                    break;
            }

            if (msaaDropdown.value == SaveSettings.msaaIni) return;
            EventExtension.MuteEventListener(msaaDropdown.onValueChanged);
            msaaDropdown.value = SaveSettings.msaaIni;
            EventExtension.UnMuteEventListener(msaaDropdown.onValueChanged);
        }

        private void OverrideAnisotropicFiltering()
        {
            if ((int) QualitySettings.anisotropicFiltering != SaveSettings.anisotropicFilteringLevelIni)
                QualitySettings.anisotropicFiltering = (AnisotropicFiltering) SaveSettings.anisotropicFilteringLevelIni;

            if (anisotropicDropdown.value == SaveSettings.anisotropicFilteringLevelIni) return;
            EventExtension.MuteEventListener(anisotropicDropdown.onValueChanged);
            anisotropicDropdown.value = SaveSettings.anisotropicFilteringLevelIni;
            EventExtension.UnMuteEventListener(anisotropicDropdown.onValueChanged);
        }

        private void OverrideRenderDistance()
        {
            if (_myCamera == null) return;

            if (Math.Abs(_myCamera.farClipPlane - SaveSettings.renderDistIni) > 0f)
                _myCamera.farClipPlane = SaveSettings.renderDistIni;

            if (!(Math.Abs(renderDistSlider.value - SaveSettings.renderDistIni) > 0f)) return;
            EventExtension.MuteEventListener(renderDistSlider.onValueChanged);
            renderDistSlider.value = SaveSettings.renderDistIni;
            EventExtension.UnMuteEventListener(renderDistSlider.onValueChanged);
        }

        private void OverrideShadowDistance()
        {
            if (Math.Abs(QualitySettings.shadowDistance - SaveSettings.shadowDistIni) > 0f)
                QualitySettings.shadowDistance = SaveSettings.shadowDistIni;

            if (!(Math.Abs(shadowDistSlider.value - SaveSettings.shadowDistIni) > 0f)) return;
            EventExtension.MuteEventListener(shadowDistSlider.onValueChanged);
            shadowDistSlider.value = SaveSettings.shadowDistIni;
            EventExtension.UnMuteEventListener(shadowDistSlider.onValueChanged);
        }

        private void OverrideShadowCascade()
        {
            if (QualitySettings.shadowCascades != SaveSettings.shadowCascadeIni)
                QualitySettings.shadowCascades = SaveSettings.shadowCascadeIni;

            if (!(Math.Abs(shadowCascadesSlider.value - SaveSettings.shadowCascadeIni) > 0f)) return;
            EventExtension.MuteEventListener(shadowCascadesSlider.onValueChanged);
            shadowCascadesSlider.value = SaveSettings.shadowCascadeIni;
            EventExtension.UnMuteEventListener(shadowCascadesSlider.onValueChanged);
        }

        private void OverrideMasterTextureQuality()
        {
            if (QualitySettings.masterTextureLimit != SaveSettings.textureLimitIni)
                QualitySettings.masterTextureLimit = SaveSettings.textureLimitIni;

            if (!(Math.Abs(masterTexSlider.value - SaveSettings.textureLimitIni) > 0f)) return;
            EventExtension.MuteEventListener(masterTexSlider.onValueChanged);
            masterTexSlider.value = SaveSettings.textureLimitIni;
            EventExtension.UnMuteEventListener(masterTexSlider.onValueChanged);
        }

        private void PresetOverride(int currentValue)
        {
            presetLabel.text = _presets[currentValue];
            shadowDistSlider.value = shadowDist[currentValue];
            msaaDropdown.value = currentValue;
        }

        private GameObject GetSelectObject()
        {
            return videoPanelSelectedObj.gameObject;
        }

        private static void FullScreen()
        {
            // Debug.Log("[VideoOptionsPanel]: Fullscreen = True");
            Screen.fullScreen = true;
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        }

        private static void ExitFullScreen()
        {
            // Debug.Log("[VideoOptionsPanel]: Fullscreen = false");
            Screen.fullScreen = false;
        }
    }
}
