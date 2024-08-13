using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zombieScript : Summon
{
    float minAttackDistance = 1.3f;
    float timeBetweenAttacks = 3f;

    enum State
    {
        Wandering,
        Moving,
        Chasing,
        Attacking,
        Dead
    }
    State state;

    Vector3 walkPoint;
    bool walkPointSet;
    float walkPointRange = 3;

    [SerializeField] LayerMask ground;

    IEnumerator enumerator = null;

    private void Awake()
    {
        walkPointSet = false;
        state = State.Wandering;

        damage = 25f;
    }


    void Update()
    {
        switch (state)
        {
            
            case State.Wandering:

                if (enumerator == null)
                {
                    Wander();

                    if (target != Vector3.zero)
                    {
                        state = State.Moving;
                    }
                }

                break;

            case State.Moving:

                if (enumerator == null)
                {
                    Move();

                    if (agent.remainingDistance <= 0)
                    {
                        target = Vector3.zero;
                        state = State.Wandering;
                    }

                    if (targetEnemy != null)
                    {
                        state = State.Chasing;
                    }
                }

                break;

            case State.Chasing:

                if (enumerator == null)
                {
                    target = targetEnemy.transform.position;
                    Move();

                    float distance = Vector3.Distance(agent.transform.position, target);

                    if (distance < minAttackDistance)
                    {
                        state = State.Attacking;
                    }
                }

                break;

            case State.Attacking:

                if (enumerator == null)
                {
                    Attack();
                }

                break;            

            case State.Dead:

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

        Actions.OnEnemyDamaged?.Invoke(targetEnemy, damage);

        enumerator = ResetAttack();
        StartCoroutine(enumerator);
    }

    IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(timeBetweenAttacks);

        agent.isStopped = false;

        state = State.Wandering;

        enumerator = null;
    }

    protected void Wander() 
    {
        if (!walkPointSet)
        {
            SearchWalkPoint();
        }

        if (walkPointSet) 
        {
            agent.SetDestination(walkPoint);
        }

        if (agent.remainingDistance <= 0) 
        {
            walkPointSet = false;
        }
    }

    private void SearchWalkPoint()
    {
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        float randomZ = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3 (transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, ground))
        {
            walkPointSet = true;
        }
        
    }

    public override string GetSummonName()
    {
        return "Zombie";    
    }

    public override void DesignateTarget(Vector3 target)
    {
        base.DesignateTarget(target);
        state = State.Moving;
    }
}
