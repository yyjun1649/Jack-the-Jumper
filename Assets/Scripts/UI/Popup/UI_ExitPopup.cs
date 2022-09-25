using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ExitPopup : UI_Popup
{
    enum Buttons
    {
        YesButton,
        NoButton,
    }

    enum Texts
    {
        YesText,
        NoText,
    }

    CsStopController cs;
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindText(typeof(Texts));
        BindButton(typeof(Buttons));

        cs = FindObjectOfType<CsStopController>();
        GetButton((int)Buttons.YesButton).gameObject.BindEvent(OnClickYesButton);
        GetButton((int)Buttons.NoButton).gameObject.BindEvent(OnClickExitButton);

        Managers.UI.SetCanvas(gameObject, true);
        return true;
    }

    void OnClickExitButton()
    {
        Debug.Log("³ª°¡±â");
        Managers.Sound.Play(Define.Sound.Effect, "Sound_Button");

        Managers.UI.FindPopup<UI_MainPopup>().OnExitButton();
        Managers.Game.SaveGame();
        Managers.UI.ClosePopupUI(this); // UI_MainPopup
    }

    void OnClickYesButton()
    {
        GPGSBinder.Inst.SaveCloud("mysave");
        Managers.Sound.Play(Define.Sound.Effect, "Sound_Button");
        Managers.Game.SaveGame();
        Application.Quit();
    }
}
