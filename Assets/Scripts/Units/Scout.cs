using UnityEngine;
using System.Collections;

public class Scout : Unit {
	
	public static float massCost = 2000;
	public static float timeCost = 100;

	public static float health = 200;
	public static float damage = 50;
	public static float attackSpeed = 30;

	private bool speedy = false;
	private bool speedyCD = false;
	private float speedTime;

	public override void ClickedAppButton(){
		if (!speedyCD) {
			//Debug.Log ("Scout Speed");
			nma.speed = 10;
			speedy = true;
			speedyCD = true;
			speedTime = 100;
		}
	}

	void FixedUpdate(){
		if (speedy) {
			speedTime--;
			if (speedTime == 0) {
				nma.speed = 5;
				//Debug.Log ("Scout Speed Off");
			}
			if (speedTime == -100) {
				speedyCD = false;
				//Debug.Log ("Scout Speed CD Off");
			}
		}
	}
}
