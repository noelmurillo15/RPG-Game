using UnityEngine;

[RequireComponent(typeof(CameraRaycaster))]
public class CursorIconSwitch : MonoBehaviour {

    CameraRaycaster camRaycaster;
    Vector2 cursorHotspot = new Vector2(96,96);

    [SerializeField] Texture2D walkCursor = null;
    [SerializeField] Texture2D whatCursor = null;
    [SerializeField] Texture2D attackCursor = null;

    // Use this for initialization
    void Start () {
        camRaycaster = GetComponent<CameraRaycaster>();
        camRaycaster.onLayerChange += OnLayerChange; //  registering
    }
	
	void OnLayerChange (Layer newlayer) { //  Only called when layer changes
        print("Layer has changed to : " + newlayer);

        switch (newlayer)
        {
            case Layer.Enemy:
                Cursor.SetCursor(attackCursor, cursorHotspot, CursorMode.Auto);
                break;
            case Layer.Walkable:
                Cursor.SetCursor(walkCursor, cursorHotspot, CursorMode.Auto);
                break;
            default:
                Cursor.SetCursor(whatCursor, cursorHotspot, CursorMode.Auto);
                break;
        }
	}

    //  TODO consider de-registering OnLayerChanged on leaving all game scenes
}
