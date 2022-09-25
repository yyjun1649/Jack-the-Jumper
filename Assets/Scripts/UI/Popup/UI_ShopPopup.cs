using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ShopPopup : UI_Popup
{

    enum Buttons
    {
       BackButton,
       ItemButton,
       PackageButton,
    }

    enum GameObjects
    {
        ShopItemContent,
    }

    enum Texts
    {
    }

    [SerializeField]
    private int ItemPage;
    [SerializeField]
    private int PackagePage;

    int curPage;
    int count = 0;
    bool flag = true;

    [SerializeField]
    Sprite unSelectButtonImage;
    [SerializeField]
    Sprite selectButtonImage;

    List<UI_ShopItem> _shopItems = new List<UI_ShopItem>();
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        BindObject(typeof(GameObjects));

        GetButton((int)Buttons.BackButton).gameObject.BindEvent(OnClickExitButton);
        GetButton((int)Buttons.ItemButton).gameObject.BindEvent(OnClickItemButton);
        GetButton((int)Buttons.PackageButton).gameObject.BindEvent(OnClickPackageButton);

        curPage = 1;
        PopulateShop();

        return true;
    }
    void OnClickItemButton()
    {
        curPage = 1;
        flag = true;
        GetButton((int)Buttons.ItemButton).image.sprite = selectButtonImage;
        GetButton((int)Buttons.PackageButton).image.sprite = unSelectButtonImage;
        count = 0;
        RefreshUI();

    }

    void OnClickPackageButton()
    {
        curPage = 1;
        flag = false;
        GetButton((int)Buttons.PackageButton).image.sprite = selectButtonImage;
        GetButton((int)Buttons.ItemButton).image.sprite = unSelectButtonImage;
        count = 0;
        RefreshUI();
    }

    void OnClickExitButton()
    {
        Debug.Log("³ª°¡±â");
        Managers.Sound.Play(Define.Sound.Effect, "Sound_Button");

        Managers.Game.Init();
        Managers.Game.SaveGame();
        Managers.UI.ClosePopupUI(this); // UI_MainPopup
    }

    public void RefreshUI()
    {
        if (_init == false)
            return;

        PopulateShop();
        FindObjectOfType<UI_MainPopup>().RefreshUI();
    }

    public void PopulateShop()
    {
        _shopItems.Clear();

        var parent = GetObject((int)GameObjects.ShopItemContent);

        foreach (Transform child in parent.transform)
            Managers.Resource.Destroy(child.gameObject);

        if (flag)
        {
            foreach (ShopItemData shopData in Managers.Data.ShopItems.Values)
            {
                if (Managers.Game.NoAds && shopData.cashType == CashType.Ads)
                    continue;

                if (shopData.folderType == FolderType.Package)
                    continue;

                count++;
                if (count < curPage)
                    continue;
                if (count > curPage + 2 || count > Managers.Data.ShopItems.Values.Count)
                {
                    count = 0;
                    break;
                }

                UI_ShopItem item = Managers.UI.MakeSubItem<UI_ShopItem>(parent.transform);
                item.SetInfo(shopData);

                _shopItems.Add(item);
            }
        }
        else
        {
            foreach (ShopItemData shopData in Managers.Data.ShopItems.Values)
            {
                if (Managers.Game.NoAds && shopData.cashType == CashType.Ads)
                    continue;

                if (shopData.folderType == FolderType.Item)
                    continue;

                count++;
                if (count < curPage)
                    continue;
                if (count > curPage + 2 || count > Managers.Data.ShopItems.Values.Count)
                {
                    count = 0;
                    break;
                }

                UI_ShopItem item = Managers.UI.MakeSubItem<UI_ShopItem>(parent.transform);
                item.SetInfo(shopData);

                _shopItems.Add(item);
            }
        }
    }
}
