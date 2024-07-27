using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplotionScript : MonoBehaviour
{
    [SerializeField] LayerMask layermask;

    float explotionDuration = 1f;

    [SerializeField] List<GameObject> enemiesToDamage;

    float damage;

    [SerializeField] SphereCollider sphereCollider;

    private void Start()
    {
        StartCoroutine(DisipateExplosion());
    }


    IEnumerator DisipateExplosion()
    {
        yield return new WaitForSeconds(explotionDuration);

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<Enemies>().EnemyHealth -= damage;
    }

    public void InitializeExplosion(float damage, float explosionRadius)
    {
        this.damage = damage;
        sphereCollider.radius = explosionRadius;
    }
}
