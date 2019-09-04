using UnityEngine;


namespace RPG.Core
{
    public class DestroyAfterEffect : MonoBehaviour
    {
        [SerializeField] GameObject objToDestroy = null;


        void Update()
        {
            if (!GetComponent<ParticleSystem>().IsAlive())
            {
                if (objToDestroy != null)
                {
                    Destroy(objToDestroy);
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}