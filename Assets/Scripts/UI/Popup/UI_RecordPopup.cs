using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_RecordPopup : UI_Popup
{
    enum Buttons
    {
        BackButton,
    }

    enum Texts
    {
        FirstRecordText,
        SecondRecordText,
        ThirdRecordText,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        GetButton((int)Buttons.BackButton).gameObject.BindEvent(OnClickExitButton);

        if (Managers.Game.FirstScore > 0)
        {
            GetText((int)Texts.FirstRecordText).text =  Managers.Game.FirstScore.ToString() + "M";
            if(Managers.Game.SecondScore > 0)
            {
                GetText((int)Texts.SecondRecordText).text = Managers.Game.SecondScore.ToString() + "M";
                if(Managers.Game.ThirdScore > 0)
                    GetText((int)Texts.ThirdRecordText).text = Managers.Game.ThirdScore.ToString() + "M";
            }
        }
        else
            GetText((int)Texts.SecondRecordText).text = "기록없음";


        Managers.UI.SetCanvas(gameObject, true);
        return true;
    }


    void OnClickExitButton()
    {
        Managers.Sound.Play(Define.Sound.Effect, "Sound_Button");
        Debug.Log("나가기");
        

        Managers.UI.ClosePopupUI(this); // UI_MainPopup
    }
}

