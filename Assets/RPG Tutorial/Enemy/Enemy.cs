// Allan Murillo : Unity RPG Core Test Project
using RPG;
using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.ThirdPerson;


public class Enemy : MonoBehaviour, IDamageable {


    [SerializeField] float currentHP = 100f;
    [SerializeField] float maxHP = 100f;

    [SerializeField] float detectionradius;
    [SerializeField] float attackradius;

    [SerializeField] float attackDmg = 5f;
    [SerializeField] float shotPerSecond = 2.5f;

    [SerializeField] GameObject projectile;
    [SerializeField] GameObject projectileSocket;

    private bool isattacking = false;

    Animator anim = null;
    GameObject player = null;
    Player playerComponent = null;



    void Start()
    {
        isattacking = false;
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerComponent = player.GetComponent<Player>();
    }

    void Update()
    {
        if(playerComponent.GetHealthAsPercentage() <= Mathf.Epsilon)
        {
            StopAllCoroutines();
            Destroy(this);  //  Stop enemy behaviour
        }

        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        if(distanceToPlayer <= attackradius && !isattacking)
        {
            //Debug.Log("I am close enough to attack player");
            InvokeRepeating("SpawnProjectile", .5f, shotPerSecond);
            isattacking = true;
            anim.SetBool("Attack", isattacking);
        }
        if(distanceToPlayer > attackradius)
        {
            isattacking = false;
            CancelInvoke();
        }

        if(distanceToPlayer <= detectionradius)
        {
            //Debug.Log("I have seen the player");
            //aiCharacterControl.SetTarget(player.transform);
            anim.SetBool("Attack", isattacking);
        }
        else
        {
            //Debug.Log("I have not seen the player");
            //aiCharacterControl.SetTarget(transform);
            isattacking = false;
            anim.SetBool("Attack", isattacking);
        }
    }

    void SpawnProjectile(){
        GameObject newProjectile = Instantiate(projectile, projectileSocket.transform.position, Quaternion.identity);
        Projectile newComponent = newProjectile.GetComponent<Projectile>();
        newComponent.damage += attackDmg;

        Vector3 unitVector = (player.transform.position - projectileSocket.transform.position).normalized;
        newProjectile.GetComponent<Rigidbody>().velocity = unitVector * newComponent.speed;
    }

    void OnDrawGizmos()
    {
        //  Draw Attack Radius
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackradius);
        //  Draw Detection Radius
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionradius);
    }

    public float healthAsPercentage { get {  return currentHP / maxHP; } }

    public void AdjustHealth(float amt)
    {
        currentHP = Mathf.Clamp(currentHP + amt, 0f, maxHP);
        bool enemyDead = (currentHP <= 0f);
        if (enemyDead)
        {
            StartCoroutine(KillEnemy());
        }
    }

    IEnumerator KillEnemy()
    {
        //  Play Death Sound (Optional)
        Debug.Log("Play Enemy Death Sound");
        //  Trigger Death Animation (Optional)
        Debug.Log("Play Enemy Death Animation");
        //  Wait a bit      
        yield return new WaitForSecondsRealtime(1f);    //  TODO : use animation length

        StopAllCoroutines();
        Destroy(gameObject);
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
}