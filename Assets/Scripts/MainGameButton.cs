using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameButton : PushableButton
{
    enum MainGameButtonPurposeEnum { Cloose1, Cloose2 }

    [SerializeField] private MainGameButtonPurposeEnum buttonPurpose = MainGameButtonPurposeEnum.Cloose1;


    protected override bool CheckCanPressButtons()
    {
        return base.CheckCanPressButtons() && CardManager.instance.canPressButtons;
    }
    public override void ButtonFullyPressed()
    {
        base.ButtonFullyPressed();
        switch (buttonPurpose)
        {
            case MainGameButtonPurposeEnum.Cloose1:
                CardManager.instance.Chose1();
                break;
            case MainGameButtonPurposeEnum.Cloose2:
                CardManager.instance.Chose2();
                break;
        }
    }
}
