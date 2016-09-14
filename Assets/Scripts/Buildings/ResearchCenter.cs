using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ResearchCenter : Building {

	private string currentUpgrade = "";

	// To add to upgrades
	private Upgrades upgrades;


	void Start(){
		upgrades = GameObject.FindGameObjectWithTag ("Upgrades").GetComponent<Upgrades> ();
	}

	protected override void ClickButton(int i){
		// If we're already building something then add this one to the queue
		if (building) {

			//	 Check for full buildQueue
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
		if (upgrades.HasUpgrade ("Research speed")) {
			multiplier = multiplier - 0.5f; // 50% less research time than the original
		}

		// Start building
		if (i == 0) {
			currentUpgrade = "Structure build speed";
			TimeToComplete = 100 * multiplier;
			CurrentlyBuildingText.text = (currentUpgrade);
			TotalTime = TimeToComplete;
		} else if (i == 1) {
			currentUpgrade = "Unit build speed";
			TimeToComplete = 100 * multiplier;
			CurrentlyBuildingText.text = (currentUpgrade);
			TotalTime = TimeToComplete;
		} else if (i == 2) {
			currentUpgrade = "Research speed";
			TimeToComplete = 200 * multiplier;
			CurrentlyBuildingText.text = (currentUpgrade);
			TotalTime = TimeToComplete;
		} else if (i == 3) {

		} else if (i == 4) {

		} else if (i == 5) {

		} else if (i == 6) {

		} else if (i == 7) {

		} else if (i == 8) {

		}
	}

	protected override void ChangeInfoPanel(int i){
		if (i == 0) {
			title.GetComponent<Text>().text = "Structure build\n" + 
				"speed";
			description.GetComponent<Text>().text = "This decreases the amount " +
				"of time it takes to build " +
				"structures by 50%";
			time.SetActive (true);
			mass.SetActive (true);
			time.GetComponent<Text>().text = "Time:100";
			mass.GetComponent<Text>().text = "";
		} else if (i == 1) {
			title.GetComponent<Text>().text = "Unit build speed";
			description.GetComponent<Text>().text = "This decreases the amount " +
				"of time it takes to build " +
				"units by 50%";
			time.SetActive (true);
			mass.SetActive (true);
			time.GetComponent<Text>().text = "Time:100";
			mass.GetComponent<Text>().text = "";
		} else if (i == 2) {
			title.GetComponent<Text>().text = "Research speed";
			description.GetComponent<Text>().text = "This decreases the amount " +
				"of time it takes to complete " +
				"a research by 50%";
			time.SetActive (true);
			mass.SetActive (true);
			time.GetComponent<Text>().text = "Time:200";
			mass.GetComponent<Text>().text = "";
		} else if (i == 3) {
		} else if (i == 4) {
		} else if (i == 5) {
		} else if (i == 6) {
		} else if (i == 7) {
		} else if (i == 8) {
		}
	}


	protected override void ResetInfoPanel(){
		title.GetComponent<Text> ().text = "Research Center";
		description.GetComponent<Text> ().text = "The Research Center is used to research many different upgrades";
		time.SetActive (false);
		mass.SetActive (false);
	}

	protected override void FinishedBuilding(){
		upgrades.AddUpgrade (currentUpgrade);
	}
}
