// Allan Murillo : Unity RPG Core Test Project
using RPG;
using UnityEngine;


public class Enemy : MonoBehaviour {


    #region Properties
    //  Detection
    [SerializeField] float detectionradius;
    [SerializeField] float attackradius;

    //  Attack
    [SerializeField] float attackDmg = 5f;
    [SerializeField] float shotPerSecond = 2.5f;
    [SerializeField] bool isattacking = false;

    [SerializeField] GameObject projectile;
    [SerializeField] GameObject projectileSocket;


    //  Health
    HealthSystem myHealth;

    //  References
    Animator anim = null;
    GameObject player = null;
    #endregion



    void Start()
    {
        isattacking = false;
        anim = GetComponent<Animator>();
        myHealth = GetComponent<HealthSystem>();
        player = GameObject.FindGameObjectWithTag("Player");
    }    

    void Update()
    {
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
            //  TODO : Enemy Movement 
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

    #region Shoot
    void SpawnProjectile(){
        GameObject newProjectile = Instantiate(projectile, projectileSocket.transform.position, Quaternion.identity);
        Projectile newComponent = newProjectile.GetComponent<Projectile>();
        newComponent.damage += attackDmg;

        Vector3 unitVector = (player.transform.position - projectileSocket.transform.position).normalized;
        newProjectile.GetComponent<Rigidbody>().velocity = unitVector * newComponent.speed;
    }
    #endregion

    #region Gizmos
    void OnDrawGizmos()
    {
        //  Draw Attack Radius
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackradius);
        //  Draw Detection Radius
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionradius);
    }
    #endregion

    #region Stats
    public void StatChange(BuffType buff, float statAmt)
    {
        //  TODO : implement and modify stat change
        switch (buff)
        {
            case BuffType.HP:
                myHealth.Heal(statAmt);
                break;
            case BuffType.MANA:
                break;
            default:
                Debug.Log("Buff type not implemented");
                break;
        }
    }
    #endregion
}