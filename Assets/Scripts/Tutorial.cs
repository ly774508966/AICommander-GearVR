 using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour {

	public GameObject[] toColor;
	public GameObject controlPivot;

	public GameObject continueButton;
	public Text continueText;

	private float lastPlayerRotation;

	// Tutorial fields
	public Text TutText;

	// Tut worker
	public GameObject worker;
	private Vector3 initialWorkerPos;
	private Vector3 initialPlayerPos;

	[HideInInspector]
	public bool highlightedContinueButton = false;

	// Step fields
	[HideInInspector]
	public int currentStep;
	[HideInInspector]
	public int totalSteps;
	[HideInInspector]
	public bool completedCurrentStep = false;
	// Step 1
	[HideInInspector]
	public bool hideShowStep = false;
	private bool movementStep = false;
	private bool unitSelectStep = false;
	private bool unitMoveStep = false;

	// To check for upgrades
	private Upgrades upgrades;

	[Header("Owned By:")]
	public HumanPlayer player;

	// Use this for initialization
	void Start () {
		upgrades = GameObject.FindGameObjectWithTag ("Respawn").GetComponent<Upgrades>();
		currentStep = 0;
		totalSteps = 8;
		// Set all the object to the players color
		for (int i = 0; i < toColor.Length; i++) {
			toColor [i].GetComponent<Image> ().color = player.playerColor;
		}

		lastPlayerRotation = controlPivot.transform.rotation.eulerAngles.y;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		
		CheckStepCompleted ();

		// Rotate with player
		float difference = controlPivot.transform.rotation.eulerAngles.y - transform.rotation.eulerAngles.y;
		if ((difference < 345 && difference > 270) || (difference < -15 && difference > -90)) {
			this.transform.RotateAround (controlPivot.transform.position, Vector3.up, controlPivot.transform.rotation.eulerAngles.y - lastPlayerRotation);
		} else if((difference > 15 && difference < 90) || (difference > -345 && difference < -270)) {
			this.transform.RotateAround (controlPivot.transform.position, Vector3.up, controlPivot.transform.rotation.eulerAngles.y - lastPlayerRotation);
		}

		lastPlayerRotation = controlPivot.transform.rotation.eulerAngles.y;

	}

	public void CheckStepCompleted(){

		// Hile/Show Step
		if (currentStep == 0 && hideShowStep) {
			completedCurrentStep = true;
		}  

		// Movement Step
		if (currentStep == 1 && (player.transform.position != initialPlayerPos || movementStep)) {
			completedCurrentStep = true;
			movementStep = true;
		}  

		// Select Unit Step
		if (currentStep == 2 && (worker.GetComponent<Unit> ().selectedBox.activeSelf || unitSelectStep)) {
			completedCurrentStep = true;
			unitSelectStep = true;
		}  

		// Move Unit Step
		if (currentStep == 3 && (worker.transform.position != initialWorkerPos || unitMoveStep)) {
			completedCurrentStep = true;
			unitMoveStep = true;
		}  

		// Build Structure Step
		if (currentStep == 4) {
			foreach (Building b in player.structures){
				if (b is UnitFactory) {
					completedCurrentStep = true;
				}
			}
		}  

		// Build Unit Step
		if (currentStep == 5) {
			foreach (Unit u in player.units){
				if (u is Scout) {
					completedCurrentStep = true;
				}
			}
		}      

		// Collect Mass Step
		if (currentStep == 6) {
			foreach (Building b in player.structures){
				if (b is MassHarvester) {
					completedCurrentStep = true;
				}
			}
		}   

		// Research Upgrade Step
		if (currentStep == 7 && upgrades.totalUpgrades > 0) {
			completedCurrentStep = true;
		}    
	}

	public void NextStep(){
		if (currentStep == 0) {
			initialPlayerPos = player.transform.position;
			TutText.text = "<color=black>Step 2 - Movement</color>\n" 
				+ "To move use the <color=black>Left Analog Stick</color> on the controller. You can also use the minimap by looking up and clicking anywhere on the map with the <color=black>Right Trigger</color>.";
		} else if (currentStep == 1) {
			TutText.text = "<color=black>Step 3 - Selecting a Unit</color>\n" 
				+ "While looking at the ground near the 'Builder' unit below press and hold the <color=black>Right Trigger</color> on the controller. Then move your head to drag a selection box around the 'Builder'. \n\nYou'll know if a unit is selected when it has a green circle below it.";
		} else if(currentStep == 2){
			initialWorkerPos = worker.transform.position;
			TutText.text = "<color=black>Step 4 - Moving a Unit</color>\n" 
				+ "Now with the 'Builder' unit selected press the \n<color=black>Right Bumper</color> on the group near the unit to move it.";
		} else if(currentStep == 3){
			TutText.text = "<color=black>Step 5 - Building a Unit Factory</color>\n" 
				+ "With the 'Builder' unit still selected press the \n<color=black>Left Trigger</color> to open its menu. Select the 'Unit Factory' from the middle menu panel, press build in the right panel and place the structure anywhere on the ground with <color=black>Right Trigger</color>.";
		} else if(currentStep == 4){
			TutText.text = "<color=black>Step 6 - Building a Unit</color>\n" 
				+ "Click the button on top of the 'Unit Factory' with the <color=black>Right Trigger</color> to open its menu. Select the 'Scout' from the menu with the <color=black>Right Trigger</color> to begin building it. \n<color=black>Tip:</color> With the menu open you can set the destination for built units with the <color=black>Left Trigger</color> while looking anywhere on the ground.";
		} else if(currentStep == 5){
			TutText.text = "<color=black>Step 7 - Collecting Mass</color>\n" 
				+ "Select the builder and use it to build a 'Mass Harvester'. This building allows for the collection of mass. Mass is used for the creation units and buildings. You can see how much mass you have under the minimap in the menu above. Your maximum mass can be increased by upgrading 'Mass Harvesters' or building new ones.";
		} else if(currentStep == 6){
			TutText.text = "<color=black>Step 8 - Researching upgrades</color>\n" 
				+ "Select the builder and use it to build a 'Research Center'. This building allows for the research of upgrades. Complete one of the upgrades. You can see your completed upgrades above the minimap in the menu above. Hover over one for details.";
		}

		currentStep++;
		completedCurrentStep = false;

		if (currentStep == totalSteps) {
			TutText.text = "<color=black>Tutorial finished</color>";
			continueText.text = "Quit";
			completedCurrentStep = true;
		}
	}

	public void PrevStep(){
		if (currentStep == 0) {
			return;
		} if (currentStep == 1) {
			TutText.text = "This dialog box will take you through the steps of the tutorial. You can only continue to the next step once the current step has been completed.\n\n<color=black>Step 1 - Hiding the tutorial</color>\nClick the '<color=black>Hide</color>' button in the bottom left of this box to toggle this box on and off\n\nPress '<color=black>Continue</color>' once you've completed the step.";
		} else if (currentStep == 2) {
			TutText.text = "<color=black>Step 2 - Movement</color>\n" 
				+ "To move use the <color=black>Left Analog Stick</color> on the controller. You can also use the minimap by looking up and clicking anywhere on the map with the <color=black>Right Trigger</color>.";
		} else if (currentStep == 3) {
			TutText.text = "<color=black>Step 3 - Selecting a Unit</color>\n" 
				+ "While looking at the ground near the 'Builder' unit below press and hold the <color=black>Right Trigger</color> on the controller. Then move your head to drag a selection box around the 'Builder'. \n\nYou'll know if a unit is selected when it has a green circle below it";
		} else if (currentStep == 4) {
			TutText.text = "<color=black>Step 4 - Moving a Unit</color>\n" 
				+ "Now with the 'Builder' unit selected press the \n<color=black>Right Bumper</color> on the group near the unit to move it.";
		} else if (currentStep == 5) {
			TutText.text = "<color=black>Step 5 - Building a Unit Factory</color>\n" 
				+ "With the 'Builder' unit still selected press the \n<color=black>Left Trigger</color> to open its menu. Select the 'Unit Factory' from the middle menu panel, press build in the right panel and place the structure anywhere on the ground with <color=black>Right Trigger</color>.";
		} else if (currentStep == 6) {
			TutText.text = "<color=black>Step 6 - Building a Unit</color>\n" 
				+ "Click the button on top of the 'Unit Factory' with the <color=black>Right Trigger</color> to open its menu. Select the 'Scout' from the menu with the <color=black>Right Trigger</color> to begin building it. \n<color=black>Tip</color>: With the menu open you can set the destination for built units with the <color=black>Left Trigger</color> while looking anywhere on the ground.";
		} else if (currentStep == 7) {
			TutText.text = "<color=black>Step 7 - Collecting Mass</color>\n" 
				+ "Select the builder and use it to build a 'Mass Harvester'. This building allows for the collection of mass. Mass is used for the creation units and buildings. You can see how much mass you have under the minimap in the menu above. Your maximum mass can be increased by upgrading 'Mass Harvesters' or building new ones.";
		} else if (currentStep == 8) {
			TutText.text = "<color=black>Step 8 - Researching upgrades</color>\n" 
				+ "Select the builder and use it to build a 'Research Center'. This building allows for the research of upgrades. Complete one of the upgrades. You can see your completed upgrades above the minimap in the menu above. Hover over one for details.";
		} else if (currentStep == totalSteps) {
			continueText.text = "Continue";
		}

		currentStep--;
		completedCurrentStep = false;
	}
}
