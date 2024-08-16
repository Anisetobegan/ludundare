using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skeletonScript : Summon
{
    float minAttackDistance = 1.5f;
    float timeBetweenAttacks = 1f;

    enum State
    {
        Idle,
        Moving,
        Chasing,
        Grabbing,
        Dead
    }
    State state;

    [SerializeField] private List<GameObject> enemiesInRange;
    private GameObject closestEnemy;

    IEnumerator enumerator = null;

    bool isGrabbing = false;

    private void Awake()
    {
        state = State.Idle;
        damage = 10f;
    }

    private void OnEnable()
    {
        Actions.OnEnemyKilled += EnemyDestroyed;
    }

    private void OnDisable()
    {
        Actions.OnEnemyKilled -= EnemyDestroyed;
    }

    void Update()
    {
        switch (state)
        {

            case State.Idle:

                if (enumerator == null)
                {
                    if (target != Vector3.zero)
                    {
                        state = State.Moving;
                    }

                    if (enemiesInRange.Count > 0)
                    {
                        targetEnemy = DetectClosestEnemy();
                        state = State.Chasing;
                    }

                    if (health <= 0)
                    {
                        state = State.Dead;
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
                        state = State.Idle;
                    }

                    if (targetEnemy != null)
                    {
                        state = State.Chasing;
                    }

                    if (health <= 0)
                    {
                        state = State.Dead;
                    }
                }

                break;

            case State.Chasing:

                if (enumerator == null)
                {
                    if (targetEnemy != null)
                    {
                        target = targetEnemy.transform.position;
                    }
                    
                    Move();

                    float distance = Vector3.Distance(agent.transform.position, target);

                    if (distance < minAttackDistance)
                    {
                        state = State.Grabbing;
                    }

                    if (health <= 0)
                    {
                        state = State.Dead;
                    }
                }

                break;

            case State.Grabbing:

                if (enumerator == null)
                {
                    if (targetEnemy != null)
                    {
                        StartGrabbing();
                    }
                    else
                    {
                        state = State.Idle;
                    }

                    if (health <= 0)
                    {
                        state = State.Dead;
                    }
                }

                break;

            case State.Dead:
                
                Die();

                break;
        }
    }

    protected override void Move()
    {
        Vector3 offset = target + (transform.position - target).normalized * (GameManager.Instance.playerColliderRadius + 1f);
        agent.SetDestination(offset);
    }

    private Enemies DetectClosestEnemy()
    {
        float leastDistance = Mathf.Infinity;
        GameObject targetPos = null;

        for (int i = 0; i < enemiesInRange.Count; i++)
        {
            float currentDistance = Vector3.Distance(agent.transform.position, enemiesInRange[i].transform.position);

            if (currentDistance < leastDistance)
            {
                leastDistance = currentDistance;
                targetPos = enemiesInRange[i].gameObject;
            }
        }
        return targetPos.GetComponent<Enemies>();
    }

    protected override void Attack()
    {
        targetEnemy.TakeDamage(damage);

        enumerator = ResetAttack();
        StartCoroutine(enumerator);
    }

    IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(timeBetweenAttacks);

        enumerator = null;
    }

    void StartGrabbing()
    {
        isGrabbing = true;
        targetEnemy.GetComponent<Enemies>().IsBeingGrabbed(isGrabbing);

        Attack();
    }


    public override void DesignateTarget(Vector3 target)
    {
        base.DesignateTarget(target);
        state = State.Moving;
    }

    public override string GetSummonName()
    {
        return "Skeleton";
    }

    private void OnTriggerEnter(Collider other)
    {
        if (enemiesInRange.Contains(other.gameObject) == false)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                enemiesInRange.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        enemiesInRange.Remove(other.gameObject);
    }

    void EnemyDestroyed(Enemies enemyRef)
    {
        isGrabbing = false;
        enemiesInRange.Remove(enemyRef.gameObject);
        target = Vector3.zero;
        targetEnemy = null;
    }

    protected override void Die()
    {
        isGrabbing = false;

        if (targetEnemy != null)
        {
            targetEnemy.GetComponent<Enemies>().IsBeingGrabbed(isGrabbing);
        }

        base.Die();        
    }
}
