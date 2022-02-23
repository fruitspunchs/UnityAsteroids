using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Parent controller of all asteroid types
public class AsteroidController : MonoBehaviour
{
    Vector2 lookDirection;
    public float speed = 0.5f;
    Rigidbody2D rigidbody2d;

    public GameObject smallAsteroid0;
    public GameObject smallAsteroid1;
    public GameObject smallAsteroid2;
    GameObject[] smallAsteroids;

    public GameObject medAsteroid0;
    public GameObject medAsteroid1;
    public GameObject medAsteroid2;
    GameObject[] medAsteroids;

    public int points = 0;
    protected GameController gameController;

    void Awake()
    {
        //Get reference to main controller
        GameObject game = GameObject.Find("Game");
        gameController = game.GetComponent<GameController>();

        //Set asteroids to choose from
        smallAsteroids = new GameObject[] { smallAsteroid0, smallAsteroid1, smallAsteroid2 };
        medAsteroids = new GameObject[] { medAsteroid0, medAsteroid1, medAsteroid2 };

        //Randomize move direction
        lookDirection.x = Random.value;
        lookDirection.y = Random.value;
        lookDirection.Normalize();

        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //Wrap position around screen
        Vector2 position = transform.position;
        if (position.x > 6.7f)
        {
            position.x = -6.7f;
        }
        else if (position.x < -6.7f)
        {
            position.x = 6.7f;
        }
        else if (position.y > 5.0f)
        {
            position.y = -5.0f;
        }
        else if (position.y < -5.0f)
        {
            position.y = 5.0f;
        }

        transform.position = position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        DestroyAsteroid();
    }

    void FixedUpdate()
    {
        //Move object
        rigidbody2d.position += lookDirection * speed * Time.deltaTime;
    }

    public virtual void DestroyAsteroid()
    {

    }

    protected GameObject getRandomMedAsteroid()
    {
        return medAsteroids[Random.Range(0, medAsteroids.Length)];
    }

    protected GameObject getRandomSmallAsteroid()
    {
        return smallAsteroids[Random.Range(0, smallAsteroids.Length)];
    }
}
