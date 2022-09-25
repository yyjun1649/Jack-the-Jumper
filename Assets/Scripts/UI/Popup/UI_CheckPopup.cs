 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UI_CheckPopup : UI_Popup
{
    enum Buttons
    {
        BackButton,
    }

    enum Images
    {
        Best1,
        Best2,
        Best3,
        Best4,
        Best5,
        Best6,
        Best7,
    }

    enum GameObjects
    {
        FirstWeek,
        SecondWeek,
    }
    List<GameObject> ImageList = new List<GameObject>();

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindButton(typeof(Buttons));
        BindImage(typeof(Images));
        BindObject(typeof(GameObjects));


        ImageList.Add(GetImage((int)Images.Best1).gameObject);
        ImageList.Add(GetImage((int)Images.Best2).gameObject);
        ImageList.Add(GetImage((int)Images.Best3).gameObject);
        ImageList.Add(GetImage((int)Images.Best4).gameObject);
        ImageList.Add(GetImage((int)Images.Best5).gameObject);
        ImageList.Add(GetImage((int)Images.Best6).gameObject);
        ImageList.Add(GetImage((int)Images.Best7).gameObject);

        if (Managers.Game.CheckSecondWeek)
        {
            GetObject((int)GameObjects.SecondWeek).SetActive(true);
            GetObject((int)GameObjects.FirstWeek).SetActive(false);
        }
        else
        {
            GetObject((int)GameObjects.SecondWeek).SetActive(false);
            GetObject((int)GameObjects.FirstWeek).SetActive(true);
        }

        for (int i = 0; i < ImageList.Count; i++)
            ImageList[i].SetActive(false);

        GetButton((int)Buttons.BackButton).gameObject.BindEvent(OnClickExitButton);

        if (Managers.Game.PreviousDate != DateTime.Now.ToString("yyyy-MM-dd"))
        {
            if (Managers.Game.DayCount == 7)
            {
                Managers.Game.DayCount = 1;
                Managers.Game.CheckSecondWeek = true;
                GetObject((int)GameObjects.SecondWeek).SetActive(true);
                GetObject((int)GameObjects.FirstWeek).SetActive(false);
            }
            else
                Managers.Game.DayCount++;

            //일일 업적 초기화
            Managers.Game.PlayCount = 0;
            Managers.Game.BuyItemCount = 0;
            Managers.Game.WatchAdsCount = 0;
            Managers.Game.AchCollections[0] = AchClearState.Reward0;
            Managers.Game.AchCollections[1] = AchClearState.Reward0;
            Managers.Game.AchCollections[2] = AchClearState.Reward0;


            //광고 시청횟수 초기화
            Managers.Game.AdsCount[0] = 3;
            Managers.Game.AdsCount[1] = 3;

            for (int i = 0; i < Managers.Game.DayCount -1 ; i++)
            {
                ImageList[i].SetActive(true);
            }

            Managers.Game.PreviousDate = DateTime.Now.ToString("yyyy-MM-dd");
            ImageList[Managers.Game.DayCount - 1].SetActive(true);
            ImageList[Managers.Game.DayCount - 1].GetComponent<Animator>().SetTrigger("On");

            switch (Managers.Game.DayCount)
            {
                case 1:
                case 3:
                case 5:
                    DayReward135();
                    break;
                case 2:
                    DayReward2();
                    break;
                case 4:
                    DayReward4();
                    break;
                case 6:
                    DayReward6();
                    break;
                case 7:
                    DayReward7();
                    break;
            }
        }
        else
        {
            for (int i = 0; i < Managers.Game.DayCount; i++)
            {
                ImageList[i].SetActive(true);
            }
        }

        Managers.Game.SaveGame();

        Managers.UI.FindPopup<UI_MainPopup>().RefreshUI();
        Managers.UI.SetCanvas(gameObject, true);
        return true;
    }


    void OnClickExitButton()
    {
        Managers.Sound.Play(Define.Sound.Effect, "Sound_Button");
        Managers.UI.ClosePopupUI(this); // UI_MainPopup
    }

    void DayReward135()
    {
        Managers.Game.StartDash++;
    }

    void DayReward2()
    {
        Managers.Game.BeanCount += 200;
    }

    void DayReward4()
    {
        Managers.Game.BeanCount += 300;
    }

    void DayReward6()
    {
        Managers.Game.BeanCount += 400;
    }

    void DayReward7()
    {
        if (Managers.Game.CheckSecondWeek)
        {
            Managers.Game.Revival++;
            Managers.Game.BeanCount += 300;
        }
        else
        {
            Managers.Game.charState[3] = CharacterConditionType.isnotEquipment;
        }
    }




}

