// Allan Murillo : Unity RPG Core Test Project
using System;
using RPG;
using UnityEngine;


public class Player : MonoBehaviour, IDamageable {


    [SerializeField] GameObject target;
    [SerializeField] float maxHP = 100f;
    [SerializeField] float currentHP = 100f;
    [SerializeField] float baseDmg = 10f;

    CameraRaycaster camRaycaster;
    float lastHitTime = 0f;
    float minTimeBetweenHits = .5f;
    float maxAttackRange = 5f;

    //  Temporarily Serialized for dubbing
    [SerializeField] Spell[] spells;

    

    void Start()
    {
        camRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        camRaycaster.onMouseOverEnemy += OnMouseOverEnemy;

        spells[0].AttachComponent(gameObject);
    }


    void OnMouseOverEnemy(Enemy enemy)
    {
        //  TODO : is target in range
        if (Input.GetMouseButtonDown(0) && IsTargetInRange(enemy.gameObject))
        {
            AttackTarget(enemy);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            AttemptSpell(0, enemy);
        }
    }

    private void AttemptSpell(int spellIndex, Enemy enemy)
    {
        var manaComponent = GetComponent<Mana>();
        var manaCost = spells[spellIndex].GetManaCost();

        if (manaComponent.IsManaAvailable(manaCost)) //  TODO : read from scriptable object
        {
            var spellParams = new SpellUseParams(enemy, baseDmg);
            spells[spellIndex].Activate(spellParams);
            manaComponent.ConsumeMana(manaCost);
        }
    }

    void AttackTarget(Enemy enemy)
    {
        if (Time.time - lastHitTime > minTimeBetweenHits)
        {           
            enemy.TakeDamage(baseDmg);
            lastHitTime = Time.time;
        }
    }
    
    private bool IsTargetInRange(GameObject target)
    {
        float distToTarget = (target.transform.position - transform.position).magnitude;
        return distToTarget <= maxAttackRange;
    }

    public void TakeDamage(float dmg)
    {
        currentHP = Mathf.Clamp(currentHP - dmg, 0f, maxHP);
    }

	public float healthAsPercentage { get { return currentHP / maxHP; } }
}