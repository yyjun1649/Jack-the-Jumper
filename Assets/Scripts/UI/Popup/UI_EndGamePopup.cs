using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class UI_EndGamePopup : UI_Popup
{
    enum Buttons
    {
        StartButton,
        EndButton,
    }

    enum Texts
    {
        BeanCount,
        ScoreText,
    }

    enum GameObjects
    {
        NewScore,
    }

    private GamePlayerController mPlayerController;

    StartData startData;
    int Score;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;


        startData = Managers.Data.Start;

        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        BindObject(typeof(GameObjects));

        GetObject((int)GameObjects.NewScore).SetActive(false);
        GetButton((int)Buttons.StartButton).gameObject.BindEvent(OnClickStartButton);
        GetButton((int)Buttons.EndButton).gameObject.BindEvent(OnClickEndButton);
        mPlayerController = FindObjectOfType<GamePlayerController>();

        Score = mPlayerController.Count;
        StartCoroutine(BeanShow());
        GetText((int)Texts.ScoreText).text = Score.ToString();
        GetText((int)Texts.BeanCount).text = "+" + mPlayerController.BeanCount.ToString();



        Managers.UI.SetCanvas(gameObject, true);
        ChangeScore();
        return true;
    }

    IEnumerator BeanShow()
    {
        while (true)
        {
            GetText((int)Texts.BeanCount).text = "+" + mPlayerController.BeanCount.ToString();
            GetText((int)Texts.BeanCount).fontSize = 90;
            yield return new WaitForSecondsRealtime(2f);
            GetText((int)Texts.BeanCount).text = Managers.Game.BeanCount.ToString();
            GetText((int)Texts.BeanCount).fontSize = 100;
            yield return new WaitForSecondsRealtime(2f);
        }
    }
    void OnClickStartButton()
    {
        Time.timeScale = 1;
        Managers.Game.UseRevive = false;
        Managers.Game.SaveGame();
        if (GPGSBinder.Inst.IsConnected)
            GPGSBinder.Inst.SaveCloud("mysave");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Managers.Sound.Play(Define.Sound.Effect, "Sound_Button");
        StopAllCoroutines();
    }

    void OnClickEndButton()
    {
        Time.timeScale = 1;
        Managers.Game.SaveGame();
        if (GPGSBinder.Inst.IsConnected)
            GPGSBinder.Inst.SaveCloud("mysave");
        LoadingSceneManager.LoadScene("MainScene");
        Managers.Sound.Play(Define.Sound.Effect, "Sound_Button");
        Managers.Game.isFirst = false;
        StopAllCoroutines();
    }

    void ChangeScore()
    {
        Time.timeScale = 0.001f;
        if(Score > Managers.Game.FirstScore)
        {
            GetObject((int)GameObjects.NewScore).SetActive(true);
            Managers.Game.SecondScore = Managers.Game.FirstScore;
            Managers.Game.FirstScore = Score;

        }else if (Score > Managers.Game.SecondScore)
        {
            Managers.Game.ThirdScore = Managers.Game.SecondScore;
            Managers.Game.SecondScore = Score;
        }
        else if (Score > Managers.Game.ThirdScore)
        {
            Managers.Game.ThirdScore = Score;
        }
    }


}
