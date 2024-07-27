using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ghostScript : Summon
{
    float explotionRadius = 5f;
    float minAttackDistance = 1.5f;
    float timeToExplode = 2f;
    enum State
    {
        Idle,
        Moving,
        Chasing,
        Exploding,
        Dead
    }
    [SerializeField] State state;

    IEnumerator enumerator = null;

    [SerializeField] private List<GameObject> enemiesInRange;
    private GameObject closestEnemy;

    [SerializeField] ExplotionScript explosionPrefab;

    private void Start()
    {
        damage = 100f;
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
                        state = State.Exploding;
                    }
                }

                break;

            case State.Exploding:

                if (enumerator == null)
                {
                    InitiateExplosion();
                }

                break;

            case State.Dead:
                break;
        }
    }

    protected override void Attack()
    {

    }

    void InitiateExplosion()
    {
        enumerator = Exploding();
        StartCoroutine(enumerator);
    }

    IEnumerator Exploding()
    {
        yield return new WaitForSeconds(timeToExplode);

        //instantiate explotion GameObject
        ExplotionScript newExplosion = Instantiate(explosionPrefab, transform.position, transform.rotation);
        newExplosion.InitializeExplosion(damage, explotionRadius);

        enumerator = null;

        state = State.Dead;

        Die();
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

    public override string GetSummonName()
    {
        return "Ghost";
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

    public override void DesignateTarget(Vector3 target)
    {
        base.DesignateTarget(target);
        state = State.Moving;
    }
}
