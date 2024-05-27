using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Summon : MonoBehaviour
{
    float health;
    float damage;
    float moveSpeed = 0.07f;
    //Enemies target;
    float attackSpeed;

    public NavMeshAgent agent;


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
        /*if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ground))
            {
                agent.SetDestination(hit.point);
            }
        }*/
    }
    protected void Die()
    {
        GameObject.Destroy(gameObject);        
    }
}
