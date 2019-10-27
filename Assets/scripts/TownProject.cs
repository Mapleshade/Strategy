using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownProject  {

	string nameP = "";
	int numberOfProject;
	int numberOfSteps;
	int remainingSteps;

	public TownProject(string nameP, int numberOfProject, int numberOfSteps){
		
		this.nameP = nameP;
		this.numberOfProject = numberOfProject;
		this.numberOfSteps = numberOfSteps;
		this.remainingSteps = numberOfSteps;
	}

	public int GetNumberOfProject() {
		return numberOfProject;
	}

	public int NextStepTownProject(){
		remainingSteps--;
		return remainingSteps;
	}
}
