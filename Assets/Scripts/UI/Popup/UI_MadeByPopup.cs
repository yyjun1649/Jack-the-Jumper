using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_MadeByPopup : UI_Popup
{
    enum Buttons
    {
        BackButton,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindButton(typeof(Buttons));

        GetButton((int)Buttons.BackButton).gameObject.BindEvent(OnClickExitButton);

        Managers.UI.SetCanvas(gameObject, true);
        return true;
    }


    void OnClickExitButton()
    {
        Managers.Sound.Play(Define.Sound.Effect, "Sound_Button");
        Debug.Log("³ª°¡±â");
        

        Managers.UI.ClosePopupUI(this); // UI_MainPopup
    }
}

