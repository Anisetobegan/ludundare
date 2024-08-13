using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class ExplotionScript : MonoBehaviour
{
    float explotionDuration = 1f;

    float damage;

    [SerializeField] SphereCollider sphereCollider;

    public enum ExplosionType
    {
        Ghost,
        Grenade
    }

    ExplosionType explosionType;

    IDamagable damagable;

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
        switch (explosionType)
        {
            case ExplosionType.Grenade:

                if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    damagable = other.GetComponent<Character>();
                    damagable.Damage(damage);
                }
                else if (other.gameObject.layer == LayerMask.NameToLayer("Clickable"))
                {
                    damagable = other.GetComponent<Summon>();
                    damagable.Damage(damage);
                }

                break;

            case ExplosionType.Ghost:

                if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    other.GetComponent<Enemies>().EnemyHealth -= damage;
                }                

                break;
        }        
    }

    public void InitializeExplosion(float damage, float explosionRadius, ExplosionType explosionType)
    {
        this.damage = damage;
        sphereCollider.radius = explosionRadius;
        this.explosionType = explosionType;
    }
}
