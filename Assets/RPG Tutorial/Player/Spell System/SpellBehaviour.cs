// Allan Murillo : Unity RPG Core Test Project
using UnityEngine;
using System.Collections;


namespace RPG {

    public abstract class SpellBehaviour : MonoBehaviour {


        #region Properties
        protected SpellConfig config;
        const float PARTICLE_CLEANUP_DELAY = 10f;
        #endregion



        public abstract void Activate(GameObject spell = null);

        public void SetConfig(SpellConfig configToAttach)
        {
            config = configToAttach;
        }

        #region Particles
        protected void PlayParticleEffect()
        {
            var particles = config.GetParticles();
            var particleObject = Instantiate(
                particles,
                transform.position,
                particles.transform.rotation);

            particleObject.transform.parent = transform;
            particleObject.GetComponent<ParticleSystem>().Play();
            StartCoroutine(DestroyParticles(particleObject));
        }

        IEnumerator DestroyParticles(GameObject particlePrefab)
        {
            while (particlePrefab.GetComponent<ParticleSystem>().isPlaying)
            {
                yield return new WaitForSeconds(PARTICLE_CLEANUP_DELAY);
            }
            Destroy(particlePrefab);
            yield return new WaitForEndOfFrame();
        }
        #endregion
    }
}