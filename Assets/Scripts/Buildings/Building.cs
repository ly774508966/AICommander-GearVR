using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Building : MonoBehaviour {

	[Header("Owned By:")]
	public HumanPlayer player;

	// Currently open menu
	public static GameObject currentMenu;

	/// Button to open buildings menu
	[Header("Structure Fields")]
	public GameObject openButton;

	/// Whole menu object
	public GameObject menu;

	// Basic menu fields
	public GameObject[] buildMenuButtons;
	public Material[] buildMenuMats;
	public GameObject buildQueueContainer;
	public GameObject selectedArrow;
	public GameObject[] buildQueueItems;
	protected GameObject lastHover;
	protected GameObject currentlySelected;
	protected Color buildMenuRest = new Color (1.0f, 1.0f, 1.0f, 100.0f/255.0f);
	protected Color buildMenuHover = new Color (1.0f, 1.0f, 1.0f, 150.0f/255.0f);
	protected Color buildMenuSelect = new Color (1.0f, 1.0f, 1.0f, 200.0f/255.0f);

	// Menu: Right info bar
	public GameObject title;
	public GameObject description;
	public GameObject time;
	public GameObject mass;
	public GameObject playerMass;
	public GameObject buildButton;

	// Menu: Left info bar
	public GameObject BuildBar;
	// Queue
	protected List<string> buildQueue = new List<string>();
	public GameObject CurrentlyBuildingButton;
	public GameObject NextBuildingLeft;
	public GameObject NextBuildingMid;
	public GameObject NextBuildingRight;

	/// Objects to color
	public Material toColorMat;
	public Image[] toColorImage;

	// Building fields
	protected float TimeToComplete = -1;
	protected float TotalTime = 1;
	protected bool building = false;
	public int btnsAmt = 1;

	// Building thats currently selected (has its menu open)
	public static GameObject selectedBuilding = null;

	// To check for upgrades
	protected Upgrades upgrades;

	// Other
	protected Animator ani;

	void Awake(){
		// Set the build bar width(x) to 0%
		BuildBar.transform.localScale = new Vector3 (0, 1, 0.82f);

		// Init
		menu.SetActive (false);
		buildQueueItems[0].SetActive (false);
		buildQueueItems[1].SetActive (false);
		buildQueueItems[2].SetActive (false);
		buildQueueItems[3].SetActive (false);
		ani = GetComponent<Animator>();
	}

	void Start(){
		upgrades = GameObject.FindGameObjectWithTag ("Respawn").GetComponent<Upgrades>();

		// Menu init
		ChangeInfoPanel(0);
		buildMenuButtons [0].GetComponent<Image> ().color = buildMenuSelect;
		currentlySelected = buildMenuButtons [0];

		// Set all the images in the menu to the owners color
		for (int i = 0; i < toColorImage.Length; i++) {
			toColorImage [i].GetComponent<Image> ().color = player.playerColor;
		}
		// Set the material to the owners color
		toColorMat.color = player.playerColor;
		buildQueueContainer.SetActive (false);
	}

	public void LateUpdate(){
		// Reset last hovered over back to rest color
		if (lastHover != null) {
			lastHover.GetComponent<Image>().color = buildMenuRest;
			lastHover = null;
		}

		// Check if we're hovering over or clicking the open button
		CheckOpenButton ();

		// Check if we're hovering over or clicking the open button
		CheckBuildButton ();

		// Check if we're hovering over or clicking any in menu buttons
		CheckMenuButtons ();
	}

	void FixedUpdate(){
		UpdateProgressBar();
		playerMass.GetComponent<Text> ().text = "<color=black>Current Mass:</color>\n" + player.currentMass;

		if (currentMenu == null) {
			selectedBuilding = null;
		}
	}

	/// Checks if open button hovered or clicked
	protected void CheckOpenButton(){
		// Reset open buttons color
		openButton.GetComponent<Image> ().color = buildMenuSelect;

		// If a button is hit by the static raycast in GameController
		if(player.mouse.cast){
			if (player.mouse.hitInfo.collider.gameObject.Equals (openButton)) {
				// Color the button
				openButton.GetComponent<Image> ().color = Color.white;
				// If we're clicking
				if(player.mouse.cAct == Mouse.ClickAction.CLICKUP){
					OpenMenu ();
				}
			}
		}
	}

	/// Opens the buildings menu
	protected void OpenMenu(){
		if (menu.activeSelf) {
			menu.SetActive (false);
			currentMenu = null;
			selectedBuilding = null;
		} else {
			if(currentMenu != null)
				currentMenu.SetActive (false);
			menu.SetActive (true);
			currentMenu = menu;
			selectedBuilding = this.gameObject;
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
					if (StartBuilding (title.GetComponent<Text> ().text, false)) {
						buildQueueContainer.SetActive (true);
						buildQueueItems[0].SetActive (true);
					}
					if (buildQueue.Count == 1) {
						buildQueueItems[1].SetActive (true);
					}  else if (buildQueue.Count == 2) {
						buildQueueItems[2].SetActive (true);
					}  else if (buildQueue.Count == 3) {
						buildQueueItems[3].SetActive (true);
					} 
				}
			}
		}
	}

	/// Checks if menu buttons hovered or clicked
	protected void CheckMenuButtons(){
		// If a button is hit by the raycast
		if(player.mouse.cast){
			
			// Go through all the buttons to check if its one of this buildings
			for(int i = 0 ; i < buildMenuButtons.Length ; i++){
				if (player.mouse.hitInfo.collider.gameObject.Equals (buildMenuButtons[i])) {

					// If this button is selected dont do anything
					if(currentlySelected != null && currentlySelected.Equals (buildMenuButtons[i])){
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

	/// Updates progress bar based on how far the current build is to completion
	protected void UpdateProgressBar(){
		if (TimeToComplete > 0) {
			building = true;
			//PlayAni(true);
			TimeToComplete--;

			// Build bar
			BuildBar.transform.localScale = new Vector3 (TimeToComplete / TotalTime, 1, 0.82f);
			BuildBar.GetComponent<RectTransform> ().anchoredPosition = new Vector2(((6.45f / 100f) * ((TimeToComplete / TotalTime)*100)) / 2 - 3.225f, 0);

		} else if(TimeToComplete == 0) {
			building = false;
			//PlayAni(false);
			TimeToComplete = -1;

			// Reset Build bar
			BuildBar.transform.localScale = new Vector3 (1, 1, 0.82f);
			BuildBar.GetComponent<RectTransform> ().anchoredPosition = new Vector2(0, 0);

			// Finished
			FinishedBuilding ();
			CurrentlyBuildingButton.GetComponent<Image> ().material = null;

			// Build queue
			if (buildQueue.Count > 0) {
				
				if (buildQueue.Count == 1) {
					buildQueueItems[1].SetActive (false);
				} else if (buildQueue.Count == 2) {
					buildQueueItems[2].SetActive (false);
				} else if (buildQueue.Count == 3) {
					buildQueueItems[3].SetActive (false);
				} 

				StartBuilding (buildQueue [0], true);
				buildQueue.RemoveAt (0);
				NextBuildingLeft.GetComponent<Image> ().material = new Material (NextBuildingMid.GetComponent<Image> ().material);
				NextBuildingMid.GetComponent<Image> ().material = new Material (NextBuildingRight.GetComponent<Image> ().material);
				NextBuildingRight.GetComponent<Image> ().material = null;

			} else {
				buildQueueItems[0].SetActive (false);
				buildQueueContainer.SetActive (false);
			}
		}
	}

	/// Play animation for the building
	protected void PlayAni(bool tf){ 
	}

	// ### INDIVIDUAL BUILDING ACTIONS ###

	/// Take action on a button clicked
	protected virtual void ClickButton(int i, bool isFromBuildQueue){
		Debug.Log ("FILL THIS WITH AN ACTION IN THE LOWER CLASSES");
	}

	protected virtual bool StartBuilding(string toBuild, bool isFromBuildQueue){
		Debug.Log ("FILL THIS WITH AN ACTION IN THE LOWER CLASSES");
		return false;
	}

	/// Take action on application button clicked while selected
	public virtual void ClickedAppButton(){
		Debug.Log ("FILL THIS WITH AN ACTION IN THE LOWER CLASSES");
	}

	protected virtual void ChangeInfoPanel(int i){
		Debug.Log ("FILL THIS WITH AN ACTION IN THE LOWER CLASSES");
	}

	/// Act on finished build from building
	protected virtual void FinishedBuilding(){
		Debug.Log ("FILL THIS WITH AN ACTION IN THE LOWER CLASSES");
	}

	protected virtual bool CanAfford(string toBuild){
		Debug.Log ("FILL THIS WITH AN ACTION IN THE LOWER CLASSES");
		return false;
	}
}
