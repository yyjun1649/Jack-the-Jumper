using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_MainPopup : UI_Popup
{
    enum Buttons
    {
        ShopButton,
        AttendanceButton,
        AchievementsButton,
        BagButton,
        CoinButton,
        OptionButton,
        ExitButton,
        RecordButton,
        ShopButton2,
        RankingButton,
    }
    enum Texts
    {
        ShopText,
        AchievementsText,
        BagText,
        CoinText,
        OptionText,
    }

    enum GameObjects
    {
        ClearIcon,
    }
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        BindObject(typeof(GameObjects));

        if (!Managers.Game.BGM)
            Managers.Sound.SetVolume(Define.Sound.Bgm, 0f);
        if (!Managers.Game.SFX)
            Managers.Sound.SetVolume(Define.Sound.Effect, 0f);

        GetButton((int)Buttons.ShopButton).gameObject.BindEvent(OnClickShopButton);
        GetButton((int)Buttons.ShopButton2).gameObject.BindEvent(OnClickShopButton);
        GetButton((int)Buttons.AchievementsButton).gameObject.BindEvent(OnClickAchievementsButton);
        GetButton((int)Buttons.BagButton).gameObject.BindEvent(OnClickBagButton);
        GetButton((int)Buttons.CoinButton).gameObject.BindEvent(OnClickCoinButton);
        GetButton((int)Buttons.OptionButton).gameObject.BindEvent(OnClickOptionButton);
        GetButton((int)Buttons.ExitButton).gameObject.BindEvent(OnClickExitButton);
        GetButton((int)Buttons.RecordButton).gameObject.BindEvent(OnClickRecordButton);
        GetButton((int)Buttons.AttendanceButton).gameObject.BindEvent(OnClickAttendanceButton);
        GetButton((int)Buttons.RankingButton).gameObject.BindEvent(OnClickRankingButton);
        GetObject((int)GameObjects.ClearIcon).SetActive(false);
        Managers.Game.isRealFirst = false;
        Managers.Game.SaveGame();
        if (GPGSBinder.Inst.IsConnected)
            GPGSBinder.Inst.SaveCloud("mysave");
        RefreshUI();
        Managers.Game.isFirst = true;
        Managers.UI.SetCanvas(gameObject, true);

        return true;
    }

    public void RefreshUI()
    {
        GetText((int)Texts.CoinText).text = Managers.Game.BeanCount.ToString();
        GetObject((int)GameObjects.ClearIcon).SetActive(false);
        int nowProgress = 0;

        foreach (AchItemData achItemData in Managers.Data.AchItems.Values)
        {
            if (achItemData.ID == 1)
                nowProgress = Managers.Game.PlayCount;
            else if (achItemData.ID == 2)
                nowProgress = Managers.Game.BuyItemCount;
            else if (achItemData.ID == 3)
                nowProgress = Managers.Game.WatchAdsCount;
            else if (achItemData.ID == 4)
                nowProgress = Managers.Game.FirstScore;
            else if (achItemData.ID == 5)
                nowProgress = Managers.Game.TouchThornCount;
            else if (achItemData.ID == 6)
                nowProgress = Managers.Game.RocketStartCount;
            else if (achItemData.ID == 7)
                nowProgress = Managers.Game.ReviveCount;
            else if (achItemData.ID == 8)
                nowProgress = Managers.Game.RideWindCount;
            else if (achItemData.ID == 9)
                nowProgress = Managers.Game.MonsterCount;

            if (nowProgress >= achItemData.Save_3 && Managers.Game.AchCollections[achItemData.ID - 1] == AchClearState.Reward2)
            {
                if (Managers.Game.AchCollections[achItemData.ID - 1] == AchClearState.Reward2)
                    GetObject((int)GameObjects.ClearIcon).SetActive(true);
            }
            else if (nowProgress >= achItemData.Save_2 && Managers.Game.AchCollections[achItemData.ID - 1] == AchClearState.Reward1)
            {
                if (Managers.Game.AchCollections[achItemData.ID - 1] == AchClearState.Reward1)
                    GetObject((int)GameObjects.ClearIcon).SetActive(true);
            }
            else if (nowProgress >= achItemData.Save_1 && Managers.Game.AchCollections[achItemData.ID - 1] == AchClearState.Reward0)
            {
                if (Managers.Game.AchCollections[achItemData.ID - 1] == AchClearState.Reward0)
                    GetObject((int)GameObjects.ClearIcon).SetActive(true);
            }
        }
    }

    void OnClickShopButton()
    {
        Debug.Log("상점 선택");
        Managers.Sound.Play(Define.Sound.Effect, "Sound_Button");


        Managers.Game.SaveGame();
        //Managers.UI.ClosePopupUI(this); // UI_MainPopup
        Managers.UI.ShowPopupUI<UI_ShopPopup>();
    }
    void OnClickAchievementsButton()
    {
        Debug.Log("업적 선택");
        Managers.Sound.Play(Define.Sound.Effect, "Sound_Button");

        Managers.Game.SaveGame();
        Managers.UI.ShowPopupUI<UI_AchievementsPopup>();
    }
    void OnClickBagButton()
    {
        Debug.Log("가방");
        Managers.Sound.Play(Define.Sound.Effect, "Sound_Button");


        Managers.Game.SaveGame();
        Managers.UI.ShowPopupUI<UI_BagPopup>();
    }
    void OnClickOptionButton()
    {
        Debug.Log("옵션 선택");
        Managers.Sound.Play(Define.Sound.Effect, "Sound_Button");


        Managers.Game.SaveGame();
        Managers.UI.ShowPopupUI<UI_OptionPopup>();
    }
    void OnClickCoinButton()
    {
        Debug.Log("돈 선택");
        Managers.Sound.Play(Define.Sound.Effect, "Sound_Button");

;
        Managers.Game.SaveGame();
        //Managers.UI.ShowPopupUI<UI_MainPopup>();
    }
    void OnClickExitButton()
    {
        Debug.Log("종료하기");
        Managers.Sound.Play(Define.Sound.Effect, "Sound_Button");
        GetButton((int)Buttons.ExitButton).gameObject.SetActive(false);

        Managers.Game.SaveGame();
        Managers.UI.ShowPopupUI<UI_ExitPopup>();
    }

    void OnClickRecordButton()
    {
        Managers.Sound.Play(Define.Sound.Effect, "Sound_Button");


        Managers.UI.ShowPopupUI<UI_RecordPopup>();
    }

    void OnClickAttendanceButton()
    {
        Managers.Sound.Play(Define.Sound.Effect, "Sound_Button");


        Managers.UI.ShowPopupUI<UI_CheckPopup>();
    }

    void OnClickRankingButton()
    {
        GPGSBinder.Inst.ReportLeaderboard(GPGSIds.leaderboard_ranking, Managers.Game.FirstScore, success => Debug.Log("Report Ranking"));
        GPGSBinder.Inst.ShowTargetLeaderboardUI(GPGSIds.leaderboard_ranking);
    }

    public void OnExitButton()
    {
        GetButton((int)Buttons.ExitButton).gameObject.SetActive(true);
    }

    void OnApplicationQuit()
    {
        if(GPGSBinder.Inst.IsConnected)
            GPGSBinder.Inst.SaveCloud("mysave");
    }

}

