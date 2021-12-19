using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class VRSceneButton : PushableButton
{
    enum ButtonPurposeEnum { StartGame, RestartLose, RestartWin, ExitGame }

    [SerializeField] private ButtonPurposeEnum buttonPurpose = ButtonPurposeEnum.StartGame;

    public override void ButtonFullyPressed()
    {
        base.ButtonFullyPressed();
        switch (buttonPurpose)
        {
            case ButtonPurposeEnum.StartGame:
                IntroManager introManager = FindObjectOfType<IntroManager>();
                introManager.PressStart();
                break;
            case ButtonPurposeEnum.RestartLose:
                EndGameManager endGameManager = FindObjectOfType<EndGameManager>();
                endGameManager.ButtonPressed();
                break;
            case ButtonPurposeEnum.RestartWin:
                WinGameManager winGameManager = FindObjectOfType<WinGameManager>();
                winGameManager.ButtonPressed();
                break;
            case ButtonPurposeEnum.ExitGame:
                ExitGameManager exitGameManager = FindObjectOfType<ExitGameManager>();
                exitGameManager.Pressed();
                break;
        }

        pushable = false;
    }
}
