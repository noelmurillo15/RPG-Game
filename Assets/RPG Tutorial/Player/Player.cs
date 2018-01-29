// Allan Murillo : Unity RPG Core Test Project
using RPG;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

public class Player : MonoBehaviour, IDamageable {


    [SerializeField] float maxHP = 100f;
    [SerializeField] float currentHP = 100f;
    [SerializeField] float baseDmg = 10f;

    IDamageable myTarget = null;

    CameraRaycaster camRaycaster = null;
    float lastHitTime = 0f;
    float minTimeBetweenHits = .5f;
    float maxAttackRange = 5f;

    //  Temporarily Serialized for dubbing
    [SerializeField] SpellConfig[] spells;
    PlayerStats stats;

    

    void Start()
    {
        stats = GetComponent<PlayerStats>();
        camRaycaster = Camera.main.GetComponent<CameraRaycaster>();

        camRaycaster.onMouseOverEnemy += OnMouseOverEnemy;
        camRaycaster.onMouseOverPlayer += OnMouseOverPlayer;
        AttachInitialSpells();
    }

    private void AttachInitialSpells()
    {
        for (int spellIndex = 0; spellIndex < spells.Length; spellIndex++)
        {
            spells[spellIndex].AttachComponent(gameObject);
        }
    }

    public float GetHealthAsPercentage()
    {
        return currentHP / maxHP;
    }

    private void ScanForSpellKeyDown()
    {
        for (int keyIndex = 0; keyIndex < spells.Length + 1; keyIndex++)
        {
            if (Input.GetKeyDown(keyIndex.ToString()))
            {
                AttemptSpell(keyIndex - 1);
            }
        }
    }

    private void AttemptSpell(int spellIndex)
    {
        if (myTarget == null)
        {
            Debug.Log("My Target is null");
            return;
        }

        var manaComponent = GetComponent<Mana>();
        var mySpell = spells[spellIndex];        

        if (manaComponent.IsManaAvailable(mySpell.GetManaCost()))
        {            
            var spellParams = new SpellUseParams(myTarget, baseDmg);
            spells[spellIndex].Activate(spellParams);
            manaComponent.AdjustMana(mySpell.GetManaCost() * -1);
        }
    }

    private bool IsTargetInRange(GameObject target)
    {
        float distToTarget = (target.transform.position - transform.position).magnitude;
        return distToTarget <= maxAttackRange;
    }

    void AttackTarget()
    {
        if (Time.time - lastHitTime > minTimeBetweenHits)
        {
            myTarget.AdjustHealth(baseDmg * -1f);
            lastHitTime = Time.time;
        }
    }

    public void AdjustHealth(float amt)
    {
        currentHP = Mathf.Clamp(currentHP + amt, 0f, maxHP);
        bool playerDead = (currentHP <= 0f);
        if (playerDead)
        {
            StartCoroutine(KillPlayer());
        }
    }

    public void StatChange(BuffType buff, float statAmt)
    {
        //  TODO : implement and modify stat change
        switch (buff)
        {
            case BuffType.HP:
                AdjustHealth(statAmt);
                break;
            case BuffType.MANA:
                break;
            default:
                Debug.Log("Buff type not implemented");
                break;
        }
    }

    void OnMouseOverEnemy(Enemy enemyToSet)
    {
        myTarget = enemyToSet;
        if (Input.GetMouseButtonDown(0) && IsTargetInRange(enemyToSet.gameObject))
        {
            AttackTarget();
        }

        if (GetHealthAsPercentage() > Mathf.Epsilon)
        {
            ScanForSpellKeyDown();
        }
    }

    void OnMouseOverPlayer()
    {
        myTarget = this;
        if (GetHealthAsPercentage() > Mathf.Epsilon)
        {
            ScanForSpellKeyDown();
        }
    }

    IEnumerator KillPlayer()
    {
        //  Play Death Sound (Optional)
        Debug.Log("Play Death Sound");
        //  Trigger Death Animation (Optional)
        Debug.Log("Play Death Animation");
        //  Wait a bit      
        yield return new WaitForSecondsRealtime(2f);    //  TODO : use audio clip length
        //  De-register from event listeners
        camRaycaster.onMouseOverEnemy -= OnMouseOverEnemy;
        camRaycaster.onMouseOverPlayer -= OnMouseOverPlayer;
        //  Reload Scene (scenemanager)        
        SceneManager.LoadScene(0);
    }    
}