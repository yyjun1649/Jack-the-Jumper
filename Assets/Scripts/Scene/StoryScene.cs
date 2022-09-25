using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StoryScene : BaseScene
{
	[SerializeField]
	private GameObject AchBox;
    [SerializeField]
    private TextMeshProUGUI AchText;
    protected override bool Init()
	{
		if (base.Init() == false)
			return false;

        Managers.Game.UseRevive = false;
		AchBox.SetActive(false);
		Managers.Game.PlayCount++;
		SceneType = Define.Scene.Game;
		Managers.Sound.Play(Define.Sound.Bgm, "Sound_BGM",0.5f);
		if (!Managers.Game.BGM)
			Managers.Sound.SetVolume(Define.Sound.Bgm,0f);
		Managers.UI.ShowPopupUI<UI_RocketStartPopup>();
        Managers.Game.SaveGame();
        StartCoroutine(CheckAchCoroutine());
		return true;
	}

	IEnumerator CheckAchCoroutine()
    {
        yield return new WaitForSeconds(5f);
        int nowProgress = 0;
        while (true)
        {
            foreach (AchItemData achItemData in Managers.Data.AchItems.Values)
            {
                float Calpha = 0.8f;
                AchBox.GetComponent<Image>().color = new Color(AchBox.GetComponent<Image>().color.r, AchBox.GetComponent<Image>().color.g, AchBox.GetComponent<Image>().color.b, Calpha);
                AchBox.transform.Find("AchAlarmText").GetComponent<TextMeshProUGUI>().color = new Color(255f, 255f, 255f, Calpha);
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

                if (nowProgress >= achItemData.Save_3 && Managers.Game.AchCollections[achItemData.ID - 1] == AchClearState.Reward2 && !achItemData.isSave3)
                {
                    Managers.Sound.Play(Define.Sound.Effect, "Sound_ClearAch");
                    achItemData.isSave3 = true;
                    AchBox.SetActive(true);
                    AchText.text = achItemData.Name + "-3 含失!";
                    yield return new WaitForSeconds(2f);
                    while (AchBox.GetComponent<Image>().color.a > 0)
                    {
                        AchBox.GetComponent<Image>().color = new Color(AchBox.GetComponent<Image>().color.r, AchBox.GetComponent<Image>().color.g, AchBox.GetComponent<Image>().color.b, Calpha);
                        AchBox.transform.Find("AchAlarmText").GetComponent<TextMeshProUGUI>().color = new Color(255f, 255f, 255f, Calpha);
                        Calpha -= 0.05f;
                        yield return new WaitForSeconds(0.05f);
                    }
                    AchBox.SetActive(false);
                }
                else if (nowProgress >= achItemData.Save_2 && Managers.Game.AchCollections[achItemData.ID - 1] == AchClearState.Reward1 && !achItemData.isSave2)
                {
                    Managers.Sound.Play(Define.Sound.Effect, "Sound_ClearAch");
                    achItemData.isSave2 = true;
                    AchBox.SetActive(true);
                    AchText.text = achItemData.Name + "-2 含失!";
                    yield return new WaitForSeconds(2f);
                    while (AchBox.GetComponent<Image>().color.a > 0)
                    {
                        AchBox.GetComponent<Image>().color = new Color(AchBox.GetComponent<Image>().color.r, AchBox.GetComponent<Image>().color.g, AchBox.GetComponent<Image>().color.b, Calpha);
                        AchBox.transform.Find("AchAlarmText").GetComponent<TextMeshProUGUI>().color = new Color(255f, 255f, 255f, Calpha);
                        Calpha -= 0.05f;
                        yield return new WaitForSeconds(0.05f);
                    }
                    AchBox.SetActive(false);
                }
                else if (nowProgress >= achItemData.Save_1 && Managers.Game.AchCollections[achItemData.ID - 1] == AchClearState.Reward0 && !achItemData.isSave1)
                {
                    Managers.Sound.Play(Define.Sound.Effect, "Sound_ClearAch");
                    achItemData.isSave1 = true;
                    AchBox.SetActive(true);
                    AchText.text = achItemData.Name + "-1 含失!";
                    yield return new WaitForSeconds(2f);
                    while (AchBox.GetComponent<Image>().color.a > 0)
                    {
                        AchBox.GetComponent<Image>().color = new Color(AchBox.GetComponent<Image>().color.r, AchBox.GetComponent<Image>().color.g, AchBox.GetComponent<Image>().color.b, Calpha);
                        AchBox.transform.Find("AchAlarmText").GetComponent<TextMeshProUGUI>().color = new Color(255f, 255f, 255f, Calpha);
                        Calpha -= 0.05f;
                        yield return new WaitForSeconds(0.05f);
                    }
                    AchBox.SetActive(false);
                }
            }
            yield return null;
        }
    }
}
