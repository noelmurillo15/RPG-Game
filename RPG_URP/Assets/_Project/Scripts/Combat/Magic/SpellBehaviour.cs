/*
 * SpellBehaviour -
 * Created By : Allan Murillo
 * Last Edited : 2/11/2021
 */

using UnityEngine;
using System.Collections;
using ANM.Control;

namespace RPG.SpellSystem
{
    public abstract class SpellBehaviour : MonoBehaviour
    {
        #region Variables
        protected PlayerController caster;
        protected SpellConfig config;
        private const float PARTICLE_CLEANUP_DELAY = 10f;
        private const float Speed = 100f;
        #endregion


        public abstract void Activate(GameObject spell = null);

        public void SetConfig(SpellConfig configToAttach)
        {
            config = configToAttach;
        }

        #region Particles

        protected void PlayParticleEffect(Transform target)
        {
            var buffPrefab = config.GetParticles();
            if (buffPrefab == null) return;
            BuffSpellConfig buffConfig = (config as BuffSpellConfig);

            Vector3 spawnPosition = target.position;
            spawnPosition.y += 1f;

            var buffObject = Instantiate(buffPrefab, spawnPosition, target.transform.rotation);
            buffObject.transform.parent = target;
            buffObject.GetComponent<ParticleSystem>().Play();
        }

        public void PlayParticleEffect(Vector3 pos, Vector3 offset)
        {
            var aoePrefab = config.GetParticles();
            if (aoePrefab == null) return;

            AoeSpellConfig aoeConfig = config as AoeSpellConfig;
            Vector3 spawnPosition = pos;
            spawnPosition.y += aoeConfig.GetOffset();

            var aoeObject = Instantiate(aoePrefab, spawnPosition, Quaternion.identity);
            aoeObject.transform.parent = null;
            aoeObject.transform.up = -offset;
            aoeObject.GetComponent<ParticleSystem>().Play();

            AoeSpellBehaviour.DealRadialDamage((int)(aoeConfig.GetDamage()), pos, aoeConfig.GetRadius());
        }

        protected void PlayParticleEffect()
        {
            var projectilePrefab = config.GetParticles();
            if (projectilePrefab == null) return;

            Vector3 spawnDirection = caster.transform.forward;
            Quaternion spawnRotation = caster.transform.rotation;
            Vector3 spawnPosition = caster.transform.position;
            spawnPosition.y += 1.64f;

            var projectileObject = Instantiate(projectilePrefab, spawnPosition, spawnRotation);
            projectileObject.transform.parent = null;
            projectileObject.GetComponent<ParticleSystem>().Play();

            var rb = projectileObject.GetComponent<Rigidbody>();
            if (rb != null) rb.AddForce(spawnDirection * Speed, ForceMode.Impulse);

            if (projectileObject.GetComponent<ProjectileApplyDamage>() != null)
            {
                float dmgToApply = ((ProjectileSpellConfig) config).GetDamage();
                ProjectileApplyDamage projDmg = projectileObject.GetComponent<ProjectileApplyDamage>();
                projDmg.enabled = true;
                projDmg.SetDamage(dmgToApply);
            }

            StartCoroutine(DestroyParticles(projectileObject));
        }

        private static IEnumerator DestroyParticles(GameObject particlePrefab)
        {
            while (particlePrefab.GetComponent<ParticleSystem>().isPlaying)
            {
                yield return new WaitForSeconds(PARTICLE_CLEANUP_DELAY);
            }

            if (particlePrefab != null) Destroy(particlePrefab);

            yield return new WaitForEndOfFrame();
        }
        #endregion
    }
}
