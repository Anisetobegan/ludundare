using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skeletonScript : Summon
{
    enum State
    {
        Idle,
        Moving,
        Attacking,
        Dead
    }
    State state;
    float range;

<<<<<<< Updated upstream
=======
    [SerializeField] private List<GameObject> enemiesInRange;
    private GameObject closestEnemy;

    IEnumerator enumerator = null;

    bool isGrabbing = false;

    private void Start()
    {
        Actions.OnEnemyKilled += EnemyDestroyed;
    }

    private void OnDestroy()
    {
        Actions.OnEnemyKilled -= EnemyDestroyed;
    }

>>>>>>> Stashed changes
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
        return "Skeleton";
    }
}
