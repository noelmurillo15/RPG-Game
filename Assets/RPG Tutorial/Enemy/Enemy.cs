using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class Enemy : MonoBehaviour {

    [SerializeField] float MaxHP = 100f;
    [SerializeField] float detectionradius;
    [SerializeField] float attackradius;

    private bool isattacking;

    AICharacterControl aiCharacterControl = null;
    GameObject player = null;
    Animator anim = null;

    float currentHP = 100f;

    public float healthAsPercentage
    {
        get
        {
            return currentHP / MaxHP;
        }
    }

    void Start()
    {
        isattacking = false;
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        aiCharacterControl = GetComponent<AICharacterControl>();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        if(distanceToPlayer <= detectionradius && distanceToPlayer > attackradius)
        {
            aiCharacterControl.SetTarget(player.transform);
            Debug.Log("I have seen the player");
            isattacking = false;
        }
        else if(distanceToPlayer <= attackradius && distanceToPlayer > 0)
        {
            Debug.Log("I am close enough to attack player");
            isattacking = true;
        }
        else
        {
            aiCharacterControl.SetTarget(transform);
            Debug.Log("I have not seen the player");
            isattacking = false;
        }
        anim.SetBool("Attack", isattacking);
    }
}