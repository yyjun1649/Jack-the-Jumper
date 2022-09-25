using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public enum ShopItemType
{
	CanBuy,
	CannotBuy,
}

public enum FolderType
{
	Item,
	Package,
}

public enum CashType
{
	Bean,
	Cash,
	Ads,
}

public class ShopItemData
{
	[XmlAttribute]
	public int ID;
	[XmlAttribute]
	public string ItemName;
	[XmlAttribute]
	public string productID;
	[XmlAttribute]
	public int ItemCount;
	[XmlAttribute]
	public int price;
	[XmlAttribute]
	public ShopItemType condition;
	[XmlAttribute]
	public CashType cashType;
	[XmlAttribute]
	public string ItemIcon;
    [XmlAttribute]
	public FolderType folderType;
}

[Serializable, XmlRoot("ArrayOfShopItemData")]
public class ShopItemDataLoader : ILoader<int, ShopItemData>
{
	[XmlElement("ShopItemData")]
	public List<ShopItemData> _shopItemData = new List<ShopItemData>();

	public Dictionary<int, ShopItemData> MakeDic()
	{
		Dictionary<int, ShopItemData> dic = new Dictionary<int, ShopItemData>();

		foreach (ShopItemData data in _shopItemData)
			dic.Add(data.ID, data);

		return dic;
	}

	public bool Validate()
	{
		return true;
	}
}