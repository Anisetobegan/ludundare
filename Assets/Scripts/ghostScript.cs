using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ghostScript : Summon
{
    float explotionRadius;
    enum State
    {
        Idle,
        Moving,
        Attacking,
        Dead
    }
    State state;
    

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
}
