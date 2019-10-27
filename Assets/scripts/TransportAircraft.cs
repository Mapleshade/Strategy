using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransportAircraft : AirplanBase {

	TransportAircraft() {
		base.NameAir = "TransportAircraft";	
		base.typeOfAirPlane = 0;
		base.damageForPlane = 10;
		base.damageForTown = 10;
		base.ActionPoints = 2;
		base.CurrentAction = 2;
	}

	void Start () {
		base.destination = transform.position;
	}



	public void MakeSteps() {
		base.MakeSteps ();
	}
}

