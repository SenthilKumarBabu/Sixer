using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine .UI;

public class ChangeToggleImage : MonoBehaviour {

	public Image Toggle1,Toggle2;
	public Sprite  _Enable, _Disable;
	public Text Text1, Text2;
    public bool istextchange;
	public Image Tick1, Tick2;
	public  void roomToggleChangeEvent(int type)
	{
		AudioPlayer.instance.PlayButtonSnd();
		if (type ==0)
		{
			Toggle1.sprite= _Enable;
			Toggle2.sprite = _Disable;
			if(Tick1!=null)
				Tick1.gameObject.SetActive(true);
			if(Tick2 != null)
                Tick2.gameObject.SetActive(false);
            if (istextchange)
            {
				Text1.color = Color.black;
				Text2.color = Color.white;
			}
		}
		else
		{
			Toggle1.sprite = _Disable;
			Toggle2.sprite = _Enable;
            if (Tick1 != null)
                Tick1.gameObject.SetActive(false);
            if (Tick2 != null)
                Tick2.gameObject.SetActive(true)	;
            if (istextchange)
            {
				Text1.color = Color.white;
				Text2.color = Color.black;
			}
		}
	}
}
