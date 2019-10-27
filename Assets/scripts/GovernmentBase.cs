using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class  GovernmentBase {


	int countStationMoney = 2;
	int countStationIron = 2;
	int countStationOil = 2;

	protected string nameGovernment = "Player";
	protected int identityGov = 0;

	protected int money = 100;
	protected int iron = 100;
	protected int oil = 100;

	protected List<AirplanBase> units;
	protected List<Town> towns;
	public List<AirplanBase> newUnits;

	public void SetStationMoney(){
		countStationMoney++;
	}

	public void SetStationIron(){
		countStationIron++;
	}

	public void SetStationOil(){
		countStationOil++;
	}

	public GovernmentBase(int id) {
		identityGov = id;
		units = new List<AirplanBase>();
		towns = new List<Town>();
		newUnits = new List<AirplanBase> ();

	}

	public string GetName() {
		return nameGovernment;
	}

	public GovernmentBase(int id, string nameGov, int money, int iron, int oil) {
		identityGov = id;
		this.nameGovernment = nameGov;
		this.money = money;
		this.iron = iron;
		this.oil = oil;
		units = new List<AirplanBase>();
		towns = new List<Town>();

		newUnits = new List<AirplanBase> ();
	}

	public void SetMoney(int differnce){
		money += differnce;
		MapInfo.current.governments [identityGov, 0] = money;
		Debug.Log (MapInfo.current.governments [identityGov, 0]);
		Debug.Log (identityGov);
	}

	public void SetIron(int differnce){
		iron += differnce;
		MapInfo.current.governments [identityGov, 1] = iron;
	}

	public AirplanBase getUnit(Vector2 v) {
		foreach (AirplanBase u in units) {
			if (u.getHex () == v) {
				return u;
			}
		}
		return null;
	}

	public void SetOil(int differnce){
		oil += differnce;
		MapInfo.current.governments [identityGov, 2] = oil;
	}

	public int GetMoney(){
		return money;
	}

	public int GetIron(){
		return iron;
	}

	public int GetOil(){
		return oil;
	}

	public void SetTown(Town town){
		towns.Add (town);
	}

	public int getTowns() {
		return towns.Count;
	}

	public void DeleteTown(Town town) {
		//foreach(Town t in towns) {
		//	if (t.GetId () == town.GetId ()) {
				towns.Remove (town);
		//		break;
		//	}
		//}
	}

	public void SetUnit(AirplanBase unit){
		units.Add (unit);
	}

	public int getUnits() {
		return units.Count;
	}

	public void DeleteUnit(AirplanBase unit) {
		//foreach(AirplanBase u in units) {
		//	if (u.GetId () == unit.GetId ()) {
				units.Remove (unit);
		//		break;
		//	}
		//}
	}

	public void NextStepGovernment(){
		newUnits = new List<AirplanBase> ();

		money += countStationOil * 10;
		iron += countStationMoney * 10;
		oil += countStationOil * 10;
		//Debug.Log ("looking throw gov " + identityGov);
		MapInfo.current.governments [identityGov, 0] = money;
		MapInfo.current.governments [identityGov, 1] = iron;
		MapInfo.current.governments [identityGov, 2] = oil;

		if (identityGov == 1) {
			foreach (AirplanBase a in units) {
				a.NextStepUnit ();
			}

			//units = newUnits;
			foreach (Town t in towns) {
				t.NextStepTown ();
			}
		} else {
			//Debug.Log ("I know I'm bot");
			//updateUnits ();

			//CheckTowns ();

			//units = newUnits;

			foreach (AirplanBase a in units) {
				//Debug.Log ("Checking unit " + a.getNameAir ());
				a.MakeSteps();
				a.NextStepUnit ();
			}
			units = newUnits;

			//Debug.Log ("Fuh, checked units too " + units.Count + ", but still think that I suck");
			CheckTowns ();
			//Debug.Log ("I think I checked town, but may be I suck");
		}
	}

	//void updateUnits() {
	//	units = UnitsNasive.GetUnits (identityGov);
	//}

	public void CheckTowns(){ 
		foreach (Town t in towns) { 
			if(t.CheckTownProject () == 0){ 
				//дописать алгоритм выбора проекта 
				t.AddProject (ChooseProject(t)); 
			} 
			t.NextStepTown (); 
		} 
	} 

	int ChooseProject(Town town){ 

		//если есть необработанный ресурс 
		if (town.CheckRes () != 0) { 
			//return town.CheckRes (); 

			switch(town.CheckRes ()) { 
			case -1: 
			case -4: 
				return 4; 
				break; 
			case -2: 
			case -5: 
				return 5; 
				break; 
			case -3: 
			case -6: 
				return 6; 
				break; 
			}//иначе другие проекты 
		} else { 
			int air = Random.Range (1, 4);
			if (town.CheckResurses (air, identityGov)) {
				return air;
			}
		} 


		return 7; 
	}

	public Town getTown(int id) {
		foreach(Town t in towns) {
			if(t.GetId() == id) {
				return t;
			}
		}
		return null;
	}
}
