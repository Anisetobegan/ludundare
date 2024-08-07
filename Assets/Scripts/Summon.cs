using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Summon : MonoBehaviour
{
    [SerializeField] float health = 100;
    [SerializeField] float maxHealth = 100;
    protected float damage;
    float moveSpeed = 0.07f;
    //Enemies target;
    float attackSpeed;

    public bool isDead = false;

    public NavMeshAgent agent;

    protected Vector3 target = Vector3.zero;
    protected GameObject targetEnemy = null;

    public float SummonHealth { get { return health; } set { health = value; } }
    public float SummonMaxHealth { get {return maxHealth; } set { maxHealth = value; } }


    // Start is called before the first frame update
    protected void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        
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

    virtual public void DesignateTarget(GameObject target)
    {
        this.targetEnemy = target;
        this.target = target.transform.position;
    }
}
