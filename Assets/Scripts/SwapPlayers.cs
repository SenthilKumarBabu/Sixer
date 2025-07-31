using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SwapPlayers : MonoBehaviour, IPointerClickHandler {

	public Text Name;
	public int index;
	// Use this for initialization
	void Start () {
		
	}
	
	public virtual void OnPointerClick(PointerEventData point) {
	}
}
