using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BulletScript : MonoBehaviour
{
    float moveSpeed = 8;

    float damage;

    float timeToDestroyBullet = 4f;

    IDamagable damagable;

    private void OnEnable()
    {
        //Destroy(gameObject, timeToDestroyBullet);
        StartCoroutine(SetBulletInactive());
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) 
        {
            damagable = other.GetComponent<Character>();
            damagable.Damage(damage);
            gameObject.SetActive(false);
        }
        else
        {
            damagable = other.GetComponent<Summon>();
            damagable.Damage(damage);
            gameObject.SetActive(false);
        }
    }

    IEnumerator SetBulletInactive()
    {
        yield return new WaitForSeconds(timeToDestroyBullet);
        gameObject.SetActive(false);
    }
}
