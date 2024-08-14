using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExtraHealth : Perks
{
    float healthToAdd = 25f;
    public override void Apply()
    {
        player.PlayerMaxHealth += healthToAdd;
        player.PlayerHealth += healthToAdd;

        player.UpdateHealthBar();
    }
}
