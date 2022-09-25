using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UI_AchItem : UI_Base
{

	enum Buttons
    {
		RewardButton,
    }
	enum Texts
	{
		AchItemName,
		GauzeText,
		Save1Text,
		Save2Text,
		Save3Text,
		RewardCountText,
	}

	enum GameObjects
	{
		StarIcon,
		StarIcon1,
		StarIcon2,
		ClearIcon,
		ClearIcon1,
		ClearIcon2,
		Gauze,
		RewardIcon,
	}

	

	AchItemData _achItemData;
	int nowProgress;
	public override bool Init()
	{
		if (base.Init() == false)
			return false;

		BindButton(typeof(Buttons));
		BindText(typeof(Texts));
		BindObject(typeof(GameObjects));

		GetButton((int)Buttons.RewardButton).gameObject.BindEvent(OnClickRewardButton);
		GetObject((int)GameObjects.ClearIcon).SetActive(false);
		GetObject((int)GameObjects.ClearIcon1).SetActive(false);
		GetObject((int)GameObjects.ClearIcon2).SetActive(false);
		GetObject((int)GameObjects.Gauze).SetActive(true);
		GetObject((int)GameObjects.StarIcon).SetActive(true);
		GetObject((int)GameObjects.StarIcon1).SetActive(true);
		GetObject((int)GameObjects.StarIcon2).SetActive(true);

		RefreshUI();

		return true;
	}
	public void SetInfo(AchItemData achItemData)
	{
		_achItemData = achItemData;
		RefreshUI();
	}

	void RefreshUI()
	{
		if (_init == false)
			return;
		GetButton((int)Buttons.RewardButton).interactable = false;
		if (_achItemData.ID == 1)
			nowProgress = Managers.Game.PlayCount;
		else if (_achItemData.ID == 2)
			nowProgress = Managers.Game.BuyItemCount;
		else if (_achItemData.ID == 3)
			nowProgress = Managers.Game.WatchAdsCount;
		else if (_achItemData.ID == 4)
			nowProgress = Managers.Game.FirstScore;
		else if (_achItemData.ID == 5)
			nowProgress = Managers.Game.TouchThornCount;
		else if (_achItemData.ID == 6)
			nowProgress = Managers.Game.RocketStartCount;
		else if (_achItemData.ID == 7)
			nowProgress = Managers.Game.ReviveCount;
		else if (_achItemData.ID == 8)
			nowProgress = Managers.Game.RideWindCount;
		else if (_achItemData.ID == 9)
			nowProgress = Managers.Game.MonsterCount;

		GetText((int)Texts.AchItemName).text = _achItemData.Name;
		GetText((int)Texts.GauzeText).text = nowProgress.ToString() + " / " + _achItemData.MaxProgress.ToString();

		if (_achItemData.Save_1 < 1000)
		{
			GetText((int)Texts.Save1Text).text = _achItemData.Save_1 + "회";
			GetText((int)Texts.Save2Text).text = _achItemData.Save_2 + "회";
			GetText((int)Texts.Save3Text).text = _achItemData.Save_3 + "회";
        }
        else
        {
			GetText((int)Texts.Save1Text).text = _achItemData.Save_1 + "M";
			GetText((int)Texts.Save2Text).text = _achItemData.Save_2 + "M";
			GetText((int)Texts.Save3Text).text = _achItemData.Save_3 + "M";
		}

		GetObject((int)GameObjects.Gauze).GetComponent<Image>().fillAmount = (float)nowProgress / (float)_achItemData.MaxProgress;

		if (Managers.Game.AchCollections[_achItemData.ID - 1] == AchClearState.Reward0)
		{
			if (_achItemData.Save_1Reward == "Bean")
			{
				GetObject((int)GameObjects.RewardIcon).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Main/item/Bean");
				GetText((int)Texts.RewardCountText).text = "X " + _achItemData.Save_1RewardCount;
			}
			else if (_achItemData.Save_1Reward == "Rocket")
			{
				GetObject((int)GameObjects.RewardIcon).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Main/item/Rocket");
				GetText((int)Texts.RewardCountText).text = "X " + _achItemData.Save_1RewardCount;
			}
			else if (_achItemData.Save_1Reward == "Revive")
			{
				GetObject((int)GameObjects.RewardIcon).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Main/item/Revive");
				GetText((int)Texts.RewardCountText).text = "X " + _achItemData.Save_1RewardCount;
			}
		}

		else if (Managers.Game.AchCollections[_achItemData.ID - 1] == AchClearState.Reward1)
		{
			GetObject((int)GameObjects.ClearIcon).SetActive(true);
			if (_achItemData.Save_2Reward == "Bean")
			{
				GetObject((int)GameObjects.RewardIcon).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Main/item/Bean");
				GetText((int)Texts.RewardCountText).text = "X " + _achItemData.Save_2RewardCount;
			}
			else if (_achItemData.Save_2Reward == "Rocket")
			{
				GetObject((int)GameObjects.RewardIcon).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Main/item/Rocket");
				GetText((int)Texts.RewardCountText).text = "X " + _achItemData.Save_2RewardCount;
			}
			else if (_achItemData.Save_2Reward == "Revive")
			{
				GetObject((int)GameObjects.RewardIcon).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Main/item/Revive");
				GetText((int)Texts.RewardCountText).text = "X " + _achItemData.Save_2RewardCount;
			}
		}

		else if (Managers.Game.AchCollections[_achItemData.ID-1] == AchClearState.Reward2)
		{
			GetObject((int)GameObjects.ClearIcon).SetActive(true);
			GetObject((int)GameObjects.ClearIcon1).SetActive(true);
			if (_achItemData.Save_3Reward == "Bean")
			{
				GetObject((int)GameObjects.RewardIcon).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Main/item/Bean");
				GetText((int)Texts.RewardCountText).text = "X " + _achItemData.Save_3RewardCount;
			}
			else if (_achItemData.Save_3Reward == "Rocket")
            {
				GetObject((int)GameObjects.RewardIcon).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Main/item/Rocket");
				GetText((int)Texts.RewardCountText).text = "X " + _achItemData.Save_3RewardCount;
			}
			else if (_achItemData.Save_3Reward == "Revive")
            {
				GetObject((int)GameObjects.RewardIcon).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Main/item/Revive");
				GetText((int)Texts.RewardCountText).text = "X " + _achItemData.Save_3RewardCount;
			}
		}

		else if(Managers.Game.AchCollections[_achItemData.ID - 1] == AchClearState.Reward3)
        {
			GetObject((int)GameObjects.ClearIcon).SetActive(true);
			GetObject((int)GameObjects.ClearIcon1).SetActive(true);
			GetObject((int)GameObjects.ClearIcon2).SetActive(true);
			GetObject((int)GameObjects.RewardIcon).gameObject.SetActive(false);
			GetText((int)Texts.RewardCountText).text = "업적완료";
		}


		if (nowProgress >= _achItemData.Save_3 && Managers.Game.AchCollections[_achItemData.ID-1] == AchClearState.Reward2)
		{
				GetButton((int)Buttons.RewardButton).interactable = true;
		}
		else if (nowProgress >= _achItemData.Save_2 && Managers.Game.AchCollections[_achItemData.ID - 1] == AchClearState.Reward1)
		{
				GetButton((int)Buttons.RewardButton).interactable = true;
		}
		else if (nowProgress >= _achItemData.Save_1 && Managers.Game.AchCollections[_achItemData.ID - 1] == AchClearState.Reward0)
		{
				GetButton((int)Buttons.RewardButton).interactable = true;
		}
	}


	void OnClickRewardButton()
    {
        if (GetButton((int)Buttons.RewardButton).interactable)
        {
			Managers.Sound.Play(Define.Sound.Effect, "Sound_ClearAch");
			if (Managers.Game.AchCollections[_achItemData.ID - 1] == AchClearState.Reward0) {
				Managers.Game.AchCollections[_achItemData.ID - 1] = AchClearState.Reward1;
				GetObject((int)GameObjects.ClearIcon).SetActive(true);
				if (_achItemData.Save_1Reward == "Bean")
				{
					Managers.Game.BeanCount += _achItemData.Save_1RewardCount;
				}
				else if (_achItemData.Save_1Reward == "Rocket")
				{
					Managers.Game.StartDash += _achItemData.Save_1RewardCount;
				}
				else if (_achItemData.Save_1Reward == "Revive")
				{
					Managers.Game.Revival += _achItemData.Save_1RewardCount;
				}
			}

			else if (Managers.Game.AchCollections[_achItemData.ID - 1] == AchClearState.Reward1) {
				Managers.Game.AchCollections[_achItemData.ID - 1] = AchClearState.Reward2;
				GetObject((int)GameObjects.ClearIcon1).SetActive(true);
				if (_achItemData.Save_2Reward == "Bean")
				{
					Managers.Game.BeanCount += _achItemData.Save_2RewardCount;
				}
				else if (_achItemData.Save_2Reward == "Rocket")
				{
					Managers.Game.StartDash += _achItemData.Save_2RewardCount;
				}
				else if (_achItemData.Save_2Reward == "Revive")
				{
					Managers.Game.Revival += _achItemData.Save_2RewardCount;
				}
			}

			else if (Managers.Game.AchCollections[_achItemData.ID - 1] == AchClearState.Reward2)
            {
				Managers.Game.AchCollections[_achItemData.ID - 1] = AchClearState.Reward3;
				GetObject((int)GameObjects.ClearIcon2).SetActive(true);
				if (_achItemData.Save_3Reward == "Bean")
				{
					Managers.Game.BeanCount += _achItemData.Save_3RewardCount;
				}
				else if (_achItemData.Save_3Reward == "Rocket")
				{
					Managers.Game.StartDash += _achItemData.Save_3RewardCount;
				}
				else if (_achItemData.Save_3Reward == "Revive")
				{
					Managers.Game.Revival += _achItemData.Save_3RewardCount;
				}
			}
			Managers.Game.SaveGame();
			if (GPGSBinder.Inst.IsConnected)
				GPGSBinder.Inst.SaveCloud("mysave");
			RefreshUI();
			Managers.UI.FindPopup<UI_MainPopup>().RefreshUI();
		}
    }
}
