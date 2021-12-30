using System.Collections;
using TMPro;
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

    [SerializeField] private TextMeshPro horninessText = null;
    [SerializeField] private TextMeshPro atmosphereTempText = null;
    [SerializeField] private TextMeshPro domSubText = null;

    private bool flashHorniness = false;
    private bool flashAtmo = false;
    private bool flashDS = false;

    // things that change on the earth.
    [SerializeField] private GameObject earthModel = null;
    [SerializeField] private GameObject earthTintBlue1 = null;
    [SerializeField] private GameObject earthTintBlue2 = null;
    [SerializeField] private GameObject earthTintRed1 = null;
    [SerializeField] private GameObject earthTintRed2 = null;
    [SerializeField] private GameObject earthParticleHeart1 = null;
    [SerializeField] private GameObject earthParticleHeart2 = null;
    [SerializeField] private GameObject earthParticleExplosion1 = null;
    [SerializeField] private GameObject earthParticleExplosion2 = null;

    [SerializeField] private Card GameOverHornyHigh = null;
    [SerializeField] private Card GameOverHornyLow = null;
    [SerializeField] private Card GameOverTempHigh = null;
    [SerializeField] private Card GameOverTempLow = null;
    [SerializeField] private Card GameOverDSHigh = null;
    [SerializeField] private Card GameOverDSLow = null;
    private int _minThreshold = 0;
    private int _lowLowThreshold = 5;
    private int _lowThreshold = 10;
    private int _highThreshold = 20;
    private int _highHighThreshold = 25;
    private int _maxThreshold = 30;
    private Rotate _earthRotate;
    private Shake _earthShake;
    private ParticleSystem _earthHeartParticleSystem1;
    private ParticleSystem _earthHeartParticleSystem2;
    private ParticleSystem _earthExplosionParticleSystem1;
    private ParticleSystem _earthExplosionParticleSystem2;


    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    private void Start()
    {
        _earthExplosionParticleSystem2 = earthParticleExplosion2.GetComponent<ParticleSystem>();
        _earthExplosionParticleSystem1 = earthParticleExplosion1.GetComponent<ParticleSystem>();
        _earthHeartParticleSystem2 = earthParticleHeart2.GetComponent<ParticleSystem>();
        _earthHeartParticleSystem1 = earthParticleHeart1.GetComponent<ParticleSystem>();
        _earthShake = earthModel.GetComponent<Shake>();
        _earthRotate = earthModel.GetComponent<Rotate>();
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
        if (cur <= _minThreshold)
        {
            // game over
            DeckManager.instance.AddCardFront(GameOverHornyLow);
        }
        // went below 5
        else if (prev > _lowLowThreshold && cur <= _lowLowThreshold)
        {
            flashHorniness = true;
            _earthRotate.rotPerFrame = new Vector3(0, 0.005f, 0);
        }
        // went back above 5
        else if (prev <= _lowLowThreshold && cur > _lowLowThreshold)
        {
            flashHorniness = false;
            _earthRotate.rotPerFrame = new Vector3(0, 0.0075f, 0);
        }

        // back in acceptable range
        if ((prev <= _lowThreshold && cur > _lowThreshold) || (prev >= _highThreshold && cur < _highThreshold))
        {
            flashHorniness = false;
            //change color to normal
            horninessText.color = Color.yellow;
            _earthRotate.rotPerFrame = new Vector3(0, 0.01f, 0);
            _earthShake.shakeModifier = 0;
        }
        // went below 10
        else if (prev > _lowThreshold && cur <= _lowThreshold)
        {
            //change color
            horninessText.color = Color.red;
            _earthRotate.rotPerFrame = new Vector3(0, 0.0075f, 0);
        }
        // went above 30
        else if (prev < _highThreshold && cur >= _highThreshold)
        {
            //change color
            horninessText.color = Color.red;
            _earthShake.shakeModifier = 1;
        }

        if (cur >= _maxThreshold)
        {
            // game over
            DeckManager.instance.AddCardFront(GameOverHornyHigh);
        }
        // went above 25
        else if (prev < _highHighThreshold && cur >= _highHighThreshold)
        {
            flashHorniness = true;
            horninessText.color = Color.red;
            _earthShake.shakeModifier = 2;
        }
        // went back below 25
        else if (prev >= _highHighThreshold && cur < _highHighThreshold)
        {
            flashHorniness = false;
            _earthShake.shakeModifier = 1;
        }

        // Update Stat text
        horninessText.text = horniness.ToString();
    }

    public void ModAtmosphereTemp(int modifier)
    {
        int prev = atmosphereTemp;
        atmosphereTemp += modifier;
        int cur = atmosphereTemp;
        if (cur <= _minThreshold)
        {
            // game over
            DeckManager.instance.AddCardFront(GameOverTempLow);
        }
        // went below 5
        else if (prev > _lowLowThreshold && cur <= _lowLowThreshold)
        {
            flashAtmo = true;
            earthTintBlue1.SetActive(true);
            earthTintBlue2.SetActive(true);
        }
        // went back above 5
        else if (prev <= _lowLowThreshold && cur > _lowLowThreshold)
        {
            flashAtmo = false;
            earthTintBlue1.SetActive(true);
            earthTintBlue2.SetActive(false);
        }

        // back in acceptable range
        if ((prev <= _lowThreshold && cur > _lowThreshold) || (prev >= _highThreshold && cur < _highThreshold))
        {
            flashAtmo = false;
            //change color to normal
            atmosphereTempText.color = Color.yellow;
            earthTintBlue1.SetActive(false);
            earthTintBlue2.SetActive(false);
            earthTintRed1.SetActive(false);
            earthTintRed2.SetActive(false);
        }
        // went below 10
        else if (prev > _lowThreshold && cur <= _lowThreshold)
        {
            //change color
            atmosphereTempText.color = Color.cyan;
            earthTintBlue1.SetActive(true);
            earthTintBlue2.SetActive(false);
        }
        // went above 20
        else if (prev < _highThreshold && cur >= _highThreshold)
        {
            //change color
            atmosphereTempText.color = Color.red;
            earthTintRed1.SetActive(true);
            earthTintRed2.SetActive(false);
        }

        if (cur >= _maxThreshold)
        {
            // game over
            DeckManager.instance.AddCardFront(GameOverTempHigh);
        }
        // went above 25
        else if (prev < _highHighThreshold && cur >= _highHighThreshold)
        {
            flashAtmo = true;
            earthTintRed1.SetActive(true);
            earthTintRed2.SetActive(true);
        }
        // went back below 25
        else if (prev >= _highHighThreshold && cur < _highHighThreshold)
        {
            flashAtmo = false;
            earthTintRed1.SetActive(true);
            earthTintRed2.SetActive(false);
        }

        // Update Stat text
        atmosphereTempText.text = atmosphereTemp.ToString();
    }

    public void ModDomSub(int modifier)
    {
        int prev = domSub;
        domSub += modifier;
        int cur = domSub;
        if (cur <= _minThreshold)
        {
            // game over
            DeckManager.instance.AddCardFront(GameOverDSLow);
        }
        // went below 5
        else if (prev > _lowLowThreshold && cur <= _lowLowThreshold)
        {
            flashDS = true;
            _earthHeartParticleSystem1.Play();
            _earthHeartParticleSystem2.Play();
        }
        // went back above 5
        else if (prev <= _lowLowThreshold && cur > _lowLowThreshold)
        {
            flashDS = false;
            _earthHeartParticleSystem1.Play();
            _earthHeartParticleSystem2.Stop();
        }

        // back in acceptable range
        if ((prev <= _lowThreshold && cur > _lowThreshold) || (prev >= _highThreshold && cur < _highThreshold))
        {
            flashDS = false;
            //change color to normal
            domSubText.color = Color.yellow;
            _earthHeartParticleSystem1.Stop();
            _earthHeartParticleSystem2.Stop();
            _earthExplosionParticleSystem1.Stop();
            _earthExplosionParticleSystem2.Stop();
        }
        // went below 10
        else if (prev > _lowThreshold && cur <= _lowThreshold)
        {
            //change color
            domSubText.color = Color.red;
            _earthHeartParticleSystem1.Play();
            _earthHeartParticleSystem2.Stop();
        }
        // went above 20
        else if (prev < _highThreshold && cur >= _highThreshold)
        {
            //change color
            domSubText.color = Color.red;
            _earthExplosionParticleSystem1.Play();
            _earthExplosionParticleSystem2.Stop();
        }

        if (cur >= _maxThreshold)
        {
            // game over
            DeckManager.instance.AddCardFront(GameOverDSHigh);
        }
        // went above 25
        else if (prev < _highHighThreshold && cur >= _highHighThreshold)
        {
            flashDS = true;
            _earthExplosionParticleSystem1.Play();
            _earthExplosionParticleSystem2.Play();
        }
        // went back below 25
        else if (prev >= _highHighThreshold && cur < _highHighThreshold)
        {
            flashDS = false;
            _earthExplosionParticleSystem1.Play();
            _earthExplosionParticleSystem2.Stop();
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
