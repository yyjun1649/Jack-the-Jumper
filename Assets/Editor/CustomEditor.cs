using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CustomEditor : EditorWindow
{
    [MenuItem("Window/��ֹ� ����")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(CustomEditor));
    }

    private void OnGUI()
    {
        GUILayout.Label("�巡���ؼ� ���ø� �˴ϴ�",EditorStyles.boldLabel);
    }
}
