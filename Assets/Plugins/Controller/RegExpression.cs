using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

public class RegExpression
{
	public const string MatchEmailPattern = @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
        
        + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
            
        + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
            
        + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";
        
	public static bool isValidEmail (string email)
    {
        if (email != null)
            return Regex.IsMatch (email, MatchEmailPattern);
        else
            return false;
    }
}