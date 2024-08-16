using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSpaceHealthBar : HealthBars
{
    Camera cam; 

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        transform.rotation = cam.transform.rotation;
    }
}
