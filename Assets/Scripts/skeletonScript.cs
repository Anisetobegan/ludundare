using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skeletonScript : Summon
{
    float minAttackDistance = 1.6f;
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
    
    private GameObject closestEnemy;

    bool isGrabbing = false;


    private void OnEnable()
    {
        Actions.OnEnemyKilled += EnemyDestroyed;

        health = maxHealth;
        isDead = false;
        state = State.Idle;
        damage = 10f;
        healthBar.gameObject.SetActive(true);
        UpdateHealthBar();
        enumerator = null;
        isGrabbing = false;
    }

    private void OnDisable()
    {
        Actions.OnEnemyKilled -= EnemyDestroyed;
    }

    protected override void Update()
    {
        for (int i = enemiesInRange.Count - 1; i >= 0; i--)
        {
            if (!enemiesInRange[i].activeInHierarchy)
            {
                enemiesInRange.RemoveAt(i);
            }
        }

        if (isDead == false)
        {
            base.Update();

            switch (state)
            {

                case State.Idle:

                    summonAudioSource.enabled = false;

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

                    summonAudioSource.enabled = true;

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

                    summonAudioSource.enabled = true;

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

    void StartGrabbing()
    {
        if (targetEnemy != null)
        {
            isGrabbing = true;
            animator.SetBool("isGrabbing", true);
            animator.SetBool("isWalking", false);
            summonAudioSource.enabled = false;
            targetEnemy.IsBeingGrabbed(isGrabbing);
        }
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
        if (enemyRef.IsEnemyDead && targetEnemy == enemyRef)
        {
            isGrabbing = false;
            enemiesInRange.Remove(enemyRef.gameObject);
            target = transform.position;
            targetEnemy = null;
            animator.SetBool("isGrabbing", false);
        }
    }

    protected override void Die()
    {
        isGrabbing = false;

        animator.SetBool("isGrabbing", false);
        animator.SetTrigger("isDead");

        if (targetEnemy != null)
        {
            targetEnemy.IsBeingGrabbed(isGrabbing);
        }

        enemiesInRange.Clear();

        base.Die();

        ObjectPoolManager.Instance.AddToPool(this);
    }
}
