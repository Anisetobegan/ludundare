using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemies : MonoBehaviour
{
    float speed;
    float health;
    float damage;
    float attackSpeed;
    float attackRange;

    protected NavMeshAgent agent;

    protected Vector3 target;

    protected void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

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

}
