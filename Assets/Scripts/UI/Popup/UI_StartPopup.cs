
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class UI_StartPopup : UI_Popup
{

    enum Buttons
    {
        TouchToStartButton,
    }
    
    enum Texts
    {
        LoginText
    }


    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        GetButton((int)Buttons.TouchToStartButton).gameObject.BindEvent(OnClickStartButton);
        GetButton((int)Buttons.TouchToStartButton).gameObject.SetActive(false);

        StartCoroutine(startCoroutine());
        return true;
    }
    void OnClickStartButton()
    {
        Managers.Sound.Play(Sound.Effect, "Sound_Button");
        Managers.Game.Init();
        Managers.Game.SaveGame();
        Managers.Game.isFirst = true;
        Managers.UI.ClosePopupUI(this);
        Managers.UI.ShowPopupUI<UI_MainPopup>();
        Managers.UI.ShowPopupUI<UI_MainTitlePopup>();
        Managers.UI.ShowPopupUI<UI_CheckPopup>();
    }

    IEnumerator startCoroutine()
    {

        GPGSBinder.Inst.Login();

        int LoginCoin = 0;
        while (true)
        {
            LoginCoin++;
            GetText((int)Texts.LoginText).text = "��ø� ��ٷ��ּ���";
            yield return new WaitForSeconds(0.33f);
            GetText((int)Texts.LoginText).text = "��ø� ��ٷ��ּ���.";
            yield return new WaitForSeconds(0.33f);
            GetText((int)Texts.LoginText).text = "��ø� ��ٷ��ּ���..";
            yield return new WaitForSeconds(0.33f);
            if (GPGSBinder.Inst.isLogin)
            {
                GPGSBinder.Inst.LoadCloud("mysave");

                int LoadCoin = 0;
                while (!GPGSBinder.Inst.IsConnected)
                {
                    GetText((int)Texts.LoginText).text = "������ �ε��� �Դϴ�";
                    yield return new WaitForSeconds(0.33f);
                    GetText((int)Texts.LoginText).text = "������ �ε��� �Դϴ�.";
                    yield return new WaitForSeconds(0.33f);
                    GetText((int)Texts.LoginText).text = "������ �ε��� �Դϴ�..";
                    yield return new WaitForSeconds(0.33f);
                }
                GetText((int)Texts.LoginText).text = "������ �ε� �Ϸ�!";
                break;
            }

            GPGSBinder.Inst.Login();
            if(LoginCoin > 5)
            {
                GetText((int)Texts.LoginText).text = "�α��� ����";
                break;
            }
        }

        GetButton((int)Buttons.TouchToStartButton).gameObject.SetActive(true);
    }

}
