using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;


public class AdIntegrate : MonoBehaviour
{
	public static AdIntegrate instance;
    public bool isInterstitialAvailable = false;
    public int CurrentSceneIndex;

    protected void Awake ()
	{
		instance = this;
		#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
		CONTROLLER.TargetPlatform = "standalone";
		#endif

		#if UNITY_WEBPLAYER
		CONTROLLER.TargetPlatform = "web";
		#endif

		#if UNITY_ANDROID
		CONTROLLER.TargetPlatform = "android";
		
		#endif

		#if UNITY_IPHONE
		CONTROLLER.TargetPlatform = "ios";
		#endif

		DontDestroyOnLoad (this);


	}

    public void GameExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        QuitTheApp();
    }
    void QuitTheApp()
    {

    }

    public void Start()
	{
        SceneManager.sceneLoaded += OnSceneLoaded;
	}

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CurrentSceneIndex = scene.buildIndex;
        StartCoroutine(checkAndRemoveEventSystems());
    }
    private IEnumerator checkAndRemoveEventSystems()
    {
        yield return new WaitForSecondsRealtime(3.0f);
        UnityEngine.EventSystems.EventSystem[] eventss = FindObjectsOfType<UnityEngine.EventSystems.EventSystem>();
        if (eventss != null && eventss.Length > 1)
        {
            if (CONTROLLER.EnableHardcodes==1)
            {
                Popup.instance.showGenericPopup("EventSystem!", "EventSystem Count:" + eventss.Length);
            }
            for (int i = 1; i < eventss.Length; i++)
            {
                Destroy(eventss[i].gameObject);
            }
        }
        else if ((eventss != null && eventss.Length == 0) || eventss == null)
        {
            GameObject eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<UnityEngine.EventSystems.EventSystem>();
            eventSystem.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
        }
    }



    public void ShowToast(string text)
	{
        GameObject prefabGO ;
		GameObject tempGO ;
		prefabGO = Resources.Load ("Prefabs/Toast")as GameObject ;
		tempGO = Instantiate (prefabGO)as GameObject ;
		tempGO.name = "Toast";
		tempGO.GetComponent <Toast > ().setMessge (text);
    }


    public bool NET_STATE = true;
	public bool checkTheInternet()
	{
        return NET_STATE;
	}

    public static readonly JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings { ObjectCreationHandling = ObjectCreationHandling.Replace };

#region READWRITEFILE FUNCTIONS

    //=================App-specific files======================================================
    public void writeInToFile(string stringData, string folderName, string fileName = "test.dat")
    {
        IsDirectoryExistsElseCreate(folderName);
        File.WriteAllText(Application.persistentDataPath + "/" + folderName + "/" + fileName, stringData);
    }

    public bool IsFileExits(string folderName, string fileName)
    {
        bool isFilePresent = false;
        string filePath = Application.persistentDataPath + "/" + folderName + "/" + fileName;
        if (File.Exists(filePath))
        {
            isFilePresent = true;
        }
        return isFilePresent;
    }

    public bool IsFolderExits(string folderName)
    {
        bool isFolderPresent = false;
        string filePath = Application.persistentDataPath + "/" + folderName;
        DirectoryInfo dirInfo = new DirectoryInfo(filePath);
        if (dirInfo.Exists)
        {
            isFolderPresent = true;
        }
        return isFolderPresent;
    }

    public string readFromFile(string folderName, string fileName = "test.dat")
    {
        string stringData = "";
        bool fileExists = File.Exists(Application.persistentDataPath + "/" + folderName + "/" + fileName);
        if (fileExists)
        {
            stringData = File.ReadAllText(Application.persistentDataPath + "/" + folderName + "/" + fileName);
        }
        return stringData;
    }

    public void deleteFile(string folderName, string fileName = "test.dat")
    {
        File.Delete(Application.persistentDataPath + "/" + folderName + "/" + fileName);
    }

    public void deleteFolder(string folderName)
    {
        if (Directory.Exists(Application.persistentDataPath + "/" + folderName))
        {
            string[] filePaths = Directory.GetFiles(Application.persistentDataPath + "/" + folderName);
            foreach (string filePath in filePaths)
            {
                File.Delete(filePath);
            }
            Directory.Delete(Application.persistentDataPath + "/" + folderName);
        }
    }

    public void deleteTheFileByFullPath(string fullPathFileName)
    {
        File.Delete(Application.persistentDataPath + "/" + fullPathFileName);
    }

    public string GetFilePath()
    {
        string stringData = "";
        stringData = Application.persistentDataPath;
        return stringData;

    }

    private void IsDirectoryExistsElseCreate(string folderName)
    {
        string filePath = Application.persistentDataPath + "/" + folderName;
        DirectoryInfo dirInfo = new DirectoryInfo(filePath);
        if (!dirInfo.Exists)
        {
            dirInfo.Create();
        }
    }
    //================================================================================
#endregion


    public void SystemSleepSettings(int idx=1)
    {
        if(idx==0)
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        else
            Screen.sleepTimeout = SleepTimeout.SystemSetting;
    }

    public void SetTimeScale(float value)
    {
        Time.timeScale = value;
    }

}

