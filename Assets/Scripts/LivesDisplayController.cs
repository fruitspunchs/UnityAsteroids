using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Displays one life of total lives
public class LivesDisplayController : MonoBehaviour
{
    public int lifeCount = 1;
    GameController gameController;

    void Start()
    {
        GameObject game = GameObject.Find("Game");
        gameController = game.GetComponent<GameController>();
        GetComponent<Image>().enabled = false;
    }

    void Update()
    {
        //Show self depending on number of lives
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
