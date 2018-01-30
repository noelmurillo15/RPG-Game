// Allan Murillo : Unity RPG Core Test Project
using UnityEngine;

// Add a UI Socket transform to your enemy
// Attach this script to the socket
// Link to a canvas prefab that contains NPC UI
public class EnemyUI : MonoBehaviour {


    Camera cameraToLookAt;



    void Start()
    {
        cameraToLookAt = Camera.main;
    }

    void LateUpdate()
    {
        transform.LookAt(cameraToLookAt.transform);
    }
}