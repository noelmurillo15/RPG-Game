/*
 * DestroyAfterEffect - 
 * Created by : Allan N. Murillo
 * Last Edited : 2/25/2020
 */

using UnityEngine;

namespace ANM.Core
{
    public class DestroyAfterEffect : MonoBehaviour
    {
        [SerializeField] private GameObject objToDestroy = null;
        private ParticleSystem _particles;

        private void Start()
        {
            _particles = GetComponent<ParticleSystem>();
        }

        private void Update()
        {
            if (_particles.IsAlive()) return;
            Destroy(objToDestroy != null ? objToDestroy : gameObject);
        }
    }
}