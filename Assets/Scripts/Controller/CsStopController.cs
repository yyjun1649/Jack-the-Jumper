using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CsStopController : MonoBehaviour
{
    bool bPaused = false;
    public bool isAds = false;
    void OnApplicationQuit()
    {
        Application.CancelQuit();

#if !UNITY_EDITOR

        System.Diagnostics.Process.GetCurrentProcess().Kill();

#endif

    }

    void OnApplicationPause(bool pause)
    {
        if (pause) {
            bPaused = true;
            if(!isAds)
                Managers.UI.ShowPopupUI<UI_PausePopup>();
        }
        else {
            if (bPaused) {
                bPaused = false;
                //todo : 내려놓은 어플리케이션을 다시 올리는 순간에 처리할 행동들
            }

        }
    }

}