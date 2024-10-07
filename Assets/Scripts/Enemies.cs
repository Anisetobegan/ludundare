using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.AI;

public class Enemies : MonoBehaviour
{
    float speed;
    [SerializeField] protected float health = 100;
    [SerializeField] protected float maxHealth = 100;
    protected float damage = 5;
    float attackSpeed;
    float attackRange;

    protected float expGiven = 30;

    public float EnemyExpGiven { get { return expGiven; } set { expGiven = value; } }

    [SerializeField] protected NavMeshAgent agent;

    [SerializeField] protected List<GameObject> alliesInRange;

    protected bool targetIsPLayer = true;

    protected IEnumerator enumerator = null;

    protected IDamagable damagable;

    protected Vector3 target;

    protected GameObject closestAlly;

    protected bool isBeingGrabbed = false;

    protected bool isDead = false;

    [SerializeField] protected HealthBars healthBar;

    [SerializeField] protected ColliderTrigger colliderTrigger;

    [SerializeField] protected Animator animator;

    public float EnemyHealth { get { return health; } set { health = value; } }

    virtual protected void Start()
    {
        UpdateHealthBar();

        colliderTrigger.SetList(alliesInRange);
    }

    virtual protected void Update()
    {
        if (health <= 0 && isDead == false)
        {
            Die();
        }
    }

    virtual protected void OnMouseDown()
    {
        Die();
    }


    virtual protected void Move()
    {

    }

    virtual protected void Attack()
    {

    }

    virtual protected void Die()
    {
        Actions.OnEnemyKilled?.Invoke(this);

        isDead = true;

        healthBar.gameObject.SetActive(false);

        //Destroy(gameObject, 1f);        
    }

    virtual public void IsBeingGrabbed(bool isStopped)
    {
        agent.isStopped = isStopped;
        isBeingGrabbed = isStopped;
    }

    virtual public void TakeDamage (float damageTaken)
    {
        health -= damageTaken;

        UpdateHealthBar();
    }

    virtual protected Vector3 DetectClosestAlly()
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
            if (targetPos.layer == LayerMask.NameToLayer("Player"))
            {
                targetIsPLayer = true;
                damagable = GameManager.Instance.PlayerGet;
            }
            else
            {
                targetIsPLayer = false;
                damagable = targetPos.GetComponent<Summon>();
            }
            return targetPos.transform.position;
        }
        targetIsPLayer = true;
        damagable = GameManager.Instance.PlayerGet;
        return GameManager.Instance.PlayerTransform.position;
    }

    public void UpdateHealthBar()
    {
        healthBar.HealthBarUpdate(health / maxHealth);
    }
}
