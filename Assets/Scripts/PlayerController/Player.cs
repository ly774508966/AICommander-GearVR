using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Player : MonoBehaviour {
	
	// Stats
	public float speedMultiplier = 1.0f;

	// Controller pivot and cursor fields
	public GameObject controllerPivot;
	public GameObject cursor;

	// Main menu Fields
	private bool paused = false;
	private GameObject PauseBtn;
	private GameObject ControlsBtn;
	private GameObject QuitBtn;
	private GameObject LastMenuHover;
	private Color LastMenuColor;

	/// The color of the player
	public static Color playerColor = new Color (200f/255f, 53f/255f, 53f/255f, 130f/255f);
	public static Color menuHoverColor;

	// Build Mode fields
	private static bool buildMode = false;
	private static string structure;
	private static GameObject currentBuildGhost = null;
	// Ghost Building prefabs
	public GameObject GTAP;
	public GameObject GRC;
	// Building prefabs
	public GameObject TAP;
	public GameObject RC;

	// Other fields
	public GameObject KMCam;
	private float moveAmount = 0.3f;
	public static Mouse mouse = new Mouse ();
	public static Keyboard keyboard = new Keyboard ();
	public GameObject selectionBox;

	/// Find objects and init fields
	void Start(){
		// Find menu buttons
		PauseBtn = GameObject.FindGameObjectWithTag ("PauseButton");
		ControlsBtn = GameObject.FindGameObjectWithTag ("ControlsButton");
		QuitBtn = GameObject.FindGameObjectWithTag ("QuitButton");

		// Init other
		Cursor.lockState = CursorLockMode.Locked;
		cursor.GetComponent<Renderer> ().material.color = playerColor;


		menuHoverColor = new Color (playerColor.r, playerColor.g, playerColor.b, 1);
	}

	/// Game Loop
	void Update() {
		// Checks if the game is paused
		if (paused) {
			if (Input.GetMouseButtonUp(0)) {
				paused = false;
				// pauseScreen.SetActive (false);
				return;
			}
			return;
		}

		// Update input from the mouse
		mouse.UpdateMouseInput ();
		// Update the players view based on the mouse input
		UpdateView();
		// Handle clicks based on the mouse input
		Clicks ();	

		// Update input from the keyboard
		keyboard.KeyboardInput ();
		// Move player based on the keyboard input
		Move ();
	}	

	/// Update the players view direction, raycast and cursor based on the mouse inputs
	private void UpdateView(){
		// Rotate view direction
		if(mouse.x != 0 || mouse.y != 0){
			Vector3 v = new Vector3(KMCam.transform.rotation.eulerAngles.x - mouse.y, KMCam.transform.rotation.eulerAngles.y + mouse.x, KMCam.transform.rotation.eulerAngles.z);
			KMCam.transform.rotation = Quaternion.Euler (v);
		}

		// Updates the direction of the controllers pivot point to be the same direction as the players view
		controllerPivot.transform.rotation = KMCam.transform.rotation;

		// Updates the raycast from the controllers pivot point
		Vector3 rayDirection = controllerPivot.transform.rotation * Vector3.forward;
		LayerMask layerMask = (1 << 2);
		layerMask = ~layerMask;
		mouse.cast = Physics.Raycast (transform.position, rayDirection, out mouse.hitInfo, 1200, layerMask);

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

	/// Hondle mouse clicks
	private void Clicks(){
		// Check for double click
		if(mouse.cAct == Mouse.ClickAction.CLICKUP){
			mouse.DoubleClick ();
		}

		// Handle main menu hover and click
		MenuHoverClick ();

		// If we're in build mode (we're trying to place a location for a building)
		if (buildMode) {
			// If there isn't a build ghost yet then this is the first buildMode frame. Create the buildGhost
			if (currentBuildGhost == null) {
				if (structure.Equals ("Tri Axis Printer")) {
					currentBuildGhost = (GameObject)GameObject.Instantiate (GTAP, mouse.hitInfo.point, new Quaternion ());
				} else if (structure.Equals ("Research Center")) {
					currentBuildGhost = (GameObject)GameObject.Instantiate (GRC, mouse.hitInfo.point, new Quaternion ());
				}
				currentBuildGhost.SetActive (false);
			}
			// If we hit something and it wasn't a UI object
			if (mouse.cast && !(mouse.hitInfo.collider.gameObject.layer == 5)) {
				// Show ghost building and reposition
				currentBuildGhost.transform.position = mouse.hitInfo.point;
				currentBuildGhost.SetActive (true);
			} else {
				// Hide ghost building
				currentBuildGhost.SetActive (false);
			}
		} else if (mouse.cAct != Mouse.ClickAction.NULL) {
			selectionBox.GetComponent<SelectionBox> ().HandleSelection (mouse);
		}

		// TODO: Handle App button on selected units
		if (mouse.cAct == Mouse.ClickAction.ACLICKUP && selectionBox.GetComponent<SelectionBox> ().selectedUnits.Count > 0) {
			// Tell first selected unit the app button has been clicked
			selectionBox.GetComponent<SelectionBox> ().selectedUnits[0].ClickedAppButton();
		}
	}

	/// Move the specified x and z from keyboard input
	private void Move(){
		if (!(keyboard.x > 0 || keyboard.x < 0 || keyboard.z > 0 || keyboard.z < 0))
			return;
		Quaternion q = transform.rotation;
		transform.rotation = KMCam.transform.rotation;
		transform.rotation = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w);
		if (speedMultiplier < 0)
			speedMultiplier = 0;
		if(keyboard.x >= moveAmount || keyboard.x <= -moveAmount || keyboard.z >= moveAmount || keyboard.z <= -moveAmount)
			transform.Translate (new Vector3 (keyboard.x * speedMultiplier, 0, -keyboard.z * speedMultiplier));
		transform.rotation = q;
	}

	// This is called to start build mode after a build structure button has been clicked
	public static void BuildMode(string structure){
		buildMode = true;
		structure = structure;
	}

	// #### MENU METHODS ####

	///  Handles main menu hovers and clicks
	public bool MenuHoverClick(){
		// If LastMenuHover is not null (ie we had been hovoring over something last frame) turn it back to normal
		if(LastMenuHover != null)
			LastMenuHover.GetComponent<Image> ().color = LastMenuColor;

		// Set the button currently being hovered over to the active hover color and check for clicks
		if (mouse.cast) {
			if (mouse.hitInfo.collider.gameObject.tag.Equals ("PauseButton")) {
				//Debug.Log ("PauseButton Hover");
				mouse.hitInfo.collider.gameObject.GetComponent<Image> ().color = menuHoverColor;
				LastMenuHover = mouse.hitInfo.collider.gameObject;
				LastMenuColor = playerColor;
				if (mouse.cAct == Mouse.ClickAction.CLICKUP) {
					//Debug.Log ("PauseButton Clicked");
					PauseButton();
				}
			} else if(mouse.hitInfo.collider.gameObject.tag.Equals ("ControlsButton")){
				//Debug.Log ("ControlsButton Hover");
				mouse.hitInfo.collider.gameObject.GetComponent<Image> ().color = menuHoverColor;
				LastMenuHover = mouse.hitInfo.collider.gameObject;
				LastMenuColor = playerColor;
				if (mouse.cAct == Mouse.ClickAction.CLICKUP) {
					//Debug.Log ("ControlsButton Clicked");
					ControlsButton();
				}
			} else if(mouse.hitInfo.collider.gameObject.tag.Equals ("QuitButton")){
				//Debug.Log ("QuitButton Hover");
				mouse.hitInfo.collider.gameObject.GetComponent<Image> ().color = menuHoverColor;
				LastMenuHover = mouse.hitInfo.collider.gameObject;
				LastMenuColor = playerColor;
				if (mouse.cAct == Mouse.ClickAction.CLICKUP) {
					//Debug.Log ("QuitButton Clicked");
					QuitButton();
				}
			} else if(mouse.hitInfo.collider.gameObject.name.Equals ("Group1")){
				mouse.hitInfo.collider.gameObject.GetComponent<Image> ().color = menuHoverColor;
				LastMenuHover = mouse.hitInfo.collider.gameObject;
				LastMenuColor = playerColor;
				if (mouse.cAct == Mouse.ClickAction.CLICKUP) {
					Debug.Log ("Group 1 Selected");
				} else if (mouse.cAct == Mouse.ClickAction.ACLICKUP) {
					Debug.Log ("Group 1 Set");
				}
			} else if(mouse.hitInfo.collider.gameObject.name.Equals ("Group2")){
				mouse.hitInfo.collider.gameObject.GetComponent<Image> ().color = menuHoverColor;
				LastMenuHover = mouse.hitInfo.collider.gameObject;
				LastMenuColor = playerColor;
				if (mouse.cAct == Mouse.ClickAction.CLICKUP) {
					Debug.Log ("Group 2 Selected");
				} else if (mouse.cAct == Mouse.ClickAction.ACLICKUP) {
					Debug.Log ("Group 2 Set");
				}
			} else if(mouse.hitInfo.collider.gameObject.name.Equals ("Group3")){
				mouse.hitInfo.collider.gameObject.GetComponent<Image> ().color = menuHoverColor;
				LastMenuHover = mouse.hitInfo.collider.gameObject;
				LastMenuColor = playerColor;
				if (mouse.cAct == Mouse.ClickAction.CLICKUP) {
					Debug.Log ("Group 3 Selected");
				} else if (mouse.cAct == Mouse.ClickAction.ACLICKUP) {
					Debug.Log ("Group 3 Set");
				}
			} else if(mouse.hitInfo.collider.gameObject.name.Equals ("Group4")){
				mouse.hitInfo.collider.gameObject.GetComponent<Image> ().color = menuHoverColor;
				LastMenuHover = mouse.hitInfo.collider.gameObject;
				LastMenuColor = playerColor;
				if (mouse.cAct == Mouse.ClickAction.CLICKUP) {
					Debug.Log ("Group 4 Selected");
				} else if (mouse.cAct == Mouse.ClickAction.ACLICKUP) {
					Debug.Log ("Group 4 Set");
				}
			} else if(mouse.hitInfo.collider.gameObject.name.Equals ("Group5")){
				mouse.hitInfo.collider.gameObject.GetComponent<Image> ().color = menuHoverColor;
				LastMenuHover = mouse.hitInfo.collider.gameObject;
				LastMenuColor = playerColor;
				if (mouse.cAct == Mouse.ClickAction.CLICKUP) {
					Debug.Log ("Group 5 Selected");
				} else if (mouse.cAct == Mouse.ClickAction.ACLICKUP) {
					Debug.Log ("Group 5 Set");
				}
			} else if(mouse.hitInfo.collider.gameObject.name.Equals ("Group6")){
				mouse.hitInfo.collider.gameObject.GetComponent<Image> ().color = menuHoverColor;
				LastMenuHover = mouse.hitInfo.collider.gameObject;
				LastMenuColor = playerColor;
				if (mouse.cAct == Mouse.ClickAction.CLICKUP) {
					Debug.Log ("Group 6 Selected");
				} else if (mouse.cAct == Mouse.ClickAction.ACLICKUP) {
					Debug.Log ("Group 6 Set");
				}
			}
		}

		return false;
	}

	/// Pause the game
	private void PauseButton(){
		Debug.Log ("MainMenu: Pause");
		// pauseScreen.SetActive (true);
		paused = true;
	}

	/// TODO: Open the controls screen
	private void ControlsButton(){
		Debug.Log ("MainMenu: TODO:Controls");
	}

	/// Quit game
	public void QuitButton(){
		Debug.Log ("MainMenu: Quit");
		LoadingScreen.show ();
		SceneManager.LoadScene(0); 
	}

}
