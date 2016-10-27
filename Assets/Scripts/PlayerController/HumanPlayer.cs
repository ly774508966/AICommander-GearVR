using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class HumanPlayer : Player {
	
	public static Color COLORTOSET = new Color (0.0f/255.0f, 214.0f/255.0f, 37.0f/255.0f, 100.0f/255.0f);
	
	// Stats
	public float moveSpeedMultiplier = 2.0f;

	[Header("Building Prefabs")] 
	public GameObject MH;
	public GameObject UF;
	public GameObject RC;
	public GameObject GMH;
	public GameObject GTAP;
	public GameObject GRC;
	public Material ghostMat;

	private bool buildMode = false;
	private bool canBuild = false;
	private Builder builder;
	private string structure;
	private GameObject currentBuildGhost = null;

	[Header("Dialog fields")]
	public Tutorial tutorial;
	public GameObject toShowHide;
	public Text HideShowText;

	[Header("Other fields")]
	public GameObject Camera;
	public GameObject MouseKeyboardCamera;
	public GameObject selectionBox;
	public Text MiniMapPos;
	public Text PlayerPos;
	public GameObject controllerPivot;
	public GameObject cursor;

	[HideInInspector]
	public Mouse mouse = new Mouse ();
	[HideInInspector]
	public Keyboard keyboard = new Keyboard ();

	// Main menu Fields
	private GameObject LastUIHover;
	private Color LastUIColor;

	/// The color of the player and the hover color
	[HideInInspector]
	public Color playerColor;
	[HideInInspector]
	public Color menuHoverColor;

	// Units and structures owned by the player
	[HideInInspector]
	public List<Unit> units = new List<Unit>();
	[HideInInspector]
	public List<Building> structures = new List<Building>();

	// Mass fields
	[HideInInspector]
	public float maxMass = 10000;
	[HideInInspector]
	public float currentMass;
	public Text massText;

	void Awake(){
		currentMass = 10000;
		playerColor = COLORTOSET;
	}

	/// Init fields
	void Start(){
		// Lock default cursor and color our in game cursor as the players color
		Cursor.lockState = CursorLockMode.Locked;
		cursor.GetComponent<Renderer> ().material.color = playerColor;
		// Create the solid version of the players color to use as a highlight for UI elements
		menuHoverColor = new Color (playerColor.r, playerColor.g, playerColor.b, 1);

		// Set Camera
		#if UNITY_EDITOR
		Camera.SetActive(false);
		Camera = MouseKeyboardCamera;
		Camera.SetActive(true);
		#endif
	}

	/// Game Loop
	void Update() {
		// Update input from the mouse
		mouse.UpdateMouseInput ();
		// Update the players view based on the mouse input
		UpdateView();
		// Handle clicks based on the mouse input
		Clicks ();	

		// Update input from the keyboard
		keyboard.KeyboardInput ();

		massText.text = "Mass: " + currentMass + " / " + maxMass;
	}	 

	void FixedUpdate() {
		// Move player based on the keyboard input
		Move ();
	}

	/// Update the players view direction, raycast and cursor based on the mouse inputs
	private void UpdateView(){
		// Rotate view direction
		if(mouse.x != 0 || mouse.y != 0){
			Vector3 v = new Vector3(Camera.transform.rotation.eulerAngles.x - mouse.y, Camera.transform.rotation.eulerAngles.y + mouse.x, Camera.transform.rotation.eulerAngles.z);
			Camera.transform.rotation = Quaternion.Euler (v);
		}

		// Updates the direction of the controllers pivot point to be the same direction as the players view
		controllerPivot.transform.rotation = Camera.transform.rotation;

		// Updates the raycast from the controllers pivot point
		Vector3 rayDirection = controllerPivot.transform.rotation * Vector3.forward;
		LayerMask layerMask = (1 << 2);
		layerMask = ~layerMask;
		mouse.cast = Physics.Raycast (transform.position, rayDirection, out mouse.hitInfo, 1000, layerMask);

		// Move controllers cursor along the z axis to the raycasts point of collision. The z axis has a max range of 30 (or the cursor gets too small)
		if (mouse.cast) {
			cursor.transform.position = mouse.hitInfo.point;
			if (cursor.transform.localPosition.z > 20) {
				cursor.transform.localPosition = new Vector3 (0, 0, 20);
			} else {
				cursor.transform.localPosition = new Vector3 (0, 0, cursor.transform.localPosition.z);
			}
		}
	}

	/// Handle mouse clicks
	private void Clicks(){
		// Reset selection
		if (Building.currentMenu != null) {
			selectionBox.GetComponent<SelectionBox> ().ResetSelection ();
		}

		// Check for double click
		if(mouse.cAct == Mouse.ClickAction.CLICKUP){
			mouse.DoubleClick ();
		}

		// Handle main menu hover and click
		UIHoverClick ();

		// If we're in build mode (we're trying to place a location for a building)
		if (buildMode) {
			HandleBuildMode ();
			return;
		} 
		// If we're not in build mode we should handle selecting units
		else if (mouse.cAct != Mouse.ClickAction.NULL) {
			selectionBox.GetComponent<SelectionBox> ().HandleSelection (mouse);
		}

		// Handle App button on selected units
		if (mouse.cAct == Mouse.ClickAction.ACLICKUP && selectionBox.GetComponent<SelectionBox> ().selectedUnits.Count > 0) {
			// Tell first selected unit the app button has been clicked
			selectionBox.GetComponent<SelectionBox> ().selectedUnits[0].ClickedAppButton();
		}

		// Handle App button on selected building
		if (mouse.cAct == Mouse.ClickAction.ACLICKUP && Building.selectedBuilding != null) {
			Building.selectedBuilding.GetComponent<Building> ().ClickedAppButton ();
		}
	}

	/// We're in build mode (we're trying to place a location for a building)
	private void HandleBuildMode(){
		
		// If there isn't a build ghost yet then this is the first buildMode frame. Create the buildGhost
		if (currentBuildGhost == null) {
			
			if (structure.Equals ("Mass Harvester")) {
				currentBuildGhost = (GameObject) GameObject.Instantiate (GMH, transform.position, transform.rotation);
				currentBuildGhost.transform.Rotate (new Vector3(0, 180, 0));
				currentBuildGhost.GetComponent<GhostBuilding> ().structureObj = (GameObject) GameObject.Instantiate (MH, currentBuildGhost.transform.position, currentBuildGhost.transform.rotation);
				currentBuildGhost.GetComponent<GhostBuilding> ().totalTime = MassHarvester.timeCost;
				currentBuildGhost.GetComponent<GhostBuilding> ().massCost = MassHarvester.massCost;
			} else if (structure.Equals ("Unit Factory")) {
				currentBuildGhost = (GameObject) GameObject.Instantiate (GTAP, transform.position, transform.rotation);
				currentBuildGhost.transform.Rotate (new Vector3(0, 180, 0));
				currentBuildGhost.GetComponent<GhostBuilding> ().structureObj = (GameObject) GameObject.Instantiate (UF, currentBuildGhost.transform.position, currentBuildGhost.transform.rotation);
				currentBuildGhost.GetComponent<GhostBuilding> ().totalTime = UnitFactory.timeCost;
				currentBuildGhost.GetComponent<GhostBuilding> ().massCost = UnitFactory.massCost;
			} else if (structure.Equals ("Research Center")) {
				currentBuildGhost = (GameObject) GameObject.Instantiate (GRC, transform.position, transform.rotation);
				currentBuildGhost.transform.Rotate (new Vector3(0, 180, 0));
				currentBuildGhost.GetComponent<GhostBuilding> ().structureObj = (GameObject) GameObject.Instantiate (RC, currentBuildGhost.transform.position, currentBuildGhost.transform.rotation);
				currentBuildGhost.GetComponent<GhostBuilding> ().totalTime = ResearchCenter.timeCost;
				currentBuildGhost.GetComponent<GhostBuilding> ().massCost = ResearchCenter.massCost;
			}
			builder.ghostObj = currentBuildGhost;
			currentBuildGhost.GetComponent<GhostBuilding> ().structureObj.SetActive (false);
			currentBuildGhost.GetComponent<GhostBuilding> ().name = structure;
			currentBuildGhost.SetActive (false);
		}

		// If the controllers raycast hit something and it wasn't a UI object
		if (mouse.cast && !(mouse.hitInfo.collider.gameObject.layer == 5)) {

			if (mouse.hitInfo.collider.gameObject.layer == 11) {
				ghostMat.color = new Color (150.0f/255.0f, 0.0f/255.0f, 0.0f/255.0f);
				canBuild = false;
			} else {
				ghostMat.color = new Color (0.0f/255.0f, 150.0f/255.0f, 0.0f/255.0f);
				canBuild = true;
			}
			
			// Show ghost building and reposition
			currentBuildGhost.transform.position = mouse.hitInfo.point;
			currentBuildGhost.SetActive (true);

			// If the player is clicking then place the ghost and tell the builder to move to location
			if (mouse.cAct == Mouse.ClickAction.CLICKUP && canBuild) {
				currentMass = currentMass - currentBuildGhost.GetComponent<GhostBuilding> ().massCost;
				currentBuildGhost.layer = 0;
				builder.SetBuildGoal (new Vector3(currentBuildGhost.transform.position.x, currentBuildGhost.transform.position.y, currentBuildGhost.transform.position.z));
				currentBuildGhost = null;
				buildMode = false;
			}
		} else {
			// Hide ghost building if there is no cast on a non UI object
			currentBuildGhost.SetActive (false);
		}
	}

	/// Move the specified x and z from keyboard input
	private void Move(){
		if (!(keyboard.x > 0 || keyboard.x < 0 || keyboard.z > 0 || keyboard.z < 0))
			return;

		// Turn to face camera dir
		Quaternion q = transform.rotation;
		transform.rotation = Camera.transform.rotation;
		transform.rotation = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w);
		// Move
		transform.Translate (new Vector3 (keyboard.x * moveSpeedMultiplier, 0, -keyboard.z * moveSpeedMultiplier));
		// Turn back
		transform.rotation = q;
	}

	// This is called to start build mode after a build structure button has been clicked
	public void BuildMode(string structure, Builder b){
		buildMode = true;
		builder = b;
		this.structure = structure;
	}

	// #### UI METHODS ####

	/// Handles UI hovers and clicks
	public void UIHoverClick(){
		// Reset back to rest state
		float mapSize = 60f;
		PlayerPos.text = ((transform.position.x + mapSize / 2).ToString ("F0") + ", " + (transform.position.z + mapSize / 2).ToString ("F0"));
		MiniMapPos.text = ""; // Set to empty string that will be changed later this frame if we're hovering over the map

		// Tutorial rest states
		tutorial.highlightedContinueButton = false;
		if (!tutorial.completedCurrentStep) { 		// If we haven't completed the current step the set the button to the disabled color
			tutorial.continueButton.GetComponent<Image> ().color = new Color (73f / 255f, 73f / 255f, 73f / 255f, 69f / 255f);
		} else { 									// Else set it to the normal color
			tutorial.continueButton.GetComponent<Image> ().color = playerColor;
		}

		// If LastUIHover is not null (ie we had been hovoring over something last frame) turn it back to its normal color
		if (LastUIHover != null) {
			LastUIHover.GetComponent<Image> ().color = LastUIColor;
			LastUIHover = null;
		}

		// Set the UI element currently being hovered over to the active hover color and check if we're clicking while hovering
		if (mouse.cast) {
			
			// In game menu UI
			if (mouse.hitInfo.collider.gameObject.tag.Equals ("PauseButton")) {
				//Debug.Log ("PauseButton Hover");
				mouse.hitInfo.collider.gameObject.GetComponent<Image> ().color = menuHoverColor;
				LastUIHover = mouse.hitInfo.collider.gameObject;
				LastUIColor = playerColor;
				if (mouse.cAct == Mouse.ClickAction.CLICKUP) {
					//Debug.Log ("PauseButton Clicked");
					PauseButton ();
				}
			} else if (mouse.hitInfo.collider.gameObject.tag.Equals ("ControlsButton")) {
				//Debug.Log ("ControlsButton Hover");
				mouse.hitInfo.collider.gameObject.GetComponent<Image> ().color = menuHoverColor;
				LastUIHover = mouse.hitInfo.collider.gameObject;
				LastUIColor = playerColor;
				if (mouse.cAct == Mouse.ClickAction.CLICKUP) {
					//Debug.Log ("ControlsButton Clicked");
					ControlsButton ();
				}
			} else if (mouse.hitInfo.collider.gameObject.tag.Equals ("QuitButton")) {
				//Debug.Log ("QuitButton Hover");
				mouse.hitInfo.collider.gameObject.GetComponent<Image> ().color = menuHoverColor;
				LastUIHover = mouse.hitInfo.collider.gameObject;
				LastUIColor = playerColor;
				if (mouse.cAct == Mouse.ClickAction.CLICKUP) {
					//Debug.Log ("QuitButton Clicked");
					QuitButton ();
				}
			} 

			// Minimap UI
			else if (mouse.hitInfo.collider.gameObject.name.Equals ("MiniMap")) {
				//Debug.Log ("MiniMap Hover");
				// Local position from the collider of the raycast at the point of collision
				Vector3 p = mouse.hitInfo.collider.gameObject.transform.InverseTransformPoint (mouse.hitInfo.point);
				// Update text to show the x and y position of where we're hovering over the map
				MiniMapPos.text = ((p.x * (mapSize / 290f)) + mapSize/2).ToString ("F0") + ", " + ((p.y * (mapSize / 290f)) + mapSize/2).ToString ("F0");
				if (mouse.cAct == Mouse.ClickAction.CLICKUP) {
					//Debug.Log ("MiniMap Clicked");
					this.transform.position = new Vector3 ((p.x * (mapSize / 290f)), this.transform.position.y, (p.y * (mapSize / 290f)));
				}
			} 

			// Quick groups UI
			else if (mouse.hitInfo.collider.gameObject.name.Equals ("Group1")) {
				mouse.hitInfo.collider.gameObject.GetComponent<Image> ().color = menuHoverColor;
				LastUIHover = mouse.hitInfo.collider.gameObject;
				LastUIColor = playerColor;
				if (mouse.cAct == Mouse.ClickAction.CLICKUP) {
					//Debug.Log ("Group 1 Selected");
					selectionBox.GetComponent<SelectionBox> ().SetSelectedUnits (WorldGUI.GetGroup (1));
				} else if (mouse.cAct == Mouse.ClickAction.ACLICKUP) {
					//Debug.Log ("Group 1 Set");
					WorldGUI.SetGroup (1, selectionBox.GetComponent<SelectionBox> ().selectedUnits);
				}
			} else if (mouse.hitInfo.collider.gameObject.name.Equals ("Group2")) {
				mouse.hitInfo.collider.gameObject.GetComponent<Image> ().color = menuHoverColor;
				LastUIHover = mouse.hitInfo.collider.gameObject;
				LastUIColor = playerColor;
				if (mouse.cAct == Mouse.ClickAction.CLICKUP) {
					//Debug.Log ("Group 2 Selected");
					selectionBox.GetComponent<SelectionBox> ().SetSelectedUnits (WorldGUI.GetGroup (2));
				} else if (mouse.cAct == Mouse.ClickAction.ACLICKUP) {
					//Debug.Log ("Group 2 Set");
					WorldGUI.SetGroup (2, selectionBox.GetComponent<SelectionBox> ().selectedUnits);
				}
			} else if (mouse.hitInfo.collider.gameObject.name.Equals ("Group3")) {
				mouse.hitInfo.collider.gameObject.GetComponent<Image> ().color = menuHoverColor;
				LastUIHover = mouse.hitInfo.collider.gameObject;
				LastUIColor = playerColor;
				if (mouse.cAct == Mouse.ClickAction.CLICKUP) {
					//Debug.Log ("Group 3 Selected");
					selectionBox.GetComponent<SelectionBox> ().SetSelectedUnits (WorldGUI.GetGroup (3));
				} else if (mouse.cAct == Mouse.ClickAction.ACLICKUP) {
					//Debug.Log ("Group 3 Set");
					WorldGUI.SetGroup (3, selectionBox.GetComponent<SelectionBox> ().selectedUnits);
				}
			} else if (mouse.hitInfo.collider.gameObject.name.Equals ("Group4")) {
				mouse.hitInfo.collider.gameObject.GetComponent<Image> ().color = menuHoverColor;
				LastUIHover = mouse.hitInfo.collider.gameObject;
				LastUIColor = playerColor;
				if (mouse.cAct == Mouse.ClickAction.CLICKUP) {
					//Debug.Log ("Group 4 Selected");
					selectionBox.GetComponent<SelectionBox> ().SetSelectedUnits (WorldGUI.GetGroup (4));
				} else if (mouse.cAct == Mouse.ClickAction.ACLICKUP) {
					//Debug.Log ("Group 4 Set");
					WorldGUI.SetGroup (4, selectionBox.GetComponent<SelectionBox> ().selectedUnits);
				}
			} else if (mouse.hitInfo.collider.gameObject.name.Equals ("Group5")) {
				mouse.hitInfo.collider.gameObject.GetComponent<Image> ().color = menuHoverColor;
				LastUIHover = mouse.hitInfo.collider.gameObject;
				LastUIColor = playerColor;
				if (mouse.cAct == Mouse.ClickAction.CLICKUP) {
					//Debug.Log ("Group 5 Selected");
					selectionBox.GetComponent<SelectionBox> ().SetSelectedUnits (WorldGUI.GetGroup (5));
				} else if (mouse.cAct == Mouse.ClickAction.ACLICKUP) {
					//Debug.Log ("Group 5 Set");
					WorldGUI.SetGroup (5, selectionBox.GetComponent<SelectionBox> ().selectedUnits);
				}
			} else if (mouse.hitInfo.collider.gameObject.name.Equals ("Group6")) {
				mouse.hitInfo.collider.gameObject.GetComponent<Image> ().color = menuHoverColor;
				LastUIHover = mouse.hitInfo.collider.gameObject;
				LastUIColor = playerColor;
				if (mouse.cAct == Mouse.ClickAction.CLICKUP) {
					//Debug.Log ("Group 6 Selected");
					selectionBox.GetComponent<SelectionBox> ().SetSelectedUnits (WorldGUI.GetGroup (6));
				} else if (mouse.cAct == Mouse.ClickAction.ACLICKUP) {
					//Debug.Log ("Group 6 Set");
					WorldGUI.SetGroup (6, selectionBox.GetComponent<SelectionBox> ().selectedUnits);
				}
			} 

			// Tutorial UI
			else if (mouse.hitInfo.collider.gameObject.name.Equals ("HideShowButton")) {
				//Debug.Log ("HideShowButton Hovered");
				mouse.hitInfo.collider.gameObject.GetComponent<Image> ().color = menuHoverColor;
				LastUIHover = mouse.hitInfo.collider.gameObject;
				LastUIColor = playerColor;
				if (mouse.cAct == Mouse.ClickAction.CLICKUP) {
					tutorial.hideShowStep = true;
					//Debug.Log ("HideShowButton Clicked");
					if (HideShowText.text.Equals ("Hide")) {
						HideShowText.text = "Show";
						toShowHide.SetActive (false);
					} else {
						HideShowText.text = "Hide";
						toShowHide.SetActive (true);
					}
				}
			} else if (mouse.hitInfo.collider.gameObject.name.Equals ("NextButton") && tutorial.completedCurrentStep) {
				//Debug.Log ("NextButton Hovered");
				mouse.hitInfo.collider.gameObject.GetComponent<Image> ().color = menuHoverColor;
				LastUIHover = mouse.hitInfo.collider.gameObject;
				LastUIColor = playerColor;
				if (mouse.cAct == Mouse.ClickAction.CLICKUP && tutorial.completedCurrentStep) {
					if (tutorial.currentStep == tutorial.totalSteps) {
						LoadingScreen.show ();
						SceneManager.LoadScene (0);
					} else {
						tutorial.NextStep ();
					}
				}
			} else if (mouse.hitInfo.collider.gameObject.name.Equals ("BackButton")) {
				//Debug.Log ("BackButton Hovered");
				mouse.hitInfo.collider.gameObject.GetComponent<Image> ().color = menuHoverColor;
				LastUIHover = mouse.hitInfo.collider.gameObject;
				LastUIColor = playerColor;
				if (mouse.cAct == Mouse.ClickAction.CLICKUP) {
					tutorial.PrevStep ();
				}
			}

			// End of raycast checking on the UI
		}
	}

	/// Pause the game
	private void PauseButton(){
		// TODO: take action when pause button is pressed
		Debug.Log ("MainMenu - TODO:Pause");
	}

	/// Open the controls screen
	private void ControlsButton(){
		// TODO: take action when controls button is pressed
		Debug.Log ("MainMenu - TODO:Controls");
	}

	/// Quit game back to start menu
	public void QuitButton(){
		LoadingScreen.show ();
		SceneManager.LoadScene(0); 
	}

}
