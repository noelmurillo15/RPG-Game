using UnityEngine;

namespace ANM.UI
{
    public class UICameraFacing : MonoBehaviour
    {
        private void LateUpdate()
        {
            transform.forward = Camera.main.transform.forward;
        }
    }
}