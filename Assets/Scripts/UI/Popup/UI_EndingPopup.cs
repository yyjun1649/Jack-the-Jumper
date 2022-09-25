using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_EndingPopup : UI_Popup
{
    enum Buttons
    {
        BackButton,
    }

    enum Texts
    {
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindText(typeof(Texts));
        BindButton(typeof(Buttons));

        GetButton((int)Buttons.BackButton).gameObject.BindEvent(OnClickBackButton);

        Managers.UI.SetCanvas(gameObject, true);
        return true;
    }

    void OnClickBackButton()
    {
        Managers.Sound.Play(Define.Sound.Effect, "Sound_Button");

        Managers.Game.SaveGame();
        Managers.UI.ClosePopupUI(this); // UI_MainPopup
    }
}
