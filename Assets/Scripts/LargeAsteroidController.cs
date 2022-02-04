using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargeAsteroidController : AsteroidController
{
    public override void DestroyAsteroid()
    {
        Destroy(gameObject);
    }
}
