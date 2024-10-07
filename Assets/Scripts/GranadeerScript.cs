using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class GranadeerScript : Enemies
{
    float timeToThrowGranade = 1f;
    float minAttackDistance = 7.5f;

    [SerializeField] private GrenadeScript grenade;

    enum State
    {
        Chasing,
        Attacking,
        Waiting,
        Die
    }

    [SerializeField] State state;

    GrenadeScript newGrenade = null;

    private void OnEnable()
    {
        Actions.OnSummonKilled += SummonDestroyed;

        health = maxHealth;
        isDead = false;
        state = State.Chasing;
        target = GameManager.Instance.PlayerTransform.position;
        healthBar.gameObject.SetActive(true);
        UpdateHealthBar();
        enumerator = null;
    }

    private void OnDisable()
    {
        Actions.OnSummonKilled -= SummonDestroyed;
    }

    protected override void Update()
    {
        if (isDead == false)
        {
            base.Update();

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

                    transform.rotation = Quaternion.RotateTowards(transform.rotation,
                        Quaternion.LookRotation((target + Vector3.up * transform.position.y - transform.position).normalized), 360f);

                    Attack();

                    break;

                case State.Waiting:

                    if (newGrenade == null || !newGrenade.gameObject.activeSelf)
                    {
                        agent.isStopped = false;
                        state = State.Chasing;

                        animator.SetTrigger("stoppedAttacking");
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
        if (enumerator == null)
        {
            agent.isStopped = true;

            animator.SetTrigger("isAttacking");

            enumerator = ThrowingGranade();
            StartCoroutine(enumerator);            
        }
    }

    IEnumerator ThrowingGranade()
    {
        yield return new WaitForSeconds(timeToThrowGranade);

        //newGrenade = Instantiate(grenade, transform.position, transform.rotation).GetComponent<GrenadeScript>();
        //newGrenade = ObjectPool.Instance.SpawnFromPool("Grenade", transform.position, transform.rotation);
        newGrenade = ObjectPoolManager.Instance.GetFromPool(grenade);
        newGrenade.transform.position = transform.position;
        newGrenade.transform.rotation = transform.rotation;

        newGrenade.InitializeGranadeTarget(target, damage);

        enumerator = null;

        state = State.Waiting;        
    }

    void SummonDestroyed(Summon summonRef)
    {
        alliesInRange.Remove(summonRef.gameObject);
        target = Vector3.zero;
        state = State.Chasing;
    }

    protected override void Die()
    {
        animator.SetTrigger("isDead");
        base.Die();
        ObjectPoolManager.Instance.AddToPool(this);
    }
}
