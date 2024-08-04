using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perks
{
    protected Character player;

    PerkData data;

    public Character Player { get { return player; } set { player = value; } }

    public PerkData Data { get { return data; } set { data = value; } }


    virtual public void Apply()
    {

    }
}
