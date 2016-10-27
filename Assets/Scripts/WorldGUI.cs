using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class WorldGUI : MonoBehaviour {
	public GameObject container;
	
	public GameObject[] toColor;
	public GameObject controlPivot;

	private float lastPlayerRotation;
	private float firstX;
	private bool firstUpdate = true;

	// Unit Quick Groups
	private static List<Unit> Group1;
	private static List<Unit> Group2;
	private static List<Unit> Group3;
	private static List<Unit> Group4;
	private static List<Unit> Group5;
	private static List<Unit> Group6;

	public Text Group1TextT;
	public Text Group2TextT;
	public Text Group3TextT;
	public Text Group4TextT;
	public Text Group5TextT;
	public Text Group6TextT;

	public static Text Group1Text;
	public static Text Group2Text;
	public static Text Group3Text;
	public static Text Group4Text;
	public static Text Group5Text;
	public static Text Group6Text;

	[Header("Owned By:")]
	public HumanPlayer player;

	void Awake(){
		Group1Text = Group1TextT;
		Group2Text = Group2TextT;
		Group3Text = Group3TextT;
		Group4Text = Group4TextT;
		Group5Text = Group5TextT;
		Group6Text = Group6TextT;
		// Init
		/*lastPlayerRotation = controlPivot.transform.rotation.eulerAngles.y;
		firstX = this.transform.localScale.x / (bottom - top);
		this.transform.localScale = new Vector3 (this.transform.localScale.x / (bottom - top), this.transform.localScale.y, this.transform.localScale.z);*/
		lastPlayerRotation = controlPivot.transform.rotation.eulerAngles.y;
	}

	// Show the menu when player looks up and lock it in place. Else rotate it so when the player looks up the menu will always be there
	void LateUpdate () {
		if (firstUpdate) {
			firstUpdate = false;
			// Set all the object to the players color
			for (int i = 0; i < toColor.Length; i++) {
				toColor [i].GetComponent<Image> ().color = player.playerColor;
			}
		}

		/*transform.position = controlPivot.transform.position;
		GetComponent<RectTransform> ().localPosition = new Vector3(GetComponent<RectTransform> ().localPosition.x, GetComponent<RectTransform> ().localPosition.y + 395, GetComponent<RectTransform> ().localPosition.z + 600);
		if (Check360Difference(lastPlayerRotation, controlPivot.transform.rotation.eulerAngles.y) > 15) {
			if (controlPivot.transform.rotation.eulerAngles.y - lastPlayerRotation > 0) {
				container.transform.rotation = Quaternion.Euler (container.transform.rotation.eulerAngles.x, container.transform.rotation.eulerAngles.y + (Check360Difference(lastPlayerRotation, controlPivot.transform.rotation.eulerAngles.y) - 15), container.transform.rotation.eulerAngles.z);
			} else {
				container.transform.rotation = Quaternion.Euler (container.transform.rotation.eulerAngles.x, container.transform.rotation.eulerAngles.y - (Check360Difference(lastPlayerRotation, controlPivot.transform.rotation.eulerAngles.y) - 15), container.transform.rotation.eulerAngles.z);
			}
			lastPlayerRotation = container.transform.rotation.eulerAngles.y;
		}*/

		/*// Rotate with player (can lose it)
		float difference = controlPivot.transform.rotation.eulerAngles.y - transform.rotation.eulerAngles.y;
		if ((difference < 345 && difference > 270) || (difference < -15 && difference > -90)) {
			this.transform.RotateAround (controlPivot.transform.position, Vector3.up, controlPivot.transform.rotation.eulerAngles.y - lastPlayerRotation);
		} else if((difference > 15 && difference < 90) || (difference > -345 && difference < -270)) {
			this.transform.RotateAround (controlPivot.transform.position, Vector3.up, controlPivot.transform.rotation.eulerAngles.y - lastPlayerRotation);
		}

		if (Check360Difference (lastPlayerRotation, transform.rotation.eulerAngles.y) > 100) {
			//Debug.Log ("HJERE");
		}

		lastPlayerRotation = controlPivot.transform.rotation.eulerAngles.y;*/

		// If the worldgui is not visable
		if (controlPivot.transform.rotation.eulerAngles.x < 270 && controlPivot.transform.rotation.eulerAngles.x > 15f) {
			// Move the menu
			container.transform.rotation = Quaternion.Euler (container.transform.rotation.eulerAngles.x, controlPivot.transform.rotation.eulerAngles.y, container.transform.rotation.eulerAngles.z);
			container.transform.localScale = new Vector3 (0, 1, 1);
		} else {
			container.transform.localScale = new Vector3 (1, 1, 1);
		}

		/*bool showingMenu = false;

		// If the vertical angle is above the top value and below looking straight up, show the full menu
		if(controlPivot.transform.rotation.eulerAngles.x < top && controlPivot.transform.rotation.eulerAngles.x > 270){
			// Set the main menu width to full
			this.transform.localScale = new Vector3 (1, this.transform.localScale.y, this.transform.localScale.z);
			showingMenu = true;
		} 
		// If the vertical angle is between top and bottom, show a partly expanded menu
		else if(controlPivot.transform.rotation.eulerAngles.x < bottom && controlPivot.transform.rotation.eulerAngles.x > top){
			float modifier = (controlPivot.transform.rotation.eulerAngles.x - bottom) * -1;
			if (modifier < 0.1) {
				// Set the menu width to 0
				this.transform.localScale = new Vector3 (0, this.transform.localScale.y, this.transform.localScale.z);
				showingMenu = false;
			} else {
				// Set the menu width to some point between 1 and 0. Getting wider as we look up
				this.transform.localScale = new Vector3 (firstX * modifier, this.transform.localScale.y, this.transform.localScale.z);
				showingMenu = true;
			}
		} 
		// If the vertical angle is below the bottom and above looking straight down, hide the menu
		else if(controlPivot.transform.rotation.eulerAngles.x < 90 || controlPivot.transform.rotation.eulerAngles.x > bottom){
			// Set the menu width to 0
			this.transform.localScale = new Vector3 (0, this.transform.localScale.y, this.transform.localScale.z);
			showingMenu = false;
		}

		// Move menu when its not showing
		if (!showingMenu) {
			this.transform.RotateAround (controlPivot.transform.position, Vector3.up, controlPivot.transform.rotation.eulerAngles.y - lastPlayerRotation);
			lastPlayerRotation = controlPivot.transform.rotation.eulerAngles.y;
		}*/
	}

	private float Check360Difference(float lastPlayerRot, float GUIRot){
		float answer = 0;
		if (lastPlayerRotation < GUIRot) {
			answer = GUIRot - lastPlayerRotation;
		} else {
			answer = lastPlayerRotation - GUIRot;
		}
		return answer;
	}

	public static void SetGroup(int group, List<Unit> selectedUnits){
		if (group == 1) {
			WorldGUI.Group1 = selectedUnits;
			WorldGUI.Group1Text.text = selectedUnits.Count.ToString();
		} else if (group == 2) {
			WorldGUI.Group2 = selectedUnits;
			WorldGUI.Group2Text.text = selectedUnits.Count.ToString();
		} else if (group == 3) {
			WorldGUI.Group3 = selectedUnits;
			WorldGUI.Group3Text.text = selectedUnits.Count.ToString();
		} else if (group == 4) {
			WorldGUI.Group4 = selectedUnits;
			WorldGUI.Group4Text.text = selectedUnits.Count.ToString();
		} else if (group == 5) {
			WorldGUI.Group5 = selectedUnits;
			WorldGUI.Group5Text.text = selectedUnits.Count.ToString();
		} else if (group == 6) {
			WorldGUI.Group6 = selectedUnits;
			WorldGUI.Group6Text.text = selectedUnits.Count.ToString();
		}
	}

	public static List<Unit> GetGroup(int group){
		if (group == 1) {
			return Group1;
		} else if (group == 2) {
			return Group2;
		} else if (group == 3) {
			return Group3;
		} else if (group == 4) {
			return Group4;
		} else if (group == 5) {
			return Group5;
		} else if (group == 6) {
			return Group6;
		}
		return null;
	}
}
