using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class UI_RocketStartPopup : UI_Popup
{
    enum Buttons
    {
        RocketButton,
        XButton,
        Button2000M,
        Button1200M,
        Button500M,
        Button0M,
    }

    enum Texts
    {
        NumText,
        RocketStartCount,
    }

    bool musicCondition = true;
    private GamePlayerController mPlayerController;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindText(typeof(Texts));
        BindButton(typeof(Buttons));

        GetText((int)Texts.NumText).text = "3";
        GetText((int)Texts.NumText).gameObject.SetActive(false);
        GetText((int)Texts.RocketStartCount).text = "º¸À¯ ¼ö: " + Managers.Game.StartDash;


        GetButton((int)Buttons.RocketButton).gameObject.BindEvent(OnClickRocketButton);
        GetButton((int)Buttons.Button0M).gameObject.BindEvent(OnClick0M);
        GetButton((int)Buttons.Button500M).gameObject.BindEvent(OnClick500M);
        GetButton((int)Buttons.Button1200M).gameObject.BindEvent(OnClick1200M);
        GetButton((int)Buttons.Button2000M).gameObject.BindEvent(OnClick2000M);
        GetButton((int)Buttons.XButton).gameObject.BindEvent(OnClickXButton);
        mPlayerController = FindObjectOfType<GamePlayerController>();
        mPlayerController.JumpButton.interactable = false;

        if (Managers.Game.StartDash == 0)
            GetButton((int)Buttons.RocketButton).interactable = false;




        GetButton((int)Buttons.Button0M).gameObject.SetActive(false);
        GetButton((int)Buttons.Button500M).gameObject.SetActive(false);
        GetButton((int)Buttons.Button1200M).gameObject.SetActive(false);
        GetButton((int)Buttons.Button2000M).gameObject.SetActive(false);

        Managers.UI.SetCanvas(gameObject, true);
        Time.timeScale = 0f;
        return true;
    }

    void OnClick0M()
    {
        mPlayerController.Count = 0;
    }
    void OnClick500M()
    {
        mPlayerController.Count = 500;
    }
    void OnClick1200M()
    {
        mPlayerController.Count = 1200;
    }
    void OnClick2000M()
    {
        mPlayerController.Count = 2000;
    }

    void OnClickRocketButton()
    {
        if (GetButton((int)Buttons.RocketButton).interactable)
        {
            Managers.Game.RocketStartCount++;
            Managers.Game.StartDash--;
            mPlayerController.RocketStart();
            Time.timeScale = 1f;
            Managers.Sound.Play(Define.Sound.Effect, "Sound_Button");
            Managers.UI.ClosePopupUI(this);
        }
    }

    void OnClickXButton()
    {
        Managers.Sound.Play(Define.Sound.Effect, "Sound_Button");
        StartCoroutine(StartCor());
    }

    IEnumerator StartCor()
    {
        mPlayerController.NormalStart();
        GetButton((int)Buttons.RocketButton).gameObject.SetActive(false);
        GetButton((int)Buttons.XButton).gameObject.SetActive(false);
        Time.timeScale = 0.001f;
        GetText((int)Texts.NumText).gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(0.5f);
        GetText((int)Texts.NumText).text = "2";
        yield return new WaitForSecondsRealtime(0.5f);
        GetText((int)Texts.NumText).text = "1";
        yield return new WaitForSecondsRealtime(0.5f);
        Managers.UI.ClosePopupUI(this);

        Time.timeScale = 1;
    }

}
