using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Controller for large asteroid variant
public class LargeAsteroidController : AsteroidController
{
    public override void DestroyAsteroid()
    {
        for (int i = 0; i < 2; i++)
        {
            Instantiate(getRandomMedAsteroid(), transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
        gameController.AddScore(points);
        gameController.playExplosion(transform.position);
    }
}
