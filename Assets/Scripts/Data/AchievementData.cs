using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public enum AchState
{
	Clear,
	UnClear,
}

public enum AchType
{
	Daily,
	Basic,
	More
}

public class AchItemData
{
	[XmlAttribute]
	public int ID;
	[XmlAttribute]
	public string Name;
    [XmlAttribute]
	public AchType Type;
	[XmlAttribute]
	public AchState condition;
	[XmlAttribute]
	public float MaxProgress;
	[XmlAttribute]
	public int StarCount;
	[XmlAttribute]
	public int Save_1;
	[XmlAttribute]
	public string Save_1Reward;
	[XmlAttribute]
	public int Save_1RewardCount;
    [XmlAttribute]
	public AchState Save_1Clear;
	[XmlAttribute]
	public int Save_2;
	[XmlAttribute]
	public string Save_2Reward;
	[XmlAttribute]
	public int Save_2RewardCount;
	[XmlAttribute]
	public AchState Save_2Clear;
	[XmlAttribute]
	public int Save_3;
	[XmlAttribute]
	public string Save_3Reward;
	[XmlAttribute]
	public int Save_3RewardCount;
	[XmlAttribute]
	public AchState Save_3Clear;
	[XmlAttribute]
	public bool isSave1;
	[XmlAttribute]
	public bool isSave2;
	[XmlAttribute]
	public bool isSave3;
}

[Serializable, XmlRoot("ArrayOfAchItemData")]
public class AchItemDataLoader : ILoader<int, AchItemData>
{
	[XmlElement("AchItemData")]
	public List<AchItemData> _achItemData = new List<AchItemData>();

	public Dictionary<int, AchItemData> MakeDic()
	{
		Dictionary<int, AchItemData> dic = new Dictionary<int, AchItemData>();

		foreach (AchItemData data in _achItemData)
			dic.Add(data.ID, data);

		return dic;
	}

	public bool Validate()
	{
		return true;
	}
}