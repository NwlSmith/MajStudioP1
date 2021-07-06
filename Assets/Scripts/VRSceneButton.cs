using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRSceneButton : MonoBehaviour
{
    enum ButtonPurposeEnum { StartGame, RestartGame, ExitGame }

    [SerializeField] private ButtonPurposeEnum buttonPurpose = ButtonPurposeEnum.StartGame;
    private bool pressed = false;

    private void Start()
    {
        
        Debug.Log($"{name} starting");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!pressed)
        {
            Debug.Log($"Triggering with {other.name}");
            if (other.CompareTag("Hand"))
            {
                Rigidbody otherRB = other.GetComponent<Rigidbody>();
                float x = otherRB.velocity.x;
                float y = otherRB.velocity.y;
                float z = otherRB.velocity.z;
                Debug.Log($"Triggering with hand, x = {x}, y = {y}, z = {z}");
                if (y < 0 && Mathf.Abs(y) > Mathf.Abs(x) && Mathf.Abs(y) > Mathf.Abs(z))
                {
                    Debug.Log("Hit button");
                    Pressed();
                }
            }
        }
    }

    public void Pressed()
    {
        StartCoroutine(PressedEnum());
    }

    private IEnumerator PressedEnum()
    {
        yield return null;
        switch (buttonPurpose)
        {
            case ButtonPurposeEnum.StartGame:
                IntroManager introManager = FindObjectOfType<IntroManager>();
                introManager.PressStart();
                break;
            case ButtonPurposeEnum.RestartGame:
                EndGameManager endGameManager = FindObjectOfType<EndGameManager>();
                endGameManager.ButtonPressed();
                break;
            case ButtonPurposeEnum.ExitGame:
                ExitGameManager exitGameManager = FindObjectOfType<ExitGameManager>();
                exitGameManager.Pressed();
                break;
        }
    }
}
