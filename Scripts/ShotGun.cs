using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShotGun : MonoBehaviour
{

    //public variables 
    public int cdMax;
    public float shootAngle;

    //private variables
    protected bool ignoreWalls;
    protected bool faceRight;
    protected bool active = false;
    protected GameObject wielder;
    protected GameObject bullet;
    protected Vector3 gunPos;
    protected float bulletSpeed;
    protected int bulletDamage;
    protected int cdCurrent;
    protected GameManager gM;


    // Match position with the wielder
    protected void Move()
    {
        if (faceRight)
        {
            this.transform.position = wielder.transform.position + gunPos;
        }
        else
        {
            this.transform.position = wielder.transform.position + new Vector3(-gunPos.x, gunPos.y, 0);
        }
    }

    protected virtual void AttackAnim(){

    }

    protected virtual void ChangeFaceDirection()
    {

    }

    // Initialize variables
    virtual public void Init(GameObject bul, Vector3 gP, bool ign, int cdM, float sA, float bS)
    {

    }

    protected virtual bool WillShoot()
    {
        return false;
    }

    protected virtual bool CheckFriendly()
    {
        return false;
    }

    public void SelfDestruction()
    {
        Destroy(this.gameObject);
        Destroy(this);
    }

    // Create a copy of "bullet", and pass parameters to its initialization function
    protected void Shoot()
    {
        cdCurrent = 0; // reset cooldown
        this.AttackAnim();
        
        //create bullet
        GameObject clone = Instantiate(bullet, transform.position, transform.rotation) as GameObject; // duplicate gameObject bullet and its script, store the new object as clone
        Bullet cloneScript = (Bullet)clone.GetComponent(typeof(Bullet)); // store the new object's script as cloneScript
        float bulletAngle;
        if (faceRight)
        {
            bulletAngle = shootAngle;
        }
        else
        {
            if (shootAngle < Mathf.PI)
                bulletAngle = Mathf.PI - shootAngle;
            else
            {
                bulletAngle = 3 * Mathf.PI - shootAngle;
            }
        }
        Debug.Log(this.CheckFriendly());
        cloneScript.Init(this.CheckFriendly(), ignoreWalls, bulletDamage, bulletAngle, bulletSpeed); // call the initialization funtion of the new bullet
    }

    protected void Start()
    {
        // access game manager
        GameObject gmobj = GameObject.Find("gamemanager");
        gM = (GameManager)gmobj.GetComponent(typeof(GameManager));
    }

    // Update is called once per frame
    protected void Update()
    {
        if (active && (gM.gameState=="Game")) // check if the gun is active
        {
            ChangeFaceDirection();
            this.Move();
            if (cdCurrent < cdMax) cdCurrent++; // increase cooldown counter by 1
            if (this.WillShoot()) Shoot();
        }
    }
}

    
