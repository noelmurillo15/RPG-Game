// Allan Murillo : Unity RPG Core Test Project
using UnityEngine;


public class Player : MonoBehaviour, IDamageable {


    [SerializeField] float maxHP = 100f;
    [SerializeField] float currentHP = 100f;
    [SerializeField] float dmgPerHit = 10f;

    [SerializeField] GameObject target;

    CameraRaycaster camRaycaster;
    float lastHitTime = 0f;
    float minTimeBetweenHits = .5f;
    float maxAttackRange = 5f;
    


    void Start()
    {
        camRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        camRaycaster.onMouseOverEnemy += OnMouseOverEnemy;
    }


    void OnMouseOverEnemy(Enemy enemy)
    {
        //  TODO : is target in range
        if (Input.GetMouseButtonDown(0) && IsTargetInRange(enemy.gameObject))
        {
            AttackTarget(enemy);
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