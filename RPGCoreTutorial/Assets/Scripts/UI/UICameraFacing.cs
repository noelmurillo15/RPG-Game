using UnityEngine;


namespace RPG.UI
{
    public class UICameraFacing : MonoBehaviour
    {
        void LateUpdate()
        {
            transform.forward = Camera.main.transform.forward;
        }
    }
}