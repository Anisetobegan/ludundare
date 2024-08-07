using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perks
{
    protected Character player;

    PerkData data;

    public Character Player { get { return player; } }

    public PerkData Data { get { return data; } set { data = value; } }

    public Perks() { player = GameManager.Instance.PlayerGet; }

    virtual public void Apply()
    {

    }
}
