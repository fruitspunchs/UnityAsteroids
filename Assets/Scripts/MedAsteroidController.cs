using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedAsteroidController : AsteroidController
{
    public override void DestroyAsteroid()
    {
        Destroy(gameObject);
    }
}
