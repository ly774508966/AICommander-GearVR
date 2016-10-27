using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MassHarvester : Building {

	public static float timeCost = 100;
	public static float massCost = 2000;

	private string currentUpgrade = "";

	private HashSet<string> personalUpgrades = new HashSet<string>();
	[Header("MassHarvester Fields")]
	public GameObject MPS;
	public GameObject alreadyUpgraded;

	private float carbyneDrillTime = 100;
	private float carbyneDrillMass = 2000;
	private float aFiltrationTime = 100;
	private float aFiltrationMass = 2000;
	private float storageTime = 100;
	private float storageMass = 2000;

	private float lastTime = 0;

	void Start(){
		upgrades = GameObject.FindGameObjectWithTag ("Respawn").GetComponent<Upgrades>();

		// Menu init
		ChangeInfoPanel(0);
		buildMenuButtons [0].GetComponent<Image> ().color = buildMenuSelect;
		currentlySelected = buildMenuButtons [0];

		// Set all the images in the menu to the owners color
		for (int i = 0; i < toColorImage.Length; i++) {
			toColorImage [i].GetComponent<Image> ().color = player.playerColor;
		}

		// Set the material to the owners color
		toColorMat.color = player.playerColor;
		buildQueueContainer.SetActive (false);

		player.maxMass = player.maxMass + 10000;
	}

	void Update(){
		
		// If already owned
		if (currentlySelected == buildMenuButtons [0] && personalUpgrades.Contains ("Carbyne Drill")) {
			alreadyUpgraded.SetActive (true);
		} else if (currentlySelected == buildMenuButtons [1] && personalUpgrades.Contains ("Advanced Filtration")) {
			alreadyUpgraded.SetActive (true);
		} else if (currentlySelected == buildMenuButtons [2] && personalUpgrades.Contains ("Storage Capsules")) {
			alreadyUpgraded.SetActive (true);
		} else {
			alreadyUpgraded.SetActive (false);
		}


		// Mass per second
		float mps = 100;

		if (personalUpgrades.Contains ("Carbyne Drill")) {
			mps = mps * 2f;
		}
		if (personalUpgrades.Contains ("Advanced Filtration")) {
			mps = mps * 2f;
		}

		MPS.SetActive (true);
		MPS.GetComponent<Text>().text = "Mass Per Second: <color=white>" + mps + "</color>";

		mass.SetActive (false);
		time.SetActive (false);

		if (Time.time > lastTime + 1) {
			player.currentMass += mps;
			if(player.currentMass > player.maxMass){
				player.currentMass = player.maxMass;
			}
			lastTime = Time.time;
		}
	}

	protected override bool StartBuilding(string toBuild, bool isFromBuildQueue){
		if ((!isFromBuildQueue && !CanAfford (toBuild)) || (!isFromBuildQueue && HasUpgrade (toBuild))) {
			return false;
		}

		// If we're already building add this to queue
		if (building) {
			// Check for full buildQueue
			if (buildQueue.Count > 2) {
				Debug.Log ("Full Queue");
				return false;
			}

			// If this isn't from the queue take the mass off
			if (!isFromBuildQueue) {
				if (toBuild.Equals("Carbyne Drill")) {
					player.currentMass = player.currentMass - carbyneDrillMass;
				} else if (toBuild.Equals("Advanced Filtration")) {
					player.currentMass = player.currentMass - aFiltrationMass;
				} else if (toBuild.Equals("Storage Capsules")) {
					player.currentMass = player.currentMass - storageMass;
				} 
			}

			// Add to buildQueue
			buildQueue.Add (toBuild);

			// Change icon mats
			if (buildQueue.Count == 1) {
				if (toBuild.Equals("Carbyne Drill")) {
					NextBuildingLeft.GetComponent<Image> ().material = new Material(buildMenuMats [0]);
				} else if (toBuild.Equals("Advanced Filtration")) {
					NextBuildingLeft.GetComponent<Image> ().material = new Material(buildMenuMats [1]);
				} else if (toBuild.Equals("Storage Capsules")) {
					NextBuildingLeft.GetComponent<Image> ().material = new Material(buildMenuMats [2]);
				} 
			} else if(buildQueue.Count == 2){
				if (toBuild.Equals("Carbyne Drill")) {
					NextBuildingMid.GetComponent<Image> ().material = new Material(buildMenuMats [0]);
				} else if (toBuild.Equals("Advanced Filtration")) {
					NextBuildingMid.GetComponent<Image> ().material = new Material(buildMenuMats [1]);
				} else if (toBuild.Equals("Storage Capsules")) {
					NextBuildingMid.GetComponent<Image> ().material = new Material(buildMenuMats [2]);
				} 
			} else if(buildQueue.Count == 3){
				if (toBuild.Equals("Carbyne Drill")) {
					NextBuildingRight.GetComponent<Image> ().material = new Material(buildMenuMats [0]);
				} else if (toBuild.Equals("Advanced Filtration")) {
					NextBuildingRight.GetComponent<Image> ().material = new Material(buildMenuMats [1]);
				} else if (toBuild.Equals("Storage Capsules")) {
					NextBuildingRight.GetComponent<Image> ().material = new Material(buildMenuMats [2]);
				} 
			}

			return true;
		}

		if (toBuild.Equals("Carbyne Drill")){
			CurrentlyBuildingButton.GetComponent<Image> ().material = new Material(buildMenuMats [0]);
			currentUpgrade = toBuild;
			TimeToComplete = carbyneDrillTime;
			TotalTime = TimeToComplete;
			if(!isFromBuildQueue)
				player.currentMass = player.currentMass - carbyneDrillMass;
		} else if (toBuild.Equals("Advanced Filtration")){
			CurrentlyBuildingButton.GetComponent<Image> ().material = new Material(buildMenuMats [1]);
			currentUpgrade = toBuild; 
			TimeToComplete = aFiltrationTime;
			TotalTime = TimeToComplete;
			if(!isFromBuildQueue)
				player.currentMass = player.currentMass - aFiltrationMass;
		} else if (toBuild.Equals("Storage Capsules")){
			CurrentlyBuildingButton.GetComponent<Image> ().material = new Material(buildMenuMats [2]);
			currentUpgrade = toBuild; 
			TimeToComplete = storageTime;
			TotalTime = TimeToComplete;
			if (!isFromBuildQueue)
				player.currentMass = player.currentMass - storageMass;
		}

		return true;
	}

	protected override void ChangeInfoPanel(int i){

		if (i == 0) {
			selectedArrow.GetComponent<RectTransform> ().anchoredPosition = new Vector2(selectedArrow.GetComponent<RectTransform> ().anchoredPosition.x, 6.4f);
			title.GetComponent<Text> ().text = "Carbyne Drill";
			description.GetComponent<Text>().text = "stronger drill material allows faster operation.\n<color=orange>200% mass per second</color>";
			time.GetComponent<Text>().text = "Time: " + carbyneDrillTime;
			mass.GetComponent<Text>().text = "Mass: " + carbyneDrillMass;
		} else if (i == 1) {
			selectedArrow.GetComponent<RectTransform> ().anchoredPosition = new Vector2(selectedArrow.GetComponent<RectTransform> ().anchoredPosition.x, 4.8f);
			title.GetComponent<Text> ().text = "Advanced Filtration";
			description.GetComponent<Text>().text = "identifies high\npriority atoms more accurately.\n<color=orange>200% mass per second</color>";
			time.GetComponent<Text>().text = "Time: " + aFiltrationTime;
			mass.GetComponent<Text>().text = "Mass: " + aFiltrationMass;
		} else if (i == 2) {
			selectedArrow.GetComponent<RectTransform> ().anchoredPosition = new Vector2(selectedArrow.GetComponent<RectTransform> ().anchoredPosition.x, 3.2f);
			title.GetComponent<Text> ().text = "Storage Capsules";
			description.GetComponent<Text>().text = "adds storage space to hold more mass.\n\n<color=orange>+10000 to max mass</color>";
			time.GetComponent<Text>().text = "Time: " + storageTime;
			mass.GetComponent<Text>().text = "Mass: " + storageMass;
		}
	}

	protected override bool CanAfford(string toBuild){
		if (toBuild.Equals("Carbyne Drill")){
			if (player.currentMass >= carbyneDrillMass) {
				return true;
			}
		} else if (toBuild.Equals("Advanced Filtration")){
			if (player.currentMass >= aFiltrationMass) {
				return true;
			}
		} else if (toBuild.Equals("Storage Capsules")){
			if (player.currentMass >= storageMass) {
				return true;
			}
		}
		return false;
	}

	/// Act on finished build from building
	protected override void FinishedBuilding(){
		if (currentUpgrade.Equals ("Storage Capsules") && !personalUpgrades.Contains (currentUpgrade)) {
			player.maxMass = player.maxMass + 10000;
		}
		personalUpgrades.Add (currentUpgrade);
	}

	protected bool HasUpgrade(string upgrade){
		if (personalUpgrades.Contains (upgrade) || currentUpgrade.Equals(upgrade) || (buildQueue.Count == 1 && buildQueue[0].Equals(upgrade)) || (buildQueue.Count == 2 && buildQueue[1].Equals(upgrade)) || (buildQueue.Count == 3 && buildQueue[2].Equals(upgrade))) {
			return true;
		}
		return false;
	}

}
