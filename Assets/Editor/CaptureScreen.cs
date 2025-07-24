using System;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class CaptureScreen {
	
    [MenuItem("Custom Tools/ScreenShot #P")]
	public static void PrintScreen()
	{
        DirectoryInfo directoryInfo = new DirectoryInfo(Application.dataPath);
        directoryInfo = directoryInfo.Parent;
        directoryInfo = new DirectoryInfo(Path.Combine(directoryInfo.FullName, "ScreenShots"));
        if (!directoryInfo.Exists)
        {
            directoryInfo.Create();
        }
		var path = Path.Combine(directoryInfo.FullName, "ScreenShot_" + Screen.width + "X" + Screen.height + "_" + GetTimeStamp(DateTime.Now) + ".png");
        ScreenCapture.CaptureScreenshot(path);
	}
	
	public static string GetTimeStamp(this DateTime value)
	{
		return value.ToString("yyyyMMddHHmmssffff");
	}

    [MenuItem("Custom Tools/Inspect Transform Positions")]
    public static void InspectPositions()
    {
        var selectedGameObject = Selection.activeGameObject;
    }
}
