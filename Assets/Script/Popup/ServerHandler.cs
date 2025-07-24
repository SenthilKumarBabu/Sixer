using UnityEngine;
using UnityEngine.Networking;

public enum ServerRequest { GET,POST}

public class ServerHandler : MonoBehaviour {

	public static ServerHandler instance = null;

	[SerializeField] private string serverURL = "http://192.168.1.220/minigolf/unity/unity.php";

	private void Awake()
	{
		if(instance==null)
		{
			instance = this;
		}
		if(instance!=this)
		{
			Destroy(gameObject);
		}
	}

	private string GetFromServer(string _action, string _key, string _value)
	{
		string response = "";
		WWWForm form = new WWWForm();
		form.AddField("action", _action);
		form.AddField(_key, _value);
		WWW wwwRequest = new WWW(serverURL, form);
		while (!wwwRequest.isDone)
			response = string.Empty;

		if (!string.IsNullOrEmpty(wwwRequest.error))
			response = "500";
		else if (string.IsNullOrEmpty(wwwRequest.text))
			response = "444";
		else
		{
			response = wwwRequest.text;
		}
		return response;
	}

	private string GetFromServer(string _action, string _key, int _value)
	{
		string response = "";
		WWWForm form = new WWWForm();
		form.AddField("action", _action);
		form.AddField(_key, _value);
		WWW wwwRequest = new WWW(serverURL, form);
		while (!wwwRequest.isDone)
			response = string.Empty;

		if (!string.IsNullOrEmpty(wwwRequest.error))
			response = "500";
		else if (string.IsNullOrEmpty(wwwRequest.text))
			response = "444";
		else
		{
			response = wwwRequest.text;
		}
		return response;
	}

	private string GetFromServer( WWWForm _form)
	{
		string response = "";
	
		WWW wwwRequest = new WWW(serverURL, _form);
		while (!wwwRequest.isDone)
			response = string.Empty;

		if (!string.IsNullOrEmpty(wwwRequest.error))
			response = "500";
		else if (string.IsNullOrEmpty(wwwRequest.text))
			response = "444";
		else
		{
			response = wwwRequest.text;
		}
		return response;
	}

	public string Request(ServerRequest _requestType,string _action, string _key, int _value)
	{
		string response = string.Empty;
		switch(_requestType)
		{
			case ServerRequest.GET:
				response = GetFromServer(_action, _key, _value);
				break;
			case ServerRequest.POST:
				response = PostToServer(_action, _key, _value.ToString());
				break;
		}
		return response;
	}

	public string Request(ServerRequest _requestType, string _action, string _key, string _value)
	{
		string response = string.Empty;
		switch (_requestType)
		{
			case ServerRequest.GET:
				response = GetFromServer(_action, _key, _value);
				break;
			case ServerRequest.POST:
				response = PostToServer(_action, _key, _value);
				break;
		}
		return response;
	}

	public string Request(ServerRequest _requestType, WWWForm _form)
	{
		string response = string.Empty;
		switch (_requestType)
		{
			case ServerRequest.GET:
				response = GetFromServer(_form);
				break;
			case ServerRequest.POST:
				response = PostToServer(_form);
				break;
		}
		return response;
	}

	private string PostToServer(string _action, string _key, string _value)
	{
		string response = string.Empty;
		WWWForm form = new WWWForm();
		form.AddField("action", _action);
		form.AddField(_key, _value);
		DownloadHandler unityWebResponseHandler;
		UnityWebRequest unityWebRequest = UnityWebRequest.Post(serverURL, form);
		unityWebResponseHandler = unityWebRequest.downloadHandler;
		unityWebRequest.SendWebRequest();
		while (!unityWebRequest.isDone)
			response = string.Empty;

		if (!string.IsNullOrEmpty(unityWebRequest.error))
			response = "500";
		else if (string.IsNullOrEmpty(unityWebResponseHandler.text))
			response = "444";
		else
		{
			response = unityWebResponseHandler.text;
		}
		return response;
	}

	private string PostToServer( WWWForm _form)
	{
		string response = string.Empty;
		DownloadHandler unityWebResponseHandler;
		UnityWebRequest unityWebRequest = UnityWebRequest.Post(serverURL, _form);
		unityWebResponseHandler = unityWebRequest.downloadHandler;
		unityWebRequest.SendWebRequest();
		while (!unityWebRequest.isDone)
			response = string.Empty;

		if (!string.IsNullOrEmpty(unityWebRequest.error))
			response = "500";
		else if (string.IsNullOrEmpty(unityWebResponseHandler.text))
			response = "444";
		else
		{
			response = unityWebResponseHandler.text;
		}
		return response;
	}


}
