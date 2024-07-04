using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    float moveSpeed = 4;

    private void Update()
    {
        Move();
    }
    private void Move()
    {
        transform.position += (transform.forward * moveSpeed)* Time.deltaTime;
    }    
}
