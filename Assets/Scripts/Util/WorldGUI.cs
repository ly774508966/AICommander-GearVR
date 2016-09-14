using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WorldGUI : MonoBehaviour {
	
	public GameObject[] toColor;
	public GameObject controlPivot;

	private float lastPlayerRotation;
	private float firstX;
	private float top = 337;
	private float bottom = 350;
	private float delayFrames = 10;

	private Queue queue = new Queue ();
	private bool showingMenu = false;


	void Awake(){
		// Set all the object to the players color
		for (int i = 0; i < toColor.Length; i++) {
			toColor [i].GetComponent<Image> ().color = Player.playerColor;
		}
		lastPlayerRotation = controlPivot.transform.rotation.eulerAngles.y;
		firstX = this.transform.localScale.x / (bottom - top);
		this.transform.localScale = new Vector3 (this.transform.localScale.x / (bottom - top), this.transform.localScale.y, this.transform.localScale.z);
	}

	// Update is called once per frame
	void LateUpdate () {
		// If the vertical angle is above the top value and below looking straight up
		if(controlPivot.transform.rotation.eulerAngles.x < top && controlPivot.transform.rotation.eulerAngles.x > 270){
			// Set the main menu width to full
			this.transform.localScale = new Vector3 (1, this.transform.localScale.y, this.transform.localScale.z);
			//this.transform.rotation = controlPivot.transform.rotation;
			//this.transform.localPosition = new Vector3 (controlPivot.transform.localPosition.x, controlPivot.transform.localPosition.y + 500, controlPivot.transform.localPosition.z + 550);
			showingMenu = true;
		} 
		// If the vertical angle is between top and bottom
		else if(controlPivot.transform.rotation.eulerAngles.x < bottom && controlPivot.transform.rotation.eulerAngles.x > top){
			float modifier = (controlPivot.transform.rotation.eulerAngles.x - bottom) * -1;
			if (modifier < 0.1) {
				// Set the main menu width to 0
				this.transform.localScale = new Vector3 (0, this.transform.localScale.y, this.transform.localScale.z);
				showingMenu = false;
			} else {
				// Set the main menu width to some point between 1 and 0. Getting higher as we look up
				this.transform.localScale = new Vector3 (firstX * modifier, this.transform.localScale.y, this.transform.localScale.z);
				showingMenu = true;
				//this.transform.rotation = controlPivot.transform.rotation;
				//this.transform.localPosition = new Vector3 (controlPivot.transform.localPosition.x, controlPivot.transform.localPosition.y + 500, controlPivot.transform.localPosition.z + 550);
			}
		} 
		// If the vertical angle is below the bottom and above looking straight down
		else if(controlPivot.transform.rotation.eulerAngles.x < 90 || controlPivot.transform.rotation.eulerAngles.x > bottom){
			// Set the main menu width to 0
			this.transform.localScale = new Vector3 (0, this.transform.localScale.y, this.transform.localScale.z);
			showingMenu = false;
		}

		// Move menu when its not showing
		if (!showingMenu) {
			this.transform.RotateAround (controlPivot.transform.position, Vector3.up, controlPivot.transform.rotation.eulerAngles.y - lastPlayerRotation);
			lastPlayerRotation = controlPivot.transform.rotation.eulerAngles.y;
		}
	}
}
