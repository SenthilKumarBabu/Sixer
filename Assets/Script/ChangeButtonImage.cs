using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChangeButtonImage : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {

	private Sprite defaultButton;
	public Sprite changeButton;
	// Use this for initialization
	void Start () {
		defaultButton = GetComponent<Image> ().sprite;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public virtual void OnPointerEnter(PointerEventData point) {
		GetComponent<Image> ().sprite = changeButton;
	}

	public virtual void OnPointerExit(PointerEventData point) {
		GetComponent<Image> ().sprite = defaultButton;
	}
	public virtual void OnPointerClick(PointerEventData point) {
		GetComponent<Image> ().sprite = defaultButton;
	}
}
