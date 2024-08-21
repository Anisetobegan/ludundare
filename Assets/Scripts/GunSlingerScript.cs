using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class GunSlingerScript : Enemies
{
    float aimTime = 1f;
    float reloadTime = 4f;
    bool isOnRange = false;
    float timeBetweenAttacks = 1f;
    bool tookCover = false;    

    int bullets = 5;

    [SerializeField] private GameObject bullet;

    enum State
    {
        Chasing,
        Aiming,
        Attacking,
        Waiting,
        Reloading,
        Die
    }

    [SerializeField] State state;

    private void Awake()
    {
        Actions.OnSummonKilled += SummonDestroyed;
    }

    private void OnDestroy()
    {
        Actions.OnSummonKilled -= SummonDestroyed;
    }

    private void Start()
    {
        state = State.Chasing;
        target = GameManager.Instance.PlayerTransform.position;
    }

    protected override void Update()
    {
        base.Update();

        switch (state)
        {
            case State.Chasing:

                if (enumerator == null)
                {

                    target = DetectClosestAlly();
                    Move();

                    if (isOnRange)
                    {
                        state = State.Aiming;
                    }
                }

                break;

            case State.Aiming:

                target = DetectClosestAlly();

                agent.isStopped = true; // NavMeshAgent.Stop is obsolete. Set NavMeshAgent.isStopped to true.

                transform.rotation = Quaternion.RotateTowards(transform.rotation, 
                    Quaternion.LookRotation((target + Vector3.up * transform.position.y - transform.position).normalized), 360f);

                if (enumerator == null) // Checks if thereLs no running coroutines
                {
                    WaitForFinishAim(); //Calls the Aiming coroutine
                }

                break;

            case State.Attacking:
                
                if (enumerator == null)
                {
                    Attack();
                }

                break;

            case State.Reloading:                

                if (enumerator == null)
                {
                    if (tookCover == false)
                    {
                        TakingCoverDestination();
                    }
                    else
                    {
                        Reloading();
                    }
                }
                
                break;

            case State.Waiting:

                if(enumerator == null)
                {
                    if(bullets > 0)
                    {                        
                        state = State.Chasing;
                    }
                    if(bullets <= 0)
                    {
                        state = State.Reloading;
                    }
                }

                break;

            case State.Die:

                //Die();

                break;
        }
    }

    protected void WaitForFinishAim()
    {        
        enumerator = Aiming();
        StartCoroutine(enumerator);        
    }

    IEnumerator Aiming() //Aims for 1 second before attacking
    {
        yield return new WaitForSeconds(aimTime);
        enumerator = null;
        state = State.Attacking;
    }

    protected override void Move()
    {
        Vector3 offset = target + (transform.position - target).normalized * (GameManager.Instance.playerColliderRadius + 1f);
        agent.SetDestination(offset);
    }

    protected override void Attack()
    {
        if (bullets > 0)
        {
            GameObject newBullet = Instantiate(bullet, transform.position, transform.rotation);
            newBullet.GetComponent<BulletScript>().InitializeBullet(damage);
        
            bullets--;        
        
            enumerator = ResetAttack();
            StartCoroutine(enumerator);

            state = State.Waiting;
        }

        if (bullets <= 0)
        {
            state = State.Reloading;
        }
                
        
    }

    IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(timeBetweenAttacks);

        agent.isStopped = false;

        enumerator = null;
    }

    protected void Reloading()
    {
        agent.isStopped = true;
        enumerator = Reload();
        StartCoroutine (enumerator);
    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(reloadTime);

        bullets = 5;

        agent.isStopped = false;

        enumerator = null;

        tookCover = false;

        state = State.Chasing;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (alliesInRange.Contains(other.gameObject) == false)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player") || other.gameObject.layer == LayerMask.NameToLayer("Clickable"))
            {
                alliesInRange.Add(other.gameObject);
            }
        }
        isOnRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        alliesInRange.Remove(other.gameObject);

        if (alliesInRange.Count == 0)
        {
            isOnRange = false;
        }
    }

    private void TakingCoverDestination()
    {
        if (isBeingGrabbed == false)
        {
            agent.isStopped = false;
            target = DetectClosestAlly();
            Vector3 oppositeDirection = transform.position + ((transform.position - target).normalized * 5f);
            agent.SetDestination(oppositeDirection);

            enumerator = TakingCover();
            StartCoroutine(enumerator);
        }
    }

    IEnumerator TakingCover()
    {
        while (agent.remainingDistance != 0)
        {
            yield return null;
        }
        enumerator = null;
        tookCover = true;
    }

    void SummonDestroyed(Summon summonRef)
    {
        alliesInRange.Remove(summonRef.gameObject);
        target = Vector3.zero;
        state = State.Chasing;
    }
}
