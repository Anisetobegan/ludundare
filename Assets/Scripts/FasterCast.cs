using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FasterCast : Perks
{
    float timeToReduce = 0.5f;

    public override void Apply()
    {
        player.PlayerCastTime -= timeToReduce;
    }
}
