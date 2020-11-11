using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TVManager : MonoBehaviour
{
    // singleton instance
    public static TVManager instance = null;

    [SerializeField] private Sprite curSprite;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    public void NewImage(Sprite newSprite)
    {
        curSprite = newSprite;
    }
}
