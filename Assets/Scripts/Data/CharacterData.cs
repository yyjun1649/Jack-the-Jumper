using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class CharacterData
{
	[XmlAttribute]
	public int ID;
	[XmlAttribute]
	public string Name;
	[XmlAttribute]
	public int price;
	[XmlAttribute]
	public string SpecialStatus;
	[XmlAttribute]
	public string introduce;
	[XmlAttribute]
	public string icon;
	[XmlAttribute]
	public string icon2;
}

[Serializable, XmlRoot("ArrayOfCharacterData")]
public class CharacterDataLoder : ILoader<int, CharacterData>
{
	[XmlElement("CharacterData")]
	public List<CharacterData> _characterData = new List<CharacterData>();

	public Dictionary<int, CharacterData> MakeDic()
	{
		Dictionary<int, CharacterData> dic = new Dictionary<int, CharacterData>();

		foreach (CharacterData data in _characterData)
			dic.Add(data.ID, data);

		return dic;
	}

	public bool Validate()
	{
		return true;
	}
}