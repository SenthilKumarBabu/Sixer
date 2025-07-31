using UnityEngine;
using UnityEngine .UI;
using System .Collections ;

public class Toast : MonoBehaviour 
{
	public Text toastMsg;

	void Awake()
	{
		this.gameObject.SetActive (true); 
	}

	public void setMessge(string msg)
	{
		toastMsg.text = msg;
		 
		if (Time.timeScale < 1 )
		{			
			StartCoroutine (DestroyToast (Time.realtimeSinceStartup+2f)); 
		}
		else			
			Invoke ("disableToast",2f); 
	}

	void  disableToast()
	{		
		Destroy (this.gameObject  ); 
	}

	private IEnumerator  DestroyToast(float time)
	{
		while (Time .realtimeSinceStartup <time )
		{
			yield return 0;
		}
		Destroy (this.gameObject  ); 
	}
}
