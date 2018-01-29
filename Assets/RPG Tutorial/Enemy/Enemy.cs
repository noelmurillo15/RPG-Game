// Allan Murillo : Unity RPG Core Test Project
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;


public class Enemy : MonoBehaviour, IDamageable {


    [SerializeField] float currentHP = 100f;
    [SerializeField] float MaxHP = 100f;

    [SerializeField] float detectionradius;
    [SerializeField] float attackradius;

    [SerializeField] float attackDmg = 9f;
    [SerializeField] float shotPerSecond = 2f;

    [SerializeField] GameObject projectile;
    [SerializeField] GameObject projectileSocket;

    private bool isattacking = false;

    Animator anim = null;
    GameObject player = null;
    Player playerComponent = null;
    AICharacterControl aiCharacterControl = null;



    void Start()
    {
        isattacking = false;
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerComponent = player.GetComponent<Player>();
        aiCharacterControl = GetComponent<AICharacterControl>();
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
            aiCharacterControl.SetTarget(player.transform);
            anim.SetBool("Attack", isattacking);
        }
        else
        {
            //Debug.Log("I have not seen the player");
            aiCharacterControl.SetTarget(transform);
            isattacking = false;
            anim.SetBool("Attack", isattacking);
        }
    }

    void SpawnProjectile(){
        GameObject newProjectile = Instantiate(projectile, projectileSocket.transform.position, Quaternion.identity);
        Projectile newComponent = newProjectile.GetComponent<Projectile>();
        newComponent.projectileDamage = attackDmg;

        Vector3 unitVector = (player.transform.position - projectileSocket.transform.position).normalized;
        newProjectile.GetComponent<Rigidbody>().velocity = unitVector * newComponent.projectileSpeed;
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

    public float healthAsPercentage { get {  return currentHP / MaxHP; } }

    public void TakeDamage(float dmg)
    {
        currentHP = Mathf.Clamp(currentHP - dmg, 0f, MaxHP);
    }
}