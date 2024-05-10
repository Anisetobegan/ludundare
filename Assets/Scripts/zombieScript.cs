using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zombieScript : Summon
{
    enum State
    {
        Idle,
        Moving,
        Attacking,
        Dead
    }
    State state;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void Attack()
    {
        base.Attack();
    }

    protected override void Move()
    {
        base.Move();
    }

    protected void Wander() 
    {

    }
}
