/// <summary>
/// 2/11/18
/// Allan Murillo
/// FPS Core Project
/// FramesPerSecond Display
/// </summary>
using UnityEngine;


public class FpsDisplay : MonoBehaviour
{
    float deltaTime = 0.0f;

    private void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }

    private void OnGUI()
    {
        int w = Screen.width, h = Screen.height;
        GUIStyle style = new GUIStyle();
        Rect rect = new Rect(w - 150, 0, w, h * 2 / 100);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * 2 / 100;
        style.normal.textColor = Color.white;
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        string text = $"{msec:0.0} ms ({fps:0.} fps)";
        GUI.Label(rect, text, style);
    }
}