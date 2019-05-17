using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{

    //Public variables
    public float speed;
    public float jumpForce;
    public int jumpTimes;
    public int jumpLimit;
    public int maxLife=3;
    public int currentLife;
    public int disableControl;
    public bool active;
    public float speedLimit;
    public Animator anim;

    //private variables
    protected GameManager gM;

    //MonoBehaviour object components
    protected Rigidbody2D rb;
    

    //Initialize variables, get references to our components
    public virtual void Init(int curHP)
    {
        
    }


    // Move left or right according to the input (arrow key)
    protected void ApplyHorizontalInput()
    {
        Vector2 moveVel = rb.velocity; //Get our current rigidbody's velocity
        float acceleration = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        if (rb.velocity.x * Input.GetAxis("Horizontal") < 0)
        {
            moveVel.x += 2.5f * acceleration; //Set add "speed" to the current acceleration, decelerates 2.5 times as fast
                                              //Note the multiply by Time.deltaTime to compensate for game clock
        }
        else if (Mathf.Abs(rb.velocity.x) < speedLimit)
        {
            moveVel.x += acceleration;
        }
        rb.velocity = moveVel; //Update our rigidbody's velocity
    }


    // Applying a upward force, note: checking jump limits
    protected void Jump()
    {
        if (jumpTimes < jumpLimit)
        {
            rb.AddForce(Vector2.up * jumpForce); //Add a upward force to our rigidbody
            jumpTimes++; //Jump count + 1
        }

    }

    // Change face direction
    protected void ChangeFaceDirection()
    {
        if (rb.velocity.x >= 0)
        {
            transform.rotation = new Quaternion(0, 180, 0, 0);
        }
        else
        {
            transform.rotation = new Quaternion(0, 0, 0, 0);
        }
        
    }

    // called by enemy bullets and enemies
    public void TakeDamage(int dmg)
    {
        currentLife--;
        if (currentLife > 0)
        {
            PlayerTakeDamageUI();
            disableControl = 20;
        }
        else
        {
            PlayerDeadUI();
            gM.gameState = "GameOver";
        }
    }

    protected void PlayerTakeDamageUI()
    {
        // Change life bar
    }

    protected void PlayerDeadUI()
    {
        // load game over screen
    }


    /* // this script "pridicts" collision before it happens
    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log(col.gameObject.name);
    }*/


    // Initialize
    protected void Start()
    {
        Init(3);
        // access game manager
        GameObject gmobj = GameObject.Find("gamemanager");
        gM = (GameManager)gmobj.GetComponent(typeof(GameManager));

        // initialize variables
        disableControl = 0;
        rb = GetComponent<Rigidbody2D>();
        jumpForce = 350;
        active = false;
        speedLimit=4;

        //play animation
        anim = GetComponent<Animator>();
        anim.Play("attack");
    }

    // Update is called once per frame
    protected void Update()
    {
        if (disableControl > 0) disableControl--;
        if (gM.gameState == "Game"&&(disableControl <= 0) && active)
        {
            ApplyHorizontalInput();
            if (Input.GetButtonDown("Jump"))
            {
                Jump();
            }
        }
        ChangeFaceDirection();
    }

    
    protected void OnCollisionEnter2D(Collision2D collision)
    {
        // reset jumpTimes if player touches the ground
        if ((collision.gameObject.tag == "platform") && (collision.gameObject.transform.position.y < transform.position.y)) //collide with platform and above it
        {
            jumpTimes = 0;
        }

        // add a upward force if stomped on a mushroom
        if ((collision.gameObject.tag == "mushroom") && (collision.gameObject.transform.position.y < transform.position.y))
        {
            rb.AddForce(Vector2.up * 700);
            jumpTimes = jumpLimit;
        }
    }
}