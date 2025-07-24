using UnityEditor;
using UnityEngine;

public class PrefsProcessor : EditorWindow
{
    enum DataType
    {
        String = 0,
        Int = 1,
        Float = 2,
    }

    private string keyInput = string.Empty, getString = string.Empty, previousGetString = string.Empty, setString = string.Empty, previousSetString = string.Empty;
    private bool isValidData = true, gotValue = false;
    private string errorMessage = "Data Type Incorrect!";
    private DataType dataType;
    private Vector2 scrollPosition = Vector2.zero;

    [MenuItem("Custom Tools/Preference Tool", priority = 25)]
    public static void ShowPrefsWindow()
    {
        GetWindow<PrefsProcessor>("Pref Tools");
    }

    private void OnGUI()
    {
        //PlayerPrefs.SetString("MyKey", "HiThere");
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, true);
        GUILayout.Label("Preference Tool", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        string _keyInput = EditorGUILayout.TextField("Key", keyInput);
        DataType _dataType = (DataType)EditorGUILayout.EnumPopup(dataType, GUILayout.Width(70));
        if (_dataType != dataType)
        {
            dataType = _dataType;
            ResetVariables();
        }
        if (_keyInput != keyInput)
        {
            keyInput = _keyInput;
            ResetVariables();
        }
        GUILayout.EndHorizontal();
        if (keyInput != string.Empty)
        {
            if (PlayerPrefs.HasKey(keyInput))
            {
                previousSetString = string.Empty;
                setString = string.Empty;
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUI.SetNextControlName("GETKEY");
                if (GUILayout.Button("GET KEY", GUILayout.Width(150)))
                {
                    GUI.FocusControl("GETKEY");
                    OnGetKey();
                }
                if (GUILayout.Button("DELETE KEY", GUILayout.Width(150)))
                {
                    isValidData = true;
                    if (PlayerPrefs.HasKey(keyInput))
                    {
                        PlayerPrefs.DeleteKey(keyInput);
                    }
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                if (gotValue)
                {
                    if (!isValidData)
                    {
                        ShowErrorMessage();
                    }
                    else
                    {
                        EditorStyles.textArea.wordWrap = true;
                        GUILayout.Label("Value", EditorStyles.boldLabel);
                        GUILayout.BeginHorizontal();
                        getString = EditorGUILayout.TextArea(getString, EditorStyles.textArea);
                        if (getString != previousGetString)
                        {
                            if (GUILayout.Button("SET KEY", GUILayout.Width(70)))
                            {
                                switch (dataType)
                                {
                                    case DataType.String:
                                        PlayerPrefs.SetString(keyInput, getString);
                                        isValidData = true;
                                        break;
                                    case DataType.Int:
                                        int intNumber;
                                        isValidData = int.TryParse(getString, out intNumber);
                                        if (isValidData)
                                        {
                                            PlayerPrefs.SetInt(keyInput, intNumber);
                                        }
                                        break;
                                    case DataType.Float:
                                        float floatNumber;
                                        isValidData = float.TryParse(getString, out floatNumber);
                                        if (isValidData)
                                        {
                                            PlayerPrefs.SetFloat(keyInput, floatNumber);
                                        }
                                        break;
                                }
                                if (!isValidData)
                                {
                                    ShowErrorMessage();
                                }
                                else
                                {
                                    previousGetString = getString;
                                }
                            }
                        }
                        GUILayout.EndHorizontal();
                    }
                }
            }
            else
            {
                gotValue = false;
                previousGetString = string.Empty;
                getString = string.Empty;
                GUILayout.BeginHorizontal();
                EditorStyles.textField.wordWrap = true;
                EditorGUILayout.PrefixLabel(new GUIContent(text: "Value"));
                setString = EditorGUILayout.TextArea(setString, EditorStyles.textField);
                if (previousSetString != setString)
                {
                    isValidData = true;
                    previousSetString = setString;
                }
                if (setString != string.Empty)
                {
                    if (GUILayout.Button("SET KEY", GUILayout.Width(70)))
                    {
                        switch (dataType)
                        {
                            case DataType.String:
                                PlayerPrefs.SetString(keyInput, setString);
                                isValidData = true;
                                break;
                            case DataType.Int:
                                int intNumber;
                                isValidData = int.TryParse(setString, out intNumber);
                                if (isValidData)
                                {
                                    PlayerPrefs.SetInt(keyInput, intNumber);
                                }
                                break;
                            case DataType.Float:
                                float floatNumber;
                                isValidData = float.TryParse(setString, out floatNumber);
                                if (isValidData)
                                {
                                    PlayerPrefs.SetFloat(keyInput, floatNumber);
                                }
                                break;
                        }
                    }
                    if (!isValidData)
                    {
                        ShowErrorMessage();
                    }
                    else
                    {
                        OnGetKey();
                    }
                }
                GUILayout.EndHorizontal();
            }
        }
        GUILayout.EndScrollView();
    }

    private void ShowErrorMessage()
    {
        EditorGUILayout.HelpBox(errorMessage, MessageType.Error);
    }

    private void ResetVariables()
    {
        gotValue = false;
        isValidData = true;
        getString = string.Empty;
        previousGetString = string.Empty;
        setString = string.Empty;
        previousSetString = string.Empty;
    }

    private void OnGetKey()
    {
        if (PlayerPrefs.HasKey(keyInput))
        {
            gotValue = true;
            switch (dataType)
            {
                case DataType.String:
                    getString = PlayerPrefs.GetString(keyInput, "ERR0RNIL");
                    isValidData = !getString.Equals("ERR0RNIL");
                    break;
                case DataType.Int:
                    getString = PlayerPrefs.GetString(keyInput, "ERR0RNIL").ToString();
                    isValidData = getString.Equals("ERR0RNIL");
                    int intNumber = PlayerPrefs.GetInt(keyInput, int.MinValue);
                    getString = intNumber.ToString();
                    isValidData &= intNumber != int.MinValue;
                    break;
                case DataType.Float:
                    float floatNumber = PlayerPrefs.GetFloat(keyInput, float.MinValue);
                    isValidData = floatNumber != float.MinValue;
                    getString = floatNumber.ToString();
                    break;
            }
            if (isValidData)
            {
                previousGetString = getString;
            }
        }
    }
}
