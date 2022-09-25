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
                //todo : �������� ���ø����̼��� �ٽ� �ø��� ������ ó���� �ൿ��
            }

        }
    }

}