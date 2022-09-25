using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_CharacterItem : UI_Base
{
	enum Buttons
	{
		BuyButton,
		EquipmentButton,
	}

	enum Texts
	{
		CharacterName,
		BuyText,
		EquipmentText,
		IntroduceText,
		SpeicalStatusText,
	}

	enum GameObjects
	{
		Icon,
		Character,
		StatusFolder
	}

	[SerializeField]
	private SpriteRenderer _spriteRenderer;

	[SerializeField]
	private Image _image;

	[SerializeField]
	private Sprite unSelectButton;
	[SerializeField]
	private Sprite selectButton;

	CharacterData _characterData;


	public override bool Init()
	{
		if (base.Init() == false)
			return false;

		BindButton(typeof(Buttons));
		BindText(typeof(Texts));
		BindObject(typeof(GameObjects));

		GetButton((int)Buttons.BuyButton).gameObject.BindEvent(OnClickBuyButton);
		GetButton((int)Buttons.EquipmentButton).gameObject.BindEvent(OnClickEquipButton);



		if (_spriteRenderer == null)
			_spriteRenderer = this.GetComponent<SpriteRenderer>();

		_spriteRenderer.enabled = false;

		if (_image == null)
			_image = this.GetComponent<Image>();



		RefreshUI();
		StartCoroutine(ChangeCoroutine());
		return true;
	}
	public void SetInfo(CharacterData characterData)
	{
		_characterData = characterData;
		RefreshUI();
	}

	void RefreshUI()
	{
		if (_init == false)
			return;
		GetObject((int)GameObjects.Icon).GetOrAddComponent<BaseController>().SetImage(_characterData.icon);
		GetObject((int)GameObjects.Character).GetOrAddComponent<BaseController>().SetAnim(_characterData.icon);
		GetText((int)Texts.CharacterName).text = _characterData.Name;
		GetText((int)Texts.IntroduceText).text = _characterData.introduce;
		GetText((int)Texts.SpeicalStatusText).text = _characterData.SpecialStatus;

		if (_characterData.price > Managers.Game.BeanCount)
			GetButton((int)Buttons.BuyButton).interactable = false;


		if (Managers.Game.charState[_characterData.ID] == CharacterConditionType.CanBuy)
		{
			GetButton((int)Buttons.BuyButton).gameObject.SetActive(true);
			GetText((int)Texts.BuyText).text = $"{_characterData.price}";
			GetButton((int)Buttons.EquipmentButton).gameObject.SetActive(false);
		}
		else if (Managers.Game.charState[_characterData.ID] == CharacterConditionType.isnotEquipment)
		{
			GetButton((int)Buttons.EquipmentButton).gameObject.SetActive(true);
			GetButton((int)Buttons.EquipmentButton).interactable = true;
			GetText((int)Texts.EquipmentText).text = "¿Â¬¯";
			GetButton((int)Buttons.BuyButton).gameObject.SetActive(false);
		}
        else if (Managers.Game.charState[_characterData.ID] == CharacterConditionType.isEquipment)
		{ 
			GetButton((int)Buttons.EquipmentButton).gameObject.SetActive(true);
			GetButton((int)Buttons.EquipmentButton).interactable = false;
			GetText((int)Texts.EquipmentText).text = "¿Â¬¯¡ﬂ";
			GetButton((int)Buttons.BuyButton).gameObject.SetActive(false);
		}
		else if (Managers.Game.charState[_characterData.ID] == CharacterConditionType.CannotBuy)
        {
			GetButton((int)Buttons.EquipmentButton).gameObject.SetActive(true);
			GetButton((int)Buttons.EquipmentButton).interactable = false;
			GetText((int)Texts.EquipmentText).text = "√‚ºÆ ∫∏ªÛ";
			GetButton((int)Buttons.BuyButton).gameObject.SetActive(false);
		}
	}

	IEnumerator ChangeCoroutine()
	{
		while (true)
		{
			_image.sprite = _spriteRenderer.sprite;
			yield return null;
		}
	}

	void OnClickBuyButton()
	{
		Debug.Log("OnClickButton");

		if (GetButton((int)Buttons.BuyButton).interactable == true)
		{
			if (Managers.Game.charState[_characterData.ID] == CharacterConditionType.CanBuy)
			{
				Managers.Game.charState[_characterData.ID] = CharacterConditionType.isnotEquipment;
				Managers.Game.BeanCount -= _characterData.price;
			}

			Managers.UI.FindPopup<UI_MainPopup>().RefreshUI();
			Managers.UI.FindPopup<UI_MainTitlePopup>().RefreshUI();
			Managers.UI.FindPopup<UI_BagPopup>().RefreshUI();
			Managers.Game.SaveGame();
			if (GPGSBinder.Inst.IsConnected)
				GPGSBinder.Inst.SaveCloud("mysave");
		}
	}

	void OnClickEquipButton()
    {
		if (Managers.Game.charState[_characterData.ID] == CharacterConditionType.isnotEquipment)
		{
			for(int i = 0; i < 4; i++)
            {
				if (Managers.Game.charState[i] == CharacterConditionType.isEquipment)
					Managers.Game.charState[i] = CharacterConditionType.isnotEquipment;


			}
			Managers.Game.Character = _characterData.ID;
			Managers.Game.charState[_characterData.ID] = CharacterConditionType.isEquipment;
			Debug.Log(Managers.Game.Character);

			Managers.UI.FindPopup<UI_MainTitlePopup>().RefreshUI();
			Managers.UI.FindPopup<UI_BagPopup>().RefreshUI();
			Managers.Game.SaveGame();
			if (GPGSBinder.Inst.IsConnected)
				GPGSBinder.Inst.SaveCloud("mysave");
		}
	}
}
