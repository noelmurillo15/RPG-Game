using RPG.Core;
using RPG.Combat;
using UnityEngine;
using RPG.Movement;


namespace RPG.Control
{
    [RequireComponent(typeof(CharacterMove))]
    public class PlayerController : MonoBehaviour
    {
        Health myHealth;


        void Start()
        {
            myHealth = GetComponent<Health>();
        }

        void Update()
        {
            if (myHealth.IsDead()) return;
            if (InteractWithCombat()) return;
            if (InteractWithMovement()) return;
        }

        bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach (var hit in hits)
            {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if (target == null) continue;

                if (!GetComponent<Fighter>().CanAttack(target.gameObject)) continue;

                if (Input.GetMouseButton(0))
                {
                    GetComponent<Fighter>().Attack(target.gameObject);
                }
                return true;
            }
            return false;
        }

        bool InteractWithMovement()
        {
            if (Physics.Raycast(GetMouseRay(), out RaycastHit m_hit))
            {
                if (Input.GetMouseButton(0))
                {
                    GetComponent<CharacterMove>().StartMoveAction(m_hit.point, 1f);
                }
                return true;
            }
            return false;
        }

        static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}