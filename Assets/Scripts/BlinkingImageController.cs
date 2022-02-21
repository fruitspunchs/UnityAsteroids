using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkingImageController : MonoBehaviour
{

    bool visible = true;
    const float blinkDuration = 1f;
    float blinkTimer = blinkDuration;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
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
