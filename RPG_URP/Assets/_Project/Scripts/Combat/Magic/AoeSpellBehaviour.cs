/*
 * AoeSpellBehaviour -
 * Created by : Allan N. Murillo
 * Last Edited : 4/7/2020
 */

using ANM.Control;
//using ANM.FPS.Npc;
using UnityEngine;
using ANM.SpellSystem;

namespace RPG.SpellSystem
{
    public class AoeSpellBehaviour : SpellBehaviour
    {
        private void Start()
        {
            caster = GetComponent<PlayerController>();
        }

        public override void Activate(GameObject spellParams = null)
        {
            caster.GetComponent<SpellUI>().LoadSpellBehaviour(this, ((AoeSpellConfig) config).GetRadius());
        }

        public static void DealRadialDamage(int baseDmg, Vector3 areaAffected, float radius)
        {
            RaycastHit[] hits = Physics.SphereCastAll(areaAffected, radius, Vector3.up, radius);

            foreach (RaycastHit hit in hits)
            {
                /*var damageable = hit.transform.root.GetComponent<NpcMaster>();

                if (damageable != null)
                {
                    Debug.Log("Radial Damage Event has been called : " + baseDmg);
                    damageable.CallEventNpcDeductHealth(baseDmg);
                }*/
            }
        }
    }
}
