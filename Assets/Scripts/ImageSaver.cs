using UnityEngine;
using System.Collections;
using System;
using System.IO;

public class ImageSaver  
{
	public static void SaveTexture (Texture2D _tex,string saveAs)
	{
		Texture2D tex = _tex;
		byte[] byteArray;
		byteArray = tex.EncodeToPNG ();
		string temp = Convert.ToBase64String (byteArray);
		PlayerPrefs.SetString (saveAs, temp); /// save it to file if u want.
		PlayerPrefs.SetInt (saveAs + "_w", tex.width);
		PlayerPrefs.SetInt (saveAs + "_h", tex.height);
	}

	public static Texture2D RetriveTexture (string savedImageName)
	{
		string temp = PlayerPrefs.GetString (savedImageName);
		int width = PlayerPrefs.GetInt (savedImageName + "_w");
		int height = PlayerPrefs.GetInt (savedImageName + "_h");
		byte[] byteArray = Convert.FromBase64String (temp);
		Texture2D tex = new Texture2D (width, height, TextureFormat.ARGB32, false);
		tex.LoadImage (byteArray);
		return tex;
	}
    public static void SaveTextureForBanner(Texture2D _tex, string imageName, string imageType, string subDir = "")
    {
        Texture2D tex = _tex;
        byte[] byteArray = null;
        try
        {
            if (imageType == "PNG" || imageType == "png")
            {
                byteArray = tex.EncodeToPNG();
            }
            else
            {
                byteArray = tex.EncodeToJPG();
            }
        }
        catch (Exception e)
        {

        }
        try
        {
            string temp = Convert.ToBase64String(byteArray);
            if (string.IsNullOrEmpty(subDir))
            {
                AdIntegrate.instance.writeInToFile(temp, "Banner", imageName + ".txt");
                //File.WriteAllText(Application.persistentDataPath + "/" + imageName + ".txt", temp);
            }
            else
            {
                AdIntegrate.instance.writeInToFile(temp, subDir, imageName + ".txt");
                //File.WriteAllText(Application.persistentDataPath + subDir + "/" + imageName + ".txt", temp);
            }
        }
        catch (Exception e)
        {

        }

        PlayerPrefs.SetInt(imageName + "_w", tex.width);
        PlayerPrefs.SetInt(imageName + "_h", tex.height);
    }

    public static Texture2D RetriveTextureForBanner(string savedImageName, string imageType, string subDir = "", bool setWrapToClamp = false)
    {
        string temp = string.Empty;
        if (string.IsNullOrEmpty(subDir))
        {
            //temp = File.ReadAllText(Application.persistentDataPath + "/" + savedImageName + ".txt");
            temp = AdIntegrate.instance.readFromFile("Banner", savedImageName + ".txt");
        }
        else
        {
            //temp = File.ReadAllText(Application.persistentDataPath + subDir + "/" + savedImageName + ".txt");
            temp = AdIntegrate.instance.readFromFile(subDir, savedImageName + ".txt");
        }
        int width = PlayerPrefs.GetInt(savedImageName + "_w");
        int height = PlayerPrefs.GetInt(savedImageName + "_h");
        byte[] byteArray = null;
        try
        {
            byteArray = Convert.FromBase64String(temp);
        }
        catch (Exception e)
        {

        }

        Texture2D tex = null;
        if (imageType == "PNG" || imageType == "png")
        {
            try
            {
#if UNITY_EDITOR
                tex = new Texture2D(width, height, TextureFormat.DXT5, false);
#elif UNITY_ANDROID
                tex = new Texture2D(width, height, TextureFormat.ETC2_RGBA8, false);
#elif UNITY_IOS
                tex = new Texture2D (1024, 1024, TextureFormat.PVRTC_RGBA4, false);
#endif
            }
            catch (Exception e)
            {

            }
        }
        else
        {
            try
            {
#if UNITY_EDITOR
                tex = new Texture2D(width, height, TextureFormat.DXT5, false);
#elif UNITY_ANDROID
                tex = new Texture2D(width, height, TextureFormat.ETC2_RGB, false);
#elif UNITY_IOS
                tex = new Texture2D (1024, 1024, TextureFormat.RGB24, false);
#endif
            }
            catch (Exception e)
            {

            }
        }
        tex.LoadImage(byteArray);
        if (setWrapToClamp)
            tex.wrapMode = TextureWrapMode.Clamp;
        return tex;
    }
}
