using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class MouseManager : MonoBehaviour {

	AirplanBase selectedUnit;
	Town selectedTown;
	GameController gameController;


	// Update is called once per frame
	void Update () {



		if(EventSystem.current.IsPointerOverGameObject()) {

			return;
		}

		Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );

		RaycastHit hitInfo;

		if( Physics.Raycast(ray, out hitInfo) ) {
			GameObject ourHitObject = hitInfo.collider.transform.gameObject;

			if (ourHitObject.GetComponent<Town> () != null ) {
				// Ah! We are over a hex!
				MouseOver_Town (ourHitObject);
			} else if (ourHitObject.GetComponentInParent<AirplanBase> () != null) {
				// We are over a unit!
				MouseOver_Unit (ourHitObject);

			} else if (ourHitObject.GetComponent<DataHex> () != null) {
				MouseOver_Hex (ourHitObject);
			}


		}


	}

	public void setGameController(GameController gameController) {
		this.gameController = gameController;
	}

	void MouseOver_Town(GameObject ourHitObject) {
		if (Input.GetMouseButton (0) && ourHitObject.GetComponent<Town> ().GetGovId () == 1) {
			selectedTown = ourHitObject.GetComponent<Town> ();
			selectedUnit = null;
			gameController.setChoosen (selectedUnit, selectedTown);
		}

		if (Input.GetMouseButton (0) && ourHitObject.GetComponent<Town> ().GetGovId () != 1 && selectedUnit!=null) {
			selectedUnit.AttackTown (ourHitObject.GetComponent<Town> ());
		}
			
	}

	void MouseOver_Hex(GameObject ourHitObject) {

		if(Input.GetMouseButtonDown(0)) {
			//MeshRenderer mr = ourHitObject.GetComponentInChildren<MeshRenderer>();
			DataHex data = ourHitObject.transform.GetComponent<DataHex>();
			if(selectedUnit != null) {
				selectedUnit.goTo (ourHitObject.transform.position, data.getX(), data.getY());
				gameController.checkSteps ();
			}
				
		}

	}

	void MouseOver_Unit(GameObject ourHitObject) {
		//Debug.Log("Raycast hit: " + ourHitObject.name );

		if(Input.GetMouseButtonDown(0) && ourHitObject.GetComponentInParent<AirplanBase>().getCountry() == 1) {
			// We have click on the unit
			selectedUnit = ourHitObject.GetComponentInParent<AirplanBase>();
			selectedTown = null;

			gameController.setChoosen (selectedUnit, selectedTown);
		}

		if(Input.GetMouseButtonDown(0) && ourHitObject.GetComponentInParent<AirplanBase>().getCountry() != 1 && selectedTown != null) {
			// We have click on the unit
			selectedTown.Attack(ourHitObject.GetComponentInParent<AirplanBase>());
		}

		if(Input.GetMouseButtonDown(0) && ourHitObject.GetComponentInParent<AirplanBase>().getCountry() != 1 && selectedUnit != null) {
			// We have click on the unit
			selectedUnit.Attack(ourHitObject.GetComponentInParent<AirplanBase>());
		}

	}

	public void setNull() {
		selectedTown = null;
		selectedTown = null;
	}
}
