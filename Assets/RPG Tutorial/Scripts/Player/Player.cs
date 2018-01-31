// Allan Murillo : Unity RPG Core Test Project
using RPG;
using UnityEngine;


public class Player : MonoBehaviour {


    #region Properties
    //  Attack
    [SerializeField] float meleeBaseDmg = 10f;
    [SerializeField] float meleeRange = 5f;
    [SerializeField] float magicBaseDmg = 20f;
    [SerializeField] float magicRange = 5f;
    [SerializeField] float attackRate = .5f;

    float lastHitTime = 0f;

    //  References
    SpellSystem myMana;
    HealthSystem myHealth;
    GameObject myTarget;
    CameraRaycaster camRaycaster;
    #endregion



    void Start()
    {
        myMana = GetComponent<SpellSystem>();
        myHealth = GetComponent<HealthSystem>();
        camRaycaster = Camera.main.GetComponent<CameraRaycaster>();        

        camRaycaster.onMouseOverEnemy += OnMouseOverEnemy;
        camRaycaster.onMouseOverPlayer += OnMouseOverPlayer;        
    }

    #region Attack
    public float BaseDamage  { get { return meleeBaseDmg; } }
    public float BaseMagicDamage { get { return magicBaseDmg; } }

    bool IsTargetInRange(GameObject target)
    {
        float distToTarget = (target.transform.position - transform.position).magnitude;
        return distToTarget <= meleeRange;
    }

    void AttackTarget()
    {
        if (Time.time - lastHitTime > attackRate)
        {
            myTarget.GetComponent<HealthSystem>().TakeDamage(meleeBaseDmg);
            lastHitTime = Time.time;
        }
    }
    #endregion

    #region Input Events   
    void OnMouseOverEnemy(MobMaster enemyToSet)
    {
        myTarget = enemyToSet.gameObject;
        if (Input.GetMouseButtonDown(0) && IsTargetInRange(enemyToSet.gameObject))
        {
            AttackTarget();
        }

        if (myHealth.GetHealthAsPercentage() > Mathf.Epsilon)
        {
            ScanForSpellKeyDown();
        }
    }

    void OnMouseOverPlayer()
    {
        myTarget = gameObject;
        if (myHealth.GetHealthAsPercentage() > Mathf.Epsilon)
        {
            ScanForSpellKeyDown();
        }
    }

    void ScanForSpellKeyDown()
    {
        for (int keyIndex = 0; keyIndex < myMana.GetSpellList().Length + 1; keyIndex++)
        {
            if (Input.GetKeyDown(keyIndex.ToString()))
            {
                myMana.AttemptSpell(keyIndex - 1, myTarget);
            }
        }
    }
    #endregion

    #region Stats
    public void StatChange(BuffType buff, float statAmt)
    {
        switch (buff)
        {
            case BuffType.HP:
                myHealth.Heal(statAmt);
                break;
            case BuffType.MANA:
                myMana.GainMana(statAmt);
                break;
            default:
                Debug.Log("Buff type not implemented");
                break;
        }
    }
    #endregion
}