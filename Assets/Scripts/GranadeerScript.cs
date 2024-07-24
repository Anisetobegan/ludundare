using System.Collections;
using System.Collections.Generic;
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

    private Vector3 target;

    [SerializeField] private List<GameObject> alliesInRange;
    private GameObject closestAlly;

    IEnumerator enumerator = null;

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
                }
                break;

            case State.Attacking:

                if (enumerator == null)
                {
                    Attack();
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
                }
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

    protected override void Attack()
    {
        agent.isStopped = true;

        newGrenade = Instantiate(grenade, transform.position, transform.rotation).GetComponent<GrenadeScript>();

        newGrenade.InitializeGranadeTarget(target);
        
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
        alliesInRange.Add(other.gameObject);
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
    }
}
