using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        aS = GetComponent<AudioSource>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("Finn_Cockpit");
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
}
