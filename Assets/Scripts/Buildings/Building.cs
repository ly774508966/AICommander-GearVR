using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Building : MonoBehaviour {

	// Currently open menu
	public static GameObject currentMenu;
	/// Button to open buildings menu
	public GameObject openButton;
	/// Whole menu object
	public GameObject Menu;
	/// Buttons in the buildings menu 0 - 8
	public GameObject[] buttons;
	public Material[] buttonMats;

	// Build menu button hover colours
	protected Color buildMenuHoverSuccess = new Color (0.5f, 0.9f, 0.5f);
	protected Color buildMenuHoverFail = new Color (0.9f, 0.5f, 0.5f);
	protected Color buildMenuHoverDisabled = new Color (0.5f, 0.5f, 0.5f);
	protected Color buildMenuNotHover = new Color (1f, 1f, 1f);

	// Menu: Left info bar
	public GameObject title;
	public GameObject description;
	public GameObject time;
	public GameObject mass;

	// Menu: Right info bar
	public GameObject CurrentlyBuildingButton;
	public GameObject NextBuildingLeft;
	public GameObject NextBuildingMid;
	public GameObject NextBuildingRight;
	public GameObject BuildBar;
	public Text CurrentlyBuildingText;

	// Building queue fields
	protected List<int> buildQueue = new List<int>();

	public GameObject[] toColor;
	public Image[] toColorImage;

	// Building fields
	protected float TimeToComplete = -1;
	protected float TotalTime = 1;
	protected bool building = false;
	public int btnsAmt = 1;

	// Other
	protected Animator ani;

	void Awake(){
		BuildBar.transform.localScale = new Vector3 (0, 1, 0.82f);
		CurrentlyBuildingText.text = "Nothing Building";

		time.SetActive(false);
		mass.SetActive(false);
		Menu.SetActive (false);
		ani = GetComponent<Animator>();
		// Set all the object to the players color
		for (int i = 0; i < toColor.Length; i++) {
			toColor [i].GetComponent<Renderer> ().material.color = Player.playerColor;
		}
		// Set all the images to the players color
		for (int i = 0; i < toColorImage.Length; i++) {
			toColorImage [i].GetComponent<Image> ().color = Player.playerColor;
		}
	}

	void Start(){
		// Set all the images to the players color
		for (int i = 0; i < toColorImage.Length; i++) {
			toColorImage [i].GetComponent<Image> ().color = Player.playerColor;
		}
	}

	public void LateUpdate(){
		// Reset all buttons TODO: reset last clicked menu item to white
		openButton.GetComponent<Image> ().color = Player.playerColor;
		for (int i = 0; i < buttons.Length; i++) {
			buttons [i].GetComponent<Image> ().material.color = buildMenuNotHover;
		}

		ResetInfoPanel();

		CheckOpenButton ();

		CheckMenuButtons ();

		UpdateProgressBar();
	}

	/// Checks if open button hovered or clicked
	protected void CheckOpenButton(){
		openButton.GetComponent<Image> ().material.color = Player.playerColor;
		// If a button is hit by the static raycast in GameController
		if(Player.mouse.cast){
			GameObject objHit = Player.mouse.hitInfo.collider.gameObject;
			if (objHit.Equals (openButton)) {
				// Color the button
				float r = Player.menuHoverColor.r + 0.4f;
				float g = Player.menuHoverColor.g + 0.4f;
				float b = Player.menuHoverColor.b + 0.4f;
				if (r > 1)
					r = 1;
				if (g > 1)
					g = 1;
				if (b > 1)
					b = 1;
				openButton.GetComponent<Image> ().material.color = new Color(r, g, b);
				// If we're clicking
				if(Player.mouse.cAct == Mouse.ClickAction.CLICKUP){
					openButton.GetComponent<Image> ().material.color = Player.playerColor;
					OpenMenu ();
				}
			}
		}
	}

	/// Opens the buildings menu
	protected void OpenMenu(){
		if (Menu.activeSelf) {
			Menu.SetActive (false);
			currentMenu = null;
		} else {
			if(currentMenu != null)
				currentMenu.SetActive (false);
			Menu.SetActive (true);
			currentMenu = Menu;
		}
	}

	/// Checks if menu buttons hovered or clicked
	protected void CheckMenuButtons(){
		// If a button is hit by the static raycast in GameController
		if(Player.mouse.cast){
			GameObject objHit = Player.mouse.hitInfo.collider.gameObject;
			for(int i = 0 ; i < buttons.Length ; i++){
				if (objHit.Equals (buttons[i])) {
					// Color the button
					ColorBuildMenuButton(i);
					// Change info panel from item
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
			buttons [i].GetComponent<Image> ().material.color = buildMenuHoverSuccess;
		} else if (false) { // TODO: Not enough cash
			buttons [i].GetComponent<Image> ().material.color = buildMenuHoverFail;
		} else {
			buttons [i].GetComponent<Image> ().material.color = buildMenuHoverDisabled;
		}
	}

	/// Updates progress bar based on how far the current build is to completion
	protected void UpdateProgressBar(){
		if (TimeToComplete > 0) {
			building = true;
			BuildBar.transform.localScale = new Vector3 (TimeToComplete / TotalTime, 1, 0.82f);
			TimeToComplete--;
			PlayAni(true);
		} else if(TimeToComplete == 0) {
			TimeToComplete = -1;
			FinishedBuilding ();
			CurrentlyBuildingButton.GetComponent<Image> ().material = null;
			CurrentlyBuildingText.text = "Nothing Building";
			building = false;
			BuildBar.transform.localScale = new Vector3 (0, 1, 0.82f);
			PlayAni(false);

				
			// Build queue
			if(buildQueue.Count > 0){
				ClickButton(buildQueue [0]);
				buildQueue.RemoveAt(0);
				NextBuildingLeft.GetComponent<Image> ().material = NextBuildingMid.GetComponent<Image> ().material;
				NextBuildingMid.GetComponent<Image> ().material = NextBuildingRight.GetComponent<Image> ().material;
				NextBuildingRight.GetComponent<Image> ().material = null;
			}
		}
	}

	/// Play animation for the building
	protected void PlayAni(bool tf){
		ani.SetBool ("isBuilding", tf);
	}

	// ### INDIVIDUAL BUILDING ACTIONS ###

	/// Take action on a button clicked
	protected virtual void ClickButton(int i){
		Debug.Log ("FILL THIS WITH AN ACTION IN THE LOWER CLASSES");
	}

	protected virtual void ChangeInfoPanel(int i){
		Debug.Log ("FILL THIS WITH AN ACTION IN THE LOWER CLASSES");
	}

	protected virtual void ResetInfoPanel(){
		Debug.Log ("FILL THIS WITH AN ACTION IN THE LOWER CLASSES");
	}

	/// Act on finished build from building
	protected virtual void FinishedBuilding(){
		Debug.Log ("FILL THIS WITH AN ACTION IN THE LOWER CLASSES");
	}
}
