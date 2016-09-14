using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour {

	/// Objects to color
	public GameObject[] toColor;
	/// Objects to color
	public GameObject selectedBox;

	// Stats
	public float creationCost = 2000;
	public float creationTime = 100;

	protected NavMeshAgent nma;

	void Awake(){
		nma = this.GetComponent<NavMeshAgent> ();
		SetColor (Player.playerColor);
	}

	/// Set all the object to the players color
	private void SetColor(Color color){
		for (int i = 0; i < toColor.Length; i++) {
			toColor [i].GetComponent<Renderer> ().material.color = color;
		}
	}

	public void SetGoal(Vector3 goal){
		nma.SetDestination (goal);
	}

	public void SelectedBox (bool setActive){
		// Set active of selected box
		selectedBox.SetActive(setActive);
		//selectedBox.GetComponent<Renderer> ().material.color = new Color ();
	}

	/// Take action on application button clicked while selected
	public virtual void ClickedAppButton(){
		Debug.Log ("ClickedAppButton: FILL THIS WITH AN ACTION IN THE LOWER CLASSES");
	}

	public virtual void Deselect(){
		//Debug.Log ("Deselect: FILL THIS WITH AN ACTION IN THE LOWER CLASSES");
	}

	void OnMouseOver(){
		SetColor (new Color (200f/255f, 0f/255f, 0f/255f));
	}

	void OnMouseExit(){
		SetColor (Player.playerColor);
	}
}
