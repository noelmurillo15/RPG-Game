using UnityEngine;

[RequireComponent(typeof(CameraRaycaster))]
public class CursorIconSwitch : MonoBehaviour {

    CameraRaycaster camRaycaster;
    Vector2 cursorHotspot = new Vector2(96,96);

    [SerializeField] Texture2D walkCursor = null;
    [SerializeField] Texture2D unknownCursor = null;
    [SerializeField] Texture2D attackCursor = null;

    [SerializeField] const int walkableLayerNumber = 8;
    [SerializeField] const int enemyLayerNumber = 9;

    // Use this for initialization
    void Start () {
        camRaycaster = GetComponent<CameraRaycaster>();
        camRaycaster.notifyLayerChangeObservers += OnLayerChange; //  registering
    }
	
	void OnLayerChange (int newlayer) { //  Only called when layer changes
        print("Layer has changed to : " + newlayer);

        switch (newlayer)
        {
            case walkableLayerNumber:
                Cursor.SetCursor(walkCursor, cursorHotspot, CursorMode.Auto);
                break;
            case enemyLayerNumber:
                Cursor.SetCursor(attackCursor, cursorHotspot, CursorMode.Auto);
                break;
            default:
                Cursor.SetCursor(unknownCursor, cursorHotspot, CursorMode.Auto);
                break;
        }
	}

    //  TODO consider de-registering OnLayerChanged on leaving all game scenes
}
