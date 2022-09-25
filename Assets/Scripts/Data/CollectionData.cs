using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

// ����, �� ��

public class CollectionData
{
	[XmlAttribute]
	public int ID;
	[XmlAttribute]
	public string Name;

}

[Serializable, XmlRoot("ArrayOfCollectionData")]
public class CollectionDataLoader : ILoader<int, CollectionData>
{
	[XmlElement("CollectionData")]
	public List<CollectionData> _collectionData = new List<CollectionData>();

	public Dictionary<int, CollectionData> MakeDic()
	{
		Dictionary<int, CollectionData> dic = new Dictionary<int, CollectionData>();

		foreach (CollectionData data in _collectionData)
			dic.Add(data.ID, data);

		return dic;
	}

	public bool Validate()
	{
		return true;
	}
}