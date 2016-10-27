using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour {
	[Header("Unit Fields")]
	/// Objects to color
	public Material toColorMat;

	public GameObject selectedBox;

	protected NavMeshAgent nma;

	[Header("Owned By:")]
	public HumanPlayer player;

	void Awake(){
		nma = this.GetComponent<NavMeshAgent> ();
	}

	void Start(){
		toColorMat.color = player.playerColor;
	}

	void Update(){
		if (selectedBox.activeSelf && player.mouse.cAct == Mouse.ClickAction.TOUCHUP) {
			SetGoal (player.mouse.hitInfo.point);
		}
	}

	public virtual void SetGoal(Vector3 goal){
		nma.SetDestination (goal);
	}

	/// Set active of selected box
	public void SelectedBox (bool setActive){
		selectedBox.SetActive(setActive);
	}

	void OnCollisionEnter (Collision col) {
		//Debug.Log ("HERERSDASFD");
	}

	// ### INDIVIDUAL UNIT ACTIONS ###

	/// Take action on application button clicked while selected
	public virtual void ClickedAppButton(){
		//Debug.Log ("ClickedAppButton: FILL THIS WITH AN ACTION IN THE LOWER CLASSES");
	}

	public virtual void Deselect(){
		//Debug.Log ("Deselect: FILL THIS WITH AN ACTION IN THE LOWER CLASSES");
	}
}
