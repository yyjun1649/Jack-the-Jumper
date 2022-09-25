using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Unity;
using static Define;
using GooglePlayGames.BasicApi.SavedGame;
using System.Text;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public enum CharacterConditionType
{
	CanBuy,
	isEquipment,
	isnotEquipment,
	CannotBuy,
}


public enum AchClearState
{
	Reward0,
	Reward1,
	Reward2,
	Reward3,
}

[Serializable]
public class GameData
{
	public string PlayerID;

	public int Character = 0;
	public bool isFirst = true;

	//¾÷Àû
	public int BeanCount;
	public int FirstScore;
	public int SecondScore;
	public int ThirdScore;
	public int StartDash;
	public int Revival;
	public bool NoAds = false;
	public int PlayCount;
	public int BuyItemCount;
	public int WatchAdsCount;
	public int TouchThornCount;
	public int RocketStartCount;
	public int ReviveCount;
	public int RideWindCount;
	public int MonsterCount;
	//

	public bool UseRevive = false;
	public bool isRealFirst = false;

	public int DayCount;
	public string PreviousDate;
	public bool BGM = true;
	public bool SFX = true;
	public bool Vibration = true;
	public bool CheckSecondWeek = false;

	public AchClearState[] AchCollections = new AchClearState[] { AchClearState.Reward0, AchClearState.Reward0, AchClearState.Reward0, AchClearState.Reward0, AchClearState.Reward0, AchClearState.Reward0, AchClearState.Reward0, AchClearState.Reward0, AchClearState.Reward0 };
	public CharacterConditionType[] charState = new CharacterConditionType[] { CharacterConditionType.isEquipment, CharacterConditionType.CanBuy, CharacterConditionType.CanBuy, CharacterConditionType.CannotBuy };
	public int[] AdsCount = new int[] {3,3,-1,-1,-1,-1};
}

public class GameManagerEx
{
	public GameData _gameData = new GameData();

	public GameData SaveData { get { return _gameData; } set { _gameData = value; } }


	public string PlayerID
    {
		get { return _gameData.PlayerID; }
		set { _gameData.PlayerID = value; }
    }
	public int Character
	{
		get { return _gameData.Character; }
		set { _gameData.Character = value; }
	}
	public bool isFirst
	{
		get { return _gameData.isFirst; }
		set { _gameData.isFirst = value; }
	}
	public int BeanCount
	{
		get { return _gameData.BeanCount; }
		set { _gameData.BeanCount = value; }
	}
	
	public int FirstScore
	{
        get { return _gameData.FirstScore; }
		set { _gameData.FirstScore = value; }
	}
	public int SecondScore
	{
        get { return _gameData.SecondScore; }
        set { _gameData.SecondScore = value; }
	}
	public int ThirdScore
	{
        get { return _gameData.ThirdScore; }
        set { _gameData.ThirdScore = value; }
	}
	public int StartDash
	{
        get { return _gameData.StartDash; }
		set { _gameData.StartDash = value; }
	}
	public int Revival
	{
        get { return _gameData.Revival; }
        set { _gameData.Revival = value; }
	}
	public bool NoAds
	{
        get { return _gameData.NoAds; }
        set {  _gameData.NoAds = value; }
	}
	public int PlayCount
	{
		get { return _gameData.PlayCount; }
		set {  _gameData.PlayCount = value; }
	}
	public int BuyItemCount
	{
		get { return _gameData.BuyItemCount; }
		set { _gameData.BuyItemCount = value; }
	}
	public int WatchAdsCount
	{
		get { return _gameData.WatchAdsCount; }
		set { _gameData.WatchAdsCount = value; }
	}
	public int TouchThornCount
	{
		get { return _gameData.TouchThornCount; }
		set { _gameData.TouchThornCount = value; }
	}
	public int RocketStartCount
	{
		get { return _gameData.RocketStartCount; }
		set { _gameData.RocketStartCount = value; }
	}
	public int ReviveCount
	{
		get { return _gameData.ReviveCount; }
		set { _gameData.ReviveCount = value; }
	}
	public int RideWindCount
	{
		get { return _gameData.RideWindCount; }
		set { _gameData.RideWindCount = value; }
	}
	public int MonsterCount
	{
		get { return _gameData.MonsterCount; }
		set { _gameData.MonsterCount = value; }
	}

	public bool UseRevive
    {
        get { return _gameData.UseRevive; }
		set { _gameData.UseRevive = value; }
    }

	public int DayCount
	{
		get { return _gameData.DayCount; }
		set { _gameData.DayCount = value; }
	}
	public string PreviousDate
	{
		get { return _gameData.PreviousDate; }
		set { _gameData.PreviousDate = value; }
	}
	public bool BGM
	{
		get { return _gameData.BGM; }
		set { _gameData.BGM = value; }
	}
	public bool SFX
	{
		get { return _gameData.SFX; }
		set { _gameData.SFX = value; }
	}
	public bool Vibration
	{
		get { return _gameData.Vibration; }
		set { _gameData.Vibration = value; }
	}

	public bool CheckSecondWeek
    {
        get{ return _gameData.CheckSecondWeek; }
        set { _gameData.CheckSecondWeek = value; }
    }

	public bool isRealFirst
    {
		get { return _gameData.isRealFirst; }
		set { _gameData.isRealFirst = value; }
    }

	public AchClearState[] AchCollections {
		get { return _gameData.AchCollections; }
		set { _gameData.AchCollections = value; }
	}

	public CharacterConditionType[] charState
	{
        get { return _gameData.charState; }
		set { _gameData.charState = value; }
	}
	public int[] AdsCount
	{
		get { return _gameData.AdsCount; }
		set { _gameData.AdsCount = value; }
	}


	public void Init()
	{
		StartData data = Managers.Data.Start;
		if (File.Exists(_path))
		{
			string fileStr = File.ReadAllText(_path);
			_gameData = JsonUtility.FromJson<GameData>(fileStr);
		}
	}

	#region Save & Load	
	public string _path = Application.persistentDataPath + "/SaveData.json";
	public void SaveGame()
	{
		string jsonStr = JsonUtility.ToJson(Managers.Game.SaveData);
		File.WriteAllText(_path, jsonStr);
		Debug.Log($"Save Game Completed : {_path}");
	}

	public bool LoadGame()
	{ 
		if (File.Exists(_path) == false)
			return false;

		string fileStr = File.ReadAllText(_path);
		GameData data = JsonUtility.FromJson<GameData>(fileStr);

		if (data != null)
		{
			Managers.Game.SaveData = data;
		}


		Debug.Log($"Save Game Loaded : {_path}");
		return true;
	}

	#endregion
}
