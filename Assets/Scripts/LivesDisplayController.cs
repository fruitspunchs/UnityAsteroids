using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivesDisplayController : MonoBehaviour
{
    public int lifeCount = 1;
    GameController gameController;

    // Start is called before the first frame update
    void Start()
    {
        GameObject game = GameObject.Find("Game");
        gameController = game.GetComponent<GameController>();
        GetComponent<Image>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameController.lives >= lifeCount)
        {
            GetComponent<Image>().enabled = true;
        }
        else
        {
            GetComponent<Image>().enabled = false;
        }
    }
}
