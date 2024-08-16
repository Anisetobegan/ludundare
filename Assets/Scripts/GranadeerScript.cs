using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class GranadeerScript : Enemies
{
    bool isOnRange = false;
    float timeToThrowGranade = 2f;
    [SerializeField] private GameObject grenade;

    enum State
    {
        Chasing,
        Attacking,
        Waiting,
        Die
    }

    State state;

    private GrenadeScript newGrenade = null; 

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

    private void Update()
    {
        switch (state)
        {
            case State.Chasing:

                if (enumerator == null)
                {

                    target = DetectClosestAlly();
                    Move();

                    if (isOnRange)
                    {
                        state = State.Attacking;
                    }

                    if (health <= 0)
                    {
                        state = State.Die;
                    }
                }
                break;

            case State.Attacking:

                if (enumerator == null)
                {
                    Attack();

                    if (health <= 0)
                    {
                        state = State.Die;
                    }
                }

                break;

            case State.Waiting:
                if (enumerator == null)
                {
                    if(newGrenade == null)
                    {
                        agent.isStopped = false;
                        state = State.Chasing;
                    }

                    if (health <= 0)
                    {
                        state = State.Die;
                    }
                }
                break;

            case State.Die:

                Die();

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
        agent.isStopped = true;

        newGrenade = Instantiate(grenade, transform.position, transform.rotation).GetComponent<GrenadeScript>();

        newGrenade.InitializeGranadeTarget(target, damage);
        
        enumerator = ThrowingGranade();
        StartCoroutine(enumerator);
        
        state = State.Waiting;
    }

    IEnumerator ThrowingGranade()
    {
        yield return new WaitForSeconds(timeToThrowGranade);
        enumerator = null;
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

    void SummonDestroyed(Summon summonRef)
    {
        alliesInRange.Remove(summonRef.gameObject);
        target = Vector3.zero;
        state = State.Chasing;
    }
}
