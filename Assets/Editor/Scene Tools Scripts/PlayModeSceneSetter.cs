using UnityEditor;
using UnityEditor.SceneManagement;

public class PlayModeSceneSetter : EditorWindow
{
    SceneAsset sceneAsset = null;
    public static string defaultSceneKey = "defaultScene";

    [MenuItem("Scene Tools/Set Default Play Scene")]
    public static void SetDefaultPlayScene()
    {
        GetWindow<PlayModeSceneSetter>(true, "Select Scene");
    }

    private void OnGUI()
    {
        if (sceneAsset == null)
        {
            if (EditorPrefs.HasKey(defaultSceneKey))
            {
                var scenePath = EditorPrefs.GetString(defaultSceneKey);
                sceneAsset = AssetDatabase.LoadAssetAtPath(scenePath, typeof(SceneAsset)) as SceneAsset;
            }
        }
        EditorGUI.BeginChangeCheck();
        sceneAsset = EditorGUILayout.ObjectField("Default Play Scene: ",sceneAsset, typeof(SceneAsset), false) as SceneAsset;
        if (EditorGUI.EndChangeCheck())
        {
            EditorPrefs.SetString(defaultSceneKey, AssetDatabase.GetAssetPath(sceneAsset));
            EditorSceneManager.playModeStartScene = sceneAsset;
        }
    }
}