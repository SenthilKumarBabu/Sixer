using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public enum Scenes
{
    Preloader,
    MainMenu,
    Ground
}

public static class ManageScene
{

    static Scenes curScene;
    /// <summary>
    /// The current scene type
    /// </summary>
    public static AsyncOperation async;

    public static Scenes CurScene
    {
        get
        {
            return curScene;
        }

        private set
        {
            curScene = value;
        }
    }

    public static string ActiveSceneName()
    {
        Scene _scene = SceneManager.GetActiveScene();
        return _scene.name;
    }

    public static void LoadScene(string _sceneName)
    {
        SceneManager.LoadScene(_sceneName, LoadSceneMode.Single);
        CurScene = (Scenes)System.Enum.Parse(typeof(Scenes), _sceneName);
    }

    public static void LoadScene(Scenes scene)
    {
        SceneManager.LoadScene(scene.ToString(), LoadSceneMode.Single);
        CurScene = scene;
    }

    public static AsyncOperation LoadSceneAsync(string _sceneName)
    {
        async = SceneManager.LoadSceneAsync(_sceneName, LoadSceneMode.Single);
        CurScene = (Scenes)System.Enum.Parse(typeof(Scenes), _sceneName);
        return async;
    }

    public static void SetCurrentScene(string sceneName)
    {
        CurScene = (Scenes)System.Enum.Parse(typeof(Scenes), sceneName);
    }

    public static void SetCurrentScene(Scenes scene)
    {
        CurScene = scene;
    }
}
