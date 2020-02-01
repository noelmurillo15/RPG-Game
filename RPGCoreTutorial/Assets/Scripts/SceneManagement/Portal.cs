using UnityEngine;


namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        private bool _hasTriggered = false;


        private void OnTriggerEnter(Collider other)
        {
            if (_hasTriggered || !other.tag.Equals("Player")) return;
            print("Portal has been triggered");
            _hasTriggered = true;
        }
    }
}