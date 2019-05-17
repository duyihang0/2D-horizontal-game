using UnityEngine;
using System.Collections;

public class Player2 : PlayerControl
{
    public override void Init(int curHP)
    {
        // attributes
        this.jumpTimes = 0;
        this.jumpLimit = 1;
        this.speed = 12;
        this.currentLife = curHP;
    }
}
