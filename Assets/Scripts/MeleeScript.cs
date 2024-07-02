using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.AI;

public class MeleeScript : Enemies
{

    float minAttackDistance = 1.3f;
    float timeBetweenAttacks = 3f;

    enum State
    {
        Chasing,
        Attacking,
        Die
    }

    State state;

    private Vector3 target;

    [SerializeField] private List<GameObject> alliesInRange;
    private GameObject closestAlly;


    private void Start()
    {
        state = State.Chasing;
        target = GameManager.Instance.PlayerTransform.position;
    }

    private void Update()
    {
        switch (state)
        {
            case State.Chasing:

                target = DetectClosestAlly();
                Move();
                
                float distance = Vector3.Distance(agent.transform.position, target);

                if (distance < minAttackDistance)
                {
                    state = State.Attacking;
                }

                break;

            case State.Attacking:

                Attack();

                break;

            case State.Die:

                break;
        }
        
    }

    protected override void Move()
    {
        Vector3 offset = target + (transform.position - target).normalized * (GameManager.Instance.playerColliderRadius + 1f);
        agent.SetDestination(offset);
    }

    protected override void Attack()
    {
        agent.isStopped = true; // NavMeshAgent.Stop is obsolete. Set NavMeshAgent.isStopped to true.

        StartCoroutine(ResetAttack());
    }

    IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(timeBetweenAttacks);

        agent.isStopped = false;

        state = State.Chasing;
    }

    private void OnTriggerEnter(Collider other)
    {
        alliesInRange.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        alliesInRange.Remove(other.gameObject);
    }

    private Vector3 DetectClosestAlly()
    {
        if (alliesInRange.Count > 0)
        {
            float leastDistance = Mathf.Infinity;
            GameObject targetPos = null;

            for (int i = 0; i < alliesInRange.Count; i++)
            {
                float currentDistance = Vector3.Distance(agent.transform.position, alliesInRange[i].transform.position);

                if (currentDistance < leastDistance)
                {
                    leastDistance = currentDistance;
                    targetPos = alliesInRange[i].gameObject;
                }
            }
            return targetPos.transform.position;
        }
        
        return GameManager.Instance.PlayerTransform.position;
    }
}
