using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Controller for small asteroid variant
public class SmallAsteroidController : AsteroidController
{
    public override void DestroyAsteroid()
    {
        Destroy(gameObject);
        gameController.AddScore(points);
        gameController.playExplosion(transform.position);
    }
}
