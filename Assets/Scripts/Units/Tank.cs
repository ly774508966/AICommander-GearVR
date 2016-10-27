using UnityEngine;
using System.Collections;

public class Tank : Unit {

	public static float massCost = 10000;
	public static float timeCost = 400;

	public static float health = 2000;
	public static float damage = 100;
	public static float attackSpeed = 10;

	private bool taunt = false;
	private bool tauntCD = false;
	private float tauntTime;

	[Header("Tank Fields")]
	public GameObject specialCircle;

	public override void ClickedAppButton(){
		if (!tauntCD) {
			//Debug.Log ("Tank Taunt");
			taunt = true;
			tauntCD = true;
			tauntTime = 200;
			specialCircle.transform.localScale = new Vector3 (1f, 0.01f, 1f);
			specialCircle.SetActive (true);
		}
	}

	void FixedUpdate(){
		if (taunt) {
			tauntTime--;

			if (tauntTime > 190) {
				specialCircle.transform.localScale = new Vector3 (specialCircle.transform.localScale.x + 1f, 0.01f, specialCircle.transform.localScale.z + 1f);
			}
			if (tauntTime < 110) {
				specialCircle.transform.localScale = new Vector3 (specialCircle.transform.localScale.x - 1f, 0.01f, specialCircle.transform.localScale.z - 1f);
			}

			if (tauntTime == 100) {
				specialCircle.SetActive (false);
				//Debug.Log ("Tank Taunt Off");
			}
			if (tauntTime == 0) {
				tauntCD = false;
				//Debug.Log ("Tank Taunt CD Off");
			}
		}
	}
}
