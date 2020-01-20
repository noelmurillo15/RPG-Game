using UnityEngine;


namespace RPG.Core
{
    public class DestroyAfterEffect : MonoBehaviour
    {
        [SerializeField] GameObject objToDestroy = null;


        private void Update()
        {
            if (GetComponent<ParticleSystem>().IsAlive()) return;
            Destroy(objToDestroy != null ? objToDestroy : gameObject);
        }
    }
}