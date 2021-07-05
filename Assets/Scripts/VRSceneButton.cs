using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRSceneButton : MonoBehaviour
{
    enum ButtonPurposeEnum { StartGame, RestartGame, ExitGame }

    [SerializeField] private ButtonPurposeEnum buttonPurpose = ButtonPurposeEnum.StartGame;
    public void Pressed()
    {
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
