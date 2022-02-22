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

    bool respawnStart = false;
    bool waitingForNextRound = false;

    public int initialAsteroidCount = 4;
    int round = 0;

    GameObject startHud;
    GameObject gameHud;

    public GameObject explosionEffect;

    public GameObject ufo;
    float spawnUfoTimer = 0.0f;
    const float SPAWN_UFO_INTERVAL = 35.0f;

    enum GameState
    {
        StartScreen,
        PreGameScreen,
        GameScreen,
        GameOverScreen,
    }

    GameState gameState;

    // Start is called before the first frame update
    void Start()
    {
        gameHud = GameObject.Find("GameHud");
        startHud = GameObject.Find("StartHud");

        spawnAreas = new SpawnRect[] { spawnRight, spawnLeft, spawnTop, spawnBottom };
        asteroidTypes = new GameObject[] { largeAsteroid0, largeAsteroid1, largeAsteroid2 };

        ShowStartScreen();
    }

    // Update is called once per frame
    void Update()
    {
        if (score > 0) scoreString = score.ToString();
        else scoreString = "00";

        switch (gameState)
        {
            case GameState.StartScreen:
                CheckPressStart();
                break;
            case GameState.GameScreen:
                CheckDead();
                CheckWin();
                break;
        }
    }

    void ShowStartScreen()
    {
        gameState = GameState.StartScreen;

        for (int i = 0; i < gameHud.transform.childCount; i++)
        {
            gameHud.transform.GetChild(i).gameObject.SetActive(false);
        }
        for (int i = 0; i < startHud.transform.childCount; i++)
        {
            startHud.transform.GetChild(i).gameObject.SetActive(true);
        }

        SpawnAsteroids(initialAsteroidCount);
    }

    void ShowPreGameScreen()
    {
        gameState = GameState.PreGameScreen;

        ClearEnemies();

        for (int i = 0; i < gameHud.transform.childCount; i++)
        {
            gameHud.transform.GetChild(i).gameObject.SetActive(true);
        }
        for (int i = 0; i < startHud.transform.childCount; i++)
        {
            startHud.transform.GetChild(i).gameObject.SetActive(false);
        }

        StartCoroutine(ShowGameScreenAfterDelay(2));
    }

    void ShowGameScreen()
    {
        gameState = GameState.GameScreen;

        GameObject player1Text = GameObject.Find("Player1Text");
        player1Text.SetActive(false);

        currentShip = Instantiate(playerShip, new Vector2(), Quaternion.identity);
        NextRound();
    }

    void CheckPressStart()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.Space))
        {
            ShowPreGameScreen();
        }
    }

    void ClearEnemies()
    {
        GameObject[] gameObjects;
        gameObjects = GameObject.FindGameObjectsWithTag("Enemy");
        int count = gameObjects.Length;

        for (int i = 0; i < count; i++)
        {
            Destroy(gameObjects[i]);
        }
    }

    void NextRound()
    {
        int asteroidCount = initialAsteroidCount + round;

        SpawnAsteroids(asteroidCount);

        round++;

        spawnUfoTimer = 0.0f;
    }

    void SpawnAsteroids(int count)
    {
        int spawnAreaIndex = Random.Range(0, spawnAreas.Length);

        for (int i = 0; i < count; i++)
        {

            SpawnRect spawnArea = spawnAreas[spawnAreaIndex];
            GameObject asteroidType = asteroidTypes[Random.Range(0, asteroidTypes.Length)];

            Instantiate(asteroidType, spawnArea.getRandomPosition(), Quaternion.identity);

            spawnAreaIndex++;
            if (spawnAreaIndex >= spawnAreas.Length - 1) spawnAreaIndex = 0;
        }
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

    void CheckWin()
    {
        GameObject[] gameObjects;
        gameObjects = GameObject.FindGameObjectsWithTag("Enemy");
        int count = gameObjects.Length;

        spawnUfoTimer += Time.deltaTime;

        if (count <= 0 && lives > 0 && !waitingForNextRound)
        {
            waitingForNextRound = true;
            StartCoroutine(StartRoundAfterDelay(2));
        }
        else if (spawnUfoTimer >= SPAWN_UFO_INTERVAL)
        {
            SpawnUFO();
            spawnUfoTimer = 0;
        }
    }

    IEnumerator StartRoundAfterDelay(float time)
    {
        yield return new WaitForSeconds(time);

        NextRound();
        waitingForNextRound = false;
    }

    void CheckDead()
    {
        if (lives >= 1)
        {
            if (currentShip == null && !respawnStart)
            {
                respawnStart = true;
                StartCoroutine(RespawnShipAfterDelay(2));
            }
        }
    }

    IEnumerator RespawnShipAfterDelay(float time)
    {
        yield return new WaitForSeconds(time);

        currentShip = Instantiate(playerShip, new Vector2(), Quaternion.identity);
        respawnStart = false;
    }

    IEnumerator ShowGameScreenAfterDelay(float time)
    {
        yield return new WaitForSeconds(time);

        ShowGameScreen();
    }

    void SpawnUFO()
    {
        int random = Random.Range(0, 2);

        float spawnX = random == 0 ? 6.6f : -6.6f;
        float spawnY = Random.Range(-4.9f, 4.9f);
        float xDirection = random == 0 ? -1.0f : 1.0f;

        GameObject ufoInst = Instantiate(ufo, new Vector2(spawnX, spawnY), Quaternion.identity);
        ufoInst.GetComponent<UFOController>().SetXDirection(xDirection);
    }

    public void playExplosion(Vector2 pos)
    {
        Instantiate(explosionEffect, pos, Quaternion.identity);
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
