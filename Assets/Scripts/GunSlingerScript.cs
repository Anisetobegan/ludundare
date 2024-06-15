using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSlingerScript : Enemies
{
    float aimTime;
    //[SerializeField] private Bullet bullet;

    enum State
    {
        Chasing,
        Reloading,
        Attacking,
        Die
    }

    State state;

    protected void Aim()
    {

    }

    protected override void Move()
    {

    }

    protected override void Attack()
    {

    }

    protected void Reloading()
    {
        
    }
}
