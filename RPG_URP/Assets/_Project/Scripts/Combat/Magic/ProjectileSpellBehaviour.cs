/*
 * ProjectileSpellBehaviour -
 * Created by : Allan N. Murillo
 * Last Edited : 4/7/2020
 */

using ANM.Control;
using UnityEngine;

namespace RPG.SpellSystem
{
    public class ProjectileSpellBehaviour : SpellBehaviour
    {
        private void Start()
        {
            caster = GetComponent<PlayerController>();
        }

        public override void Activate(GameObject spellParams = null)
        {
            FireProjectile(spellParams);
        }

        private void FireProjectile(GameObject spellParams)
        {
            PlayParticleEffect();
        }
    }
}
