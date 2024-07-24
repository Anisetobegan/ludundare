using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skeletonScript : Summon
{
    float minAttackDistance = 1.5f;
    float timeBetweenAttacks = 3f;

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
        Actions.OnEnemyKilled += EnemyDestroyed;
    }
    
    private void Start()
    {
        state = State.Idle;
    }

    private void OnDestroy()
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
                        state = State.Grabbing;
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
                }

                break;

            case State.Dead:

                isGrabbing = false;
                targetEnemy.GetComponent<Enemies>().IsBeingGrabbed(isGrabbing);
                Die();

                break;
        }
    }

    protected override void Move()
    {
        Vector3 offset = target + (transform.position - target).normalized * (GameManager.Instance.playerColliderRadius + 1f);
        agent.SetDestination(offset);
    }

    private GameObject DetectClosestEnemy()
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
        return targetPos;
    }

    protected override void Attack()
    {
        
    }

    void StartGrabbing()
    {
        isGrabbing = true;
        targetEnemy.GetComponent<Enemies>().IsBeingGrabbed(isGrabbing);
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
            enemiesInRange.Add(other.gameObject);
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
}
