using UnityEngine;
using System.Collections;

public class Player1 : PlayerControl
{
    public override void Init(int curHP)
    {
        // attributes
        this.jumpTimes = 0;
        this.jumpLimit = 2;
        this.speed = 10;
        this.currentLife = curHP;
    }
}
