/*
 * FpsDisplay - Displays current Fps to Gui
 * Created by : Allan N. Murillo
 * Last Edited : 1/12/2021
 */

using UnityEngine;
using ANM.Framework.Managers;

namespace ANM.Utils
{
    public class FpsDisplay : MonoBehaviour
    {
        private int _fps;
        private float _delay;
        private string _fpsText;
        private bool _displayFps = true;
        private const float DesignWidth = 1920;
        private const float DesignHeight = 1080;


        private void OnEnable() => GameManager.Instance?.AttachFpsDisplay(this);

        private void OnDisable() => GameManager.Instance?.AttachFpsDisplay();

        private void Update()
        {
            if (!_displayFps) return;
            _fps = (int)(1f / Time.unscaledDeltaTime);
            _delay -= Time.deltaTime;
        }

        private void OnGUI()
        {
            if (!_displayFps) return;
            
            //  Dynamic Scaling
            var resX = Screen.width / DesignWidth;
            var resY = Screen.height / DesignHeight;
            GUI.matrix = Matrix4x4.TRS(new Vector3(0, 0, 0), Quaternion.identity, new Vector3(resX, resY, 1));
            
            //  Custom Style
            var style = new GUIStyle();
            var rect = new Rect(DesignWidth - DesignWidth * 0.5f, 0, 50, 32);
            style.alignment = TextAnchor.UpperCenter;
            style.normal.textColor = Color.white;
            style.fontSize = 18;
            
            //  FPS Display
            if (_delay < Time.deltaTime)
            {
                _delay = Time.deltaTime + 1f;
                _fpsText = $"{Mathf.Clamp(_fps, 0, 244)} fps";
            }

            GUI.Label(rect, _fpsText, style);
        }

        public void ToggleFpsDisplay(bool b) => _displayFps = b;
    }
}
