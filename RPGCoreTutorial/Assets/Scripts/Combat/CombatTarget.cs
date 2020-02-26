/*
 * CombatTarget - 
 * Created by : Allan N. Murillo
 * Last Edited : 2/25/2020
 */

using ANM.Attributes;
using ANM.Control;
using UnityEngine;

namespace ANM.Combat
{
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour, IRaycastable
    {
        public CursorType GetCursorType()
        {
            return CursorType.COMBAT;
        }    //    IRaycastable

        public bool HandleRayCast(PlayerController controller)
        {
            if (!Fighter.CanAttack(gameObject)) return false;

            if (Input.GetMouseButton(0))
            { controller.GetComponent<Fighter>().Attack(gameObject); }

            return true;
        }    //    IRaycastable
    }
}