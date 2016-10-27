using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UnitFactory : Building {

	// Building prefabs
	[Header("UnitFactory Fields")]
	public GameObject builderPrefab;
	public GameObject scoutPrefab;
	public GameObject tankPrefab;

	// Where units go after being built
	public GameObject flag;
	public GameObject spawnSpot;

	// Units to show in right panel
	public GameObject builder;
	public GameObject scout;
	public GameObject tank;
	public Material builderMat;
	public Material scoutMat;
	public Material tankMat;

	// Unit currently building
	private GameObject currentPrefab;

	public static float timeCost = 200;
	public static float massCost = 5000;
	public GameObject health;
	public GameObject damage;
	public GameObject attackSpeed;

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

		// UF Start init
		builderMat.color = player.playerColor;
		scoutMat.color = player.playerColor;
		tankMat.color = player.playerColor;
	}

	void Update(){
		if (menu.activeSelf) {
			flag.SetActive (true);
		} else {
			flag.SetActive (false);
		}
	}

	protected override bool StartBuilding(string toBuild, bool isFromBuildQueue){
		if (!isFromBuildQueue && !CanAfford (toBuild)) {
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
				if (toBuild.Equals("Builder")) {
					player.currentMass = player.currentMass - Builder.massCost;
				} else if (toBuild.Equals("Scout")) {
					player.currentMass = player.currentMass - Scout.massCost;
				} else if (toBuild.Equals("Tank")) {
					player.currentMass = player.currentMass - Tank.massCost;
				} 
			}

			// Add to buildQueue
			buildQueue.Add (toBuild);

			// Change icon mats
			if (buildQueue.Count == 1) {
				if (toBuild.Equals("Builder")) {
					NextBuildingLeft.GetComponent<Image> ().material = new Material(buildMenuMats [0]);
				} else if (toBuild.Equals("Scout")) {
					NextBuildingLeft.GetComponent<Image> ().material = new Material(buildMenuMats [1]);
				} else if (toBuild.Equals("Tank")) {
					NextBuildingLeft.GetComponent<Image> ().material = new Material(buildMenuMats [2]);
				} 
			} else if(buildQueue.Count == 2){
				if (toBuild.Equals("Builder")) {
					NextBuildingMid.GetComponent<Image> ().material = new Material(buildMenuMats [0]);
				} else if (toBuild.Equals("Scout")) {
					NextBuildingMid.GetComponent<Image> ().material = new Material(buildMenuMats [1]);
				} else if (toBuild.Equals("Tank")) {
					NextBuildingMid.GetComponent<Image> ().material = new Material(buildMenuMats [2]);
				} 
			} else if(buildQueue.Count == 3){
				if (toBuild.Equals("Builder")) {
					NextBuildingRight.GetComponent<Image> ().material = new Material(buildMenuMats [0]);
				} else if (toBuild.Equals("Scout")) {
					NextBuildingRight.GetComponent<Image> ().material = new Material(buildMenuMats [1]);
				} else if (toBuild.Equals("Tank")) {
					NextBuildingRight.GetComponent<Image> ().material = new Material(buildMenuMats [2]);
				} 
			}

			return true;
		}

		// Multiplier for upgrade time for all units
		float multiplier = 1.0f;
		/*if (upgrades.HasUpgrade ("Factory Gearbox")) {
			multiplier = multiplier - 0.5f; // 50% less build time than the original
		}*/

		if (toBuild.Equals("Builder")){
			CurrentlyBuildingButton.GetComponent<Image> ().material = new Material(buildMenuMats [0]);
			currentPrefab = builderPrefab;
			TimeToComplete = Builder.timeCost * multiplier;
			TotalTime = TimeToComplete;
			if(!isFromBuildQueue)
				player.currentMass = player.currentMass - Builder.massCost;
		} else if (toBuild.Equals("Scout")){
			CurrentlyBuildingButton.GetComponent<Image> ().material = new Material(buildMenuMats [1]);
			currentPrefab = scoutPrefab; 
			TimeToComplete = Scout.timeCost * multiplier;
			TotalTime = TimeToComplete;
			if(!isFromBuildQueue)
				player.currentMass = player.currentMass - Scout.massCost;
		} else if (toBuild.Equals("Tank")){
			CurrentlyBuildingButton.GetComponent<Image> ().material = new Material(buildMenuMats [2]);
			currentPrefab = tankPrefab; 
			TimeToComplete = Tank.timeCost * multiplier;
			TotalTime = TimeToComplete;
			if(!isFromBuildQueue)
				player.currentMass = player.currentMass - Tank.massCost;
		}

		return true;
	}

	protected override void ChangeInfoPanel(int i){
		// Multiplier for upgrade time for all units
		float multiplier = 1.0f;
		/*if (upgrades.HasUpgrade ("Factory Gearbox")) {
			multiplier = multiplier - 0.5f; // 50% less build time than the original
		}*/
		builder.SetActive (false);
		scout.SetActive (false);
		tank.SetActive (false);

		if (i == 0) {
			selectedArrow.GetComponent<RectTransform> ().anchoredPosition = new Vector2(selectedArrow.GetComponent<RectTransform> ().anchoredPosition.x, 6.4f);
			title.GetComponent<Text>().text = "Builder";
			description.GetComponent<Text>().text = "This unit cannot attack.\n<color=white>Special ability</color>: Can build structures";
			time.GetComponent<Text>().text = "Time: " + (Builder.timeCost * multiplier);
			mass.GetComponent<Text>().text = "Mass: " + Builder.massCost;
			health.GetComponent<Text>().text = "<color=orange>" + Builder.health + "</color>";
			damage.GetComponent<Text>().text = "<color=orange>-</color>";
			attackSpeed.GetComponent<Text>().text = "<color=orange>-</color>";
			builder.SetActive (true);
		} else if (i == 1) {
			selectedArrow.GetComponent<RectTransform> ().anchoredPosition = new Vector2(selectedArrow.GetComponent<RectTransform> ().anchoredPosition.x, 4.8f);
			title.GetComponent<Text>().text = "Scout";
			description.GetComponent<Text>().text = "Weak, fast unit.\n<color=white>Special ability</color>: 30% movement speed for 5 seconds";
			time.GetComponent<Text>().text = "Time: " + (Scout.timeCost * multiplier);
			mass.GetComponent<Text>().text = "Mass: " + Scout.massCost;
			health.GetComponent<Text>().text = "<color=orange>" + Scout.health + "</color>";
			damage.GetComponent<Text>().text = "<color=orange>" + Scout.damage + "</color>";
			attackSpeed.GetComponent<Text>().text = "<color=orange>" + Scout.attackSpeed + "</color>";
			scout.SetActive (true);
		} else if (i == 2) {
			selectedArrow.GetComponent<RectTransform> ().anchoredPosition = new Vector2(selectedArrow.GetComponent<RectTransform> ().anchoredPosition.x, 3.2f);
			title.GetComponent<Text>().text = "Tank";
			description.GetComponent<Text>().text = "Strong, slow unit.\n<color=white>Special ability</color>: Draws all surrounding enemy fire for 1 second";
			time.GetComponent<Text>().text = "Time: " + (Tank.timeCost * multiplier);
			mass.GetComponent<Text>().text = "Mass: " + Tank.massCost;
			health.GetComponent<Text>().text = "<color=orange>" + Tank.health + "</color>";
			damage.GetComponent<Text>().text = "<color=orange>" + Tank.damage + "</color>";
			attackSpeed.GetComponent<Text>().text = "<color=orange>" + Tank.attackSpeed + "</color>";
			tank.SetActive (true);
		}
	}

	protected override bool CanAfford(string toBuild){
		if (toBuild.Equals("Builder")){
			if (player.currentMass >= Builder.massCost) {
				return true;
			}
		} else if (toBuild.Equals("Scout")){
			if (player.currentMass >= Scout.massCost) {
				return true;
			}
		} else if (toBuild.Equals("Tank")){
			if (player.currentMass >= Tank.massCost) {
				return true;
			}
		}
		return false;
	}

	protected override void FinishedBuilding(){
		GameObject unit = (GameObject)Instantiate (currentPrefab, spawnSpot.transform.position, this.transform.rotation);
		unit.GetComponent<Unit>().player = player;
		unit.GetComponent<Unit> ().SetGoal (flag.transform.position);
		player.units.Add (unit.GetComponent<Unit>());
	}

	public override void ClickedAppButton(){
		flag.transform.position = player.mouse.hitInfo.point;
	}

}
