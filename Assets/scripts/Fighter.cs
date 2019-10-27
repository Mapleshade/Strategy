using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : AirplanBase {

	Fighter () {
		base.NameAir = "Fighter";	
		base.typeOfAirPlane = 1;
		base.damageForPlane = 25;
		base.damageForTown = 10;
		base.ActionPoints = 5;
		base.CurrentAction = 5;
	}

	void Start () {
		base.destination = transform.position;
	}


	public void MakeSteps() {
		base.MakeSteps ();
	}

}
