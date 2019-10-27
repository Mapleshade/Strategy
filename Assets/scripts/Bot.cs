using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot : MonoBehaviour {

	int IDBot;

	GovernmentBase gov;

	public void SetInfo(int IDBot, GovernmentBase gov) {
		this.IDBot = IDBot;
		this.gov = gov;
	}

	public void nextStepBot () {
		gov.CheckTowns ();
}
}
