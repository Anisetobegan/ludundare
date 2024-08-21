using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Summon : MonoBehaviour, IDamagable
{
    [SerializeField] protected float health = 100;
    [SerializeField] protected float maxHealth = 100;
    protected float damage;
    protected float moveSpeed = 0.07f;

    public bool isDead = false;

    public NavMeshAgent agent;

    protected Vector3 target = Vector3.zero;
    protected Enemies targetEnemy = null;

    [SerializeField] protected HealthBars healthBar;

    public float SummonHealth { get { return health; } set { health = value; } }
    public float SummonMaxHealth { get {return maxHealth; } set { maxHealth = value; } }


    // Start is called before the first frame update
    virtual protected void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        UpdateHealthBar();
    }

    // Update is called once per frame
    virtual protected void Update()
    {
        if (health <= 0)
        {
            Die();
        }
    }

    virtual protected void Attack()
    {

    }

    virtual protected void Move()
    {
        
    }
    virtual protected void Die()
    {
        isDead = true;
        Actions.OnSummonKilled?.Invoke(this);
        GameObject.Destroy(gameObject);        
    }

    virtual public string GetSummonName()
    {
        return "";
    }

    virtual public void DesignateTarget(Vector3 target)
    {
        this.target = target;
        targetEnemy = null;
    }

    virtual public void DesignateTarget(Enemies target)
    {
        this.targetEnemy = target;
        this.target = target.transform.position;
    }

    void TakeDamage(float damage)
    {
        health -= damage;

        UpdateHealthBar();
    }

    public void Damage(float damage)
    {
        TakeDamage(damage);
    }

    public void UpdateHealthBar()
    {
        healthBar.HealthBarUpdate(health / maxHealth);
    }

    public void KillSummon()
    {
        Die();
    }
}
