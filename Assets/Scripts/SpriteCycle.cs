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
        if(timer < 9)
        {
            timer++;
        }
        else
        {
            timer = 0;
            if(spriteCounter + 2 > sprites.Length)
            {
                spriteCounter = 0;
            }
            else
            {
                spriteCounter++;
            }
            
            for (int i = 0; i < srs.Length; i++)
            {
                srs[i].sprite = sprites[spriteCounter];
            }
        }
    }
}
