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

    private IEnumerator Sequence()
    {
        
        
        
        yield return null;
    }

    public void NextScene()
    {
        SceneManager.LoadScene("IntroScene");
    }
}
