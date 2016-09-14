using UnityEngine;
using System.Collections;

public class Builder : Unit {

	public GameObject UI; 

	private bool interfaceOpen = false;

	/// Buttons in the buildings menu 0 - 8
	public GameObject[] buttons;
	// Amount of buttons that are active
	public int btnsAmt = 1;

	// Menu: Right Info bar
	public GameObject InfoPanelRight;
	public TextMesh ItemName;
	public TextMesh ItemDescription;
	public TextMesh ItemTime;
	public TextMesh ItemMass;

	// Build menu button hover colours
	protected Color buildMenuHoverSuccess = new Color (0.5f, 0.9f, 0.5f);
	protected Color buildMenuHoverFail = new Color (0.9f, 0.5f, 0.5f);
	protected Color buildMenuHoverDisabled = new Color (0.5f, 0.5f, 0.5f);
	protected Color buildMenuNotHover = new Color (1f, 1f, 1f);


	void FixedUpdate(){
		// Reset all buttons TODO: reset last clicked menu item to white
		for (int i = 0; i < buttons.Length; i++) {
			buttons [i].GetComponent<Renderer> ().material.color = buildMenuNotHover;
		}
		
		// Reset RightInfoPanel
		ItemName.text = "";
		ItemDescription.text = "";
		ItemTime.text = "";
		ItemMass.text = "";

		CheckMenuButtons ();
	}

	/// Checks if menu buttons hovered or clicked
	protected void CheckMenuButtons(){
		InfoPanelRight.SetActive (false);
		// If a button is hit by the static raycast in GameController
		if(Player.mouse.cast){
			if (Player.mouse.hitInfo.collider == null) {
				return;
			}
			GameObject objHit = Player.mouse.hitInfo.collider.gameObject;
			for(int i = 0 ; i < buttons.Length ; i++){
				if (objHit.Equals (buttons[i])) {
					// Color the button
					ColorBuildMenuButton(i);
					// Change info panel from item
					InfoPanelRight.SetActive (true);
					ChangeInfoPanel(i);
					// If we're clicking
					if(Player.mouse.cAct == Mouse.ClickAction.CLICKUP){
						ClickButton (i);
					}
				}
			}
		}
	}

	/// Color the button
	protected void ColorBuildMenuButton(int i){
		if ((btnsAmt - 1) >= i) {
			buttons [i].GetComponent<Renderer> ().material.color = buildMenuHoverSuccess;
		} else if (false) { // TODO: Not enough cash
			buttons [i].GetComponent<Renderer> ().material.color = buildMenuHoverFail;
		} else {
			buttons [i].GetComponent<Renderer> ().material.color = buildMenuHoverDisabled;
		}
	}

	protected void ClickButton(int i){
		if (i == 0){
			Debug.Log ("BuildMode: Mass Harvester");
		} else if (i == 1){
			Debug.Log ("BuildMode: Tri Axis Printer");
			Player.BuildMode ("Tri Axis Printer");
		} else if (i == 2){
			Debug.Log ("BuildMode: Research Center");
			Player.BuildMode ("Research Center");
		} else if (i == 3){
		} else if (i == 4){
		} else if (i == 5){
		} else if (i == 6){
		} else if (i == 7){
		} else if (i == 8){
		}
		ToggleBuildInterface ();
	}

	protected void ChangeInfoPanel(int i){
		if (i == 0) {
			ItemName.text = "Mass Harvester";
			ItemDescription.text = "Use this unit to build\n" +
				"structures to expand your\n" +
				"reach into the physical\n" +
				"world";
			ItemTime.text = "Time:\n 1000";
			ItemMass.text = "Mass:\n 4000";
		} else if (i == 1) {
			ItemName.text = "Tri Axis Printer";
			ItemDescription.text = "Use this unit to build\n" +
				"structures to expand your\n" +
				"reach into the physical\n" +
				"world";
			ItemTime.text = "Time:\n 2000";
			ItemMass.text = "Mass:\n 8000";
		} else if (i == 2) {
			ItemName.text = "Research Center";
			ItemDescription.text = "Use this unit to build\n" +
				"structures to expand your\n" +
				"reach into the physical\n" +
				"world";
			ItemTime.text = "Time:\n 3000";
			ItemMass.text = "Mass:\n 10000";
		} else if (i == 3) {
			InfoPanelRight.SetActive (false);
		} else if (i == 4) {
			InfoPanelRight.SetActive (false);
		} else if (i == 5) {
			InfoPanelRight.SetActive (false);
		} else if (i == 6) {
			InfoPanelRight.SetActive (false);
		} else if (i == 7) {
			InfoPanelRight.SetActive (false);
		} else if (i == 8) {
			InfoPanelRight.SetActive (false);
		}
	}

	// App button methods
	
	public override void ClickedAppButton(){
		ToggleBuildInterface ();
	}

	void ToggleBuildInterface (){
		interfaceOpen = !interfaceOpen;
		UI.SetActive (interfaceOpen);
	}

	public override void Deselect(){
		if (interfaceOpen) {
			ToggleBuildInterface ();
		}
	}

}
