using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Builder : Unit {
	
	public static float massCost = 4000;
	public static float timeCost = 200;

	public static float health = 500;
	public static float damage = -1;
	public static float attackSpeed = -1;

	[Header("Builder Fields")]
	public Image[] toColorImage;

	public GameObject UI; 
	public GameObject buildButton; 
	protected GameObject currentlySelected;
	protected GameObject lastHover;

	/// Buttons in the buildings menu 0 - 8
	public GameObject[] buttons;
	// Amount of buttons that are active
	public int btnsAmt = 1;

	// Build menu button hover colours
	protected Color buildMenuRest = new Color (1.0f, 1.0f, 1.0f, 100.0f/255.0f);
	protected Color buildMenuHover = new Color (1.0f, 1.0f, 1.0f, 150.0f/255.0f);
	protected Color buildMenuSelect = new Color (1.0f, 1.0f, 1.0f, 200.0f/255.0f);

	[Header("Builder Menu Fields")]

	// Menu: Left info bar
	public GameObject title;
	public GameObject description;
	public GameObject time;
	public GameObject mass;

	private bool movingToBuildZone = false;
	private bool building = false;

	[Header("Builder Animation Fields")]
	public Animator buildingAni;

	[HideInInspector]
	public GameObject ghostObj;

	public GameObject selectedArrow;
	// Structures
	public GameObject MH;
	public GameObject UF;
	public GameObject RC;
	public Material MHMat;
	public Material UFMat;
	public Material RCMat;

	void Start(){
		// Menu init
		ChangeInfoPanel(0);
		buttons [0].GetComponent<Image> ().color = buildMenuSelect;
		currentlySelected = buttons [0];

		toColorMat.color = player.playerColor;
		UI.SetActive (false);
		if (player != null) {
			// Set all the images to the players color
			for (int i = 0; i < toColorImage.Length; i++) {
				toColorImage [i].color = player.playerColor;
			}
		}

		// Builder Start init
		MHMat.color = player.playerColor;
		UFMat.color = player.playerColor;
		RCMat.color = player.playerColor;
	}

	void LateUpdate(){
		// Reset last hovered over back to rest color
		if (lastHover != null) {
			lastHover.GetComponent<Image>().color = buildMenuRest;
			lastHover = null;
		}

		// Check if we're hovering over or clicking the open button
		CheckBuildButton ();

		// Check if we're hovering over or clicking any in menu buttons
		CheckMenuButtons ();
	}

	void FixedUpdate(){
		buildingAni.SetBool ("isBuilding", false);
		// if we've reached the build zone
		if (movingToBuildZone && IsInRange(nma.destination)) {
			if (ghostObj.GetComponent<GhostBuilding> ().mainBuilder == null) {
				nma.ResetPath ();
				movingToBuildZone = false;
				building = true;
				ghostObj.GetComponent<GhostBuilding> ().toHideShow.SetActive (true);
				ghostObj.GetComponent<GhostBuilding> ().mainBuilder = this.gameObject;
			} else {
				nma.ResetPath ();
				movingToBuildZone = false;
				building = false;
			}
		} 
		// We're building the structure
		else if(building){
			buildingAni.SetBool ("isBuilding", true);
			RotateTowards(ghostObj.transform);
			BuildStructure ();
		}
	}

	private void BuildStructure(){
		if (ghostObj.GetComponent<GhostBuilding> ().timeSpent < ghostObj.GetComponent<GhostBuilding> ().totalTime) {
			// Building NOT complete
			ghostObj.GetComponent<GhostBuilding> ().timeSpent++;
			ghostObj.GetComponent<GhostBuilding> ().current.transform.localScale = new Vector3((ghostObj.GetComponent<GhostBuilding> ().timeSpent / ghostObj.GetComponent<GhostBuilding> ().totalTime), 1, 1);
			ghostObj.GetComponent<GhostBuilding> ().percentage.text = Mathf.Round((ghostObj.GetComponent<GhostBuilding> ().timeSpent / ghostObj.GetComponent<GhostBuilding> ().totalTime) * 100) + "%";
		} else {
			// Building complete
			building = false;
			ghostObj.GetComponent<GhostBuilding> ().structureObj.transform.position = ghostObj.transform.position;
			ghostObj.GetComponent<GhostBuilding> ().structureObj.GetComponent<Building> ().player = player;
			ghostObj.GetComponent<GhostBuilding> ().structureObj.SetActive (true);
			player.structures.Add(ghostObj.GetComponent<GhostBuilding> ().structureObj.GetComponent<Building>());
			GameObject.Destroy (ghostObj);
		}
	}

	float rotationSpeed = 10f;
	private void RotateTowards(Transform target){
		Vector3 direction = (target.position - transform.position).normalized;
		Quaternion lookRotation = Quaternion.LookRotation (direction);
		transform.rotation = Quaternion.Slerp (transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
	}

	private bool IsInRange(Vector3 target){
		float distance = Vector3.Distance (transform.position, target);
		return distance < 3;
	}

	// MENU METHODS

	/// Checks if menu buttons hovered or clicked
	protected void CheckMenuButtons(){
		// If a button is hit by the static raycast in GameController
		if(player.mouse.cast){
			for(int i = 0 ; i < buttons.Length ; i++){
				if (player.mouse.hitInfo.collider.gameObject.Equals (buttons[i])) {

					// If this button is selected dont do anything
					if(currentlySelected != null && currentlySelected.Equals (buttons[i])){
						return;
					}

					// Set last hover and color the button
					lastHover = player.mouse.hitInfo.collider.gameObject;
					player.mouse.hitInfo.collider.gameObject.GetComponent<Image>().color = buildMenuHover;

					// If we're clicking
					if(player.mouse.cAct == Mouse.ClickAction.CLICKUP){
						lastHover = null;
						if(currentlySelected != null)
							currentlySelected.GetComponent<Image>().color = buildMenuRest;
						currentlySelected = player.mouse.hitInfo.collider.gameObject;
						currentlySelected.GetComponent<Image>().color = buildMenuSelect;
						ChangeInfoPanel(i);
					}
				}
			}
		}
	}

	/// Checks if build button hovered or clicked
	protected void CheckBuildButton(){
		// Reset build buttons color
		buildButton.GetComponent<Image> ().color = buildMenuRest;

		// If a button is hit by the static raycast in GameController
		if(player.mouse.cast){
			if (player.mouse.hitInfo.collider.gameObject.Equals (buildButton)) {
				// Color the button
				buildButton.GetComponent<Image> ().color = buildMenuSelect;
				// If we're clicking
				if(player.mouse.cAct == Mouse.ClickAction.CLICKUP){
					if (CanAfford (title.GetComponent<Text>().text)) {
						player.BuildMode (title.GetComponent<Text>().text, this);
						ToggleBuildInterface ();
					}
				}
			}
		}
	}

	private bool CanAfford(string toBuild){
		if (toBuild.Equals("Mass Harvester")){
			if (player.currentMass >= MassHarvester.massCost) {
				return true;
			}
		} else if (toBuild.Equals("Unit Factory")){
			if (player.currentMass >= UnitFactory.massCost) {
				return true;
			}
		} else if (toBuild.Equals("Research Center")){
			if (player.currentMass >= ResearchCenter.massCost) {
				return true;
			}
		}
		return false;
	}

	public override void SetGoal(Vector3 goal){
		nma.SetDestination (goal);
		if (ghostObj != null) {
			ghostObj.GetComponent<GhostBuilding> ().mainBuilder = null;
		}
		building = false;
		if (player.mouse.cast && player.mouse.hitInfo.collider.gameObject.tag.Equals ("Ghost")) {
			ghostObj = player.mouse.hitInfo.collider.gameObject;
			if (ghostObj.GetComponent<GhostBuilding> ().mainBuilder == null) {
				movingToBuildZone = true;
			} else {
				movingToBuildZone = false;
				nma.ResetPath ();
			}
		} else {
			movingToBuildZone = false;
		}
	}

	public void SetBuildGoal(Vector3 goal){
		nma.SetDestination (goal);
		movingToBuildZone = true;
	}

	protected void ChangeInfoPanel(int i){

		MH.SetActive (false);
		UF.SetActive (false);
		RC.SetActive (false);

		if (i == 0) {
			selectedArrow.GetComponent<RectTransform> ().anchoredPosition = new Vector2(selectedArrow.GetComponent<RectTransform> ().anchoredPosition.x, 6.4f);
			title.GetComponent<Text>().text = "Mass Harvester";
			description.GetComponent<Text> ().text = "allows you to harvest mass from the ground";
			time.SetActive (true);
			mass.SetActive (true);
			time.GetComponent<Text>().text = "Time: " + MassHarvester.timeCost;
			mass.GetComponent<Text>().text = "Mass: " + MassHarvester.massCost;
			MH.SetActive (true);
		} else if (i == 1) {
			selectedArrow.GetComponent<RectTransform> ().anchoredPosition = new Vector2(selectedArrow.GetComponent<RectTransform> ().anchoredPosition.x, 4.8f);
			title.GetComponent<Text>().text = "Unit Factory";
			description.GetComponent<Text> ().text = "allows you to manufacture many different units";
			time.SetActive (true);
			mass.SetActive (true);
			time.GetComponent<Text>().text = "Time: " + UnitFactory.timeCost;
			mass.GetComponent<Text>().text = "Mass: " + UnitFactory.massCost;
			UF.SetActive (true);
		} else if (i == 2) {
			selectedArrow.GetComponent<RectTransform> ().anchoredPosition = new Vector2(selectedArrow.GetComponent<RectTransform> ().anchoredPosition.x, 3.2f);
			title.GetComponent<Text>().text = "Research Center";
			description.GetComponent<Text>().text = "allows you to complete many different upgrades";
			time.SetActive (true);
			mass.SetActive (true);
			time.GetComponent<Text>().text = "Time: " + ResearchCenter.timeCost;
			mass.GetComponent<Text>().text = "Mass: " + ResearchCenter.massCost;
			RC.SetActive (true);
		}
	}

	// Sets panel back to its rest state (when no buttons are being highlighted
	protected void ResetInfoPanel(){
		title.GetComponent<Text> ().text = "Builder";
		description.GetComponent<Text> ().text = "Use this unit to build " +
			"structures to expand your " +
			"reach into the physical " +
			"world";
		time.SetActive (false);
		mass.SetActive (false);
	}

	// APP BUTTON METHODS
	
	public override void ClickedAppButton(){
		ToggleBuildInterface ();
	}

	void ToggleBuildInterface (){
		UI.SetActive (!UI.activeInHierarchy);
	}

	public override void Deselect(){
		if (UI.activeInHierarchy) {
			ToggleBuildInterface ();
		}
	}

}
