using UnityEngine;

/// Author: Gopinath N

public class DebugLogger 
{
	///<summary>
	/// colorType: 1 = red, 2 = blue, 3 = green, 4= yellow, default = orange
	/// </summary>
	bool canShowDebug = true;

	public static void PrintWithColor(string content,int colortype=4)
    {
		if (Debug.unityLogger.logEnabled) {
			switch (colortype) {
			case 1:
				Debug.Log ("<color=red>" + content + "</color>");
				break;
			case 2:
				Debug.Log ("<color=blue>" + content + "</color>");
				break;
			case 3:
				Debug.Log ("<color=green>" + content + "</color>");
				break;
			case 4:
#if UNITY_EDITOR
					Debug.Log("<color=yellow>" + content + "</color>");
#else
					Debug.Log ("<color=black>" + content + "</color>");
#endif
					break;
			default:
				Debug.Log ("<color=orange>" + content + "</color>");
				break;
			}
		}
        
	}
    
    public static void PrintWithBold(string content)
    {
		if (Debug.unityLogger.logEnabled) {
			Debug.Log ("<b>" + content + "</b>");
		}
    }


    public static void PrintWithItalic(string content)
    {
		if (Debug.unityLogger.logEnabled) {
			Debug.Log ("<i>" + content + "</i>");
		}
    }

    public static void PrintWithSize(string content,int size=20)
    {
		if (Debug.unityLogger.logEnabled) {
			string sizeTxt = "<size=" + size + ">";
			Debug.Log (sizeTxt + content + "</size>");
		}
    }

	public static void Print(string content)
	{
		if (Debug.unityLogger.logEnabled) {
			Debug.Log (content);
		}
	}

	public static void PrintError(string content)
	{
		if (Debug.unityLogger.logEnabled) {
			Debug.LogError (content);
		}
	}


}
