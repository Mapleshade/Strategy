using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnitsNasive {

	public static List<GameObject> list = new List<GameObject>();

	public static List<AirplanBase> GetUnits(int govId) {
		List<AirplanBase> list1 = new List<AirplanBase> ();
		foreach (GameObject g in list) {
			if (g.GetComponent<AirplanBase> ().getCountry () == govId)
				list1.Add (g.GetComponent<AirplanBase>());
		}
		return list1;
	}
}
