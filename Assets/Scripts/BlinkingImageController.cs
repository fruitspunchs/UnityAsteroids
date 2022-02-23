using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Controller for blinking text
public class BlinkingImageController : MonoBehaviour
{
    bool visible = true;
    const float blinkDuration = 1f;
    float blinkTimer = blinkDuration;

    void Update()
    {
        //Show/Hide images every interval
        blinkTimer -= Time.deltaTime;

        if (blinkTimer <= 0)
        {
            if (visible)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.GetComponent<Image>().enabled = false;
                }

                visible = false;
            }
            else
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.GetComponent<Image>().enabled = true;
                }

                visible = true;
            }

            blinkTimer = blinkDuration;
        }
    }
}
