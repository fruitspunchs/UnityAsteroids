using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallAsteroidController : AsteroidController
{
    public override void DestroyAsteroid()
    {
        Destroy(gameObject);
        gameController.AddScore(points);
    }
}
