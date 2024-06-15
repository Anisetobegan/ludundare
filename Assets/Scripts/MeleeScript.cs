using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class MeleeScript : Enemies
{
    
    enum State
    {
        Chasing,
        Attacking,
        Die
    }

    State state;

    protected override void Move()
    {

    }

    protected override void Attack()
    {
        
    }
}
