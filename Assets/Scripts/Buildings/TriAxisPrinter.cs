using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TriAxisPrinter : Building {

	// Where units go after being built
	private Vector3 buildFlag;

	// Building prefabs
	public GameObject builderPrefab;
	public GameObject scoutPrefab;
	public GameObject meleePrefab;
	public GameObject rangePrefab;

	private GameObject currentPrefab;

	// To check for upgrades
	private Upgrades upgrades;


	void Start(){
		upgrades = GameObject.FindGameObjectWithTag ("Upgrades").GetComponent<Upgrades>();
		buildFlag = new Vector3 (this.transform.position.x - 10, this.transform.position.y, this.transform.position.z);
	}

	protected override void ClickButton(int i){
		if (building) {

			// Check for full buildQueue
			if (buildQueue.Count > 2) {
				Debug.Log ("Full Queue");
				return;
			}

			// Add to buildQueue
			buildQueue.Add (i);

			// Change icon mats
			if (buildQueue.Count == 1) {
				NextBuildingLeft.GetComponent<Image> ().material = buttonMats [i];
			} else if(buildQueue.Count == 2){
				NextBuildingMid.GetComponent<Image> ().material = buttonMats [i];
			} else if(buildQueue.Count == 3){
				NextBuildingRight.GetComponent<Image> ().material = buttonMats [i];
			}

			return;
		}

		// Set currently building to the material of the button we clicked
		CurrentlyBuildingButton.GetComponent<Image> ().material = buttonMats [i];

		// Multiplier for upgrade time for all units
		float multiplier = 1.0f;
		if (upgrades.HasUpgrade ("Unit build speed")) {
			multiplier = multiplier - 0.5f; // 50% less build time than the original
		}

		if (i == 0){
			currentPrefab = builderPrefab;
			CurrentlyBuildingText.text = ("Builder");
			TimeToComplete = currentPrefab.GetComponent<Unit> ().creationTime * multiplier;
			TotalTime = TimeToComplete;
		} else if (i == 1){
			currentPrefab = scoutPrefab; 
			CurrentlyBuildingText.text = ("Scout");
			TimeToComplete = currentPrefab.GetComponent<Unit> ().creationTime * multiplier;
			TotalTime = TimeToComplete;
		} else if (i == 2){
			currentPrefab = meleePrefab; 
			CurrentlyBuildingText.text = ("Melee");
			TimeToComplete = currentPrefab.GetComponent<Unit> ().creationTime * multiplier;
			TotalTime = TimeToComplete;
		} else if (i == 3){
			currentPrefab = rangePrefab; 
			CurrentlyBuildingText.text = ("Range");
			TimeToComplete = currentPrefab.GetComponent<Unit> ().creationTime * multiplier;
			TotalTime = TimeToComplete;
		} else if (i == 4){
		} else if (i == 5){
		} else if (i == 6){
		} else if (i == 7){
		} else if (i == 8){
		}
	}

	protected override void ChangeInfoPanel(int i){
		if (i == 0) {
			title.GetComponent<Text>().text = "Builder";
			description.GetComponent<Text>().text = "Use this unit to build " +
			"structures to expand your " +
			"reach into the physical " +
			"world";
			time.SetActive (true);
			mass.SetActive (true);
			time.GetComponent<Text>().text = "Time:200";
			mass.GetComponent<Text>().text = "Mass:1000";
		} else if (i == 1) {
			title.GetComponent<Text>().text = "Scout";
			description.GetComponent<Text>().text = "Use this unit to reveal " +
				"the surrounding lands " +
				"searching for treasures " +
				"or your enemies";
			time.SetActive (true);
			mass.SetActive (true);
			time.GetComponent<Text>().text = "Time:120";
			mass.GetComponent<Text>().text = "Mass:500";
		} else if (i == 2) {
			title.GetComponent<Text>().text = "Melee";
			description.GetComponent<Text>().text = "Use this unit to build " +
				"structures to expand your " +
				"reach into the physical " +
				"world";
			time.SetActive (true);
			mass.SetActive (true);
			time.GetComponent<Text>().text = "Time:200";
			mass.GetComponent<Text>().text = "Mass:1500";
		} else if (i == 3) {
			title.GetComponent<Text>().text = "Range";
			description.GetComponent<Text>().text = "Use this unit to build " +
				"structures to expand your " +
				"reach into the physical " +
				"world";
			time.SetActive (true);
			mass.SetActive (true);
			time.GetComponent<Text>().text = "Time:400";
			mass.GetComponent<Text>().text = "Mass:2000";
		} else if (i == 4) {
		} else if (i == 5) {
		} else if (i == 6) {
		} else if (i == 7) {
		} else if (i == 8) {
		}
	}


	protected override void ResetInfoPanel(){
		title.GetComponent<Text> ().text = "Tri-Axis Printer";
		description.GetComponent<Text> ().text = "The Research Center is used to research many different upgrades";
		time.SetActive (false);
		mass.SetActive (false);
	}

	protected override void FinishedBuilding(){
		SpawnUnit ((GameObject)Instantiate (currentPrefab, this.transform.position, this.transform.rotation));
	}

	/// Spawns the unit provided and sets their goal to the build flag
	protected void SpawnUnit(GameObject unit){
		// Builder
		Builder builder = unit.GetComponent<Builder> ();
		if (builder != null) {
			builder.SetGoal (buildFlag);
			return;
		}

		// Scout
		Scout scout = unit.GetComponent<Scout> ();
		if (scout != null) {
			scout.SetGoal (buildFlag);
			return;
		}

		// Melee
		Melee melee = unit.GetComponent<Melee> ();
		if (melee != null) {
			melee.SetGoal (buildFlag);
			return;
		}

		// Range
		Range range = unit.GetComponent<Range> ();
		if (range != null) {
			range.SetGoal (buildFlag);
			return;
		}
	}

}
