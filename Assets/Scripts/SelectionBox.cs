using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectionBox : MonoBehaviour {

	// Selection
	private bool makingSelection = false;
	private Vector3 firstClickPoint;
	private Vector3 lastClickPoint;
	public List<Unit> selectedUnits = new List<Unit>();

	void Awake(){
		selectedUnits = new List<Unit>();
	}

	public void HandleSelection(Mouse mouse){
		// If we've started clicking on the battlefield (somewhere that isn't a UI object)
		if (mouse.cAct == Mouse.ClickAction.CLICKDOWN && mouse.cast && !(mouse.hitInfo.collider.gameObject.layer == 5)) {
			// Reset any previous selection
			ResetSelection ();
			// Show the selection box
			this.gameObject.SetActive (true);
			makingSelection = true;
			// Set first click point
			firstClickPoint = mouse.hitInfo.point;
			lastClickPoint = mouse.hitInfo.point;
			// Gets rid of any open structure menus
			if(Building.currentMenu != null){
				Building.currentMenu.SetActive (false);
				Building.currentMenu = null;
			}
			// Change box
			transform.localScale = new Vector3(0.01f, transform.localScale.y, 0.01f);
			transform.localPosition = new Vector3(firstClickPoint.x - 0.05f, transform.position.y, firstClickPoint.z - 0.05f);
		} 
		else if(mouse.cAct == Mouse.ClickAction.CLICKING && mouse.cast && makingSelection){
			// Work out distance from origin selection point to current
			lastClickPoint = mouse.hitInfo.point - firstClickPoint;
			// Change box
			transform.localScale = new Vector3(0.1f * lastClickPoint.x, transform.localScale.y, 0.1f * lastClickPoint.z);
			transform.localPosition = new Vector3(firstClickPoint.x + 0.1f * 5 * lastClickPoint.x, transform.position.y, firstClickPoint.z + 0.1f * 5 * lastClickPoint.z);
		} else if(mouse.cAct == Mouse.ClickAction.CLICKUP && makingSelection){
			if (mouse.cast) {
				lastClickPoint = mouse.hitInfo.point;
			}

			// Put units into selected unit list
			selectedUnits = GetSelectedUnits();
			// Hide the selection box
			this.gameObject.SetActive (false);
			makingSelection = false;
		}
	}

	/// Sets all currently selected units to unselected and creates a fresh empty inner list
	public void ResetSelection (){
		foreach(Unit unit in selectedUnits){
			// Do something to all selected units
			unit.SelectedBox (false);
			unit.Deselect ();
		}

		selectedUnits = new List<Unit>();
	}

	/// Returns the inner list
	public List<Unit> GetSelectedUnits (){
		return selectedUnits;
	}

	/// Set unit to selected and add it to the selected list
	void OnTriggerEnter (Collider collider){
		Unit unit = collider.gameObject.GetComponent<Unit>();
		if (unit != null) {
			selectedUnits.Add (unit);
			unit.SelectedBox (true);
		}
	}

	/// Deselect unit and add it to the selected list 
	void OnTriggerExit (Collider collider){
		Unit unit = collider.gameObject.GetComponent<Unit>();
		if (selectedUnits.Contains(unit)) {
			selectedUnits.Remove (unit);
			unit.SelectedBox (false);
		}
	}
}
