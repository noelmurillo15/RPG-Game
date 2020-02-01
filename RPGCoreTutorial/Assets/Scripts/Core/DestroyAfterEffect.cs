using UnityEngine;

namespace ANM.Core
{
    public class DestroyAfterEffect : MonoBehaviour
    {
        [SerializeField] private GameObject objToDestroy = null;


        private void Update()
        {
            if (GetComponent<ParticleSystem>().IsAlive()) return;
            Destroy(objToDestroy != null ? objToDestroy : gameObject);
        }
    }
}