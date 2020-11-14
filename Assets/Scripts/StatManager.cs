using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatManager : MonoBehaviour
{

    // singleton isntance
    public static StatManager instance = null;

    public int biodiversity = 0;
    public int horniness = 20;
    public int atmosphereTemp = 20;
    public int domSub = 20;

    [SerializeField] private TextMesh horninessText = null;
    [SerializeField] private TextMesh atmosphereTempText = null;
    [SerializeField] private TextMesh domSubText = null;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    private void Start()
    {
        horninessText.text = horniness.ToString();
        atmosphereTempText.text = atmosphereTemp.ToString();
        domSubText.text = domSub.ToString();

    }

    public void ModBiodiversity(int modifier)
    {
        biodiversity += modifier;
        // Update Stat
    }

    public void ModHorniness(int modifier)
    {
        horniness += modifier;
        // Update Stat
        horninessText.text = horniness.ToString();
    }

    public void ModAtmosphereTemp(int modifier)
    {
        atmosphereTemp += modifier;
        // Update Stat
        atmosphereTempText.text = atmosphereTemp.ToString();
    }

    public void ModDomSub(int modifier)
    {
        domSub += modifier;
        // Update Stat
        domSubText.text = domSub.ToString();
    }
}
