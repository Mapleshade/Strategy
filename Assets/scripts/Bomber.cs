using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomber : AirplanBase {

	Bomber() {
		base.NameAir = "Bomber";	
		base.typeOfAirPlane = 2;
		base.damageForPlane = 10;
		base.damageForTown = 20;
		base.ActionPoints = 3;
		base.CurrentAction = 3;
	}

	void Start () {
		base.destination = transform.position;
	}

	public void MakeSteps() {
		base.MakeSteps ();
	}
}
