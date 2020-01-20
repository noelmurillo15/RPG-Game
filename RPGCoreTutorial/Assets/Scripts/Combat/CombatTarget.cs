using UnityEngine;
using RPG.Control;
using RPG.Attributes;


namespace RPG.Combat
{
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour, IRaycastable
    {

        #region Interface
        public CursorType GetCursorType()
        {
            return CursorType.COMBAT;
        }   //  IRaycastable

        public bool HandleRayCast(PlayerController controller)
        {
            if (!Fighter.CanAttack(gameObject)) return false;

            if (Input.GetMouseButton(0))
            { controller.GetComponent<Fighter>().Attack(gameObject); }

            return true;
        }   //  IRaycastable
        #endregion
    }
}