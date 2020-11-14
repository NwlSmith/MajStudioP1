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

    //  0 = too low, game over
    //  5 = getting very low, text flashing
    // 10 = getting low, warning, text red
    // 20 = perfectly normal :)
    // 30 = getting high, warning, text flashing
    // 35 = getting very high, text flashing
    // 40 = too high, game over
    [SerializeField]
    private int[] thresholds = { 0, 5, 10, 20, 30, 35, 40 };

    [SerializeField] private TextMesh horninessText = null;
    [SerializeField] private TextMesh atmosphereTempText = null;
    [SerializeField] private TextMesh domSubText = null;

    private bool flashHorniness = false;
    private bool flashAtmo = false;
    private bool flashDS = false;

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

        StartCoroutine(FlashHorniness());
        StartCoroutine(FlashAtmo());
        StartCoroutine(FlashDS());
    }

    public void ModBiodiversity(int modifier)
    {
        biodiversity += modifier;
        // Update Stat
    }

    public void ModHorniness(int modifier)
    {
        int prev = horniness;
        horniness += modifier;
        int cur = horniness;
        if (cur <= 0)
        {
            // game over
        }
        // went below 5
        else if (prev > 5 && cur <= 5)
        {
            flashHorniness = true;
        }
        // went back above 5
        else if (prev <= 5 && cur > 5)
        {
            flashHorniness = false;
        }

        // back in acceptable range
        if ((prev <= 10 && cur > 10) || (prev >= 30 && cur < 30))
        {
            flashHorniness = false;
            //change color to normal
            horninessText.color = Color.yellow;
        }
        // went below 10
        else if (prev > 10 && cur <= 10)
        {
            //change color
            horninessText.color = Color.red;
        }
        // went above 30
        else if (prev < 30 && cur >= 30)
        {
            //change color
            horninessText.color = Color.red;
        }

        if (cur >= 40)
        {
            // game over
        }
        // went above 35
        else if (prev < 35 && cur >= 35)
        {
            flashHorniness = true;
        }
        // went back below 35
        else if (prev >= 35 && cur < 35)
        {
            flashHorniness = false;
        }

        // Update Stat text
        horninessText.text = horniness.ToString();
    }

    public void ModAtmosphereTemp(int modifier)
    {
        int prev = atmosphereTemp;
        atmosphereTemp += modifier;
        int cur = atmosphereTemp;
        if (cur <= 0)
        {
            // game over
        }
        // went below 5
        else if (prev > 5 && cur <= 5)
        {
            flashAtmo = true;
        }
        // went back above 5
        else if (prev <= 5 && cur > 5)
        {
            flashAtmo = false;
        }

        // back in acceptable range
        if ((prev <= 10 && cur > 10) || (prev >= 30 && cur < 30))
        {
            flashAtmo = false;
            //change color to normal
            atmosphereTempText.color = Color.yellow;
        }
        // went below 10
        else if (prev > 10 && cur <= 10)
        {
            //change color
            atmosphereTempText.color = Color.cyan;
        }
        // went above 30
        else if (prev < 30 && cur >= 30)
        {
            //change color
            atmosphereTempText.color = Color.red;
        }

        if (cur >= 40)
        {
            // game over
        }
        // went above 35
        else if (prev < 35 && cur >= 35)
        {
            flashAtmo = true;
        }
        // went back below 35
        else if (prev >= 35 && cur < 35)
        {
            flashAtmo = false;
        }

        // Update Stat text
        atmosphereTempText.text = atmosphereTemp.ToString();
    }

    public void ModDomSub(int modifier)
    {
        int prev = domSub;
        domSub += modifier;
        int cur = domSub;
        if (cur <= 0)
        {
            // game over
        }
        // went below 5
        else if (prev > 5 && cur <= 5)
        {
            flashDS = true;
        }
        // went back above 5
        else if (prev <= 5 && cur > 5)
        {
            flashDS = false;
        }

        // back in acceptable range
        if ((prev <= 10 && cur > 10) || (prev >= 30 && cur < 30))
        {
            flashDS = false;
            //change color to normal
            domSubText.color = Color.yellow;
        }
        // went below 10
        else if (prev > 10 && cur <= 10)
        {
            //change color
            domSubText.color = Color.red;
        }
        // went above 30
        else if (prev < 30 && cur >= 30)
        {
            //change color
            domSubText.color = Color.red;
        }

        if (cur >= 40)
        {
            // game over
        }
        // went above 35
        else if (prev < 35 && cur >= 35)
        {
            flashDS = true;
        }
        // went back below 35
        else if (prev >= 35 && cur < 35)
        {
            flashDS = false;
        }

        // Update Stat text
        domSubText.text = domSub.ToString();
    }

    private IEnumerator FlashHorniness()
    {
        WaitForSeconds wait = new WaitForSeconds(.25f);
        bool on = true;
        while (true)
        {
            if (flashHorniness && on)
            {
                horninessText.color = new Color(horninessText.color.r, horninessText.color.g, horninessText.color.b, 0);
                on = false;
            }
            else if (!on)
            {
                horninessText.color = new Color(horninessText.color.r, horninessText.color.g, horninessText.color.b, 1);
                on = true;
            }
            yield return wait;
        }
    }

    private IEnumerator FlashAtmo()
    {
        WaitForSeconds wait = new WaitForSeconds(.25f);
        bool on = true;
        while (true)
        {
            if (flashAtmo && on)
            {
                atmosphereTempText.color = new Color(atmosphereTempText.color.r, atmosphereTempText.color.g, atmosphereTempText.color.b, 0);
                on = false;
            }
            else if (!on)
            {
                atmosphereTempText.color = new Color(atmosphereTempText.color.r, atmosphereTempText.color.g, atmosphereTempText.color.b, 1);
                on = true;
            }
            yield return wait;
        }
    }

    private IEnumerator FlashDS()
    {
        WaitForSeconds wait = new WaitForSeconds(.25f);
        bool on = true;
        while (true)
        {
            if (flashDS && on)
            {
                domSubText.color = new Color(domSubText.color.r, domSubText.color.g, domSubText.color.b, 0);
                on = false;
            }
            else if (!on)
            {
                domSubText.color = new Color(domSubText.color.r, domSubText.color.g, domSubText.color.b, 1);
                on = true;
            }
            yield return wait;
        }
    }
}
