/*
 * AudioOptionsPanel - Handles displaying / configuring audio options
 * Created by : Allan N. Murillo
 * Last Edited : 7/10/2020
 */

using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using ANM.Framework.Audio;
using ANM.Framework.Utils;
using ANM.Framework.Managers;
using ANM.Framework.Extensions;
using UnityEngine.EventSystems;
using AudioType = ANM.Framework.Audio.AudioType;

namespace ANM.Framework.Options
{
    public class AudioOptionsPanel : MonoBehaviour
    {
        [SerializeField] private Slider audioMasterVolumeSlider = null;
        [SerializeField] private Slider effectsVolumeSlider = null;
        [SerializeField] private Slider backgroundVolumeSlider = null;
        [SerializeField] private Button audioPanelSelectedObj = null;

        private Animator _audioPanelAnimator;
        private AudioSource _soundTrackSource;
        private AudioSource _sfxSource;
        private GameObject _panel;


        private void Start()
        {
            _audioPanelAnimator = GetComponent<Animator>();
            _panel = _audioPanelAnimator.transform.GetChild(0).gameObject;

            foreach (var effect in AudioController.instance.tracks)
            {
                if (effect.audioObj[0].type == AudioType.St01)
                {
                    _soundTrackSource = effect.source;
                }
                else if (effect.audioObj[0].type == AudioType.Sfx01)
                {
                    _sfxSource = effect.source;
                }
            }

            _panel.SetActive(false);
        }

        public void AudioPanelIn(EventSystem eventSystem)
        {
            _panel.SetActive(true);
            eventSystem.SetSelectedGameObject(GetSelectObject());
            audioPanelSelectedObj.OnSelect(null);
        }

        public IEnumerator SaveAudioSettings()
        {
            SaveSettings.masterVolumeIni = audioMasterVolumeSlider.value;
            SaveSettings.effectVolumeIni = effectsVolumeSlider.value;
            SaveSettings.backgroundVolumeIni = backgroundVolumeSlider.value;
            yield return StartCoroutine(CoroutineUtilities.WaitForRealTime(0.5f));
            GameManager.Instance.SaveGameSettings();
            _panel.SetActive(false);
        }

        public IEnumerator RevertAudioSettings()
        {
            audioMasterVolumeSlider.value = SaveSettings.masterVolumeIni;
            effectsVolumeSlider.value = SaveSettings.effectVolumeIni;
            backgroundVolumeSlider.value = SaveSettings.backgroundVolumeIni;
            yield return StartCoroutine(CoroutineUtilities.WaitForRealTime(0.5f));
            _panel.SetActive(false);
        }

        public void ApplyAudioSettings()
        {
            OverrideMasterVolume();
            OverrideBackgroundVolume();
            OverrideEffectsVolume();
        }

        public void UpdateMasterVolume(float amount)
        {
            AudioListener.volume = amount;
        }

        public void UpdateEffectsVolume(float amount)
        {
            _sfxSource.volume = amount;
        }

        public void UpdateBackgroundVolume(float amount)
        {
            _soundTrackSource.volume = amount;
        }

        private void OverrideMasterVolume()
        {
            if (Math.Abs(AudioListener.volume - SaveSettings.masterVolumeIni) > 0f)
                AudioListener.volume = SaveSettings.masterVolumeIni;

            if (!(Math.Abs(audioMasterVolumeSlider.value - SaveSettings.masterVolumeIni) > 0f)) return;
            EventExtension.MuteEventListener(audioMasterVolumeSlider.onValueChanged);
            audioMasterVolumeSlider.value = SaveSettings.masterVolumeIni;
            EventExtension.UnMuteEventListener(audioMasterVolumeSlider.onValueChanged);
        }

        private void OverrideBackgroundVolume()
        {
            if (_soundTrackSource != null && Math.Abs(_soundTrackSource.volume - SaveSettings.backgroundVolumeIni) > 0f)
                _soundTrackSource.volume = SaveSettings.backgroundVolumeIni;

            if (!(Math.Abs(backgroundVolumeSlider.value - SaveSettings.backgroundVolumeIni) > 0f)) return;
            EventExtension.MuteEventListener(backgroundVolumeSlider.onValueChanged);
            backgroundVolumeSlider.value = SaveSettings.backgroundVolumeIni;
            EventExtension.UnMuteEventListener(backgroundVolumeSlider.onValueChanged);
        }

        private void OverrideEffectsVolume()
        {
            if (_sfxSource != null && Math.Abs(_sfxSource.volume - SaveSettings.effectVolumeIni) > 0f)
                _sfxSource.volume = SaveSettings.effectVolumeIni;

            if (!(Math.Abs(effectsVolumeSlider.value - SaveSettings.effectVolumeIni) > 0f)) return;
            EventExtension.MuteEventListener(effectsVolumeSlider.onValueChanged);
            effectsVolumeSlider.value = SaveSettings.effectVolumeIni;
            EventExtension.UnMuteEventListener(effectsVolumeSlider.onValueChanged);
        }

        private GameObject GetSelectObject()
        {
            return audioPanelSelectedObj.gameObject;
        }
    }
}
