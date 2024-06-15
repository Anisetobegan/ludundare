using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GranadeerScript : Enemies
{

    //[SerializeField] private Bullet bullet;
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
