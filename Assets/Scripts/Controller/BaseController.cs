using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
using System;
using UnityEngine.UI;

public class BaseController : UI_Base
{
	Image _image;
	Animator _animator;
	public override bool Init()
	{
		if (base.Init() == false)
			return false;
		_animator = GetComponent<Animator>();
		_image = GetComponent<Image>();
		return true;
	}

	
	public void SetImage(string path)
	{
		Init();
		_image.sprite = Managers.Resource.Load<Sprite>(path);
	}

	public void SetAnim(string path)
    {
		Init();
		_animator.runtimeAnimatorController = Resources.Load(path) as RuntimeAnimatorController;
	}
}
