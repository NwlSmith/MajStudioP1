using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogoSequenceScript : MonoBehaviour
{

    [SerializeField] private GameObject _tentacle = null;
    [SerializeField] private Light _spotlight1 = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator Sequence()
    {
        
        
        
        yield return null;
        SceneManager.LoadScene("IntroScene");
    }
}
