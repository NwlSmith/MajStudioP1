using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LogoSequenceScript : MonoBehaviour
{

    [SerializeField] private GameObject _tentacle = null;
    [SerializeField] private Light _spotlight1 = null;

    private bool _loadingScene = false;
    [SerializeField] private Image _image = null;
    

    private void Update()
    {
        if(UnityEngine.InputSystem.Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene("IntroScene");
            LoadScene();
        }
    }

    public void NextScene()
    {
        LoadScene();
    }

    private void LoadScene()
    {
        if (_loadingScene) return;
        
        StartCoroutine(LoadSceneEnum());
    }

    private IEnumerator LoadSceneEnum()
    {
        _loadingScene = true;

        float duration = 2f;
        if (MiscInput.instance.primaryButtonClicked)
        {
            duration = .2f;
        }

        float elapsedTime = 0f;
        Color init = new Color(0, 0, 0, 0);
        Color target = new Color(0, 0, 0, 1);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            _image.color = Color.Lerp(init, target, elapsedTime / duration);
            yield return null;
        }

        _image.color = target;
        
        AsyncOperation loading = SceneManager.LoadSceneAsync("IntroScene");

        DontDestroyOnLoad(this);

        while (!loading.isDone)
        {
            yield return null;
        }

        yield return new WaitForSeconds(.1f);
        
        Destroy(gameObject);
    }
}