using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CustomEditor : EditorWindow
{
    [MenuItem("Window/장애물 생성")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(CustomEditor));
    }

    private void OnGUI()
    {
        GUILayout.Label("드래그해서 쓰시면 됩니다",EditorStyles.boldLabel);
    }
}
