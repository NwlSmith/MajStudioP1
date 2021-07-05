using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class EndGameManager : MonoBehaviour
{
    public SpriteRenderer sr;
    public Sprite[] Explosion;
    int explodeF;

    int timer;
    int timerLimit;
    public int explodeLimit;
    
    bool exploding;

    public GameObject earth;

    public ParticleSystem ps;

    public GameObject text;

    AudioSource aS;
    public AudioClip explosion;

    [SerializeField] private bool inVR = true;
    [SerializeField] private Camera mainCam;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        aS = GetComponent<AudioSource>();

        if (!inVR)
        {
            XRRig rig = FindObjectOfType<XRRig>();
            rig.gameObject.SetActive(false);
            
        }
        else
        {
            //FindObjectOfType<Canvas>().gameObject.SetActive(false);
            mainCam.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if(UnityEngine.InputSystem.Keyboard.current.rKey.wasPressedThisFrame)
        {
            ButtonPressed();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer++;

        if(timer == 100)
        {
            exploding = true;
            earth.SetActive(false);
            timerLimit = timer + explodeLimit;
            aS.clip = explosion;
            aS.Play();
            ps.Play();
        }

        if(exploding)
        {
            if(timer > timerLimit)
            {
                if (explodeF + 1f < Explosion.Length)
                {
                    timerLimit = timer + explodeLimit;
                    explodeF++;
                    sr.sprite = Explosion[explodeF];
                    float i = (25f - explodeF) / 25;
                    sr.color = new Color(1, 1, 1, i);
                    Debug.Log(i);
                }
                else
                {
                    exploding = false;
                }
            }
        }

        if(timer == 325)
        {
            text.SetActive(true);
        }
    }

    public void ButtonPressed()
    {
        SceneManager.LoadScene("Finn_Cockpit");
    }
}
