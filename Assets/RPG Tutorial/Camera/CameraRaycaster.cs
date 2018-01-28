using UnityEngine;
using UnityEngine.EventSystems;


public class CameraRaycaster : MonoBehaviour {


    #region Cursor Properties
    [SerializeField] int[] layerPriorities;

    [SerializeField] Texture2D walkCursor = null;
    [SerializeField] Texture2D targetCursor = null;
    [SerializeField] Vector2 cursorHotspot = new Vector2(0,0);
    
    const int WALKABLE_LAYER = 9; //  Must match walkable layer in unity
    float maxRaycastDepth = 100f; // Hard coded value

	// Setup delegates for broadcasting layer changes to other classes
    public delegate void OnMouseOverTerrain(Vector3 destination); // declare new delegate type
    public event OnMouseOverTerrain onMouseOverTerrain; // instantiate an observer set

	public delegate void OnMouseOverEnemy(Enemy enemy); // declare new delegate type
	public event OnMouseOverEnemy onMouseOverEnemy; // instantiate an observer set
    #endregion



    void Update()
	{
        // Check if pointer is over an interactable UI element
        if (EventSystem.current.IsPointerOverGameObject())
        {
            //  TODO : Implement UI Interaction 
            return; // Stop looking for other objects
        }
        else
        {
            PerformRaycasts();
        }
	}

    void PerformRaycasts()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //  Specify Layer Priorities
        if (RaycastForEnemy(ray)) { return; }   //  Enemies are top priority
        if (RaycastForTerrain(ray)) { return; }
    }

    bool RaycastForEnemy(Ray ray)
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, maxRaycastDepth))
        {

            var gameObjectHit = hitInfo.collider.gameObject;
            var enemyhit = gameObjectHit.GetComponent<Enemy>();

            if (enemyhit)
            {
                Cursor.SetCursor(targetCursor, cursorHotspot, CursorMode.Auto);
                onMouseOverEnemy(enemyhit);
                return true;
            }
        }
        return false;
    }

    bool RaycastForTerrain(Ray ray)
    {
        RaycastHit hitInfo;
        LayerMask terrainLayerMask = 1 << WALKABLE_LAYER;

        if (Physics.Raycast(ray, out hitInfo, maxRaycastDepth, terrainLayerMask))
        {
            Cursor.SetCursor(walkCursor, cursorHotspot, CursorMode.Auto);
            onMouseOverTerrain(hitInfo.point);
            return true;
        }
        return false;
    }
}