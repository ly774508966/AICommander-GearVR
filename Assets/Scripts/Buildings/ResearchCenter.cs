using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ResearchCenter : Building {

	private string currentUpgrade = "";

	public static float timeCost = 300;
	public static float massCost = 1600;

	public GameObject alreadyUpgraded;

	void Update(){

		// If already owned
		if (currentlySelected == buildMenuButtons [0] && upgrades.HasUpgrade ("Worker Hydraulics")) {
			alreadyUpgraded.SetActive (true);
		} else if (currentlySelected == buildMenuButtons [1] && upgrades.HasUpgrade ("Factory Gearbox")) {
			alreadyUpgraded.SetActive (true);
		} else if (currentlySelected == buildMenuButtons [2] && upgrades.HasUpgrade ("Particle Accelerator")) {
			alreadyUpgraded.SetActive (true);
		} else if (currentlySelected == buildMenuButtons [3] && upgrades.HasUpgrade ("Targeting Virus")) {
			alreadyUpgraded.SetActive (true);
		} else {
			alreadyUpgraded.SetActive (false);
		}

	}

	protected override bool StartBuilding(string toBuild, bool isFromBuildQueue){
		if (!isFromBuildQueue && HasUpgrade (toBuild)) {
			return false;
		}

		// If we're already building add this to queue
		if (building) {
			// Check for full buildQueue
			if (buildQueue.Count > 2) {
				Debug.Log ("Full Queue");
				return false;
			}

			// Add to buildQueue
			buildQueue.Add (toBuild);

			// Change icon mats
			if (buildQueue.Count == 1) {
				if (toBuild.Equals("Worker Hydraulics")) {
					NextBuildingLeft.GetComponent<Image> ().material = new Material(buildMenuMats [0]);
				} else if (toBuild.Equals("Factory Gearbox")) {
					NextBuildingLeft.GetComponent<Image> ().material = new Material(buildMenuMats [1]);
				} else if (toBuild.Equals("Particle Accelerator")) {
					NextBuildingLeft.GetComponent<Image> ().material = new Material(buildMenuMats [2]);
				} else if (toBuild.Equals("Targeting Virus")) {
					NextBuildingLeft.GetComponent<Image> ().material = new Material(buildMenuMats [3]);
				} 
			} else if(buildQueue.Count == 2){
				if (toBuild.Equals("Worker Hydraulics")) {
					NextBuildingMid.GetComponent<Image> ().material = new Material(buildMenuMats [0]);
				} else if (toBuild.Equals("Factory Gearbox")) {
					NextBuildingMid.GetComponent<Image> ().material = new Material(buildMenuMats [1]);
				} else if (toBuild.Equals("Particle Accelerator")) {
					NextBuildingMid.GetComponent<Image> ().material = new Material(buildMenuMats [2]);
				} else if (toBuild.Equals("Targeting Virus")) {
					NextBuildingLeft.GetComponent<Image> ().material = new Material(buildMenuMats [3]);
				} 
			} else if(buildQueue.Count == 3){
				if (toBuild.Equals("Worker Hydraulics")) {
					NextBuildingRight.GetComponent<Image> ().material = new Material(buildMenuMats [0]);
				} else if (toBuild.Equals("Factory Gearbox")) {
					NextBuildingRight.GetComponent<Image> ().material = new Material(buildMenuMats [1]);
				} else if (toBuild.Equals("Particle Accelerator")) {
					NextBuildingRight.GetComponent<Image> ().material = new Material(buildMenuMats [2]);
				} else if (toBuild.Equals("Targeting Virus")) {
					NextBuildingLeft.GetComponent<Image> ().material = new Material(buildMenuMats [3]);
				} 
			}

			return true;
		}

		// Multiplier for upgrade time for all units
		float multiplier = 1.0f;
		/*if (upgrades.HasUpgrade ("Factory Gearbox")) {
			multiplier = multiplier - 0.5f; // 50% less build time than the original
		}*/

		if (toBuild.Equals("Worker Hydraulics")){
			CurrentlyBuildingButton.GetComponent<Image> ().material = new Material(buildMenuMats [0]);
			currentUpgrade = toBuild;
			TimeToComplete = Builder.timeCost * multiplier;
			TotalTime = TimeToComplete;
		} else if (toBuild.Equals("Factory Gearbox")){
			CurrentlyBuildingButton.GetComponent<Image> ().material = new Material(buildMenuMats [1]);
			currentUpgrade = toBuild;
			TimeToComplete = Scout.timeCost * multiplier;
			TotalTime = TimeToComplete;
		} else if (toBuild.Equals("Particle Accelerator")){
			CurrentlyBuildingButton.GetComponent<Image> ().material = new Material(buildMenuMats [2]);
			currentUpgrade = toBuild;
			TimeToComplete = Tank.timeCost * multiplier;
			TotalTime = TimeToComplete;
		} else if (toBuild.Equals("Targeting Virus")){
			CurrentlyBuildingButton.GetComponent<Image> ().material = new Material(buildMenuMats [3]);
			currentUpgrade = toBuild;
			TimeToComplete = Tank.timeCost * multiplier;
			TotalTime = TimeToComplete;
		}

		return true;
	}

	protected override void ChangeInfoPanel(int i){
		// Multiplier for upgrade time for all units
		float multiplier = 1.0f;
		if (upgrades.HasUpgrade ("Particle Accelerator")) {
			multiplier = multiplier - 0.5f; // 50% less research time than the original
		}

		if (i == 0) {
			selectedArrow.GetComponent<RectTransform> ().anchoredPosition = new Vector2(selectedArrow.GetComponent<RectTransform> ().anchoredPosition.x, 6.4f);
			title.GetComponent<Text> ().text = "Worker Hydraulics";
			description.GetComponent<Text>().text = upgrades.WH.description;
			time.GetComponent<Text>().text = "Time: " + (upgrades.WH.timeCost * multiplier);
			mass.GetComponent<Text>().text = "";
		} else if (i == 1) {
			selectedArrow.GetComponent<RectTransform> ().anchoredPosition = new Vector2(selectedArrow.GetComponent<RectTransform> ().anchoredPosition.x, 4.8f);
			title.GetComponent<Text>().text = "Factory Gearbox";
			description.GetComponent<Text>().text = upgrades.FG.description;
			time.GetComponent<Text>().text = "Time: " + (upgrades.FG.timeCost * multiplier);
			mass.GetComponent<Text>().text = "";
		} else if (i == 2) {
			selectedArrow.GetComponent<RectTransform> ().anchoredPosition = new Vector2(selectedArrow.GetComponent<RectTransform> ().anchoredPosition.x, 3.2f);
			title.GetComponent<Text>().text = "Particle Accelerator";
			description.GetComponent<Text>().text = upgrades.PA.description;
			time.GetComponent<Text>().text = "Time: " + (upgrades.PA.timeCost * multiplier);
			mass.GetComponent<Text>().text = "";
		} else if (i == 3) {
			selectedArrow.GetComponent<RectTransform> ().anchoredPosition = new Vector2(selectedArrow.GetComponent<RectTransform> ().anchoredPosition.x, 1.6f);
			title.GetComponent<Text>().text = "Targeting Virus";
			description.GetComponent<Text>().text = upgrades.TV.description;
			time.GetComponent<Text>().text = "Time: " + (upgrades.TV.timeCost * multiplier);
			mass.GetComponent<Text>().text = "";
		}
	}

	protected override bool CanAfford(string toBuild){
		return true;
	}

	protected override void FinishedBuilding(){
		upgrades.AddUpgrade (currentUpgrade);
	}

	protected bool HasUpgrade(string upgrade){
		if (upgrades.HasUpgrade (upgrade) || currentUpgrade.Equals(upgrade) || (buildQueue.Count == 1 && buildQueue[0].Equals(upgrade)) || (buildQueue.Count == 2 && buildQueue[1].Equals(upgrade)) || (buildQueue.Count == 3 && buildQueue[2].Equals(upgrade))) {
			return true;
		}
		return false;
	}
}
