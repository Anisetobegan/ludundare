using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonExtraHealth : Perks
{
    float healthToAdd = 20f;
    public override void Apply()
    {
        player.SummonMaxHealth += healthToAdd;
        player.ApplyPerks();
    }
}
