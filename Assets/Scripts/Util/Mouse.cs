using UnityEngine;using UnityEngine.UI;
using System.Collections;

public class Mouse {

	// TODO: Split ClickAction enum into 2 seperate enums to allow for detecting a simultaneous push of both buttons.

	/// Normal Click (Daydream touchpad click)
	/// CLICKDOWN, CLICKING, CLICKUP
	/// Application Click (Daydream application button click)
	/// ACLICKDOWN, ACLICKING, ACLICKUP, NULL
	public enum ClickAction
	{
		TOUCHDOWN,
		TOUCHING,
		TOUCHUP, 
		CLICKDOWN,
		CLICKING,
		CLICKUP, 
		ACLICKDOWN,
		ACLICKING,
		ACLICKUP, 
		NULL
	};

	// Position related
	public float x = 0;
	public float y = 0;
	public bool cast = false;
	public RaycastHit hitInfo;

	// Clicking related
	private float clickLastTime = -1;
	public ClickAction cAct = ClickAction.NULL;

	/// Checks for input from the Mouse
	public void UpdateMouseInput(){

		// See if we should move screen
		x = Input.GetAxisRaw ("Mouse X");
		y = Input.GetAxisRaw ("Mouse Y");

		// Set current action
		if (Input.GetButtonDown ("Fire1")) {
			cAct = ClickAction.CLICKDOWN;
		} else if (Input.GetButton ("Fire1")) {
			cAct = ClickAction.CLICKING;
		} else if (Input.GetButtonUp ("Fire1")) {
			cAct = ClickAction.CLICKUP;
		} else if (Input.GetButtonDown ("Fire2")) {
			cAct = ClickAction.ACLICKDOWN;
		} else if (Input.GetButton ("Fire2")) {
			cAct = ClickAction.ACLICKING;
		} else if (Input.GetButtonUp ("Fire2")) {
			cAct = ClickAction.ACLICKUP;
		} else if (Input.GetButtonDown ("Fire3")) {
			cAct = ClickAction.TOUCHDOWN;
		} else if (Input.GetButton ("Fire3")) {
			cAct = ClickAction.TOUCHING;
		} else if (Input.GetButtonUp ("Fire3")) {
			cAct = ClickAction.TOUCHUP;
		} else {
			cAct = ClickAction.NULL;
		}
	}

	/// Checks to see if last time DoubleClick was called was less than 0.5 second(s) ago
	public void DoubleClick(){
		float thisTime = Time.time;
		if (thisTime < clickLastTime + 0.5f) {
			//Debug.Log ("Double Click");
			clickLastTime = -1;
		} else {
			clickLastTime = thisTime;
		}
	} 
	
}
