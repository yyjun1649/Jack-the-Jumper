using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Define
{
	public const int MAX_COLLECTION_COUNT = 100;

	public enum UIEvent
	{
		Click,
		Pressed,
		PointerDown,
		PointerUp,
	}

	public enum Scene
	{
		Unknown,
		Dev,
		Game,
	}

	public enum Sound
	{
		Bgm,
		Effect,
		Speech,
		Max,
	}

	public enum AnimState
	{
		None,
		Idle,
		Sweat,
		Walking,
		Working,
		Attack,
	}

	

	#region 텍스트 코드

	public const int Start = 10001;
	public const int Shop = 10002;

	#endregion
}
