using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class Enemy : MonoBehaviour {

    [SerializeField] float MaxHP = 100f;
    [SerializeField] float attackRadius;

    AICharacterControl aiCharacterControl = null;
    GameObject player = null;
    Animator anim = null;

    float currentHP = 100f;
    int speedHash = Animator.StringToHash("Speed");

    public float healthAsPercentage
    {
        get
        {
            return currentHP / MaxHP;
        }
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        aiCharacterControl = GetComponent<AICharacterControl>();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        if(distanceToPlayer <= attackRadius)
        {
            aiCharacterControl.SetTarget(player.transform);
            anim.SetFloat(speedHash, distanceToPlayer);
        }
        else
        {
            aiCharacterControl.SetTarget(transform);
        }
    }
}