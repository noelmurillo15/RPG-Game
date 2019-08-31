/// <summary>
/// 2/13/18
/// Allan Murillo
/// RPG Core Project
/// PlayerMaster.cs
/// </summary>
using UnityEngine;


namespace RPG.Stats
{
    public class PlayerMaster : Character
    {
        #region Properties
        SpellSystem myMana;
        CharacterStats characterStats;
        private float lastHitTime = 0f;
        #endregion


        void Start()
        {
            myMana = GetComponent<SpellSystem>();
            characterStats = GetComponent<CharacterStats>();

            CameraRaycaster camRaycaster = Camera.main.GetComponent<CameraRaycaster>();
            camRaycaster.onMouseOverEnemy += OnMouseOverEnemy;
            camRaycaster.onMouseOverPlayer += OnMouseOverPlayer;
            camRaycaster.onMouseOverTerrain += OnMouseOverTerrain;
        }

        #region Attack   
        bool IsTargetInRange(Transform target)
        {
            float distToTarget = (target.position - transform.position).magnitude;
            return distToTarget <= characterStats.MeleeRange;
        }

        void AttackSelectedTarget()
        {
            if (Time.time - lastHitTime > characterStats.AttackRate)
            {
                if (AttackTarget.GetComponent<Character>())
                {
                    AttackTarget.GetComponent<Character>().CallEventCharacterTakeDamage(characterStats.PhysicalAttack);
                }
                lastHitTime = Time.time;
            }
        }
        #endregion

        #region Input Events   
        void OnMouseOverEnemy(Character enemyToSet)
        {
            myAttackTarget = enemyToSet.transform;
            if (Input.GetMouseButtonDown(0))
            {
                if (IsTargetInRange(enemyToSet.transform))
                {
                    AttackSelectedTarget();
                }
            }
            ScanForSpellKeyDown();
        }

        void OnMouseOverPlayer()
        {
            myAttackTarget = gameObject.transform;
            ScanForSpellKeyDown();
        }

        void OnMouseOverTerrain(Vector3 dest)
        {
            if (Input.GetMouseButtonDown(0))
            {
                myAttackTarget = null;
                MyNavAgent.SetDestination(dest);
            }
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