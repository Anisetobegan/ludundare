using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.AI;

public class MeleeScript : Enemies
{
    
    enum State
    {
        Chasing,
        Attacking,
        Die
    }

    State state;

    private void Update()
    {
        target = GameManager.Instance.PlayerTransform.position;
        Move();
        Debug.DrawLine(transform.position, target + (transform.position - GameManager.Instance.PlayerTransform.position).normalized * (GameManager.Instance.playerColliderRadius + 0.1f), Color.red);
    }

    protected override void Move()
    {
        Vector3 offset = target + (transform.position - GameManager.Instance.PlayerTransform.position).normalized * (GameManager.Instance.playerColliderRadius + 0.7f);
        agent.SetDestination(offset);
    }

    protected override void Attack()
    {
        
    }
}
