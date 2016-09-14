﻿using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Keyboard {

	public float x = 0;
	public float z = 0;
	
	private float moveAmount = 0.3f;

	/// Checks for input from the Keyboard and updates x and z
	public void KeyboardInput(){
		x = 0;
		z = 0;

		// Gather input data
		if (Input.GetAxis ("Horizontal") > 0) {
			x = moveAmount;
		} else if (Input.GetAxis ("Horizontal") < 0) {
			x = -moveAmount;
		}
		if (Input.GetAxis ("Vertical") > 0) {
			z = -moveAmount;
		} else if (Input.GetAxis ("Vertical") < 0) {
			z = moveAmount;
		}

		// Check for esc button
		if(Input.GetButtonUp("Cancel")){
			#if UNITY_EDITOR
			EditorApplication.isPlaying = false;
			#else
			Application.Quit();
			#endif
		}
	}
	
}