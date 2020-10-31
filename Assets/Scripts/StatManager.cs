using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatManager : MonoBehaviour
{

    // singleton isntance
    public static StatManager instance = null;

    public int biodiversity = 0;
    public int horniness = 0;
    public int atmosphereTemp = 0;
    public int domSub = 0;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    public void ModBiodiversity(int modifier)
    {
        biodiversity += modifier;
    }

    public void ModHorniness(int modifier)
    {
        horniness += modifier;
    }

    public void ModAtmosphereTemp(int modifier)
    {
        atmosphereTemp += modifier;
    }

    public void ModDomSub(int modifier)
    {
        domSub += modifier;
    }
}
