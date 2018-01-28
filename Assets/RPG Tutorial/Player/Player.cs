// Allan Murillo : Unity RPG Core Test Project
using System;
using RPG;
using UnityEngine;


public class Player : MonoBehaviour, IDamageable {


    [SerializeField] GameObject target;
    [SerializeField] float maxHP = 100f;
    [SerializeField] float currentHP = 100f;
    [SerializeField] float dmgPerHit = 10f;

    CameraRaycaster camRaycaster;
    float lastHitTime = 0f;
    float minTimeBetweenHits = .5f;
    float maxAttackRange = 5f;

    //  Temporarily Serialized for dubbing
    [SerializeField] SpellConfig spell1;

    

    void Start()
    {
        camRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        camRaycaster.onMouseOverEnemy += OnMouseOverEnemy;

        spell1.AddComponent(gameObject);
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
            AttemptSpell1(enemy);
        }
    }

    private void AttemptSpell1(Enemy enemy)
    {
        var manaComponent = GetComponent<Mana>();

        if (manaComponent.IsManaAvailable(10f)) //  TODO : read from scriptable object
        {
            manaComponent.ConsumeMana(10f);

            //  TODO : Use Ability
        }
    }

    void AttackTarget(Enemy enemy)
    {
        if (Time.time - lastHitTime > minTimeBetweenHits)
        {           
            enemy.TakeDamage(dmgPerHit);
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