using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteCycle : MonoBehaviour
{
    // Start is called before the first frame update

    public SpriteRenderer[] srs;
    public Sprite[] sprites;
    int spriteCounter;
    int timer;

    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(timer < 3)
        {
            timer++;
            Debug.Log("Timer Go Up");
        }
        else
        {
            timer = 0;
            if(spriteCounter > sprites.Length)
            {
                Debug.Log("Counter to 0");
                spriteCounter = 0;
            }
            else
            {
                Debug.Log("Counter go up");
                spriteCounter++;
            }
            
            for (int i = 0; i < srs.Length; i++)
            {
                srs[i].sprite = sprites[spriteCounter];
            }
        }
    }
}
