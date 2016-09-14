using UnityEngine;
using System.Collections;

public class UIRotation : MonoBehaviour {

	private GameObject player;
	private Vector3 direction;
	private Vector3 lookRot;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Head");
	}

	// Update is called once per frame
	void Update () {
		Move ();
	}

	void Move(){
		direction = (player.transform.position - transform.position).normalized;
		lookRot = Quaternion.LookRotation (direction).eulerAngles;

		transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, lookRot.y, transform.rotation.z));
		transform.Rotate (new Vector3 (0, 180, 0));
	}


}
