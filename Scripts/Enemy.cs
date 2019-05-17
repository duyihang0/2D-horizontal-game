using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{

    public bool flip;

    // private variables
    int damage;
    int currentHealth;
    int maxHealth;
    float speed;
    float direction;
    float walkRange;
    float distanceWalked;
    bool posDirection;


    //Initialize variables, intialize UI elements
    public void Init(int dmg, int maxHP, float spd, float dir, float wr)
    {
        damage = dmg;
        maxHealth = maxHP;
        currentHealth = maxHealth;
        speed = spd;
        direction = dir;
        distanceWalked=0;
        walkRange=wr;
    }


    void Move()
    {
        if (distanceWalked <= walkRange)
        {
            // move speed units in the angle direction, note: multiply by Time.deltaTime to compensate for game clock
            this.transform.position += speed * Time.deltaTime * new Vector3(Mathf.Cos(direction), Mathf.Sin(direction), 0);
            distanceWalked += speed * Time.deltaTime;
        }
        else
        {
            distanceWalked =0 ;
            if (flip)
            {
                if (direction > Mathf.PI / 2 && direction < Mathf.PI * 3 / 2)
                {
                    this.transform.rotation = new Quaternion(0, 180, 0, 0);
                }
                else
                {
                    this.transform.rotation = new Quaternion(0, 0, 0, 0);
                }

            }
            direction += Mathf.PI;
            if (direction >= 2 * Mathf.PI)
            {
                direction -= 2 * Mathf.PI;
            }
        }
    }


    public void SelfDestruction()
    {
        Destroy(this.gameObject);
        Destroy(this);
    }


    // Called by a "bullet" when it gets hit by a bullet
    public void TakeDamage(int dmg) {
        this.currentHealth -= dmg;
        if (currentHealth > 0)
        {
            TakeDamageUI();
        } else
        {
            EnemyDeadUI();
            SelfDestruction();
        }
    }

    // hit test player
    private void OnTriggerEnter2D(Collider2D col)
    {
        
        if (col.gameObject.tag=="player")
        {
            Rigidbody2D playerrB=col.gameObject.GetComponent<Rigidbody2D>();
            PlayerControl playerScript = (PlayerControl)col.GetComponent(typeof(PlayerControl));
            playerrB.velocity = new Vector3(0,0,0);
            playerrB.AddForce(250 * (col.gameObject.transform.position - transform.position) 
                                  / (col.gameObject.transform.position - transform.position).magnitude);
            if (playerScript.disableControl <= 0)
            {
                playerScript.TakeDamage(damage);
            }
        }
    }

        // Update UI elements
        private void TakeDamageUI()
    {
        // Update Health Bar
        // Play enemy damaged animation
    }

    // Update UI elements
    private void EnemyDeadUI()
    {
        // Delete Health Bar
        // Increase score
        // Play enemy dead animation
    }


    // Update is called once per frame
    void Update()
    {
        {
            this.Move();
        }
    }
}
