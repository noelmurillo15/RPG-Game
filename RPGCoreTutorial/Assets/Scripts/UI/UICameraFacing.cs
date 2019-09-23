using UnityEngine;


namespace RPG.UI
{
    public class UICameraFacing : MonoBehaviour
    {
        void Update()
        {
            transform.forward = Camera.main.transform.forward;
        }
    }
}