using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Upgrades : MonoBehaviour {

	private List<string> upgrades;

	void Awake(){
		upgrades = new List<string> ();
	}

	/// Adds the specified upgrade to the upgrade list. If the list already contains the upgrade then an error is printed.
	public void AddUpgrade (string upgrade){
		// If we have the upgrade already do nothing
		if (HasUpgrade (upgrade)) {
			Debug.LogError ("Upgrade '" + upgrade + "' already exists.");
			return;
		}
		// Add the upgrade
		upgrades.Add (upgrade);
	}

	/// Returns true if the upgrade list contains the specified upgrade
	public bool HasUpgrade (string upgrade){
		return upgrades.Contains (upgrade);
	}

}
