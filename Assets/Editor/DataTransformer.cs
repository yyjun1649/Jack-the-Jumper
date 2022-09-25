using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using System.Xml.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System;
using UnityEngine;
using static Define;
using System.Linq;

public class DataTransformer : EditorWindow
{
	[MenuItem("Tools/RemoveSaveData")]
	public static void RemoveSaveData()
	{
		string path = Application.persistentDataPath + "/SaveData.json";
		if (File.Exists(path))
		{
			File.Delete(path);
			Debug.Log("SaveFile Deleted");
		}
		else
		{
			Debug.Log("No SaveFile Detected");
		}
	}

	[MenuItem("Tools/ParseExcel")]
	public static void ParseExcel()
	{
		ParseStartData();
		ParseAchItemData();
		ParseShopItemData();
	}

	static void ParseStartData()
	{
		StartData startData;

		#region ExcelData
		string[] lines = Resources.Load<TextAsset>($"Data/Excel/StartData").text.Split("\n");

		// 두번째 라인까지 스킵
		string[] row = lines[2].Replace("\r", "").Split(',');

		startData = new StartData()
		{
			ID = int.Parse(row[0]),
		};
		#endregion

		string xmlString = ToXML(startData);
		File.WriteAllText($"{Application.dataPath}/Resources/Data/StartData.xml", xmlString);
		AssetDatabase.Refresh();
	}

	static void ParseAchItemData()
    {

    }
	
	static void ParseShopItemData()
    {
		List<ShopItemData> shopDatas = new List<ShopItemData>();

		#region ExcelData
		string[] lines = Resources.Load<TextAsset>($"Data/Excel/ShopItemData").text.Split("\n");

		// 첫번째 라인까지 스킵
		for (int y = 2; y < lines.Length; y++)
		{
			string[] row = lines[y].Replace("\r", "").Split(',');
			if (row.Length == 0)
				continue;
			if (string.IsNullOrEmpty(row[0]))
				continue;

			ShopItemData shopData = new ShopItemData()
			{
				ID = int.Parse(row[0]),
				ItemName = row[1],
				productID = row[2],
				ItemCount = int.Parse(row[3]),
				price = int.Parse(row[4]),
				condition = (row[5] == "CanBuy" ? ShopItemType.CanBuy : ShopItemType.CannotBuy),
				cashType = (row[6] == "Cash" ? CashType.Cash : row[6] == "Ads" ? CashType.Ads : CashType.Bean),
				ItemIcon = row[7],
				folderType = (row[8] == "Item" ? FolderType.Item : FolderType.Package)
			};

			shopDatas.Add(shopData);

		}
		#endregion

		string xmlString = ToXML(new ShopItemDataLoader() { _shopItemData = shopDatas });
		File.WriteAllText($"{Application.dataPath}/Resources/Data/ShopItemData.xml", xmlString);
		AssetDatabase.Refresh();
	}

	#region XML 유틸
	public sealed class ExtentedStringWriter : StringWriter
	{
		private readonly Encoding stringWriterEncoding;

		public ExtentedStringWriter(StringBuilder builder, Encoding desiredEncoding) : base(builder)
		{
			this.stringWriterEncoding = desiredEncoding;
		}

		public override Encoding Encoding
		{
			get
			{
				return this.stringWriterEncoding;
			}
		}
	}

	public static string ToXML<T>(T obj)
	{
		using (ExtentedStringWriter stringWriter = new ExtentedStringWriter(new StringBuilder(), Encoding.UTF8))
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
			xmlSerializer.Serialize(stringWriter, obj);
			return stringWriter.ToString();
		}
	}
	#endregion
}
