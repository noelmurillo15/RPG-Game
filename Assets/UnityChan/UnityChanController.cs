using UnityEngine;

public class UnityChanController : MonoBehaviour {

    public Animator anim;
    public Rigidbody rbody;

    private float inputH, inputV;
    private int horizontalHash, verticalHash;

    private bool run;

	// Use this for initialization
	void Start () {
        run = false;
        anim = GetComponent<Animator>();
        rbody = GetComponent<Rigidbody>();
        horizontalHash = Animator.StringToHash("Horizontal");
        verticalHash = Animator.StringToHash("Vertical");
    }
	
	// Update is called once per frame
	void Update () {
        //  IDLE
        if (Input.GetKeyDown("1"))
        {
            anim.Play("WAIT01", -1, 0f);
        }
        if (Input.GetKeyDown("2"))
        {
            anim.Play("WAIT02", -1, 0f);
        }
        if (Input.GetKeyDown("3"))
        {
            anim.Play("WAIT03", -1, 0f);
        }
        if (Input.GetKeyDown("4"))
        {
            anim.Play("WAIT04", -1, 0f);
        }

        //  DAMAGE
        if (Input.GetMouseButtonDown(0))
        {
            int n = Random.Range(0, 2);

            if (n == 0)
                anim.Play("DAMAGED00", -1, 0f);
            else
                anim.Play("DAMAGED01", -1, 0f);
        }

        //  RUN
        if (Input.GetKey(KeyCode.LeftShift))
        {
            run = true;
        }
        else
        {
            run = false;
        }

        //  JUMP
        if (Input.GetKey(KeyCode.Space))
        {
            anim.SetBool("Jump", true);
        }
        else
        {
            anim.SetBool("Jump", false);
        }

        //  WALKING
        inputH = Input.GetAxis("Horizontal");
        inputV = Input.GetAxis("Vertical");

        anim.SetFloat(horizontalHash, inputH);
        anim.SetFloat(verticalHash, inputV);
        anim.SetBool("Run", run);

        float moveX = inputH * 20f * Time.deltaTime;
        float moveZ = inputV * 50f * Time.deltaTime;

        if(moveZ <= 0f)
        {
            moveX = 0f;
        }
        else if(run)
        {
            moveZ *= 2f;
        }

        rbody.velocity = new Vector3(moveX, 0f, moveZ);
    }
}
