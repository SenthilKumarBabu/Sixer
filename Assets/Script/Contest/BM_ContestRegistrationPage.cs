using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BM_ContestRegistrationPage : MonoBehaviour
{
   public static BM_ContestRegistrationPage instance;
    public GameObject Holder;
    public Text Title, Desc;
	public InputField GivenMailID,GivenMobileNumber;
    public GameObject AcceptTick;

    [HideInInspector]
    public string ContestTitle, ContestDesc, ContestTermCondt;

    private void Awake()
    {
        instance = this;
    }

    public void ShowMe()
    {
        if(instance==null)
            instance = this;
        pageFrom = -1;
        gameObject.SetActive(true);
        Holder.SetActive(true);
        Title.text = ContestTitle;
        Desc.text = ContestDesc;

        GivenMailID.text = string.Empty;
        GivenMobileNumber.text = string.Empty;
        SingleBtnPopup.SetActive(false);
        AcceptTick.SetActive(false);
    }

    public void HideMe()
    {
        gameObject.SetActive(false);
        MultiplayerPage.instance.ContinueToBatMP();       
        GivenMobileNumber.text = string.Empty;
        SingleBtnPopup.SetActive(false);
        Holder.SetActive(false);
    }

    private int pageFrom = -1;
	public void SetPageFrom(int val)
    {
        pageFrom = val;
    }
           
    public void goToContestURL ()
	{
		Application.OpenURL (ContestTermCondt);
	}
        
    public void SubmitButtonEvent()
    {
        if(!AcceptTick.activeSelf)
        {
            showErrorPopup(text: "Please accept terms and condtions");
            return;
        }
        if (string.IsNullOrEmpty(GivenMailID.text))
        {
            showErrorPopup(text:"Please enter your email id");
            return;
        }
        if (GivenMailID.text.Length < 5)
        {
            showErrorPopup(text: "Invalid email id!");
            return;
        }
        if (GivenMailID.text.Length >= 5)
        {
            GivenMailID.text = RemoveSpaces(GivenMailID.text);
            bool IsVerifiedEmail, IsValidEmail;
            IsVerifiedEmail = VerifyEmailAddress(GivenMailID.text);
            IsValidEmail = RegExpression.isValidEmail(GivenMailID.text);
            if (!IsVerifiedEmail || !IsValidEmail)
            {
                showErrorPopup(text: "Please enter a valid email id");
                return;
            }

            if (string.IsNullOrEmpty(GivenMobileNumber.text))
            {
                showErrorPopup(text:"Please enter your mobile number");
                return;
            }
            if (GivenMobileNumber.text.Length < 10)
            {
                showErrorPopup(text:"Please enter your valid mobile number");
                return;
            }
            StartCoroutine(sendDataToServer());
        }

    }
    IEnumerator sendDataToServer()
	{
        MultiplayerPage.instance.ShowLoadingScreen();
        yield return 0;// StartCoroutine(NetworkManager.Instance.CheckInternetConnection());
        if (!AdIntegrate.instance.checkTheInternet())// NetworkManager.Instance.IsNetworkConnected)
        {
            MultiplayerPage.instance.HideLoadingScreen();
            showErrorPopup("Check your Internet Connection");
		}
		else
		{
            MultiplayerPage.instance.ShowLoadingScreen();

            WWWForm form = new WWWForm();
			WWW download;
            
            form.AddField("action", "BatMultContestRegister");
			form.AddField("user_id", CONTROLLER.UserID);		
			form.AddField("email", GivenMailID.text);
			form.AddField("mobile", GivenMobileNumber.text);

			form.AddField("deviceId", CONTROLLER.DeviceID);
            form.AddField("bv", CONTROLLER.CURRENT_VERSION);
            form.AddField("platform", CONTROLLER.TargetPlatform);

            download = new WWW (CONTROLLER.BASE_URL, form);
			yield return download;

            if (!string.IsNullOrEmpty (download.error))
			{
                MultiplayerPage.instance.HideLoadingScreen();
            }
			else
			{
				getDataResponse (download.text);
			}
		}
	}

	private void getDataResponse(string _data)
	{
        SimpleJSON.JSONNode node = SimpleJSON.JSONNode.Parse(_data);
        string _status = ""+node["BatMultContestRegister"]["status"];		
		
		if(_status == "0")
		{
            //Failed to register
            ShowTitlePopup("REGISTRATION STATUS", "Oops! Something went wrong.\n Please try again");
		}
		else if(_status == "1")
		{
            //register Success			
            ShowTitlePopup("REGISTRATION STATUS", "You have been registered successfully!");
            AdIntegrate.instance.isRegisteredContestUser = true;
            Holder.SetActive(false);
            Invoke("HideMe", 2f);
        }
		else if(_status == "2")
		{
            //already register
            ShowTitlePopup("REGISTRATION STATUS", "Hey! Looks like you are already registered");
            Invoke("HideMe", 2f);
            Holder.SetActive(false);
        }

        MultiplayerPage.instance.HideLoadingScreen();
    }
    
	private string RemoveSpaces (string str)
	{
		int StrLen = str.Length;
		if (StrLen <= 0)
		{
			return "";
		}
		else
		{
			for (int i = 0; i< StrLen; i++)
			{
				if (str [i] == ' ')
				{
					string split1 = "";
					string split2 = "";
					split1 = str.Substring (0, i);
					split2 = str.Substring (i + 1, StrLen - i - 1);
					str = split1 + split2;
					StrLen--;
					i--;
					if (StrLen <= 0)
					{
						break;
					}
				}
			}
			return str;
		}
	}

    public GameObject SingleBtnPopup;
    public Text singleBtnText;
    
    private void showErrorPopup(string text)
    {
        SingleBtnPopup.SetActive(true);
        singleBtnText.text = text;
    }
    public void ButtonClick(int idx)
    {
        switch(idx)
        {
            case 0://singlBtn popu OK
                SingleBtnPopup.SetActive(false);
                break;
            case 1: //Skip Btn event
                HideMe();
                break;
            case 2: //Accept Tick                
                    AcceptTick.SetActive(!AcceptTick.activeSelf);
                break;
        }
    }
    private void ShowTitlePopup(string title,string content)
    {
        showErrorPopup(content);
    }

    private static bool VerifyEmailAddress(string address)
    {
        string[] atCharacter;
        string[] dotCharacter;
        atCharacter = address.Split("@"[0]);
        if (atCharacter.Length == 2)
        {
            dotCharacter = atCharacter[1].Split("."[0]);
            if (dotCharacter.Length >= 2)
            {
                if (dotCharacter[dotCharacter.Length - 1].Length == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
}