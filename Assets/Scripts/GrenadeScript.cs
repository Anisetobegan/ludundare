using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeScript : MonoBehaviour
{
    float moveSpeed = 5;

    [SerializeField] private AnimationCurve curve;

    Vector3 target;

    float grenadeHeight = 3;

    float damage;

    float explosionRadius = 3f;

    IEnumerator enumerator = null;

    IDamagable damagable;

    [SerializeField] ExplotionScript explosionPrefab;


    private void Update()
    {
        if (enumerator == null)
        {
            enumerator = Move();
            StartCoroutine(enumerator);
        }
    }
    IEnumerator Move()
    {
        Vector3 directionVector = target - transform.position;
        float totalDistance = directionVector.magnitude; //stores the distance between the starting position of the grenade and the target
        directionVector.Normalize();

        float distanceTraveled = 0f;

        Vector3 newPos = transform.position;

        while (distanceTraveled <= totalDistance)
        {
            Vector3 deltaPath = directionVector * (moveSpeed * Time.deltaTime);

            newPos += deltaPath;

            distanceTraveled += deltaPath.magnitude; //adds the distance traveled each frame

            newPos.y = transform.localScale.y + (grenadeHeight * curve.Evaluate(distanceTraveled / totalDistance));

            transform.position = newPos;

            yield return null;
        }

        Explode();
        
    }

    private void Explode()
    {
        ExplotionScript newExplosion = Instantiate(explosionPrefab, transform.position, transform.rotation);
        newExplosion.InitializeExplosion(damage, explosionRadius, ExplotionScript.ExplosionType.Grenade);

        Destroy(gameObject);
    }


    public void InitializeGranadeTarget(Vector3 target, float grenadierDamage)
    {
        this.target = target;
        this.damage = grenadierDamage;
    }
}
