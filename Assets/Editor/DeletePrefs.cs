using System.IO;
using UnityEditor;
using UnityEngine;

public class DeletePrefs
{
    [MenuItem("Custom Tools/Data/Clear Preferences", priority = 1)]
    static void DeleteAllPrefs()
    {
        PlayerPrefs.DeleteAll();
    }

    [MenuItem("Custom Tools/Data/Clear All", priority = 2)]
    static void DeletePersistentDataPath()
    {
        if (Directory.Exists(Application.persistentDataPath))
        {
            Directory.Delete(Application.persistentDataPath, true);
        }
        DeleteAllPrefs();
    }

    [MenuItem("Custom Tools/Data/Show In Finder", priority = 3)]
    static void ShowInFinder()
    {
        EditorUtility.RevealInFinder(Application.persistentDataPath);
    }
}