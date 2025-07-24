#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class SpeedUp : EditorWindow
{
    [MenuItem("Custom Tools/Speed-up gameplay", priority = 27)]
    public static void ShowPrefsWindow()
    {
        GetWindow<SpeedUp>("Speed Up");
    }

    private void OnGUI()
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Slow-down", GUILayout.Height(75), GUILayout.Width(position.width/3)))
        {
            Time.timeScale /= 2;
        }
        if (GUILayout.Button("Time = 1", GUILayout.Height(75), GUILayout.Width(position.width/3)))
        {
            Time.timeScale = 1;
        }
        if (GUILayout.Button("Speed-up", GUILayout.Height(75), GUILayout.Width(position.width/3)))
        {
            Time.timeScale += 2;
        }
        GUILayout.EndHorizontal();

        if (GUILayout.Button("Time = 0", GUILayout.Height(75), GUILayout.Width(position.width)))
        {
            Time.timeScale = 0;
        }
    }

}
#endif