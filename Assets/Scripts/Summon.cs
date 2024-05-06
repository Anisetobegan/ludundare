using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Summon : MonoBehaviour
{
    float health;
    float damage;
    float moveSpeed = 0.07f;
    //Enemies target;
    float attackSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
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
    protected void Die()
    {
        GameObject.Destroy(gameObject);        
    }
}
