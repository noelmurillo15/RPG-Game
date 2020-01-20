using UnityEngine;


namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        bool hasTriggered = false;


        private void OnTriggerEnter(Collider other)
        {
            if (hasTriggered || !other.tag.Equals("Player")) return;
            print("Portal has been triggered");
            hasTriggered = true;
        }
    }
}