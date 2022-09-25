using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_AchievementsPopup : UI_Popup
{
    enum Buttons
    {
        BackButton,
        DailyButton,
        BasicButton,
        MoreButton,
    }

    enum GameObjects
    {
        AchItemContent,
    }

    [SerializeField]
    private int DailyPages;
    [SerializeField]
    private int BasicPages;
    [SerializeField]
    private int MorePages;

    [SerializeField]
    Sprite selectButton;
    [SerializeField]
    Sprite unSelectButton;

    List<UI_AchItem> _AchItems = new List<UI_AchItem>();
    StartData _startData;
    string folderType;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindButton(typeof(Buttons));
        BindObject(typeof(GameObjects));

        folderType = "Daily";
        GetButton((int)Buttons.BackButton).gameObject.BindEvent(OnClickBackButton);
        GetButton((int)Buttons.DailyButton).gameObject.BindEvent(OnClickDailyButton);
        GetButton((int)Buttons.BasicButton).gameObject.BindEvent(OnClickBasicButton);
        GetButton((int)Buttons.MoreButton).gameObject.BindEvent(OnClickMoreButton);
        //GetButton((int)Buttons.PreviousButton).gameObject.BindEvent(OnClickPreviousButton);

        PopulateAch();
        return true;
    }

    void OnClickBackButton()
    {
        Debug.Log("뒤로가기");
        Managers.Sound.Play(Define.Sound.Effect, "Sound_Button");
        Managers.Game.SaveGame();
        Managers.UI.ClosePopupUI(this); // UI_MainPopup
    }

    void OnClickBasicButton()
    {
        folderType = "Basic";
        GetButton((int)Buttons.BasicButton).image.sprite = selectButton;
        GetButton((int)Buttons.DailyButton).image.sprite = unSelectButton;
        GetButton((int)Buttons.MoreButton).image.sprite = unSelectButton;
        PopulateAch();
    }

    void OnClickDailyButton()
    {
        folderType = "Daily";
        GetButton((int)Buttons.BasicButton).image.sprite = unSelectButton;
        GetButton((int)Buttons.DailyButton).image.sprite = selectButton;
        GetButton((int)Buttons.MoreButton).image.sprite = unSelectButton;
        PopulateAch();
    }

    void OnClickMoreButton()
    {
        folderType = "More";
        GetButton((int)Buttons.BasicButton).image.sprite = unSelectButton;
        GetButton((int)Buttons.DailyButton).image.sprite = unSelectButton;
        GetButton((int)Buttons.MoreButton).image.sprite = selectButton;
        PopulateAch();
    }

    public void PopulateAch()
    {
        _AchItems.Clear();

        var parent = GetObject((int)GameObjects.AchItemContent);

        foreach (Transform child in parent.transform)
            Managers.Resource.Destroy(child.gameObject);

        if (folderType == "Daily")
        {
            foreach (AchItemData achData in Managers.Data.AchItems.Values)
            {
                if (achData.Type != AchType.Daily) continue;

                UI_AchItem item = Managers.UI.MakeSubItem<UI_AchItem>(parent.transform);
                item.SetInfo(achData);

                _AchItems.Add(item);
            }
        }
        else if (folderType == "Basic")
        {
            foreach (AchItemData achData in Managers.Data.AchItems.Values)
            {
                if (achData.Type != AchType.Basic) continue;

                UI_AchItem item = Managers.UI.MakeSubItem<UI_AchItem>(parent.transform);
                item.SetInfo(achData);

                _AchItems.Add(item);
            }
        }
        else if (folderType == "More")
        {
            foreach (AchItemData achData in Managers.Data.AchItems.Values)
            {

                if (achData.Type != AchType.More) continue; 

                UI_AchItem item = Managers.UI.MakeSubItem<UI_AchItem>(parent.transform);
                item.SetInfo(achData);

                _AchItems.Add(item);
            }
        }
    }
}
