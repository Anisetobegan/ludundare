using DG.Tweening;
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

    [SerializeField] protected Animator animator;

    [SerializeField] protected ColliderTrigger colliderTrigger;

    [SerializeField] protected List<GameObject> enemiesInRange;

    [SerializeField] protected Transform summonModel;

    [SerializeField] protected AudioSource summonAudioSource;

    protected Vector3 summonModelScale;

    protected IEnumerator enumerator = null;

    public float SummonHealth { get { return health; } set { health = value; } }
    public float SummonMaxHealth { get {return maxHealth; } set { maxHealth = value; } }


    // Start is called before the first frame update
    virtual protected void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        UpdateHealthBar();

        colliderTrigger.SetList(enemiesInRange);

        summonModelScale = summonModel.localScale;
    }

    // Update is called once per frame
    virtual protected void Update()
    {
        if (health <= 0 && isDead == false)
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
        target = Vector3.zero;
        targetEnemy = null;
        healthBar.gameObject.SetActive(false);
        isDead = true;
        Actions.OnSummonKilled?.Invoke(this);
        //Destroy(gameObject, 1f);
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

        summonModel.localScale = summonModelScale;

        summonModel.DOShakeScale(0.1f, 2f, 15);
    }

    protected Enemies DetectClosestEnemy()
    {
        float leastDistance = Mathf.Infinity;
        GameObject targetPos = null;

        for (int i = 0; i < enemiesInRange.Count; i++)
        {
            float currentDistance = Vector3.Distance(transform.position, enemiesInRange[i].transform.position);

            if (currentDistance < leastDistance)
            {
                leastDistance = currentDistance;
                targetPos = enemiesInRange[i].gameObject;
            }
        }
        return targetPos.GetComponent<Enemies>();
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
