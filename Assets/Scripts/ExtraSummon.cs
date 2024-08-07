using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraSummon : Perks
{
    public override void Apply ()
    {
        player.MaxSummons += 1;
    }
}
