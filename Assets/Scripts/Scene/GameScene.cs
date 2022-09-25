using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameScene : BaseScene
{
	protected override bool Init()
	{
		if (base.Init() == false)
			return false;

		SceneType = Define.Scene.Game;

		if (Managers.Game.isFirst)
			Managers.UI.ShowPopupUI<UI_StartPopup>();
		else
		{
			
			Managers.UI.ShowPopupUI<UI_MainPopup>();
			Managers.UI.ShowPopupUI<UI_MainTitlePopup>();
		}

		Managers.Sound.Play(Define.Sound.Bgm, "Sound_LobbyBGM", 0.5f);
		Debug.Log("Init");
		return true;
	}

}
