/*
 * SaveSettings - Save/Loads game settings (audio, video) to/from a JSON file
 * Created by : Allan N. Murillo
 * Last Edited : 3/2/2021
 */

using System.IO;
using UnityEngine;

namespace ANM.Framework.Options
{
    [System.Serializable]
    public class SaveSettings
    {
        private static string _jsonString;
        private static string _fileName = "/GameSettings.json";

        public float masterVolume;
        public float effectVolume;
        public float backgroundVolume;
        public int currentQualityLevel;
        public int msaa;
        public float renderDist;
        public float shadowDist;
        public int textureLimit;
        public int anisotropicFilteringLevel;
        public int shadowCascade;
        public bool displayFps;
        public bool fullscreen;

        internal static float MasterVolumeIni;
        internal static float EffectVolumeIni;
        internal static float BackgroundVolumeIni;
        internal static int CurrentQualityLevelIni;
        internal static int MsaaIni;
        internal static float RenderDistIni;
        internal static float ShadowDistIni;
        internal static int TextureLimitIni;
        internal static int AnisotropicFilteringLevelIni;
        internal static int ShadowCascadeIni;
        internal static bool DisplayFpsIni;
        internal static bool FullscreenIni;
        internal static bool SettingsLoadedIni;


        private static object CreateJsonObj(string jsonString)
        {
            return JsonUtility.FromJson<SaveSettings>(jsonString);
        }

        public bool LoadGameSettings()
        {
            //Debug.Log("[SaveSettings]: LoadGameSettings");
            var filePath = Application.persistentDataPath + _fileName;
            //Debug.Log("[SaveSettings]: file path - " + filePath);
            if (!VerifyDirectory(filePath)) return false;
            //Debug.Log("[SaveSettings]: Overwriting GameSettings()");
            OverwriteGameSettings(File.ReadAllText(filePath));
            return true;
        }

        public void SaveGameSettings()
        {
            var filePath = Application.persistentDataPath + _fileName;
            if (VerifyDirectory(filePath))
            {
                File.Delete(filePath);
            }

            masterVolume = MasterVolumeIni;
            effectVolume = EffectVolumeIni;
            backgroundVolume = BackgroundVolumeIni;
            renderDist = RenderDistIni;
            shadowDist = ShadowDistIni;
            msaa = MsaaIni;
            textureLimit = TextureLimitIni;
            currentQualityLevel = CurrentQualityLevelIni;
            shadowCascade = ShadowCascadeIni;
            anisotropicFilteringLevel = AnisotropicFilteringLevelIni;
            displayFps = DisplayFpsIni;
            fullscreen = FullscreenIni;

            _jsonString = JsonUtility.ToJson(this);
            File.WriteAllText(filePath, _jsonString);
        }

        private void OverwriteGameSettings(string jsonString)
        {
            var jsonObj = (SaveSettings) CreateJsonObj(jsonString);
            MasterVolumeIni = jsonObj.masterVolume;
            EffectVolumeIni = jsonObj.effectVolume;
            BackgroundVolumeIni = jsonObj.backgroundVolume;
            RenderDistIni = jsonObj.renderDist;
            ShadowDistIni = jsonObj.shadowDist;
            MsaaIni = jsonObj.msaa;
            TextureLimitIni = jsonObj.textureLimit;
            CurrentQualityLevelIni = jsonObj.currentQualityLevel;
            ShadowCascadeIni = jsonObj.shadowCascade;
            AnisotropicFilteringLevelIni = jsonObj.anisotropicFilteringLevel;
            DisplayFpsIni = jsonObj.displayFps;
            FullscreenIni = jsonObj.fullscreen;
            SettingsLoadedIni = true;
        }

        public static void DefaultSettings()
        {
            MasterVolumeIni = 0.5f;
            EffectVolumeIni = 1f;
            BackgroundVolumeIni = 0.8f;
            CurrentQualityLevelIni = 2;
            MsaaIni = 2;
            RenderDistIni = 500.0f;
            ShadowDistIni = 150;
            ShadowCascadeIni = 3;
            TextureLimitIni = 0;
            AnisotropicFilteringLevelIni = 1;
            DisplayFpsIni = false;
            FullscreenIni = false;
            SettingsLoadedIni = true;
        }

        private bool VerifyDirectory(string filePath)
        {
            var result = File.Exists(filePath);
            //Debug.Log("[SaveSettings]: VerifyDirectory - " + filePath + " - " + result);
            return result;
        } 

        #region External JS LIBRARY

#if UNITY_WEBGL && !UNITY_EDITOR
        [System.Runtime.InteropServices.DllImport("__Internal")]
        static extern void InitializeJsLib();

        public void Initialize() => InitializeJsLib();
#else
        public void Initialize() => SettingsLoadedIni = LoadGameSettings();
#endif

        #endregion
    }
}
