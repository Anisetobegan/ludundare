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

    private void OnEnable()
    {
        Actions.OnSummonKilled += SummonDestroyed;
    }

    private void OnDisable()
    {
        Actions.OnSummonKilled -= SummonDestroyed;
    }
    protected override void Start()
    {
        base.Start();

        state = State.Chasing;
        target = GameManager.Instance.PlayerTransform.position;

        colliderTrigger.GetList(alliesInRange);
    }

    protected override void Update()
    {
        if (isDead == false)
        {
            base.Update();

            switch (state)
            {
                case State.Chasing:

                    if (enumerator == null)
                    {
                        target = DetectClosestAlly();
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

                case State.Die:

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
        agent.isStopped = true; // NavMeshAgent.Stop is obsolete. Set NavMeshAgent.isStopped to true.

        animator.SetTrigger(Random.Range(0, 2) == 0 ? (Random.Range(0, 2) == 0 ? "isAttackingLeft" : "isAttackingRight") : 
            (Random.Range(0, 2) == 0 ? "isKickingLeft" : "isKickingRight"));

        damagable.Damage(damage);

        enumerator = ResetAttack();
        StartCoroutine(enumerator);
    }

    IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(timeBetweenAttacks);

        agent.isStopped = false;

        state = State.Chasing;

        enumerator = null;

        animator.SetTrigger("stoppedAttacking");
    }

    void SummonDestroyed(Summon summonRef)
    {
        alliesInRange.Remove(summonRef.gameObject);
        target = Vector3.zero;
    }
    protected override void Die()
    {
        animator.SetTrigger("isDead");
        this.enabled = false;
        base.Die();
    }
}
