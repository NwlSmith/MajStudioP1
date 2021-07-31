using UnityEngine;
using UnityEngine.SceneManagement;

public class LogoSequenceScript : MonoBehaviour
{

    [SerializeField] private GameObject _tentacle = null;
    [SerializeField] private Light _spotlight1 = null;
    

    private void Update()
    {
        if(UnityEngine.InputSystem.Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene("IntroScene");
        }
    }

    public void NextScene()
    {
        SceneManager.LoadScene("IntroScene");
    }
}
