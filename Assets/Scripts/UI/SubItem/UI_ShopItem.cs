using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.Purchasing;
using UnityEngine.UI;


public class UI_ShopItem : UI_Base
{
	enum Buttons
	{
		BuyButton,
		AdsButton,
	}

	enum Texts
	{
		ItemName,
		BuyText,
		AdsCountText,
		AdsTipText,
	}

	enum GameObjects
	{
		ItemIcon
	}

	ShopItemData _shopItemData;
	bool flag = true;
	public override bool Init()
	{
		if (base.Init() == false)
			return false;



		BindButton(typeof(Buttons));
		BindText(typeof(Texts));
		BindObject(typeof(GameObjects));

		GetButton((int)Buttons.BuyButton).gameObject.BindEvent(OnClickButton);
		GetButton((int)Buttons.AdsButton).gameObject.BindEvent(OnClickAdsButton);

		RefreshUI();

		return true;
	}

	public void SetInfo(ShopItemData shopItemData)
	{
		_shopItemData = shopItemData;
		RefreshUI();
	}

	public void RefreshUI()
	{
		if (_init == false)
			return;

		Managers.UI.FindPopup<UI_MainPopup>().RefreshUI();

		GetObject((int)GameObjects.ItemIcon).GetOrAddComponent<BaseController>().SetImage(_shopItemData.ItemIcon);
		GetButton((int)Buttons.AdsButton).gameObject.SetActive(false);
		GetText((int)Texts.AdsCountText).gameObject.SetActive(false);
		GetText((int)Texts.AdsTipText).gameObject.SetActive(false);
		GetButton((int)Buttons.AdsButton).interactable = false;

		if (Managers.Game.PreviousDate != DateTime.Now.ToString("yyyy-MM-dd"))
		{
			Managers.Game.AdsCount[_shopItemData.ID] = 3;
		}

		if (Managers.Game.AdsCount[_shopItemData.ID] < 1)
		{
			GetButton((int)Buttons.AdsButton).interactable = false;
			GetText((int)Texts.AdsCountText).text = "0 / 3";
        }
        else
        {
			GetButton((int)Buttons.AdsButton).interactable = true;
        }

		if (_shopItemData.ItemName == "스타트 대쉬" || _shopItemData.ItemName == "부활권")
			GetText((int)Texts.ItemName).text = _shopItemData.ItemName + " X " + _shopItemData.ItemCount.ToString();
		else if (_shopItemData.ItemName == "콩")
			GetText((int)Texts.ItemName).text = _shopItemData.ItemName + " " + _shopItemData.ItemCount.ToString() + "개";
		else if (_shopItemData.ItemName == "광고제거")
			GetText((int)Texts.ItemName).text = "광고제거";
		else if (_shopItemData.ItemName == "광고제거콩")
			GetText((int)Texts.ItemName).text = "광고제거 + 콩 3000개";

		if (_shopItemData.cashType == CashType.Bean)
			GetText((int)Texts.BuyText).text = _shopItemData.price + " 콩";

		if (_shopItemData.cashType == CashType.Cash || _shopItemData.cashType == CashType.Ads)
			GetText((int)Texts.BuyText).text = _shopItemData.price + " 원";



		if(_shopItemData.cashType == CashType.Bean)
        {
			if (_shopItemData.price > Managers.Game.BeanCount)
				_shopItemData.condition = ShopItemType.CannotBuy;
			else if(_shopItemData.price <= Managers.Game.BeanCount)
				_shopItemData.condition = ShopItemType.CanBuy;
		}

		if (Managers.Game.AdsCount[_shopItemData.ID] != -1)
		{
			GetText((int)Texts.AdsCountText).text = Managers.Game.AdsCount[_shopItemData.ID].ToString() + " / 3";
			GetButton((int)Buttons.AdsButton).gameObject.SetActive(true);
			GetText((int)Texts.AdsCountText).gameObject.SetActive(true);
			GetText((int)Texts.AdsTipText).gameObject.SetActive(true);
		}

		if (_shopItemData.condition == ShopItemType.CanBuy)
		{
			GetButton((int)Buttons.BuyButton).interactable = true;
		}
		else
		{
			GetButton((int)Buttons.BuyButton).interactable = false;
		}



		if (_shopItemData.cashType == CashType.Ads) {
			if (Managers.Game.NoAds)
			{
				GetButton((int)Buttons.BuyButton).interactable = false;
				Destroy(this);
			}
		}
	}

	void OnClickButton()
	{
		if (GetButton((int)Buttons.BuyButton).interactable == true)
		{
			Debug.Log("OnClickButton");

			if (_shopItemData.condition == ShopItemType.CanBuy)
			{
				if (_shopItemData.cashType == CashType.Bean)
				{
					Managers.Game.BeanCount -= _shopItemData.price;
					Managers.Game.BuyItemCount++;
					if (_shopItemData.ItemName == "스타트 대쉬")
						Managers.Game.StartDash++;
					else if (_shopItemData.ItemName == "부활권")
						Managers.Game.Revival++;
				}
				else if (_shopItemData.cashType == CashType.Cash)
				{
					Managers.IAP.Purchase(_shopItemData.productID, (product, failureReason) =>
					{
						Debug.Log($"Purchase Done {product.transactionID} {failureReason}");
						// 성공했는지 확인
						if (failureReason == PurchaseFailureReason.Unknown)
							GiveReward();
					});
				}
				else if (_shopItemData.cashType == CashType.Ads)
				{
					if (!Managers.Game.NoAds)
					{
						Managers.IAP.Purchase(_shopItemData.productID, (product, failureReason) =>
						{
							Debug.Log($"Purchase Done {product.transactionID} {failureReason}");
						// 성공했는지 확인
							if (failureReason == PurchaseFailureReason.Unknown)
								GiveReward();
						});
                    }
                    else
                    {
						Managers.Sound.Play(Define.Sound.Effect, "Sound_Wrong");
                    }
				}
			}
			Managers.Game.SaveGame();
			RefreshUI();
		}
	}

	void OnClickAdsButton()
	{
		if (GetButton((int)Buttons.AdsButton).interactable == true)
		{
			if (flag)
			{
				flag = false;

				Managers.Sound.Play(Define.Sound.Bgm, "Sound_LobbyBGM", 0.5f);
				RefreshUI();
				Managers.Ads.ShowRewardedAds(() => { GiveReward(); });
				Invoke("Changeflag", 2f);
			}
		}
    }

	void GiveReward()
	{
		if (_shopItemData.ItemName == "콩")
		{
			Managers.Game.BeanCount += _shopItemData.ItemCount;
			Managers.Game.BuyItemCount++;
		}

		else if (_shopItemData.ItemName == "광고제거")
		{
			Managers.Game.NoAds = true;
			Managers.Game.BuyItemCount++;
		}
		else if (_shopItemData.ItemName == "광고제거콩")
		{
			Managers.Game.NoAds = true;
			Managers.Game.BeanCount += _shopItemData.ItemCount;
			Managers.Game.BuyItemCount++;
		}
		else if (_shopItemData.ItemName == "스타트 대쉬")
		{
			Managers.Game.WatchAdsCount++;
			Managers.Game.StartDash++;
			Managers.Game.AdsCount[0]--;
		}
		else if (_shopItemData.ItemName == "부활권")
		{
			Managers.Game.WatchAdsCount++;
			Managers.Game.Revival++;
			Managers.Game.AdsCount[1]--;
		}

		Managers.Game.SaveGame();
		if (GPGSBinder.Inst.IsConnected)
			GPGSBinder.Inst.SaveCloud("mysave");

		RefreshUI();
	}

	void Changeflag()
    {
		flag = true;
    }

}
