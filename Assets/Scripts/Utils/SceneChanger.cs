#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SceneChanger : MonoBehaviour
{

	[MenuItem("ChangeScene/Preloader/Normal")]
	public static void Preloader()
	{
		EditorApplication.OpenScene("Assets/Scenes/Preloader.unity");
	}
    [MenuItem("ChangeScene/Preloader/Save")]
    public static void PreloaderSave()
    {
        EditorApplication.SaveScene();
        EditorApplication.OpenScene("Assets/Scenes/Preloader.unity");
    }

    [MenuItem("ChangeScene/MainMenu/Normal")]
	public static void MainMenu()
	{
		EditorApplication.OpenScene("Assets/Scenes/MainMenu.unity");
	}
    [MenuItem("ChangeScene/MainMenu/Save")]
    public static void MainMenuSave()
    {
        EditorApplication.SaveScene();
        EditorApplication.OpenScene("Assets/Scenes/MainMenu.unity");
    }


	[MenuItem("ChangeScene/Ground/Normal")]
	public static void GroundNormal()
	{
		EditorApplication.OpenScene("Assets/Scenes/Ground.unity");
	}


	[MenuItem("ChangeScene/Ground/Save")]
	public static void GroundNormalSave()
	{
		EditorApplication.SaveScene();
		EditorApplication.OpenScene("Assets/Scenes/Ground.unity");
	}


}
#endif