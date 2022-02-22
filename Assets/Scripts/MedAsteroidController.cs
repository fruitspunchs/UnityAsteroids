using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedAsteroidController : AsteroidController
{
    public override void DestroyAsteroid()
    {
        for (int i = 0; i < 2; i++)
        {
            Instantiate(getRandomSmallAsteroid(), transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
        gameController.AddScore(points);
        gameController.playExplosion(transform.position);
    }
}
