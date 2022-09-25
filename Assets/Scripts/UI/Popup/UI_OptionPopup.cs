using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_OptionPopup : UI_Popup
{
    enum Buttons
    {
        BackButton,
        BgmButton,
        SFXButton,
        VibrationButton,
        EtcButton
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

        GetButton((int)Buttons.BackButton).gameObject.BindEvent(OnClickExitButton);
        GetButton((int)Buttons.BgmButton).gameObject.BindEvent(OnClickBgmButton);
        GetButton((int)Buttons.SFXButton).gameObject.BindEvent(OnClickSFXButton);
        GetButton((int)Buttons.VibrationButton).gameObject.BindEvent(OnClickVibButton);
        GetButton((int)Buttons.EtcButton).gameObject.BindEvent(OnClickEtcButton);

        Managers.UI.SetCanvas(gameObject, true);
        RefreshUI();
        return true;
    }

    void RefreshUI()
    {
        if (_init == false)
            return;

        if (!Managers.Game.BGM)
            GetButton((int)Buttons.BgmButton).image.sprite = Managers.Resource.Load<Sprite>("Sprites/Main/Common/Button");

        if (!Managers.Game.SFX)
            GetButton((int)Buttons.SFXButton).image.sprite = Managers.Resource.Load<Sprite>("Sprites/Main/Common/Button");

        if (!Managers.Game.Vibration)
            GetButton((int)Buttons.VibrationButton).image.sprite = Managers.Resource.Load<Sprite>("Sprites/Main/Common/Button");
    }

    void OnClickBgmButton()
    {
        Managers.Sound.Play(Define.Sound.Effect, "Sound_Button");
        Managers.Game.BGM = !Managers.Game.BGM;
        if (!Managers.Game.BGM)
        {
            GetButton((int)Buttons.BgmButton).image.sprite = Managers.Resource.Load<Sprite>("Sprites/Main/Common/Button");
            Managers.Sound.SetVolume(Define.Sound.Bgm, 0f);
        }
        else
        {
            GetButton((int)Buttons.BgmButton).image.sprite = Managers.Resource.Load<Sprite>("Sprites/Main/Common/OnButton");
            Managers.Sound.SetVolume(Define.Sound.Bgm, 0.5f);
        }
        Managers.Game.SaveGame();

    }
    void OnClickSFXButton()
    {
        Managers.Sound.Play(Define.Sound.Effect, "Sound_Button");
        Managers.Game.SFX = !Managers.Game.SFX;
        if (!Managers.Game.SFX)
        {
            GetButton((int)Buttons.SFXButton).image.sprite = Managers.Resource.Load<Sprite>("Sprites/Main/Common/Button");
            Managers.Sound.SetVolume(Define.Sound.Effect, 0f);
        }
        else
        {
            GetButton((int)Buttons.SFXButton).image.sprite = Managers.Resource.Load<Sprite>("Sprites/Main/Common/OnButton");
            Managers.Sound.SetVolume(Define.Sound.Effect, 1f);
        }
        Managers.Game.SaveGame();
    }
    void OnClickVibButton()
    {
        Managers.Sound.Play(Define.Sound.Effect, "Sound_Button");
        Managers.Game.Vibration = !Managers.Game.Vibration;
        if (!Managers.Game.Vibration)
        {
            GetButton((int)Buttons.VibrationButton).image.sprite = Managers.Resource.Load<Sprite>("Sprites/Main/Common/Button");
        }
        else
        {
            GetButton((int)Buttons.VibrationButton).image.sprite = Managers.Resource.Load<Sprite>("Sprites/Main/Common/OnButton");
        }
        Managers.Game.SaveGame();
    }

    void OnClickEtcButton()
    {
        Managers.Sound.Play(Define.Sound.Effect, "Sound_Button");
        Managers.UI.ClosePopupUI(this);
        Managers.UI.ShowPopupUI<UI_MadeByPopup>();
    }

    void OnClickExitButton()
    {
        Managers.Sound.Play(Define.Sound.Effect, "Sound_Button");

        Managers.Game.SaveGame();
        Managers.UI.ClosePopupUI(this); // UI_MainPopup
    }
}

