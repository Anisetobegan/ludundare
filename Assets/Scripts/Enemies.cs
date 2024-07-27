using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemies : MonoBehaviour
{
    float speed;
    [SerializeField] protected float health = 100;
    protected float damage = 5;
    float attackSpeed;
    float attackRange;

    protected float expGiven = 30;

    public float EnemyExpGiven { get { return expGiven; } set { expGiven = value; } }

    [SerializeField] protected NavMeshAgent agent;

    public float EnemyHealth { get { return health; } set { health = value; } }

    virtual protected void OnMouseDown()
    {
        Actions.OnEnemyKilled?.Invoke(this);

        Die();
    }


    virtual protected void Move()
    {

    }

    virtual protected void Attack()
    {

    }

    virtual protected void Die()
    {
        Destroy(gameObject);
    }

    virtual public void IsBeingGrabbed(bool isStopped)
    {
        agent.isStopped = isStopped;
    }

}
