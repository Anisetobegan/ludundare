using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class zombieScript : Summon
{
    float minAttackDistance = 1.6f;
    float timeBetweenAttacks = 2f;

    enum State
    {
        Wandering,
        Moving,
        Chasing,
        Attacking,
        Dead
    }
    [SerializeField] State state;

    Vector3 walkPoint;
    bool walkPointSet;
    float walkPointRange = 3;

    [SerializeField] LayerMask ground;

    protected override void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        UpdateHealthBar();
    }

    private void Awake()
    {
        walkPointSet = false;
        state = State.Wandering;

        damage = 25f;

        enemiesInRange = null;
        colliderTrigger = null;
    }

    private void OnEnable()
    {
        Actions.OnEnemyKilled += EnemyDestroyed;
    }

    private void OnDisable()
    {
        Actions.OnEnemyKilled -= EnemyDestroyed;
    }


    protected override void Update()
    {
        if (isDead == false)
        {
            base.Update();

            switch (state)
            {

                case State.Wandering:

                    Wander();

                    if (target != Vector3.zero)
                    {
                        state = State.Moving;
                    }

                    break;

                case State.Moving:

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

                    break;

                case State.Chasing:

                    if (targetEnemy != null)
                    {
                        target = targetEnemy.transform.position;
                    }

                    Move();

                    float distance = Vector3.Distance(agent.transform.position, target);

                    if (distance < minAttackDistance)
                    {
                        state = State.Attacking;
                    }

                    break;

                case State.Attacking:

                    if (targetEnemy != null)
                    {
                        Attack();
                    }

                    break;

                case State.Dead:

                    //Die();

                    break;
            }
        }
    }

    protected override void Move()
    {
        Vector3 offset = target + (transform.position - target).normalized * (GameManager.Instance.playerColliderRadius + 1f);
        agent.SetDestination(offset);
    }

    protected override void Attack()
    {
        if (enumerator == null)
        {
            agent.isStopped = true; // NavMeshAgent.Stop is obsolete. Set NavMeshAgent.isStopped to true.

            animator.SetTrigger(Random.Range(0, 2) == 0 ? "isAttackingLeft" : "isAttackingRight");
            targetEnemy.TakeDamage(damage);

            enumerator = ResetAttack();
            StartCoroutine(enumerator);
        }
    }

    IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(timeBetweenAttacks);

        agent.isStopped = false;

        state = State.Wandering;

        enumerator = null;

        animator.SetTrigger("stoppedAttacking");
    }

    protected void Wander() 
    {
        if (agent != null)
        {
            if (walkPointSet == false)
            {
                SearchWalkPoint();
            }
            else
            {
                agent.SetDestination(walkPoint);

                if (agent.remainingDistance <= 0)
                {
                    walkPointSet = false;
                }
            }            
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

    void EnemyDestroyed(Enemies enemyRef)
    {
        target = Vector3.zero;
        targetEnemy = null;
        state = State.Wandering;
    }

    protected override void Die()
    {
        animator.SetTrigger("isDead");
        this.enabled = false;
        base.Die();
    }
}
