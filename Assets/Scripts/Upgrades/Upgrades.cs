using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Upgrades : MonoBehaviour {

	public Upgrade WH;
	public Upgrade FG;
	public Upgrade PA;
	public Upgrade TV;
	public Material mWH;
	public Material mFG;
	public Material mPA;
	public Material mTV;

	public GameObject[] buttons;
	public int totalUpgrades = 0;

	public GameObject map;
	public GameObject infoPanel;
	public Text title;
	public Text description;

	protected Color colorHover = new Color (0.5f, 0.5f, 0.5f);
	protected GameObject lastHover;
	protected Color lastHoverColor;

	private List<string> upgrades = new List<string> ();

	[Header("Owned By:")]
	public HumanPlayer player;

	void Awake(){
		WH = new WorkerHydraulics();
		WH.mat = mWH;
		FG = new FactoryGearbox();
		FG.mat = mFG;
		PA = new ParticleAccelerator();
		PA.mat = mPA;
		TV = new TargetingVirus();
		TV.mat = mTV;
	}

	void LateUpdate(){
		CheckMenuButtons ();
	}

	/// Checks if menu buttons hovered or clicked
	protected void CheckMenuButtons(){
		if (lastHover != null) {
			lastHover.GetComponent<Image> ().material.color = lastHoverColor;
			lastHover = null;
			map.SetActive (true);
			infoPanel.SetActive (false);
		}
		// If a button is hit by the static raycast in GameController
		if(player.mouse.cast){
			for(int i = 0 ; i < buttons.Length ; i++){
				if (player.mouse.hitInfo.collider.gameObject.Equals (buttons[i])) {
					lastHover = player.mouse.hitInfo.collider.gameObject;
					lastHoverColor = player.mouse.hitInfo.collider.gameObject.GetComponent<Image> ().material.color;
					player.mouse.hitInfo.collider.gameObject.GetComponent<Image> ().material.color = colorHover;
					map.SetActive (false);

					infoPanel.SetActive (true);
					title.text = upgrades [i];
					if (upgrades [i].Equals ("Worker Hydraulics")) {
						description.text = WH.description;
					} else if (upgrades [i].Equals ("Factory Gearbox")) {
						description.text = FG.description;
					} else if (upgrades [i].Equals ("Particle Accelerator")) {
						description.text = PA.description;
					} else if (upgrades [i].Equals ("Targeting Virus")) {
						description.text = TV.description;
					}
				}
			}
		}
	}

	/// Adds the specified upgrade to the upgrade list. If the list already contains the upgrade then an error is printed.
	public void AddUpgrade (string upgrade){
		// If we have the upgrade already do nothing
		if (HasUpgrade (upgrade)) {
			Debug.LogError ("Upgrade '" + upgrade + "' already exists.");
			return;
		}

		// Apply the upgrade
		if(upgrade.Equals("Worker Hydraulics")) {
			WH.completed = true;
			buttons [totalUpgrades].GetComponent<Image> ().material = WH.mat;
		} else if (upgrade.Equals("Factory Gearbox")){
			FG.completed = true;
			buttons [totalUpgrades].GetComponent<Image> ().material = FG.mat;
		} else if (upgrade.Equals("Particle Accelerator")){
			PA.completed = true;
			buttons [totalUpgrades].GetComponent<Image> ().material = PA.mat;
		} else if (upgrade.Equals("Targeting Virus")){
			TV.completed = true;
			buttons [totalUpgrades].GetComponent<Image> ().material = TV.mat;
		}
		buttons [totalUpgrades].SetActive (true);
		upgrades.Add (upgrade);

		totalUpgrades++;
	}

	/// Returns true if the upgrade list contains the specified upgrade
	public bool HasUpgrade (string upgrade){
		if(upgrade.Equals("Worker Hydraulics") && WH.completed) {
			return true;
		} else if (upgrade.Equals("Factory Gearbox") && FG.completed){
			return true;
		} else if (upgrade.Equals("Particle Accelerator") && PA.completed){
			return true;
		} else if (upgrade.Equals("Targeting Virus") && TV.completed){
			return true;
		}
		return false;
	}

}
