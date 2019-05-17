using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{

    // public variables
    public string gameState;// includes "Menu", "Game", "Pause", "GameOver"
    public bool controlingPlayer1;
    public static Player1 player1;
    public static Player2 player2;
    public static GameObject cam;
    

    // private variables
    static List<FriendlyGun> friendlyGunList = new List<FriendlyGun>();


    // Create a copy of "friendlygun", and pass parameters to its initialization function
    private FriendlyGun CreateFriendlyGun(GameObject bullet, Vector3 gunPos, bool ign, int cdMax, float shootAngle, float bulletSpeed)
    {
        GameObject clone = Instantiate(GameObject.Find("friendlygun")); // duplicate gameObject friendlygun and its script, store the new object as clone
        FriendlyGun cloneScript = (FriendlyGun)clone.GetComponent(typeof(FriendlyGun)); // store the new object's script as cloneScript
        cloneScript.Init(bullet, gunPos, ign, cdMax, shootAngle, bulletSpeed);     // call the initialization funtion of the new FriendlyGun
        return cloneScript;
    }

    // create shotgun (consist of 3 FriendlyGuns)
    List<FriendlyGun> CreateShotgun(GameObject bullet, Vector3 gunPos, bool ign, int cdMax, float shootAngle, float bulletSpeed)
    {
        List<FriendlyGun> lst = new List<FriendlyGun>();
        lst.Add(CreateFriendlyGun(bullet, gunPos, ign, cdMax, shootAngle - Mathf.PI / 12, bulletSpeed));
        lst.Add(CreateFriendlyGun(bullet, gunPos, ign, cdMax, shootAngle, bulletSpeed));
        lst.Add(CreateFriendlyGun(bullet, gunPos, ign, cdMax, shootAngle + Mathf.PI / 12, bulletSpeed));
        return lst;
    }

    // Removes all instances of "friendlyGun"s within list lst
    private void ClearFriendlyGunList(List<FriendlyGun> lst)
    {
        int c = lst.Count;
        for (int i = 0; i < c; i++)
        {
            lst[0].SelfDestruction();
            lst.Remove(lst[0]);
        }
    }


    void CreatePlayer1Weapon()
    {
        ClearFriendlyGunList(friendlyGunList);
        GameObject bullet = GameObject.Find("bullet2");
        friendlyGunList = CreateShotgun(bullet, new Vector3(1, 0, 0), false, 60, 0, 30);
    }


    void CreatePlayer2Weapon()
    {
        ClearFriendlyGunList(friendlyGunList);
        GameObject bullet = GameObject.Find("bullet1");
        friendlyGunList.Add(CreateFriendlyGun(bullet, new Vector3(1, 0, 0), true, 30, 0, 20));
    }

    // get the position of current player controling
    public Vector3 GetPlayerPosn()
    {
        if (controlingPlayer1)
        {
            return player1.transform.position;
        }
        else
        {
            return player2.transform.position;
        }
    }

    //switch camera
    void SwitchCamera(int num)
    {
        switch (num)
        {
            case 1:
                cam.transform.position = new Vector3(1, 2, -10);
                break;
            case 2:
                cam.transform.position = new Vector3(39, 2, -10);
                break;
            case 3:
                cam.transform.position = new Vector3(77, 2, -10);
                break;
            case 4:
                cam.transform.position = new Vector3(115, 2, -10);
                break;
        }

    }

    // create a copy of "enemy", set its original position, and pass parameters to its initialization function
    private Enemy CreateEnemy(Vector3 initialPos, int dmg, int maxHP, float spd, float dir, float wr,bool fl)
    {
        GameObject clone = Instantiate(GameObject.Find("enemy")); // duplicate gameObject enemy and its script, store the new object as clone
        clone.transform.position = initialPos; // set intial position
        Enemy cloneScript = (Enemy)clone.GetComponent(typeof(Enemy)); // store the new object's script as cloneScript
        cloneScript.Init(dmg, maxHP, spd, dir, wr);     // call the initialization funtion of the new Enemy
        cloneScript.flip = fl;
        return cloneScript;
    }

    private void Start()
    {
        gameState = "Game";
        cam = GameObject.Find("Camera1");
        player1 = (Player1)GameObject.Find("player1").GetComponent(typeof(Player1));
        player2 = (Player2)GameObject.Find("player2").GetComponent(typeof(Player2));
        player1.active = true;
        controlingPlayer1 = true;
        CreatePlayer1Weapon();
        // spawn enemy
        CreateEnemy(new Vector3(11.5f, 1.5f, 0), 1, 3, 2, Mathf.PI / 2, 7, false);
        CreateEnemy(new Vector3(3.5f, 7.5f, 0), 1, 3, 2, Mathf.PI, 8, true);

        CreateEnemy(new Vector3(40.5f, 9.15f, 0), 1, 3, 3, Mathf.PI, 5, true);
        CreateEnemy(new Vector3(57f, 3, 0), 1, 3, 3, Mathf.PI*11/12, 7, true);

        CreateEnemy(new Vector3(71.5f, 3.5f, 0), 1, 3, 4, Mathf.PI, 8.5f, true);
        CreateEnemy(new Vector3(77.5f, 1, 0), 1, 3, 3, Mathf.PI /2, 7, false);
        CreateEnemy(new Vector3(81f, 8, 0), 1, 3, 3, Mathf.PI *3/2, 7, false);

        Enemy boss = CreateEnemy(new Vector3(119, 3, 0), 1, 6, 5, Mathf.PI*14/15, 12, true);
        boss.transform.localScale = new Vector3(0.75f, 0.75f, 0.7f);
    }

    void SwitchPlayer()
    {
        if (controlingPlayer1)
        {
            //switch character position
            Vector3 posn = player1.transform.position;
            player1.transform.position = posn + new Vector3(0, 23, 0);
            player2.transform.position = posn;
            player1.active = false;
            player2.active = true;
            player2.Init(player1.currentLife);
            controlingPlayer1 = !controlingPlayer1;
            CreatePlayer2Weapon();
        } else
        {
            Vector3 posn = player2.transform.position;
            player2.transform.position = posn + new Vector3(0, 23, 0);
            player1.transform.position = posn;
            player2.active = false;
            player1.active = true;
            player1.Init(player2.currentLife);
            controlingPlayer1 = !controlingPlayer1;
            CreatePlayer1Weapon();
        }

    }

    // Update is called once per frame
    void Update()
    {

        // switch camera
        if (GetPlayerPosn().x < 20)
        {
            SwitchCamera(1);
        }
        else if (GetPlayerPosn().x < 58.39)
        {
            SwitchCamera(2);
        }
        else if (GetPlayerPosn().x < 96.76)
        {
            SwitchCamera(3);
        }
        else 
        {
            SwitchCamera(4);
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SwitchPlayer();
        }
    }
}
