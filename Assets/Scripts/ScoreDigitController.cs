using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDigitController : MonoBehaviour
{
    public int index = 0;
    GameController gameController;

    public Sprite digit0;
    public Sprite digit1;
    public Sprite digit2;
    public Sprite digit3;
    public Sprite digit4;
    public Sprite digit5;
    public Sprite digit6;
    public Sprite digit7;
    public Sprite digit8;
    public Sprite digit9;
    Sprite[] digitSprites;
    // Start is called before the first frame update
    void Start()
    {
        GameObject game = GameObject.Find("Game");
        gameController = game.GetComponent<GameController>();
        digitSprites = new Sprite[] { digit0, digit1, digit2, digit3, digit4, digit5, digit6, digit7, digit8, digit9 };
    }

    // Update is called once per frame
    void Update()
    {
        int flipIndex = gameController.scoreString.Length - 1 - index;
        if (flipIndex >= 0)
        {
            int digit = (int)Char.GetNumericValue(gameController.scoreString[flipIndex]);
            GetComponent<Image>().enabled = true;
            GetComponent<Image>().sprite = digitSprites[digit];
        }
        else
        {
            GetComponent<Image>().enabled = false;
        }
    }
}
