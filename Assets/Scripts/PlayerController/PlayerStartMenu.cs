using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PlayerStartMenu : MonoBehaviour {

	// Controller pivot and cursor fields
	public GameObject controllerPivot;
	public GameObject cursor;
	public Image[] ToColor;

	// Color selector fields
	private float currentColor;
	public static float selectedColor = 2;
	private GameObject LastHover;
	private GameObject LastSoundObject;
	private Color LastHoverColor;
	private Color menuRestColor;
	private Color menuSelectColor;
	private Color menuHoverColor;
	private Color ColorNotSelected = Color.black;
	private Color ColorSelected = new Color (238f/255f, 238f/255f, 238f/255f);

	private Color Color1Rest = new Color (200f/255f, 53f/255f, 53f/255f, 100f/255f);
	private Color Color2Rest = new Color (0f/255f, 224f/255f, 255f/255f, 100f/255f);
	private Color Color3Rest = new Color (0f/255f, 214f/255f, 37f/255f, 100f/255f);
	private Color Color4Rest = new Color (255f/255f, 185f/255f, 0f/255f, 100f/255f);
	/*private Color Color1Hover = new Color (1, 1, 1, 150f/255f);
	private Color Color2Hover = new Color (1, 1, 1, 150f/255f);
	private Color Color3Hover = new Color (1, 1, 1, 150f/255f);
	private Color Color4Hover = new Color (1, 1, 1, 150f/255f);*/
	private Color Color1Hover = new Color (200f/255f, 53f/255f, 53f/255f, 150f/255f);
	private Color Color2Hover = new Color (0f/255f, 224f/255f, 255f/255f, 150f/255f);
	private Color Color3Hover = new Color (0f/255f, 214f/255f, 37f/255f, 150f/255f);
	private Color Color4Hover = new Color (255f/255f, 185f/255f, 0f/255f, 150f/255f);
	/*private Color Color1Select = new Color (1, 1, 1, 200f/255f);
	private Color Color2Select = new Color (1, 1, 1, 200f/255f);
	private Color Color3Select = new Color (1, 1, 1, 200f/255f);
	private Color Color4Select = new Color (1, 1, 1, 200f/255f);*/
	private Color Color1Select = new Color (200f/255f, 53f/255f, 53f/255f, 200f/255f);
	private Color Color2Select = new Color (0f/255f, 224f/255f, 255f/255f, 200f/255f);
	private Color Color3Select = new Color (0f/255f, 214f/255f, 37f/255f, 200f/255f);
	private Color Color4Select = new Color (255f/255f, 185f/255f, 0f/255f, 200f/255f);

	protected Color menuRest = new Color (1.0f, 1.0f, 1.0f, 100.0f/255.0f);
	protected Color menuHover = new Color (1.0f, 1.0f, 1.0f, 150.0f/255.0f);
	protected Color menuSelect = new Color (1.0f, 1.0f, 1.0f, 200.0f/255.0f);
	public GameObject Color1BG;
	public GameObject Color2BG;
	public GameObject Color3BG;
	public GameObject Color4BG;

	// Other fields
	public GameObject KMCam;
	public static Mouse mouse = new Mouse ();
	public static Keyboard keyboard = new Keyboard ();

	// New fields
	[Header("Middle panel")]
	public GameObject[] buildMenuButtons;
	protected GameObject currentlySelected;

	[Header("Right panel")]
	public GameObject[] RPButtons;
	public Text RPTitle;

	public GameObject RPTut;
	public GameObject RPNew;
	public GameObject RPLoad;
	public GameObject RPExit;
	// New game Panel
	public Text RPNewEPlayers;
	private float enemyPlayers = 0;
	public Text RPStartMass;
	private float startMass = 10000;
	public Text RPGameSpeed;
	private float gameSpeed = 1;

	public AudioSource source;
	public AudioClip clipHover;
	public AudioClip clipOpen;
	private float vol = 0.6f;

	/// Find objects and init fields
	void Awake(){
		// Init other
		Cursor.lockState = CursorLockMode.Locked;
		currentColor = selectedColor;
		if (selectedColor == 1) {
			for (int i = 0; i < ToColor.Length; i++) {
				ToColor [i].color = Color1Rest;
			}
			cursor.GetComponent<Renderer> ().material.color = Color1Rest;
			menuRestColor = Color1Rest;
			menuHoverColor = new Color (1, 1, 1, 150f/255f);
			menuSelectColor = Color1Select;
			Color1BG.GetComponent<Image> ().color = ColorSelected;
			Color2BG.GetComponent<Image> ().color = ColorNotSelected;
			Color3BG.GetComponent<Image> ().color = ColorNotSelected;
			Color4BG.GetComponent<Image> ().color = ColorNotSelected;
		} else if(selectedColor == 2){
			for (int i = 0; i < ToColor.Length; i++) {
				ToColor [i].color = Color2Rest;
			}
			cursor.GetComponent<Renderer> ().material.color = Color2Rest;
			menuRestColor = Color2Rest;
			menuHoverColor = new Color (1, 1, 1, 150f/255f);
			menuSelectColor = Color2Select;
			Color1BG.GetComponent<Image> ().color = ColorNotSelected;
			Color2BG.GetComponent<Image> ().color = ColorSelected;
			Color3BG.GetComponent<Image> ().color = ColorNotSelected;
			Color4BG.GetComponent<Image> ().color = ColorNotSelected;
		} else if(selectedColor == 3){
			for (int i = 0; i < ToColor.Length; i++) {
				ToColor [i].color = Color3Rest;
			}
			cursor.GetComponent<Renderer> ().material.color = Color3Rest;
			menuRestColor = Color3Rest;
			menuHoverColor = new Color (1, 1, 1, 150f/255f);
			menuSelectColor = Color3Select;
			Color1BG.GetComponent<Image> ().color = ColorNotSelected;
			Color2BG.GetComponent<Image> ().color = ColorNotSelected;
			Color3BG.GetComponent<Image> ().color = ColorSelected;
			Color4BG.GetComponent<Image> ().color = ColorNotSelected;
		} else if(selectedColor == 4){
			for (int i = 0; i < ToColor.Length; i++) {
				ToColor [i].color = Color4Rest;
			}
			cursor.GetComponent<Renderer> ().material.color = Color4Rest;
			menuRestColor = Color4Rest;
			menuHoverColor = new Color (1, 1, 1, 150f/255f);
			menuSelectColor = Color4Select;
			Color1BG.GetComponent<Image> ().color = ColorNotSelected;
			Color2BG.GetComponent<Image> ().color = ColorNotSelected;
			Color3BG.GetComponent<Image> ().color = ColorNotSelected;
			Color4BG.GetComponent<Image> ().color = ColorSelected;
		}
	}

	void Start(){
		// Menu init
		RPTitle.text = "Tutorial";
		currentlySelected = buildMenuButtons [0];
		SetUIColor ();

		RPTut.SetActive(true);
		RPNew.SetActive(false);
		RPLoad.SetActive(false);
		RPExit.SetActive(false);
	}

	private void SetUIColor(){
		for (int i = 0; i < buildMenuButtons.Length; i++) {
			buildMenuButtons [i].GetComponent<Image>().material.color = menuRestColor;
		}
		currentlySelected.GetComponent<Image>().material.color = menuSelectColor;

		for (int i = 0; i < RPButtons.Length; i++) {
			RPButtons [i].GetComponent<Image>().material.color = menuRestColor;
		}
	}

	/// Game Loop
	void Update() {

		// Color changed
		if (currentColor != selectedColor) {
			currentColor = selectedColor;
		}

		// Update input from the mouse
		mouse.UpdateMouseInput ();
		// Update the players view based on the mouse input
		UpdateView();
		// Handle clicks based on the mouse input
		Clicks ();	

		// Update input from the keyboard
		keyboard.KeyboardInput ();
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

	/// Handle mouse clicks
	private void Clicks(){
		// Set the last button hovered over back to normal rest color
		if (LastHover != null) {
			LastHover.GetComponent<Image> ().material.color = LastHoverColor;
			LastHover = null;
		}

		// Set the button currently being hovered over to the active hover color and check for clicks
		if (mouse.cast) {
			// Get the colliders gameobject name
			GameObject obj = mouse.hitInfo.collider.gameObject;

			// Middle Panel
			if (obj.name.Equals ("Tutorial")) {
				if(obj != LastSoundObject){
					source.PlayOneShot (clipHover, vol);
				}
				// If this button is selected dont do anything
				if(currentlySelected != null && currentlySelected.Equals (buildMenuButtons[0])){
					LastSoundObject = obj;
					return;
				}
				// Set hover
				obj.GetComponent<Image> ().material.color = menuHoverColor;
				LastHover = obj;
				LastHoverColor = menuRestColor;
				// Clicked
				if (mouse.cAct == Mouse.ClickAction.CLICKUP) {
					source.PlayOneShot (clipOpen, vol * 0.6f);
					LastHover = null;
					if(currentlySelected != null)
						currentlySelected.GetComponent<Image>().material.color = menuRestColor;
					currentlySelected = obj;
					currentlySelected.GetComponent<Image>().material.color = menuSelectColor;
					RPTitle.text = "Tutorial";
					RPTut.SetActive(true);
					RPNew.SetActive(false);
					RPLoad.SetActive(false);
					RPExit.SetActive(false);
				}
			} else if (obj.name.Equals ("New")) {
				if(obj != LastSoundObject){
					source.PlayOneShot (clipHover, vol);
				}
				// If this button is selected dont do anything
				if(currentlySelected != null && currentlySelected.Equals (buildMenuButtons[1])){
					LastSoundObject = obj;
					return;
				}
				// Set hover
				obj.GetComponent<Image> ().material.color = menuHoverColor;
				LastHover = obj;
				LastHoverColor = menuRestColor;
				// Clicked
				if (mouse.cAct == Mouse.ClickAction.CLICKUP) {
					source.PlayOneShot (clipOpen, vol * 0.6f);
					LastHover = null;
					if(currentlySelected != null)
						currentlySelected.GetComponent<Image>().material.color = menuRestColor;
					currentlySelected = obj;
					currentlySelected.GetComponent<Image>().material.color = menuSelectColor;
					RPTitle.text = "New Game";
					RPNew.SetActive(true);
					RPTut.SetActive(false);
					RPLoad.SetActive(false);
					RPExit.SetActive(false);
				}
			} else if(obj.name.Equals ("Load")){
				if(obj != LastSoundObject){
					source.PlayOneShot (clipHover, vol);
				}
				// If this button is selected dont do anything
				if(currentlySelected != null && currentlySelected.Equals (buildMenuButtons[2])){
					LastSoundObject = obj;
					return;
				}
				// Set hover
				obj.GetComponent<Image> ().material.color = menuHoverColor;
				LastHover = obj;
				LastHoverColor = menuRestColor;
				// Clicked
				if (mouse.cAct == Mouse.ClickAction.CLICKUP) {
					source.PlayOneShot (clipOpen, vol * 0.6f);
					LastHover = null;
					if(currentlySelected != null)
						currentlySelected.GetComponent<Image>().material.color = menuRestColor;
					currentlySelected = obj;
					currentlySelected.GetComponent<Image>().material.color = menuSelectColor;
					RPTitle.text = "Load Game";
					RPLoad.SetActive(true);
					RPTut.SetActive(false);
					RPNew.SetActive(false);
					RPExit.SetActive(false);
				}
			} else if(obj.name.Equals ("Exit")){
				if(obj != LastSoundObject){
					source.PlayOneShot (clipHover, vol);
				}
				// If this button is selected dont do anything
				if(currentlySelected != null && currentlySelected.Equals (buildMenuButtons[3])){
					LastSoundObject = obj;
					return;
				}
				// Set hover
				obj.GetComponent<Image> ().material.color = menuHoverColor;
				LastHover = obj;
				LastHoverColor = menuRestColor;
				// Clicked
				if (mouse.cAct == Mouse.ClickAction.CLICKUP) {
					source.PlayOneShot (clipOpen, vol * 0.6f);
					LastHover = null;
					if(currentlySelected != null)
						currentlySelected.GetComponent<Image>().material.color = menuRestColor;
					currentlySelected = obj;
					currentlySelected.GetComponent<Image>().material.color = menuSelectColor;
					RPTitle.text = "Exit Game";
					RPExit.SetActive(true);
					RPTut.SetActive(false);
					RPNew.SetActive(false);
					RPLoad.SetActive(false);
				}
			} 

			// Right panel
			else if(obj.name.Equals ("RPExit")){
				if(obj != LastSoundObject){
					source.PlayOneShot (clipHover, vol);
				}
				// Set hover
				obj.GetComponent<Image> ().material.color = menuHoverColor;
				LastHover = obj;
				LastHoverColor = menuRestColor;
				// Clicked
				if (mouse.cAct == Mouse.ClickAction.CLICKUP) {
					source.PlayOneShot (clipOpen, vol * 0.6f);
					#if UNITY_EDITOR
					EditorApplication.isPlaying = false;
					#else
					Application.Quit();
					#endif
				}
			} else if(obj.name.Equals ("RPNewStart")){
				if(obj != LastSoundObject){
					source.PlayOneShot (clipHover, vol);
				}
				// Set hover
				obj.GetComponent<Image> ().material.color = menuHoverColor;
				LastHover = obj;
				LastHoverColor = menuRestColor;
				// Clicked
				if (mouse.cAct == Mouse.ClickAction.CLICKUP) {
					source.PlayOneShot (clipOpen, vol * 0.6f);
					// Set player color
					if(currentColor == 1){
						HumanPlayer.COLORTOSET = Color1Rest;
					} else if(currentColor == 2){
						HumanPlayer.COLORTOSET = Color2Rest;
					} else if(currentColor == 3){
						HumanPlayer.COLORTOSET = Color3Rest;
					} else if(currentColor == 4){
						HumanPlayer.COLORTOSET = Color4Rest;
					}
					// Load map
					LoadingScreen.show ();
					SceneManager.LoadScene(2);
				}
			} 

			// New game options
			else if(obj.name.Equals ("PlusEPlayers")){
				if(obj != LastSoundObject){
					source.PlayOneShot (clipHover, vol);
				}
				// Set hover
				obj.GetComponent<Image> ().material.color = menuSelect;
				LastHover = obj;
				LastHoverColor = menuRest;
				// Clicked
				if (mouse.cAct == Mouse.ClickAction.CLICKUP) {
					if(enemyPlayers != 3)
						enemyPlayers++;
					RPNewEPlayers.text = "" + enemyPlayers;
				}
			} else if(obj.name.Equals ("MinusEPlayers")){
				if(obj != LastSoundObject){
					source.PlayOneShot (clipHover, vol);
				}
				// Set hover
				obj.GetComponent<Image> ().material.color = menuSelect;
				LastHover = obj;
				LastHoverColor = menuRest;
				// Clicked
				if (mouse.cAct == Mouse.ClickAction.CLICKUP) {
					if(enemyPlayers != 0)
						enemyPlayers--;
					RPNewEPlayers.text = "" + enemyPlayers;
				}
			} else if(obj.name.Equals ("PlusMass")){
				if(obj != LastSoundObject){
					source.PlayOneShot (clipHover, vol);
				}
				// Set hover
				obj.GetComponent<Image> ().material.color = menuSelect;
				LastHover = obj;
				LastHoverColor = menuRest;
				// Clicked
				if (mouse.cAct == Mouse.ClickAction.CLICKUP) {
					if(startMass != 99000)
						startMass += 1000;
					RPStartMass.text = "" + startMass;
				}
				if (mouse.cAct == Mouse.ClickAction.ACLICKUP) {
					if(startMass < 89000)
						startMass += 10000;
					else
						startMass = 99000;
					RPStartMass.text = "" + startMass;
				}
			} else if(obj.name.Equals ("MinusMass")){
				if(obj != LastSoundObject){
					source.PlayOneShot (clipHover, vol);
				}
				// Set hover
				obj.GetComponent<Image> ().material.color = menuSelect;
				LastHover = obj;
				LastHoverColor = menuRest;
				// Clicked
				if (mouse.cAct == Mouse.ClickAction.CLICKUP) {
					if(startMass != 2000)
						startMass -= 1000;
					RPStartMass.text = "" + startMass;
				}
				if (mouse.cAct == Mouse.ClickAction.ACLICKUP) {
					if(startMass > 12000)
						startMass -= 10000;
					else
						startMass = 2000;
					RPStartMass.text = "" + startMass;
				}
			} else if(obj.name.Equals ("PlusSpeed")){
				if(obj != LastSoundObject){
					source.PlayOneShot (clipHover, vol);
				}
				// Set hover
				obj.GetComponent<Image> ().material.color = menuSelect;
				LastHover = obj;
				LastHoverColor = menuRest;
				// Clicked
				if (mouse.cAct == Mouse.ClickAction.CLICKUP) {
					if(gameSpeed != 2)
						gameSpeed += 0.25f;
					RPGameSpeed.text = "<size=12>x</size>" + gameSpeed;
				}
			} else if(obj.name.Equals ("MinusSpeed")){
				if(obj != LastSoundObject){
					source.PlayOneShot (clipHover, vol);
				}
				// Set hover
				obj.GetComponent<Image> ().material.color = menuSelect;
				LastHover = obj;
				LastHoverColor = menuRest;
				// Clicked
				if (mouse.cAct == Mouse.ClickAction.CLICKUP) {
					if(gameSpeed != 0.25f)
						gameSpeed -= 0.25f;
					RPGameSpeed.text = "<size=12>x</size>" + gameSpeed;
				}
			}

			// Color buttons
			else if(obj.name.Equals ("Color1")){
				// Set hover
				obj.GetComponent<Image> ().material.color = Color1Hover;
				LastHover = obj;
				LastHoverColor = Color1Select;
				// Clicked
				if (mouse.cAct == Mouse.ClickAction.CLICKUP) {
					source.PlayOneShot (clipHover, vol);
					selectedColor = 1;
					Color1BG.GetComponent<Image> ().color = ColorSelected;
					Color2BG.GetComponent<Image> ().color = ColorNotSelected;
					Color3BG.GetComponent<Image> ().color = ColorNotSelected;
					Color4BG.GetComponent<Image> ().color = ColorNotSelected;

					for (int i = 0; i < ToColor.Length; i++) {
						ToColor [i].color = Color1Hover;
					}
					cursor.GetComponent<Renderer> ().material.color = Color1Rest;
					menuRestColor = Color1Rest;
					menuHoverColor = new Color (1, 1, 1, 150f/255f);
					menuSelectColor = Color1Select;

					SetUIColor ();
				}
			} else if(obj.name.Equals ("Color2")){
				// Set hover
				obj.GetComponent<Image> ().material.color = Color2Hover;
				LastHover = obj;
				LastHoverColor = Color2Select;
				// Clicked
				if (mouse.cAct == Mouse.ClickAction.CLICKUP) {
					source.PlayOneShot (clipHover, vol);
					selectedColor = 2;
					Color1BG.GetComponent<Image> ().color = ColorNotSelected;
					Color2BG.GetComponent<Image> ().color = ColorSelected;
					Color3BG.GetComponent<Image> ().color = ColorNotSelected;
					Color4BG.GetComponent<Image> ().color = ColorNotSelected;

					for (int i = 0; i < ToColor.Length; i++) {
						ToColor [i].color = Color2Hover;
					}
					cursor.GetComponent<Renderer> ().material.color = Color2Rest;
					menuRestColor = Color2Rest;
					menuHoverColor = new Color (1, 1, 1, 150f/255f);
					menuSelectColor = Color2Select;

					SetUIColor ();
				}
			} else if(obj.name.Equals ("Color3")){
				// Set hover
				obj.GetComponent<Image> ().material.color = Color3Hover;
				LastHover = obj;
				LastHoverColor = Color3Select;
				// Clicked
				if (mouse.cAct == Mouse.ClickAction.CLICKUP) {
					source.PlayOneShot (clipHover, vol);
					selectedColor = 3;
					Color1BG.GetComponent<Image> ().color = ColorNotSelected;
					Color2BG.GetComponent<Image> ().color = ColorNotSelected;
					Color3BG.GetComponent<Image> ().color = ColorSelected;
					Color4BG.GetComponent<Image> ().color = ColorNotSelected;

					for (int i = 0; i < ToColor.Length; i++) {
						ToColor [i].color = Color3Hover;
					}
					cursor.GetComponent<Renderer> ().material.color = Color3Rest;
					menuRestColor = Color3Rest;
					menuHoverColor = new Color (1, 1, 1, 150f/255f);
					menuSelectColor = Color3Select;

					SetUIColor ();
				}
			} else if(obj.name.Equals ("Color4")){
				// Set hover
				obj.GetComponent<Image> ().material.color = Color4Hover;
				LastHover = obj;
				LastHoverColor = Color4Select;
				// Clicked
				if (mouse.cAct == Mouse.ClickAction.CLICKUP) {
					source.PlayOneShot (clipHover, vol);
					selectedColor = 4;
					Color1BG.GetComponent<Image> ().color = ColorNotSelected;
					Color2BG.GetComponent<Image> ().color = ColorNotSelected;
					Color3BG.GetComponent<Image> ().color = ColorNotSelected;
					Color4BG.GetComponent<Image> ().color = ColorSelected;

					for (int i = 0; i < ToColor.Length; i++) {
						ToColor [i].color = Color4Hover;
					}
					cursor.GetComponent<Renderer> ().material.color = Color4Rest;
					menuRestColor = Color4Rest;
					menuHoverColor = new Color (1, 1, 1, 150f/255f);
					menuSelectColor = Color4Select;

					SetUIColor ();
				}
			}

			LastSoundObject = obj;
		}
	}
}
