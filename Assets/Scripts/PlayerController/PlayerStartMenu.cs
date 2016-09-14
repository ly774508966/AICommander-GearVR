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
	public Material touchUpMat;
	public Material touchDownMat;
	public Material clickDownMat;
	public Image[] ToColor;

	// Color selector fields
	private float currentColor;
	public static float selectedColor = 2;
	private GameObject LastHover;
	private Color LastHoverColor;
	private Color menuRestColor;
	private Color menuHoverColor;
	private Color ColorNotSelected = Color.black;
	private Color ColorSelected = new Color (238f/255f, 238f/255f, 238f/255f);
	private Color Color1Hover = new Color (200f/255f, 53f/255f, 53f/255f, 100f/255f);
	private Color Color2Hover = new Color (0f/255f, 224f/255f, 255f/255f, 100f/255f);
	private Color Color3Hover = new Color (0f/255f, 214f/255f, 37f/255f, 100f/255f);
	private Color Color4Hover = new Color (255f/255f, 185f/255f, 0f/255f, 100f/255f);
	private Color Color1Rest = new Color (200f/255f, 53f/255f, 53f/255f);
	private Color Color2Rest = new Color (0f/255f, 224f/255f, 255f/255f);
	private Color Color3Rest = new Color (0f/255f, 214f/255f, 37f/255f);
	private Color Color4Rest = new Color (255f/255f, 185f/255f, 0f/255f);
	public GameObject Color1BG;
	public GameObject Color2BG;
	public GameObject Color3BG;
	public GameObject Color4BG;

	// Other fields
	public GameObject KMCam;
	public static Mouse mouse = new Mouse ();
	public static Keyboard keyboard = new Keyboard ();

	/// Find objects and init fields
	void Awake(){
		// Init other
		Cursor.lockState = CursorLockMode.Locked;
		currentColor = selectedColor;
		if (selectedColor == 1) {
			for (int i = 0; i < ToColor.Length; i++) {
				ToColor [i].color = Color1Hover;
			}
			cursor.GetComponent<Renderer> ().material.color = Color1Hover;
			menuRestColor = Color1Hover;
			menuHoverColor = new Color (Color1Hover.r, Color1Hover.g, Color1Hover.b, 1);
			Color1BG.GetComponent<Image> ().color = ColorSelected;
			Color2BG.GetComponent<Image> ().color = ColorNotSelected;
			Color3BG.GetComponent<Image> ().color = ColorNotSelected;
			Color4BG.GetComponent<Image> ().color = ColorNotSelected;
		} else if(selectedColor == 2){
			for (int i = 0; i < ToColor.Length; i++) {
				ToColor [i].color = Color2Hover;
			}
			cursor.GetComponent<Renderer> ().material.color = Color2Hover;
			menuRestColor = Color2Hover;
			menuHoverColor = new Color (Color2Hover.r, Color2Hover.g, Color2Hover.b, 1);
			Color1BG.GetComponent<Image> ().color = ColorNotSelected;
			Color2BG.GetComponent<Image> ().color = ColorSelected;
			Color3BG.GetComponent<Image> ().color = ColorNotSelected;
			Color4BG.GetComponent<Image> ().color = ColorNotSelected;
		} else if(selectedColor == 3){
			for (int i = 0; i < ToColor.Length; i++) {
				ToColor [i].color = Color3Hover;
			}
			cursor.GetComponent<Renderer> ().material.color = Color3Hover;
			menuRestColor = Color3Hover;
			menuHoverColor = new Color (Color3Hover.r, Color3Hover.g, Color3Hover.b, 1);
			Color1BG.GetComponent<Image> ().color = ColorNotSelected;
			Color2BG.GetComponent<Image> ().color = ColorNotSelected;
			Color3BG.GetComponent<Image> ().color = ColorSelected;
			Color4BG.GetComponent<Image> ().color = ColorNotSelected;
		} else if(selectedColor == 4){
			for (int i = 0; i < ToColor.Length; i++) {
				ToColor [i].color = Color4Hover;
			}
			cursor.GetComponent<Renderer> ().material.color = Color4Hover;
			menuRestColor = Color4Hover;
			menuHoverColor = new Color (Color4Hover.r, Color4Hover.g, Color4Hover.b, 1);
			Color1BG.GetComponent<Image> ().color = ColorNotSelected;
			Color2BG.GetComponent<Image> ().color = ColorNotSelected;
			Color3BG.GetComponent<Image> ().color = ColorNotSelected;
			Color4BG.GetComponent<Image> ().color = ColorSelected;
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
		// Check for double click
		if(mouse.cAct == Mouse.ClickAction.CLICKUP){
			mouse.DoubleClick ();
		}

		// Set the last button hovered over back to normal rest color
		if (LastHover != null) {
			LastHover.GetComponent<Image> ().color = LastHoverColor;
			LastHover = null;
		}

		// Set the button currently being hovered over to the active hover color and check for clicks
		if (mouse.cast) {
			if (mouse.hitInfo.collider.gameObject.tag.Equals ("NewButton")) {
				//Debug.Log ("NewButton Hover");
				mouse.hitInfo.collider.gameObject.GetComponent<Image> ().color = menuHoverColor;
				LastHover = mouse.hitInfo.collider.gameObject;
				LastHoverColor = menuRestColor;
				if (mouse.cAct == Mouse.ClickAction.CLICKUP) {
					//Debug.Log ("NewButton Clicked");
					if(currentColor == 1){
						Player.playerColor = Color1Hover;
					} else if(currentColor == 2){
						Player.playerColor = Color2Hover;
					} else if(currentColor == 3){
						Player.playerColor = Color3Hover;
					} else if(currentColor == 4){
						Player.playerColor = Color4Hover;
					}
					LoadingScreen.show ();
					SceneManager.LoadScene(1);
				}
			} else if(mouse.hitInfo.collider.gameObject.tag.Equals ("LoadButton")){
				//Debug.Log ("LoadButton Hover");
				mouse.hitInfo.collider.gameObject.GetComponent<Image> ().color = menuHoverColor;
				LastHover = mouse.hitInfo.collider.gameObject;
				LastHoverColor = menuRestColor;
				if (mouse.cAct == Mouse.ClickAction.CLICKUP) {
					//Debug.Log ("LoadButton Clicked");
				}
			} else if(mouse.hitInfo.collider.gameObject.tag.Equals ("ExitButton")){
				//Debug.Log ("ExitButton Hover");
				mouse.hitInfo.collider.gameObject.GetComponent<Image> ().color = menuHoverColor;
				LastHover = mouse.hitInfo.collider.gameObject;
				LastHoverColor = menuRestColor;
				if (mouse.cAct == Mouse.ClickAction.CLICKUP) {
					//Debug.Log ("ExitButton Clicked");
					#if UNITY_EDITOR
					EditorApplication.isPlaying = false;
					#else
					Application.Quit();
					#endif
				}
			} else if(mouse.hitInfo.collider.gameObject.name.Equals ("Color1")){
				//Debug.Log ("Color1 Hover");
				mouse.hitInfo.collider.gameObject.GetComponent<Image> ().color = Color1Hover;
				LastHover = mouse.hitInfo.collider.gameObject;
				LastHoverColor = Color1Rest;
				if (mouse.cAct == Mouse.ClickAction.CLICKUP) {
					//Debug.Log ("Color1 Clicked");
					selectedColor = 1;
					Color1BG.GetComponent<Image> ().color = ColorSelected;
					Color2BG.GetComponent<Image> ().color = ColorNotSelected;
					Color3BG.GetComponent<Image> ().color = ColorNotSelected;
					Color4BG.GetComponent<Image> ().color = ColorNotSelected;

					for (int i = 0; i < ToColor.Length; i++) {
						ToColor [i].color = Color1Hover;
					}
					cursor.GetComponent<Renderer> ().material.color = Color1Hover;
					menuRestColor = Color1Hover;
					menuHoverColor = new Color(Color1Hover.r, Color1Hover.g, Color1Hover.b, 1);
				}
			} else if(mouse.hitInfo.collider.gameObject.name.Equals ("Color2")){
				//Debug.Log ("Color2 Hover");
				mouse.hitInfo.collider.gameObject.GetComponent<Image> ().color = Color2Hover;
				LastHover = mouse.hitInfo.collider.gameObject;
				LastHoverColor = Color2Rest;
				if (mouse.cAct == Mouse.ClickAction.CLICKUP) {
					//Debug.Log ("Color2 Clicked");
					selectedColor = 2;
					Color1BG.GetComponent<Image> ().color = ColorNotSelected;
					Color2BG.GetComponent<Image> ().color = ColorSelected;
					Color3BG.GetComponent<Image> ().color = ColorNotSelected;
					Color4BG.GetComponent<Image> ().color = ColorNotSelected;

					for (int i = 0; i < ToColor.Length; i++) {
						ToColor [i].color = Color2Hover;
					}
					cursor.GetComponent<Renderer> ().material.color = Color2Hover;
					menuRestColor = Color2Hover;
					menuHoverColor = new Color(Color2Hover.r, Color2Hover.g, Color2Hover.b, 1);
				}
			} else if(mouse.hitInfo.collider.gameObject.name.Equals ("Color3")){
				//Debug.Log ("Color3 Hover");
				mouse.hitInfo.collider.gameObject.GetComponent<Image> ().color = Color3Hover;
				LastHover = mouse.hitInfo.collider.gameObject;
				LastHoverColor = Color3Rest;
				if (mouse.cAct == Mouse.ClickAction.CLICKUP) {
					//Debug.Log ("Color3 Clicked");
					selectedColor = 3;
					Color1BG.GetComponent<Image> ().color = ColorNotSelected;
					Color2BG.GetComponent<Image> ().color = ColorNotSelected;
					Color3BG.GetComponent<Image> ().color = ColorSelected;
					Color4BG.GetComponent<Image> ().color = ColorNotSelected;

					for (int i = 0; i < ToColor.Length; i++) {
						ToColor [i].color = Color3Hover;
					}
					cursor.GetComponent<Renderer> ().material.color = Color3Hover;
					menuRestColor = Color3Hover;
					menuHoverColor = new Color(Color3Hover.r, Color3Hover.g, Color3Hover.b, 1);
				}
			} else if(mouse.hitInfo.collider.gameObject.name.Equals ("Color4")){
				//Debug.Log ("Color4 Hover");
				mouse.hitInfo.collider.gameObject.GetComponent<Image> ().color = Color4Hover;
				LastHover = mouse.hitInfo.collider.gameObject;
				LastHoverColor = Color4Rest;
				if (mouse.cAct == Mouse.ClickAction.CLICKUP) {
					//Debug.Log ("Color4 Clicked");
					selectedColor = 4;
					Color1BG.GetComponent<Image> ().color = ColorNotSelected;
					Color2BG.GetComponent<Image> ().color = ColorNotSelected;
					Color3BG.GetComponent<Image> ().color = ColorNotSelected;
					Color4BG.GetComponent<Image> ().color = ColorSelected;

					for (int i = 0; i < ToColor.Length; i++) {
						ToColor [i].color = Color4Hover;
					}
					cursor.GetComponent<Renderer> ().material.color = Color4Hover;
					menuRestColor = Color4Hover;
					menuHoverColor = new Color(Color4Hover.r, Color4Hover.g, Color4Hover.b, 1);
				}
			}
		}
	}
}
