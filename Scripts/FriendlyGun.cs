using UnityEngine;
using System.Collections;

public class FriendlyGun : ShotGun
{

    //initialize variables
    override public void Init(GameObject bul, Vector3 gP, bool ign, int cdM, float sA, float bS)
    {
        // access game manager
        GameObject gmobj = GameObject.Find("gamemanager");
        gM = (GameManager)gmobj.GetComponent(typeof(GameManager));
        if (gM.controlingPlayer1)
        {
            wielder = GameObject.Find("player1");
        }
        else
        {
            wielder = GameObject.Find("player2");
        }

        // set attributes
        this.cdMax = cdM;
        this.gunPos = gP;
        this.shootAngle = sA;
        this.bullet = bul;
        this.ignoreWalls = ign;
        this.bulletSpeed = bS;

        // for all ShotGuns
        this.active = true;
        this.bulletDamage = 1;
        this.cdCurrent = cdMax;

    }

    protected override bool CheckFriendly()
    {
        return true;
    }

    protected override void AttackAnim()
    {
        Animator anim = wielder.GetComponent<Animator>();
        anim.Play("attack");
    }

    protected override void ChangeFaceDirection()
    {
        Rigidbody2D rb = wielder.GetComponent<Rigidbody2D>();
        faceRight = rb.velocity.x >= 0;
    }

    protected override bool WillShoot()
    {
        return (Input.GetButtonDown("Fire1") && (cdCurrent >= cdMax)); // shoot if ctrl key is down and cooldown's ready
    }
}
