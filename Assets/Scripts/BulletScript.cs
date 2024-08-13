using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BulletScript : MonoBehaviour
{
    float moveSpeed = 4;

    float damage;

    float timeToDestroyBullet = 4f;

    IDamagable damagable;

    private void Awake()
    {
        StartCoroutine(DestroyBullet());
    }

    private void Update()
    {
        Move();
    }
    private void Move()
    {
        transform.position += (transform.forward * moveSpeed)* Time.deltaTime;
    }

    public void InitializeBullet(float gunSlingerDamage)
    {
        this.damage = gunSlingerDamage;
    }

    IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(timeToDestroyBullet);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) 
        {
            damagable = other.GetComponent<Character>();
            damagable.Damage(damage);
            Destroy(gameObject);
        }
        else
        {
            damagable = other.GetComponent<Summon>();
            damagable.Damage(damage);
            Destroy(gameObject);
        }
    }
}
