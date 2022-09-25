using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class UI_BagPopup : UI_Popup
{
    enum Buttons
    {
        ExitButton,
        NextButton,
        PreviousButton
    }

    enum Texts
    {
    }

    enum GameObjects
    {
        CharacterContent,
    }

    enum Images
    {
        CharacterButton,
    }


    StartData startData;
    int Charint;
    List<CharacterData> items;
    UI_CharacterItem _characterItems;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;


        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        BindObject(typeof(GameObjects));

        GetButton((int)Buttons.ExitButton).gameObject.BindEvent(OnClickExitButton);
        GetButton((int)Buttons.NextButton).gameObject.BindEvent(OnClickNextButton);
        GetButton((int)Buttons.PreviousButton).gameObject.BindEvent(OnClickPreviousButton);

        Charint = Managers.Game.Character;
        PopulateCharacter();
        

        return true;
    }

    void OnClickNextButton()
    {
        Managers.Sound.Play(Define.Sound.Effect, "Sound_Button");
        if (Charint < 3)
            Charint++;
        else
            Charint = 0;
        _characterItems.SetInfo(Managers.Data.Characters[Charint]);
    }

    void OnClickPreviousButton()
    {
        Managers.Sound.Play(Define.Sound.Effect, "Sound_Button");
        if (Charint > 0)
            Charint--;
        else
            Charint = 3;
        _characterItems.SetInfo(Managers.Data.Characters[Charint]);
    }

    void OnClickExitButton()
    {
        Debug.Log("³ª°¡±â");
        Managers.Sound.Play(Define.Sound.Effect, "Sound_Button");

        Managers.Game.SaveGame();
        Managers.UI.ClosePopupUI(this); // UI_MainPopup
    }

    public void RefreshUI()
    {
        if (_init == false)
            return;

        PopulateCharacter();
    }

    public void PopulateCharacter()
    {
        var parent = GetObject((int)GameObjects.CharacterContent);

        items = new List<CharacterData>();

        foreach (Transform child in parent.transform)
            Managers.Resource.Destroy(child.gameObject);

        foreach (CharacterData characterData in Managers.Data.Characters.Values)
        {
            items.Add(characterData);

            //_characterItems.Add(item);
        }

        _characterItems = Managers.UI.MakeSubItem<UI_CharacterItem>(parent.transform);
        _characterItems.transform.localScale = new Vector3(4, 4, 4);
        _characterItems.SetInfo(Managers.Data.Characters[Charint]);
    }
}
