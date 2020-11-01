using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoommateManager : MonoBehaviour
{

    public static RoommateManager instance = null;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    // UNIMPLEMENTED
    public void AddResponse(string response)
    {
        // IMPLEMENT
    }
}
