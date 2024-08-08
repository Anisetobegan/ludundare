using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastCoolDown : Perks
{
    float timeToReduce = 0.25f;

    public override void Apply()
    {
        player.PlayerCastCoolDown -= timeToReduce;
    }
}
