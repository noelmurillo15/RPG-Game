using UnityEngine;


public class Portal : MonoBehaviour
{
    bool hasTriggered = false;

    void OnTriggerEnter(Collider other) {
        if(!hasTriggered && other.tag.Equals("Player")){
            print("Portal has been triggered");
            hasTriggered = true;
        }
    }
}
