using UnityEditor;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public class DefaultSceneSetter
{
    static DefaultSceneSetter()
    {
        if (EditorPrefs.HasKey(PlayModeSceneSetter.defaultSceneKey))
        {
            var scenePath = EditorPrefs.GetString(PlayModeSceneSetter.defaultSceneKey);
            EditorSceneManager.playModeStartScene = AssetDatabase.LoadAssetAtPath(scenePath, typeof(SceneAsset)) as SceneAsset;
        }
    }
}