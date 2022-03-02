using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Main game controller
public class GameController : MonoBehaviour
{
    //Rectangle asteroid spawn areas
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
    public int startLives = 3;
    public const int MAX_LIVES = 15;

    public const int NEW_LIFE_SCORE_TRESHOLD = 10000;
    int newLifeScore = 0;

    bool respawnStart = false;
    bool waitingForNextRound = false;

    public int initialAsteroidCount = 4;
    int round = 0;

    GameObject startHud;
    GameObject gameHud;

    public GameObject explosionEffect;

    public GameObject bigUfo;
    public GameObject smallUfo;

    float spawnUfoTimer = 0.0f;
    public float spawnUfoInterval = 35.0f;

    enum GameState
    {
        StartScreen,
        PreGameScreen,
        GameScreen,
        PreGameOverScreen,
        InputHighScoresScreen,
        HighScoresScreen,
    }

    GameState gameState;

    void Start()
    {
        //Get reference to gamestate UI parents
        gameHud = GameObject.Find("GameHud");
        startHud = GameObject.Find("StartHud");

        //Set objects to choose from
        spawnAreas = new SpawnRect[] { spawnRight, spawnLeft, spawnTop, spawnBottom };
        asteroidTypes = new GameObject[] { largeAsteroid0, largeAsteroid1, largeAsteroid2 };

        ShowStartScreen();

        HighScores highScores = new HighScores();
    }

    void Update()
    {
        //Convert score to string for score display
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

        //Clear enemies from previous game
        ClearEnemies();

        //Reset game values
        score = 0;
        lives = startLives;
        newLifeScore = 0;
        round = 0;

        //Show StartHud and hide GameHud
        for (int i = 0; i < gameHud.transform.childCount; i++)
        {
            gameHud.transform.GetChild(i).gameObject.SetActive(false);
        }
        for (int i = 0; i < startHud.transform.childCount; i++)
        {
            startHud.transform.GetChild(i).gameObject.SetActive(true);
        }

        //Spawn some asteroids
        SpawnAsteroids(initialAsteroidCount);
    }

    void ShowPreGameScreen()
    {
        gameState = GameState.PreGameScreen;

        //Clear asteroids from start screen
        ClearEnemies();

        //Show GameHud and hide StartHud
        for (int i = 0; i < gameHud.transform.childCount; i++)
        {
            gameHud.transform.GetChild(i).gameObject.SetActive(true);
        }
        for (int i = 0; i < startHud.transform.childCount; i++)
        {
            startHud.transform.GetChild(i).gameObject.SetActive(false);
        }

        //Hide game over text
        GameObject gameOverText = GameObject.Find("GameOverText");
        SetTextVisibility(gameOverText, false);

        //Start game after delay
        StartCoroutine(ShowGameScreenAfterDelay(2));
    }

    //Show/Hide text
    void SetTextVisibility(GameObject gameObject, bool isVisible)
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            gameObject.transform.GetChild(i).gameObject.GetComponent<Image>().enabled = isVisible;
        }
    }

    void ShowGameScreen()
    {
        gameState = GameState.GameScreen;

        //Hide player 1 text
        GameObject player1Text = GameObject.Find("Player1Text");
        SetTextVisibility(player1Text, false);

        //Spawn player ship
        currentShip = Instantiate(playerShip, new Vector2(), Quaternion.Euler(0, 0, 90));

        //Start next round
        NextRound();
    }

    void CheckPressStart()
    {
        //Check for input on start screen then start game
        if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.Space))
        {
            ShowPreGameScreen();
        }
    }

    //Clear all enemies
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

    //Start next round
    void NextRound()
    {
        int asteroidCount = initialAsteroidCount + round;

        SpawnAsteroids(asteroidCount);

        round++;

        spawnUfoTimer = 0.0f;
    }

    //Spawn asteroids evenly distributed in designated areas
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

    //Add score and check if a new life is earned
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

    //Check if round is won and spawn ufos regularly if not
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
        else if (spawnUfoTimer >= spawnUfoInterval)
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

    //Check if player is dead and respawn or show game over
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
        else
        {
            gameState = GameState.PreGameOverScreen;
            GameObject gameOverText = GameObject.Find("GameOverText");
            SetTextVisibility(gameOverText, true);
            StartCoroutine(ShowStartScreenAfterDelay(3.0f));
        }
    }

    IEnumerator RespawnShipAfterDelay(float time)
    {
        yield return new WaitForSeconds(time);

        currentShip = Instantiate(playerShip, new Vector2(), Quaternion.Euler(0, 0, 90));
        respawnStart = false;
    }

    IEnumerator ShowGameScreenAfterDelay(float time)
    {
        yield return new WaitForSeconds(time);

        ShowGameScreen();
    }

    IEnumerator ShowStartScreenAfterDelay(float time)
    {
        yield return new WaitForSeconds(time);

        ShowStartScreen();
    }

    //Spawn a big or small ufo
    void SpawnUFO()
    {
        int random = Random.Range(0, 2);

        //Spawn left or rightof the screen
        float spawnX = random == 0 ? 6.6f : -6.6f;
        float spawnY = Random.Range(-4.9f, 4.9f);
        float xDirection = random == 0 ? -1.0f : 1.0f;

        //Randomly choose big or small ufo
        random = Random.Range(0, 2);
        GameObject ufoType = random == 0 ? smallUfo : bigUfo;

        //Set ufo move direction
        GameObject ufoInst = Instantiate(ufoType, new Vector2(spawnX, spawnY), Quaternion.identity);
        ufoInst.GetComponent<UFOController>().SetXDirection(xDirection);
    }

    //Play explosion effect
    public void playExplosion(Vector2 pos)
    {
        Instantiate(explosionEffect, pos, Quaternion.identity);
    }
}

//Class to define rectangle areas in coordinate space
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

    //Get random position in rectangle
    public Vector2 getRandomPosition()
    {
        float x = Random.Range(xMin, xMax);
        float y = Random.Range(yMin, yMax);

        return new Vector2(x, y);
    }
}

class HighScores
{
    public List<Score> scoreList = new List<Score>();

    public void Add(string name, int score)
    {
        Score newScore = new Score(name, score);
        scoreList.Add(newScore);

        scoreList.Sort((x, y) => y.score.CompareTo(x.score));

        int count = scoreList.Count;
        if (count > 10)
        {
            scoreList.RemoveRange(10, count - 10);
        }
    }

    public bool checkIfHighScore(int score)
    {
        foreach (Score i in scoreList)
        {
            if (score > i.score)
            {
                return true;
            }
        }

        return false;
    }
}

public class Score
{
    public string name;
    public int score;

    public Score(string name, int score)
    {
        this.name = name;
        this.score = score;
    }
}
