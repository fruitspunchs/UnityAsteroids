using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    SpawnRect spawnRight = new SpawnRect(5.0f, 6.0f, -5.0f, 5.0f);
    SpawnRect spawnLeft = new SpawnRect(-6.0f, -5.0f, -5.0f, 5.0f);
    SpawnRect spawnTop = new SpawnRect(-6.0f, 6.0f, 4.0f, 5.0f);
    SpawnRect spawnBottom = new SpawnRect(-6.0f, 6.0f, -5.0f, -4.0f);

    public GameObject largeAsteroid0;
    public GameObject largeAsteroid1;
    public GameObject largeAsteroid2;

    public GameObject playerShip;
    GameObject currentShip;

    public int score = 0;
    public string scoreString = "";

    float respawnTimer = 2f;
    float respawnDuration = 2f;
    bool respawnStart = false;

    // Start is called before the first frame update
    void Start()
    {
        SpawnRect[] spawnAreas = { spawnRight, spawnLeft, spawnTop, spawnBottom };
        GameObject[] asteroidTypes = { largeAsteroid0, largeAsteroid1, largeAsteroid2 };

        int spawnAreaIndex = Random.Range(0, spawnAreas.Length);

        for (int i = 0; i < 5; i++)
        {
            SpawnRect spawnArea = spawnAreas[spawnAreaIndex];
            GameObject asteroidType = asteroidTypes[Random.Range(0, asteroidTypes.Length)];

            Instantiate(asteroidType, spawnArea.getRandomPosition(), Quaternion.identity);

            spawnAreaIndex++;
            if (spawnAreaIndex >= spawnAreas.Length - 1) spawnAreaIndex = 0;
        }

        currentShip = Instantiate(playerShip, new Vector2(), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        respawnTimer -= Time.deltaTime;

        if (currentShip == null && !respawnStart)
        {
            respawnTimer = respawnDuration;
            respawnStart = true;
        }
        else if (currentShip == null && respawnTimer < 0)
        {
            currentShip = Instantiate(playerShip, new Vector2(), Quaternion.identity);
            respawnStart = false;
        }

        score += 1;
        scoreString = score.ToString();
    }
}

class SpawnRect
{
    private float xMin;
    private float xMax;
    private float yMin;
    private float yMax;

    public SpawnRect(float xMin, float xMax, float yMin, float yMax)
    {
        this.xMin = xMin;
        this.xMax = xMax;
        this.yMin = yMin;
        this.yMax = yMax;
    }

    public Vector2 getRandomPosition()
    {
        float x = Random.Range(xMin, xMax);
        float y = Random.Range(yMin, yMax);

        return new Vector2(x, y);
    }
}
