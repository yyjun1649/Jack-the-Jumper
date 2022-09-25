using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_MainTitlePopup : UI_Popup
{
    enum Buttons
    {
        ModeSelectButton,
    }

    enum Texts
    {
        ModeSelectText,
    }

    enum GameObjects
    {
        Character,
        Icon,
    }

    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    [SerializeField]
    private Image _image;

    StartData _startData;
    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        Managers.Game.UseRevive = false;

        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        BindObject(typeof(GameObjects));

        GetButton((int)Buttons.ModeSelectButton).gameObject.BindEvent(OnClickModeSelectButton);

        _startData = Managers.Data.Start;

        if (_spriteRenderer == null)
            _spriteRenderer = this.GetComponent<SpriteRenderer>();

        _spriteRenderer.enabled = false;

        if (_image == null)
            _image = this.GetComponent<Image>();

        StartCoroutine(ChangeCoroutine());
        RefreshUI();
        Managers.UI.SetCanvas(gameObject, true);
        Managers.Game.SaveGame();
        return true;
    }

    public void RefreshUI()
    {
        if (_init == false)
            return;

        foreach (CharacterData characterData in Managers.Data.Characters.Values) {
            if (Managers.Game.Character == characterData.ID)
            {
                GetObject((int)GameObjects.Character).GetOrAddComponent<BaseController>().SetAnim(characterData.icon);
                break;
            }
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

    void OnClickModeSelectButton()
    {
        Debug.Log("게임시작");
        Managers.Sound.Play(Define.Sound.Effect, "Sound_Button");
        if (GPGSBinder.Inst.IsConnected)
            GPGSBinder.Inst.SaveCloud("mysave");
        Managers.Game.Init();
        Managers.Game.SaveGame();
        Managers.UI.ClosePopupUI(this);
        LoadingSceneManager.LoadScene("PlayScene");
    }

}
