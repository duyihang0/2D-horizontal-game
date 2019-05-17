using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{ 

    // private variables
    bool friendly;
    public bool ignoreWalls;
    int damage;
    float speed;
    float direction;


    //Initialize variables
    public void Init(bool fr, bool igw, int dmg, float dir, float spd)
    {
        friendly = fr;
        ignoreWalls = igw;
        damage = dmg;
        speed = spd;
        direction = dir;
        if (dir>0.5f*Mathf.PI&& dir < 1.5f * Mathf.PI)
        {
            transform.rotation = new Quaternion(0, 180, 0, 0); 
        }
    }


    void Move()
    {
        // move speed units in the angle direction, note: adjusted by Time.deltaTime to compensate for game clock
        this.transform.position += speed * Time.deltaTime * new Vector3(Mathf.Cos(direction), Mathf.Sin(direction), 0);
    }

    public void SelfDestruction()
    {
        Destroy(this.gameObject);
        Destroy(this);
    }


    // Hit tests
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "platform" || col.gameObject.tag == "wall")
        {
            if (!ignoreWalls)
            {
                SelfDestruction();
            }
        }
        else if (col.gameObject.tag == "enemy" && friendly)
        {
            Enemy enemyScript = (Enemy)col.GetComponent(typeof(Enemy));
            enemyScript.TakeDamage(damage);
            SelfDestruction();
        }
        else if (col.gameObject.tag=="player" && !friendly)
        {

        }
    }

    void Start()
    {
        
    }


    // Update is called once per frame
    void Update () {
        Move();
    }
}
