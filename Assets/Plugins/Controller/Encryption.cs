using UnityEngine;
using System.Collections;

public class Encryption {

	public static ArrayList AlphaArray1 = new ArrayList ();
	public static ArrayList AlphaArray2 = new ArrayList ();
	public static ArrayList AlphaArray3 = new ArrayList ();
	public static ArrayList AlphaArray4 = new ArrayList ();
	public static ArrayList AlphaArray5 = new ArrayList ();
	public static ArrayList AlphaArray6 = new ArrayList ();
	public static ArrayList AlphaArray7 = new ArrayList ();
	public static ArrayList AlphaArray8 = new ArrayList ();
	public static ArrayList AlphaArray9 = new ArrayList ();
	public static ArrayList AlphaArray10 = new ArrayList ();
	public static string SpecialCharacterString = "#$%()*+,-.:;<>@";
	public static string SpecialNumericString = "0123456789";
	
	public static void Initialize ()
	{
		AlphaArray1.Add ("A");
		AlphaArray1.Add ("s");
		AlphaArray1.Add ("n");
		AlphaArray1.Add ("L");
		AlphaArray1.Add ("b");
		AlphaArray1.Add ("E");
		AlphaArray1.Add ("W");
		AlphaArray1.Add ("u");
		AlphaArray1.Add ("j");
		AlphaArray1.Add ("o");
		AlphaArray1.Add ("V");
		
		AlphaArray2.Add ("k");
		AlphaArray2.Add ("E");
		AlphaArray2.Add ("y");
		AlphaArray2.Add ("S");
		AlphaArray2.Add ("D");
		AlphaArray2.Add ("q");
		AlphaArray2.Add ("L");
		AlphaArray2.Add ("b");
		AlphaArray2.Add ("i");
		AlphaArray2.Add ("R");
		AlphaArray2.Add ("Z");
		
		AlphaArray3.Add ("n");
		AlphaArray3.Add ("G");
		AlphaArray3.Add ("J");
		AlphaArray3.Add ("A");
		AlphaArray3.Add ("m");
		AlphaArray3.Add ("C");
		AlphaArray3.Add ("h");
		AlphaArray3.Add ("T");
		AlphaArray3.Add ("s");
		AlphaArray3.Add ("Q");
		AlphaArray3.Add ("p");
		
		AlphaArray4.Add ("T");
		AlphaArray4.Add ("Y");
		AlphaArray4.Add ("o");
		AlphaArray4.Add ("l");
		AlphaArray4.Add ("s");
		AlphaArray4.Add ("W");
		AlphaArray4.Add ("B");
		AlphaArray4.Add ("i");
		AlphaArray4.Add ("J");
		AlphaArray4.Add ("X");
		AlphaArray4.Add ("N");
		
		AlphaArray5.Add ("d");
		AlphaArray5.Add ("I");
		AlphaArray5.Add ("n");
		AlphaArray5.Add ("e");
		AlphaArray5.Add ("S");
		AlphaArray5.Add ("h");
		AlphaArray5.Add ("K");
		AlphaArray5.Add ("U");
		AlphaArray5.Add ("M");
		AlphaArray5.Add ("a");
		AlphaArray5.Add ("R");
		
		AlphaArray6.Add ("P");
		AlphaArray6.Add ("a");
		AlphaArray6.Add ("R");
		AlphaArray6.Add ("m");
		AlphaArray6.Add ("S");
		AlphaArray6.Add ("i");
		AlphaArray6.Add ("V");
		AlphaArray6.Add ("C");
		AlphaArray6.Add ("j");
		AlphaArray6.Add ("O");
		AlphaArray6.Add ("q");
		
		AlphaArray7.Add ("n");
		AlphaArray7.Add ("E");
		AlphaArray7.Add ("x");
		AlphaArray7.Add ("t");
		AlphaArray7.Add ("W");
		AlphaArray7.Add ("a");
		AlphaArray7.Add ("V");
		AlphaArray7.Add ("m");
		AlphaArray7.Add ("U");
		AlphaArray7.Add ("l");
		AlphaArray7.Add ("i");
		
		AlphaArray8.Add ("M");
		AlphaArray8.Add ("G");
		AlphaArray8.Add ("A");
		AlphaArray8.Add ("u");
		AlphaArray8.Add ("n");
		AlphaArray8.Add ("J");
		AlphaArray8.Add ("C");
		AlphaArray8.Add ("p");
		AlphaArray8.Add ("i");
		AlphaArray8.Add ("L");
		AlphaArray8.Add ("t");
		
		AlphaArray9.Add ("x");
		AlphaArray9.Add ("B");
		AlphaArray9.Add ("I");
		AlphaArray9.Add ("e");
		AlphaArray9.Add ("R");
		AlphaArray9.Add ("g");
		AlphaArray9.Add ("K");
		AlphaArray9.Add ("n");
		AlphaArray9.Add ("a");
		AlphaArray9.Add ("H");
		AlphaArray9.Add ("s");
		
		AlphaArray10.Add ("z");
		AlphaArray10.Add ("v");
		AlphaArray10.Add ("D");
		AlphaArray10.Add ("o");
		AlphaArray10.Add ("S");
		AlphaArray10.Add ("i");
		AlphaArray10.Add ("L");
		AlphaArray10.Add ("U");
		AlphaArray10.Add ("b");
		AlphaArray10.Add ("M");
		AlphaArray10.Add ("a");
	}
	
	public static string SecuritySystem (int pointsEarned, int randArrayNumber)
	{
		string PointToEncrypt = "" + pointsEarned;
		string EncryptedPoint = "";
		ArrayList SelectedArrayList = new ArrayList ();
		int i;
		
		if (randArrayNumber == 1)
		{
			SelectedArrayList = AlphaArray1;
		}
		else if (randArrayNumber == 2)
		{
			SelectedArrayList = AlphaArray2;
		}
		else if (randArrayNumber == 3)
		{
			SelectedArrayList = AlphaArray3;
		}
		else if (randArrayNumber == 4)
		{
			SelectedArrayList = AlphaArray4;
		}
		else if (randArrayNumber == 5)
		{
			SelectedArrayList = AlphaArray5;
		}
		else if (randArrayNumber == 6)
		{
			SelectedArrayList = AlphaArray6;
		}
		else if (randArrayNumber == 7)
		{
			SelectedArrayList = AlphaArray7;
		}
		else if (randArrayNumber == 8)
		{
			SelectedArrayList = AlphaArray8;
		}
		else if (randArrayNumber == 9)
		{
			SelectedArrayList = AlphaArray9;
		}
		else if (randArrayNumber == 10)
		{
			SelectedArrayList = AlphaArray10;
		}
		
		for (i=0;i<PointToEncrypt.Length;i++)
		{
			int resultIndex = 10;
			string tempStr = "" + PointToEncrypt[i];
			string addJunk = "" + SpecialCharacterString[Random.Range(0, SpecialCharacterString.Length)];
			string addNumeric = "" + SpecialNumericString[Random.Range(0, SpecialNumericString.Length)];
			addJunk += addNumeric;
			if (tempStr != "-")
			{
				resultIndex = int.Parse (tempStr);
			}
			EncryptedPoint += addJunk + SelectedArrayList[resultIndex];
		}
		return EncryptedPoint;
	}


	public static string encryptPointsSystem (long pointsEarned, int keyValue)
	{
		string PointToEncrypt = "" + pointsEarned;
		string EncryptedPoint = "";
		ArrayList SelectedArrayList = new ArrayList ();
		int i;

		if (keyValue == 1)
		{
			SelectedArrayList = AlphaArray1;
		}
		else if (keyValue == 2)
		{
			SelectedArrayList = AlphaArray2;
		}
		else if (keyValue == 3)
		{
			SelectedArrayList = AlphaArray3;
		}
		else if (keyValue == 4)
		{
			SelectedArrayList = AlphaArray4;
		}
		else if (keyValue == 5)
		{
			SelectedArrayList = AlphaArray5;
		}
		else if (keyValue == 6)
		{
			SelectedArrayList = AlphaArray6;
		}
		else if (keyValue == 7)
		{
			SelectedArrayList = AlphaArray7;
		}
		else if (keyValue == 8)
		{
			SelectedArrayList = AlphaArray8;
		}
		else if (keyValue == 9)
		{
			SelectedArrayList = AlphaArray9;
		}
		else if (keyValue == 10)
		{
			SelectedArrayList = AlphaArray10;
		}

		for (i=0;i<PointToEncrypt.Length;i++)
		{
			int resultIndex = 10;
			string tempStr = "" + PointToEncrypt[i];
			string addJunkChar = "" + SpecialCharacterString[Random.Range(0, SpecialCharacterString.Length)];
			string addNumeric = "" + SpecialNumericString[Random.Range(0, SpecialNumericString.Length)];
			addJunkChar += addNumeric;
			if (tempStr != "-")
			{
				resultIndex = int.Parse (tempStr);
			}
			EncryptedPoint += addJunkChar + SelectedArrayList[resultIndex];
		}
		return EncryptedPoint;
	}

	public static string decryptPointsSystem (string encryptedPoint, int keyValue)
	{
		ArrayList SelectedArrayList = new ArrayList ();
		int i;

		if (keyValue == 1)
		{
			SelectedArrayList = AlphaArray1;
		}
		else if (keyValue == 2)
		{
			SelectedArrayList = AlphaArray2;
		}
		else if (keyValue == 3)
		{
			SelectedArrayList = AlphaArray3;
		}
		else if (keyValue == 4)
		{
			SelectedArrayList = AlphaArray4;
		}
		else if (keyValue == 5)
		{
			SelectedArrayList = AlphaArray5;
		}
		else if (keyValue == 6)
		{
			SelectedArrayList = AlphaArray6;
		}
		else if (keyValue == 7)
		{
			SelectedArrayList = AlphaArray7;
		}
		else if (keyValue == 8)
		{
			SelectedArrayList = AlphaArray8;
		}
		else if (keyValue == 9)
		{
			SelectedArrayList = AlphaArray9;
		}
		else if (keyValue == 10)
		{
			SelectedArrayList = AlphaArray10;
		}

		int loopCount = (encryptedPoint.Length / 2) - 1;
		if (loopCount < 1)
		{
			loopCount = 1;
		}
		int startIndex = 0;
		int count = 2;
		int totalPoint = 0;
		string expectedValue = "";
		string decryptedString = "";
		for (i = 0; i < loopCount; i++)
		{
			if (startIndex + count < encryptedPoint.Length)
			{
				encryptedPoint = encryptedPoint.Remove (startIndex, count);
				expectedValue += encryptedPoint[0];
				encryptedPoint = encryptedPoint.Remove (0,1);

				/*	totalPoint = SelectedArrayList.IndexOf (""+expectedValue[i]);
				decryptedString += totalPoint;	*/	//changed this because it doesnt decrypt negative values.
				if(SelectedArrayList.IndexOf (""+expectedValue[i])==10)
				{
					decryptedString += "-";
				}					
				else
				{
					totalPoint = SelectedArrayList.IndexOf (""+expectedValue[i]);
					decryptedString += totalPoint;
				}			
			}
		}
		return decryptedString;
	}

}
