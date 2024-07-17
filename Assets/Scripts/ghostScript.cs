using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ghostScript : Summon
{
    float explotionRadius;
    enum State
    {
        Idle,
        Moving,
        Attacking,
        Dead
    }
    State state;

    Vector3 target = Vector3.zero;
    GameObject targetEnemy = null;

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void Attack()
    {

    }

    protected override void Move()
    {
        
    }

    public override string GetSummonName()
    {
        return "Ghost";
    }

    public override void DesignateTarget(Vector3 target)
    {
        this.target = target;
        targetEnemy = null;
    }

    public override void DesignateTarget(GameObject target)
    {
        this.targetEnemy = target;
        this.target = target.transform.position;
    }
}
