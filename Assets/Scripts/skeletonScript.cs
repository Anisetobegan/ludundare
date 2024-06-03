using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skeletonScript : Summon
{
    enum State
    {
        Idle,
        Moving,
        Attacking,
        Dead
    }
    State state;
    float range;


    

    // Update is called once per frame
    void Update()
    {

    }

    protected override void Attack()
    {
        
    }

    protected override void Move()
    {
        
    }

    public override string GetSummonName()
    {
        return "Skeleton";
    }

}
