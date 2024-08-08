using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraMoveSpeed : Perks
{
    float moveSpeedToAdd = 1f;

    public override void Apply()
    {
        player.PlayerMoveSpeed += moveSpeedToAdd;
    }
}
