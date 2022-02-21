using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    SpawnRect spawnRight = new SpawnRect(5.0f, 6.0f, -5.0f, 5.0f);
    SpawnRect spawnLeft = new SpawnRect(-6.0f, -5.0f, -5.0f, 5.0f);
    SpawnRect spawnTop = new SpawnRect(-6.0f, 6.0f, 4.0f, 5.0f);
    SpawnRect spawnBottom = new SpawnRect(-6.0f, 6.0f, -5.0f, -4.0f);

    SpawnRect[] spawnAreas;
    GameObject[] asteroidTypes;

    public GameObject largeAsteroid0;
    public GameObject largeAsteroid1;
    public GameObject largeAsteroid2;

    public GameObject playerShip;
    GameObject currentShip;

    public int score = 0;
    public string scoreString = "";

    public int lives = 3;
    public const int MAX_LIVES = 5;

    public const int NEW_LIFE_SCORE_TRESHOLD = 10000;
    int newLifeScore = 0;

    float respawnTimer = 2f;
    float respawnDuration = 2f;
    bool respawnStart = false;
    bool waitingForNextRound = false;

    public int initialAsteroidCount = 4;
    int round = 0;

    // Start is called before the first frame update
    void Start()
    {
        spawnAreas = new SpawnRect[] { spawnRight, spawnLeft, spawnTop, spawnBottom };
        asteroidTypes = new GameObject[] { largeAsteroid0, largeAsteroid1, largeAsteroid2 };

        currentShip = Instantiate(playerShip, new Vector2(), Quaternion.identity);

        NextRound();
    }

    void NextRound()
    {
        int asteroidCount = initialAsteroidCount + round;
        int spawnAreaIndex = Random.Range(0, spawnAreas.Length);

        for (int i = 0; i < asteroidCount; i++)
        {

            SpawnRect spawnArea = spawnAreas[spawnAreaIndex];
            GameObject asteroidType = asteroidTypes[Random.Range(0, asteroidTypes.Length)];

            Instantiate(asteroidType, spawnArea.getRandomPosition(), Quaternion.identity);

            spawnAreaIndex++;
            if (spawnAreaIndex >= spawnAreas.Length - 1) spawnAreaIndex = 0;
        }

        round++;
    }

    // Update is called once per frame
    void Update()
    {
        respawnTimer -= Time.deltaTime;

        if (lives >= 1)
        {
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
        }

        if (score > 0) scoreString = score.ToString();
        else scoreString = "00";

        CheckWin();
    }

    public void AddScore(int score)
    {
        this.score += score;

        newLifeScore += score;
        if (newLifeScore >= NEW_LIFE_SCORE_TRESHOLD)
        {
            lives++;
            newLifeScore -= NEW_LIFE_SCORE_TRESHOLD;
        }

        if (lives > MAX_LIVES) lives = MAX_LIVES;
    }

    public void CheckWin()
    {
        GameObject[] gameObjects;
        gameObjects = GameObject.FindGameObjectsWithTag("Enemy");
        int count = gameObjects.Length;

        if (count <= 0 && lives > 0 && !waitingForNextRound)
        {
            waitingForNextRound = true;
            StartCoroutine(startRoundAfterDelay(2));
        }
    }

    IEnumerator startRoundAfterDelay(float time)
    {
        yield return new WaitForSeconds(time);

        NextRound();
        waitingForNextRound = false;
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
