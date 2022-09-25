using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PausePopup : UI_Popup
{
    enum Buttons
    {
        StartButton,
        EndButton,
        MusicButton,
    }

    enum Texts
    {
        NumText,
    }

    enum GameObjects
    {
        Folder,
    }

    [SerializeField]
    private Sprite MusicOn;
    [SerializeField]
    private Sprite MusicOff;


    private EnemyController enemyController;
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        BindObject(typeof(GameObjects));
        GetText((int)Texts.NumText).text = "3";
        GetText((int)Texts.NumText).gameObject.SetActive(false);
        GetButton((int)Buttons.StartButton).gameObject.BindEvent(OnClickStartButton);
        GetButton((int)Buttons.EndButton).gameObject.BindEvent(OnClickEndButton);
        GetButton((int)Buttons.MusicButton).gameObject.BindEvent(OnClickMusicButton);

        enemyController = FindObjectOfType<EnemyController>();
        enemyController.pause = true;

        if(Managers.Game.BGM)
            GetButton((int)Buttons.MusicButton).transform.GetComponent<Image>().sprite = MusicOn;
        else
            GetButton((int)Buttons.MusicButton).transform.GetComponent<Image>().sprite = MusicOff;

        Time.timeScale = 0f;
        Managers.UI.SetCanvas(gameObject, true);
        return true;
    }

    void OnClickStartButton()
    {
        StartCoroutine(StartCor());
    }

    void OnClickEndButton()
    {
        Time.timeScale = 1;
        Managers.Game.isFirst = false;
        Managers.Game.SaveGame();
        if (GPGSBinder.Inst.IsConnected)
            GPGSBinder.Inst.SaveCloud("mysave");
        LoadingSceneManager.LoadScene("MainScene");
    }

    void OnClickMusicButton()
    {
        Managers.Game.BGM = !Managers.Game.BGM;
        if (Managers.Game.BGM)
        {
            GetButton((int)Buttons.MusicButton).transform.GetComponent<Image>().sprite = MusicOn;
            Managers.Sound.SetVolume(Define.Sound.Bgm, 0.5f);
        }
        else
        {
            GetButton((int)Buttons.MusicButton).transform.GetComponent<Image>().sprite = MusicOff;
            Managers.Sound.SetVolume(Define.Sound.Bgm, 0f);
        }
        Managers.Sound.Play(Define.Sound.Effect, "Sound_Button");
        Managers.Game.SaveGame();
    }

    IEnumerator StartCor()
    {
        GetObject((int)GameObjects.Folder).SetActive(false);

        Time.timeScale = 0.001f;
        GetText((int)Texts.NumText).gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(0.5f);
        GetText((int)Texts.NumText).text = "2";
        yield return new WaitForSecondsRealtime(0.5f);
        GetText((int)Texts.NumText).text = "1";
        yield return new WaitForSecondsRealtime(0.5f);
        Managers.UI.ClosePopupUI(this);

        enemyController.pause = false;
        Time.timeScale = 1;
    }
}
