// Allan Murillo : Unity RPG Core Test Project
using RPG;
using UnityEngine;


public class PlayerMaster : Character {


    #region Properties
    //  Attack
    [SerializeField] float meleeBaseDmg = 10f;
    [SerializeField] float meleeRange = 5f;
    [SerializeField] float magicBaseDmg = 20f;
    [SerializeField] float magicRange = 5f;
    [SerializeField] float attackRate = .5f;

    float lastHitTime = 0f;

    SpellSystem myMana;
    CameraRaycaster camRaycaster;
    #endregion



    void Start()
    {
        myMana = GetComponent<SpellSystem>();
        camRaycaster = Camera.main.GetComponent<CameraRaycaster>();        

        camRaycaster.onMouseOverEnemy += OnMouseOverEnemy;
        camRaycaster.onMouseOverPlayer += OnMouseOverPlayer;        
    }

    #region Attack
    public float BaseDamage  { get { return meleeBaseDmg; } }
    public float BaseMagicDamage { get { return magicBaseDmg; } }

    bool IsTargetInRange(Transform target)
    {
        float distToTarget = (target.position - transform.position).magnitude;
        return distToTarget <= meleeRange;
    }

    void AttackSelectedTarget()
    {
        if (Time.time - lastHitTime > attackRate)
        {
            if (AttackTarget.GetComponent<Character>())
            {
                Debug.Log("Player has dealt damage to attack target");
                AttackTarget.GetComponent<Character>().CallEventCharacterTakeDamage(BaseDamage);
            }   
            lastHitTime = Time.time;
        }
    }
    #endregion

    #region Input Events   
    void OnMouseOverEnemy(MobMaster enemyToSet)
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