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
    [SerializeField] State state;

    [SerializeField] private List<GameObject> enemiesInRange;
    private GameObject closestEnemy;

    IEnumerator enumerator = null;

    bool isGrabbing = false;

    private void Awake()
    {
        state = State.Idle;
        damage = 10f;

        colliderTrigger.GetList(enemiesInRange);
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

                case State.Idle:

                    if (target != Vector3.zero)
                    {
                        state = State.Moving;
                        animator.SetBool("isWalking", true);
                    }

                    if (enemiesInRange.Count > 0)
                    {
                        targetEnemy = DetectClosestEnemy();
                        state = State.Chasing;
                        animator.SetBool("isWalking", true);
                    }

                    break;

                case State.Moving:


                    if (agent.remainingDistance <= 0)
                    {
                        target = Vector3.zero;
                        state = State.Idle;
                        animator.SetBool("isWalking", false);
                    }

                    if (targetEnemy != null)
                    {
                        state = State.Chasing;
                        animator.SetBool("isWalking", true);
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
                        state = State.Grabbing;
                        StartGrabbing();
                    }

                    break;

                case State.Grabbing:

                    if (targetEnemy != null)
                    {
                        Attack();
                    }
                    else
                    {
                        state = State.Idle;
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

    void StartGrabbing()
    {
        isGrabbing = true;
        animator.SetBool("isGrabbing", true);
        animator.SetBool("isWalking", false);
        targetEnemy.GetComponent<Enemies>().IsBeingGrabbed(isGrabbing);

        
    }

    protected override void Attack()
    {
        if (enumerator == null)
        {
            targetEnemy.TakeDamage(damage);

            enumerator = ResetAttack();
            StartCoroutine(enumerator);
        }
    }

    IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(timeBetweenAttacks);

        enumerator = null;
    }

    public override void DesignateTarget(Vector3 target)
    {
        base.DesignateTarget(target);
        Move();
    }

    public override string GetSummonName()
    {
        return "Skeleton";
    }

    void EnemyDestroyed(Enemies enemyRef)
    {
        isGrabbing = false;
        enemiesInRange.Remove(enemyRef.gameObject);
        target = Vector3.zero;
        targetEnemy = null;
        animator.SetBool("isGrabbing", false);
    }

    protected override void Die()
    {
        isGrabbing = false;

        animator.SetTrigger("isDead");

        if (targetEnemy != null)
        {
            targetEnemy.GetComponent<Enemies>().IsBeingGrabbed(isGrabbing);
        }

        this.enabled = false;

        base.Die();        
    }
}
