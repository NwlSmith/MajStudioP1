using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendTVAnim : MonoBehaviour
{
    public GameObject TVScreenPlane;
    private Material TVScreenMat;

    [SerializeField]
    private Texture[] TVSprites = null;

    private Texture[] TVSprites1 = new Texture[4];
    private Texture[] TVSprites2 = new Texture[4];
    private Texture[] TVSprites3 = new Texture[4];
    private Texture[] TVSprites4 = new Texture[4];
    private Texture[] TVSprites5 = new Texture[4];

    private Texture[] activeGroup = new Texture[4];
    private float groupTimer;
    private float groupTimerMax;

    private float spriteFlipTimer;
    private int spriteNum;

    void Start()
    {
        spriteNum = 0;
        groupTimerMax = Random.Range(2, 5);
        groupTimer = 0;

        //set all the sprites or whatever. idk why i didn't just make these ones public instead. im dumb
        TVSprites1[0] = TVSprites[0];
        TVSprites1[1] = TVSprites[1];
        TVSprites1[2] = TVSprites[2];
        TVSprites1[3] = TVSprites[3];

        TVSprites2[0] = TVSprites[4];
        TVSprites2[1] = TVSprites[5];
        TVSprites2[2] = TVSprites[6];
        TVSprites2[3] = TVSprites[7];

        TVSprites3[0] = TVSprites[8];
        TVSprites3[1] = TVSprites[9];
        TVSprites3[2] = TVSprites[10];
        TVSprites3[3] = TVSprites[11];

        TVSprites4[0] = TVSprites[12];
        TVSprites4[1] = TVSprites[13];
        TVSprites4[2] = TVSprites[14];
        TVSprites4[3] = TVSprites[15];

        TVSprites5[0] = TVSprites[16];
        TVSprites5[1] = TVSprites[17];
        TVSprites5[2] = TVSprites[18];
        TVSprites5[3] = TVSprites[19];

        PickSpriteGroup(Random.Range(0, 5));

    }

    void Update()
    {
        if(groupTimer < groupTimerMax)
        {
            groupTimer += Time.deltaTime;
        }
        else
        {
            groupTimerMax = Random.Range(2, 5);
            groupTimer = 0;
            PickSpriteGroup(Random.Range(0, 5));
        }

        if(spriteFlipTimer < .1)
        {
            spriteFlipTimer += Time.deltaTime;
        }
        else
        {
            spriteFlipTimer = 0;
            this.GetComponent<Renderer>().material.SetTexture("_BaseMap", activeGroup[spriteNum]);
            //print("spriteFlipTimer = " + spriteFlipTimer);
            if (spriteNum < 3)
            {
                spriteNum++;
            }
            else
            {
                spriteNum = 0;

            }
            
        }

    }

    void PickSpriteGroup(int groupNum)
    {
        if(groupNum == 0)
        {
            for (int i = 0; i < activeGroup.Length; i++)
            {
                activeGroup[i] = TVSprites1[i];
                //print("SOMETHINGS BEING SET");
            }
        }else if (groupNum == 1)
        {
            for (int i = 0; i < activeGroup.Length; i++)
            {
                activeGroup[i] = TVSprites2[i];
            }
        }
        else if (groupNum == 2)
        {
            for (int i = 0; i < activeGroup.Length; i++)
            {
                activeGroup[i] = TVSprites3[i];
            }
        }
        else if (groupNum == 3)
        {
            for (int i = 0; i < activeGroup.Length; i++)
            {
                activeGroup[i] = TVSprites4[i];
            }
        }
        else if (groupNum == 4)
        {
            for (int i = 0; i < activeGroup.Length; i++)
            {
                activeGroup[i] = TVSprites5[i];
            }
        }
    }

       
}
