using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	private GovernmentBase[] goverments;
	private AirplanBase choosenPlane;
	private Town choosenTown;
	private GameMenu gayMenu;
	public Sprite[] sprites;
	public Image choosen;
	public MouseManager mm;
	public GameObject townProject;
	public GameObject planeButtons;
	public GameObject buildTown;

	void Start() {
		goverments = new GovernmentBase[MapInfo.current.playerCount +1];
		gayMenu = gameObject.GetComponent<GameMenu> ();
		for (int i = 0; i < MapInfo.current.playerCount +1; i++) {
			goverments [i] = new GovernmentBase (i);
		}

		mm.setGameController (this);
	}

	public void LoadGoverments() {
		for (int i = 0; i < MapInfo.current.playerCount + 1; i++) {
			goverments [i] = new GovernmentBase (i, MapInfo.current.govNames [i], MapInfo.current.governments [i, 0], MapInfo.current.governments [i, 1], MapInfo.current.governments [i, 2]);
		}
		gayMenu.GetParametrs ();
	}

	public void checkSteps() {
		gayMenu.GetParametrs ();
		if (choosenPlane != null) {
			if (choosenPlane.getCurrentPoint () < 1) {
				foreach (CanvasGroup cg in planeButtons.GetComponentsInChildren<CanvasGroup>()) {
					cg.interactable = false;
				}
			} else {
				foreach (CanvasGroup cg in planeButtons.GetComponentsInChildren<CanvasGroup>()) {
					cg.interactable = true;
				}
			}
		} 
		if (choosenTown != null) {

		}
	}

	public void HealPlane() {
		choosenPlane.Heal ();
	}

	public void BuildTown() {
		choosenPlane.gameObject.GetComponent<TransportAircraft> ().putATown ();
		choosenPlane = null;
		choosen.sprite = sprites [3];
		buildTown.SetActive (false);
		planeButtons.SetActive (false);
	}

	public void setChoosen(AirplanBase plane, Town town) {
		gayMenu.GetParametrs ();
		choosenPlane = plane;
		choosenTown = town;

		if (choosenPlane != null) {
			switch (choosenPlane.getTypeOfAirplane ()) {
			case 0:
			case 3:
			case 6:

				choosen.sprite = sprites [0];
				break;
			case 1:
			case 4:
			case 7:

				choosen.sprite = sprites [1];
				break;
			case 2:
			case 5:
			case 8:

				choosen.sprite = sprites [2];
				break;

			}
			checkSteps ();
			planeButtons.SetActive (true);
			if (choosenPlane.getTypeOfAirplane () == 0)
				buildTown.SetActive (true);
			else
				buildTown.SetActive (false);
			townProject.SetActive (false);
			switch (choosenPlane.getCountry ()) {
			case 1:
				choosen.color = Color.red;
				break;
			case 2:
				choosen.color = Color.blue;
				break;
			case 3:
				choosen.color = Color.green;
				break;
			case 4:
				choosen.color = Color.yellow;
				break;
			case 5:
				choosen.color = Color.magenta;
				break;
			case 6:
				choosen.color = Color.black;
				break;
			case 7:
				choosen.color = Color.red * Color.yellow;
				break;
			case 8:
				choosen.color = Color.green * Color.red;
				break;
			case 9:
				choosen.color = Color.blue * Color.red;
				break;
			default:
				choosen.color = Color.white;
				break;
			}
		} else {
			choosen.sprite = sprites [3];
			buildTown.SetActive (false);
			planeButtons.SetActive (false);
			townProject.SetActive (true);
			switch (choosenTown.GetGovId()) {
			case 1:
				choosen.color = Color.red;
				break;
			case 2:
				choosen.color = Color.blue;
				break;
			case 3:
				choosen.color = Color.green;
				break;
			case 4:
				choosen.color = Color.yellow;
				break;
			case 5:
				choosen.color = Color.magenta;
				break;
			case 6:
				choosen.color = Color.black;
				break;
			case 7:
				choosen.color = Color.red * Color.yellow;
				break;
			case 8:
				choosen.color = Color.green * Color.red;
				break;
			case 9:
				choosen.color = Color.blue * Color.red;
				break;
			default:
				choosen.color = Color.white;
				break;
			}
		}

	}

	public void NextStep(){
		choosenPlane = null;
		choosenTown = null;
		mm.setNull ();
		buildTown.SetActive (false);
		planeButtons.SetActive (false);
		townProject.SetActive (false);
		MapInfo.current.steps++;
		foreach (GovernmentBase g in goverments){
			//Debug.Log ("foreach of gover " + g.GetName());
			g.NextStepGovernment ();
		}
		checkSteps ();
	}

	public GovernmentBase getGoverment(int id) {
		return goverments [id];
	}

	public void setGoverment(int id, GovernmentBase govBase) {
		goverments [id] = govBase;
		MapInfo.current.govNames [id] = govBase.GetName ();
		MapInfo.current.governments [id, 0] = govBase.GetMoney();
		MapInfo.current.governments [id, 1] = govBase.GetIron();
		MapInfo.current.governments [id, 2] = govBase.GetOil();

	}

	public void SetNewProject(int type) {
		choosenTown.AddProject (type);
	}

	public void UpgradePlane() {
		choosenPlane.UpdateAirPlane();
		}
}
