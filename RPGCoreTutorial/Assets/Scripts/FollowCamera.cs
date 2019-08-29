using UnityEngine;


namespace RPG.Core
{    
    public class FollowCamera : MonoBehaviour {
        [SerializeField] Transform target;


        void LateUpdate()
        {
            transform.localPosition = target.localPosition;
        }
    }
}