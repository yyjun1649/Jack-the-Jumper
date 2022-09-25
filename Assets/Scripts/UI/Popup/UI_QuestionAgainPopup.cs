using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class UI_QuestionAgainPopup : UI_Popup
{
    enum Buttons
    {
        AdsButton,
        ItemButton,
        XButton
    }

    enum Texts
    {
        ScoreText,
        ItemCount,
    }

    bool musicCondition = true;
    private GamePlayerController mPlayerController;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        Managers.Sound.Clear();
        Managers.Sound.Play(Define.Sound.Effect, "Sound_Die");

        BindText(typeof(Texts));
        BindButton(typeof(Buttons));

        GetButton((int)Buttons.AdsButton).gameObject.BindEvent(OnClickAdsButton);
        GetButton((int)Buttons.ItemButton).gameObject.BindEvent(OnClickItemButton);
        GetButton((int)Buttons.XButton).gameObject.BindEvent(OnClickXButton);
        mPlayerController = FindObjectOfType<GamePlayerController>();
        GetText((int)Texts.ScoreText).text = mPlayerController.Count.ToString() + " M";
        GetText((int)Texts.ItemCount).text = Managers.Game.Revival.ToString();

        if (Managers.Game.UseRevive) {
            GetButton((int)Buttons.AdsButton).interactable = false;
            GetButton((int)Buttons.ItemButton).interactable = false;
        }

        Managers.UI.SetCanvas(gameObject, true);
        return true;
    }

    void OnClickAdsButton()
    {
        // 광고시청
        if (!Managers.Game.UseRevive) {
            FindObjectOfType<CsStopController>().isAds = true;
            Managers.Sound.Play(Define.Sound.Effect, "Sound_Button");
            Managers.Ads.ShowRewardedAds(() => { GiveReward(); });
        }
    }

    void OnClickItemButton()
    {
        // 아이템사용
        if (Managers.Game.Revival != 0 && !Managers.Game.UseRevive)
        {
            FindObjectOfType<CsStopController>().isAds = false;
            Managers.Sound.Play(Define.Sound.Effect, "Sound_Button");
            Managers.Game.Revival--;
            mPlayerController.Revive();
            Managers.UI.CloseAllPopupUI();
        }
    }

    void OnClickXButton()
    {
        FindObjectOfType<CsStopController>().isAds = false;
        Managers.Sound.Play(Define.Sound.Effect, "Sound_Button");
        Managers.UI.ClosePopupUI(this);
        Managers.UI.ShowPopupUI<UI_EndGamePopup>();
    }

    void GiveReward()
    {
        FindObjectOfType<GamePlayerController>().Revive();
        Managers.UI.ClosePopupUI(this);
        FindObjectOfType<CsStopController>().isAds = false;
    }
}
