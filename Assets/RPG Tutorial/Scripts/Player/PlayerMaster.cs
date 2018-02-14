/// <summary>
/// 2/13/18
/// Allan Murillo
/// RPG Core Project
/// PlayerMaster.cs
/// </summary>
using UnityEngine;


namespace RPG {

    public class PlayerMaster : Character {


        #region Properties
        SpellSystem myMana;
        CameraRaycaster camRaycaster;
        private float lastHitTime = 0f;
        #endregion



        void Awake()
        {
            myMana = GetComponent<SpellSystem>();
            camRaycaster = Camera.main.GetComponent<CameraRaycaster>();

            camRaycaster.onMouseOverEnemy += OnMouseOverEnemy;
            camRaycaster.onMouseOverPlayer += OnMouseOverPlayer;

            Initialize();
        }

        #region Attack   
        bool IsTargetInRange(Transform target)
        {
            float distToTarget = (target.position - transform.position).magnitude;
            return distToTarget <= MeleeRange;
        }

        void AttackSelectedTarget()
        {
            if (Time.time - lastHitTime > AttackRate)
            {
                if (AttackTarget.GetComponent<Character>())
                {
                    Debug.Log("Melee Damage");
                    AttackTarget.GetComponent<Character>().CallEventCharacterTakeDamage(PhysicalAttack);
                }
                lastHitTime = Time.time;
            }
        }
        #endregion

        #region Input Events   
        void OnMouseOverEnemy(Character enemyToSet)
        {
            AttackTarget = enemyToSet.transform;
            CallEventSetCharacterNavTarget(AttackTarget);
            if (Input.GetMouseButtonDown(0) && IsTargetInRange(AttackTarget))
            {
                AttackSelectedTarget();
            }
            ScanForSpellKeyDown();
        }

        void OnMouseOverPlayer()
        {
            AttackTarget = gameObject.transform;
            ScanForSpellKeyDown();
        }

        void ScanForSpellKeyDown()
        {
            for (int keyIndex = 0; keyIndex < myMana.GetSpellList().Length + 1; keyIndex++)
            {
                if (Input.GetKeyDown(keyIndex.ToString()))
                {
                    myMana.AttemptSpell(keyIndex - 1, AttackTarget.gameObject);
                }
            }
        }
        #endregion
    }
}