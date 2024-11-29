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

    private GameObject closestEnemy;

    [SerializeField] ExplotionScript explosionPrefab;
    [SerializeField] AudioClip explosionClip;


    private void OnEnable()
    {
        Actions.OnEnemyKilled += EnemyDestroyed;

        health = maxHealth;
        isDead = false;
        damage = 100f;
        state = State.Idle;
        healthBar.gameObject.SetActive(true);
        UpdateHealthBar();
        enumerator = null;
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

                    Move();

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
                        state = State.Exploding;
                    }

                    break;

                case State.Exploding:

                    if (targetEnemy != null)
                    {
                        InitiateExplosion();
                    }

                    break;

                case State.Dead:

                    //Die();

                    break;
            }
        }
    }    

    void InitiateExplosion()
    {
        if (enumerator == null)
        {
            enumerator = Exploding();
            StartCoroutine(enumerator);
        }
    }

    IEnumerator Exploding()
    {
        animator.SetTrigger("isExploding");

        yield return new WaitForSeconds(timeToExplode);

        //instantiate explotion GameObject
        //ExplotionScript newExplosion = Instantiate(explosionPrefab, transform.position, transform.rotation);
        //GameObject newExplosion = ObjectPool.Instance.SpawnFromPool("GhostExplosion", transform.position, transform.rotation);
        AudioManager.Instance.PlaySFX(explosionClip);

        ExplotionScript newExplosion = ObjectPoolManager.Instance.GetFromPool(explosionPrefab);
        newExplosion.transform.position = transform.position;
        newExplosion.transform.rotation = transform.rotation;

        newExplosion.InitializeExplosion(damage, explotionRadius, ExplotionScript.ExplosionType.Ghost);

        enumerator = null;

        Die();        
    }

    protected override void Move()
    {
        Vector3 offset = target + (transform.position - target).normalized * (GameManager.Instance.playerColliderRadius + 1f);
        agent.SetDestination(offset);
    }

    public override string GetSummonName()
    {
        return "Ghost";
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

    public override void DesignateTarget(Vector3 target)
    {
        base.DesignateTarget(target);
        Move();
    }

    void EnemyDestroyed(Enemies enemyRef)
    {
        enemiesInRange.Remove(enemyRef.gameObject);
        target = Vector3.zero;
        targetEnemy = null;
        state = State.Idle;
    }

    protected override void Die()
    {
        base.Die();
        ObjectPoolManager.Instance.AddToPool(this);
    }
}
