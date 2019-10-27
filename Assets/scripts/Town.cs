using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Town : MonoBehaviour {

	string nameT = "Town";
	int identityGov;
	int identityTown;
	Vector2 coordinate;
	List<Resourse> resourses;
	int radius = 3;
	int damage = 30;

	int heal = 100;

	Queue<TownProject> projects;
	Grid grid;

	public Image image;

	Town() {
		projects = new Queue<TownProject> ();
	}

	void Start() {
		switch (identityGov) {
		case 1:
			image.color = Color.red;
			break;
		case 2:
			image.color = Color.blue;
			break;
		case 3:
			image.color = Color.green;
			break;
		case 4:
			image.color = Color.yellow;
			break;
		case 5:
			image.color = Color.magenta;
			break;
		case 6:
			image.color = Color.black;
			break;
		case 7:
			image.color = Color.red * Color.yellow;
			break;
		case 8:
			image.color = Color.green * Color.red;
			break;
		case 9:
			image.color = Color.blue * Color.red;
			break;
		default:
			image.color = Color.white;
			break;
		}
		gameObject.GetComponentInChildren<Canvas> ().worldCamera = Camera.main;
	}

	public int GetId() {
		return identityTown;
	}

	public void setTown(string nameT, int identityGov, int identityTown, int x, int y){
		this.nameT = nameT;
		this.identityGov = identityGov;
		this.identityTown = identityTown;
		coordinate = new Vector2 (x, y);
		resourses = new List<Resourse> ();
		switch (identityGov) {
		case 1:
			image.color = Color.red;
			break;
		case 2:
			image.color = Color.blue;
			break;
		case 3:
			image.color = Color.green;
			break;
		case 4:
			image.color = Color.yellow;
			break;
		case 5:
			image.color = Color.magenta;
			break;
		case 6:
			image.color = Color.black;
			break;
		case 7:
			image.color = Color.red * Color.yellow;
			break;
		case 8:
			image.color = Color.green * Color.red;
			break;
		case 9:
			image.color = Color.blue * Color.red;
			break;
		default:
			image.color = Color.white;
			break;
		}
	}

	public void SetGrid(Grid grid) {
		this.grid = grid;
	}

	//какой из проектов: 1 - транспортник,2 - бомбардировщик, 3 -истребитель, 4 - разработать железо, 5 - разработать золото, 6 - разработать нефть, 0 - бесконечный проект
	public void AddProject(int project){

		if (CheckResurses(project,identityGov)) {
			
			TownProject pr;

			switch (project) {
			case 1:
				pr = new TownProject ("Транспортный самолёт", project, 5);
				projects.Enqueue (pr);
				break;
			case 2: 
				pr = new TownProject ("Бомбардировщик", project, 5);
				projects.Enqueue (pr);
				break;
			case 3: 
				pr = new TownProject ("Истребитель", project, 5);
				projects.Enqueue (pr);
				break;
			case 4:
				pr = new TownProject ("Разработать железо", project, 5);
				projects.Enqueue (pr);
				break;
			case 5:
				pr = new TownProject ("Разработать золото", project, 5);
				projects.Enqueue (pr);
				break;
			case 6:
				pr = new TownProject ("Разработать нефть", project, 5);
				projects.Enqueue (pr);
				break;
			default:
				pr = new TownProject ("Увеличить производство золота", project, 20);
				projects.Enqueue (pr);
				break;
			}

		}


	}

	public void AddResource(Resourse res) {
		switch (res.GetTypeRes()) {
		case -7:
			grid.gameController.getGoverment (res.GetTypeRes ()).SetStationIron ();
			break;
		case -8:
			grid.gameController.getGoverment (res.GetTypeRes ()).SetStationMoney();
			break;
		case -9:
			grid.gameController.getGoverment (res.GetTypeRes ()).SetStationOil ();
			break;
		}
		grid.gameController.getGoverment (identityGov).SetStationIron ();
		resourses.Add (res);
	}

	public void SetHeal(int heal) {
		this.heal += heal;
		image.fillAmount = this.heal / 100f;
	}

	public void CheckHeal(int govId) {
		if (heal <= 0) {
			grid.GetGameController().getGoverment(govId).DeleteTown (this);
			grid.GetGameController().getGoverment(govId).SetTown (this);
			for(int x = 0; x<MapInfo.current.gridWidth; x++){
				for(int y=0; y<MapInfo.current.gridHeigth; y++) {
					if (MapInfo.current.owners [x, y] == identityGov * 100 + identityTown) {
						MapInfo.current.owners [x, y] = govId * 100 + identityTown;
					}
				}
			}
			identityGov = govId;
			heal = 100;
			switch (identityGov) {
			case 1:
				image.color = Color.red;
				break;
			case 2:
				image.color = Color.blue;
				break;
			case 3:
				image.color = Color.green;
				break;
			case 4:
				image.color = Color.yellow;
				break;
			case 5:
				image.color = Color.magenta;
				break;
			case 6:
				image.color = Color.black;
				break;
			case 7:
				image.color = Color.red * Color.yellow;
				break;
			case 8:
				image.color = Color.green * Color.red;
				break;
			case 9:
				image.color = Color.blue * Color.red;
				break;
			default:
				image.color = Color.white;
				break;
			}
		}
		image.fillAmount = this.heal / 100f;
	}


	public void SetResourses(List<Resourse> list) {
		resourses = list;
	}

	public int GetGovId() {
		return identityGov;
	}

	void CancelProjects(){
		projects.Clear();
	}

	public void Attack(AirplanBase victim){
		Vector2 vCor = victim.getHex ();
		if (CouldAttackHere ((int)coordinate.x, (int)coordinate.y, (int)vCor.x, (int)vCor.y, radius)) {
			victim.setHeal (-30);
		}
	}

	bool CouldAttackHere(int curX, int curY, int toX, int toY, int radius) {

		//Debug.Log("Hell here" + stepsLeft);

		bool res = false;

		if (toX == curX && toY == curY) {
			return true;
		}
		if(radius > 0)
			foreach (Vector2 v in getNeighbours(curX, curY)) {
				if ((int)v.x == toX && (int)v.y == toY) {
					return true;
				}
			}


		if (radius > 0) {
			foreach (Vector2 v in getNeighbours(curX, curY)) {
				//Debug.Log ((int)v.x + " , " + (int)v.y);
				if (CouldAttackHere ((int)v.x, (int)v.y, toX, toY, radius - 1)) {
					return true;
				}
			}
		} else {
			return false;
		}

		//Debug.Log (res);
		return res;
	}

	List<Vector2> getNeighbours(int x, int y) {

		List<Vector2> list = new List<Vector2> ();

		if (y + 1 < MapInfo.current.gridHeigth && MapInfo.current.prefMap [x, y + 1] < MapInfo.current.hillHeigth)
			list.Add (new Vector2 (x, y + 1));

		if (x + 1 < MapInfo.current.gridWidth && MapInfo.current.prefMap [x + 1, y] < MapInfo.current.hillHeigth)
			list.Add (new Vector2 (x + 1, y));

		if (y - 1 >= 0 && MapInfo.current.prefMap [x, y - 1] < MapInfo.current.hillHeigth)
			list.Add (new Vector2 (x, y - 1));


		if (x - 1 >= 0 && MapInfo.current.prefMap [x - 1, y] < MapInfo.current.hillHeigth)
			list.Add (new Vector2 (x - 1, y));


		if (y % 2 == 0) {

			if (y + 1 < MapInfo.current.gridHeigth && x - 1 >= 0 && MapInfo.current.prefMap [x - 1, y + 1] < MapInfo.current.hillHeigth)
				list.Add (new Vector2 (x - 1, y + 1));

			if (y - 1 >= 0 && x - 1 >= 0 && MapInfo.current.prefMap [x - 1, y - 1] < MapInfo.current.hillHeigth)
				list.Add (new Vector2 (x - 1, y - 1));

		} else {

			if (y + 1 < MapInfo.current.gridHeigth && x + 1  < MapInfo.current.gridWidth && MapInfo.current.prefMap [x + 1, y + 1] < MapInfo.current.hillHeigth)
				list.Add (new Vector2 (x + 1, y + 1));

			if (y - 1 >= 0 && x + 1 < MapInfo.current.gridWidth && MapInfo.current.prefMap [x + 1, y - 1] < MapInfo.current.hillHeigth)
				list.Add (new Vector2 (x + 1, y - 1));
		}

		return list;
	}

	public bool HasFreeIron() {
		foreach (Resourse res in resourses) {
			if (!res.GetActive () && (res.GetTypeRes () != -1 || res.GetTypeRes () != -4)) {
				return true;
			}
		}
		return false;
	}

	public bool HasFreeMoney() {
		foreach (Resourse res in resourses) {
			if (!res.GetActive () && (res.GetTypeRes () != -2 || res.GetTypeRes () != -5)) {
				return true;
			}
		}
		return false;
	}

	public bool HasFreeOil() {
		foreach (Resourse res in resourses) {
			if (!res.GetActive () && (res.GetTypeRes () != -3 || res.GetTypeRes () != -6)) {
				return true;
			}
		}
		return false;
	}

	public Vector2 GetHex() {
		return coordinate;
	}

	public bool CheckResurses(int project, int identityGov){

		int money = grid.GetGameController().getGoverment(identityGov).GetMoney();
		int iron = grid.GetGameController().getGoverment(identityGov).GetIron();
		int oil = grid.GetGameController().getGoverment(identityGov).GetOil();


		switch (project) {
		case 1:
			
			//pr = new TownProject ("Транспортный самолёт", project, 5);
			if (money >= 50 && iron >= 2 && oil >= 1) {
				grid.GetGameController().getGoverment(identityGov).SetMoney(-50);
				grid.GetGameController().getGoverment(identityGov).SetIron(-2);
				grid.GetGameController().getGoverment(identityGov).SetOil(-1);
				return true;
			} else {
				return false;
			}
			break;

		case 2: 
			
			//pr = new TownProject ("Бомбардировщик", project, 5);
			if (money >= 100 && iron >= 4 && oil >= 2) {
				grid.GetGameController().getGoverment(identityGov).SetMoney(-100);
				grid.GetGameController().getGoverment(identityGov).SetIron(-4);
				grid.GetGameController().getGoverment(identityGov).SetOil(-2);
				return true;
			} else {
				return false;
			}
			break;

		case 3: 
			
			//pr = new TownProject ("Истребитель", project, 5);
			if (money >= 150 && iron >= 3 && oil >= 3) {
				grid.GetGameController().getGoverment(identityGov).SetMoney(-150);
				grid.GetGameController().getGoverment(identityGov).SetIron(-3);
				grid.GetGameController().getGoverment(identityGov).SetOil(-3);
				return true;
			} else {
				return false;
			}
			break;

		case 4:
			
			//pr = new TownProject ("Разработать железо", project, 5);
			if (money >= 50) {
				grid.GetGameController().getGoverment(identityGov).SetMoney(-50);
				return true;
			} else {
				return false;
			}
			break;

		case 5:
			
			//pr = new TownProject ("Разработать золото", project, 5);
			if (money >= 25) {
				grid.GetGameController().getGoverment(identityGov).SetMoney(-25);
				return true;
			} else {
				return false;
			}
			break;

		case 6:
			
			//pr = new TownProject ("Разработать нефть", project, 5);
			if (money >= 50) {
				grid.GetGameController().getGoverment(identityGov).SetMoney(-50);
				return true;
			} else {
				return false;
			}
			break;

		default:
			
			//pr = new TownProject ("Увеличить производство золота", project, 20);
			return true;
			break;

		}


			
		return false;
	}

	public void NextStepTown(){
		TownProject pr = null;
		if(projects.Count != 0)
		pr = projects.Peek ();
		HasVictim (getNeighbours((int)coordinate.x, (int)coordinate.y));
		if (pr != null) {
			int step = pr.NextStepTownProject ();
			if (step == 0) {

				Transform transPref;
				Transform tr;
				Vector2 gridPos;
				AirplanBase data;

				switch (projects.Dequeue ().GetNumberOfProject()) {
				case 1:
					transPref = grid.GetUnitPrefFinder ().GetAirPref (0);
					tr = Instantiate (transPref) as Transform;
					gridPos = coordinate;
					//Debug.Log (CalcWorldPos (gridPos));
					tr.position = grid.CalcWorldPos (gridPos);
					data = tr.GetComponent<TransportAircraft> ();
					data.setGrid (grid);
					data.setCountry (identityGov);
					data.setCoordinate (gridPos);
					grid.GetGameController ().getGoverment (identityGov).SetUnit (data);
					//grid.GetGameController ().getGoverment (identityGov).newUnits.Add (data);
					tr.name = tr.GetComponent<TransportAircraft>().getNameAir() + identityGov; 
					MapInfo.current.unitMap [(int)coordinate.x, (int)coordinate.y] = identityGov * 100 + data.getTypeOfAirplane ();
					break;
				case 2: 
					transPref = grid.GetUnitPrefFinder ().GetAirPref (2);
					tr = Instantiate (transPref) as Transform;
					gridPos = coordinate;
					//Debug.Log (CalcWorldPos (gridPos));
					tr.position = grid.CalcWorldPos (gridPos);
					data = tr.GetComponent<Bomber>();
					data.setGrid (grid);
					data.setCountry (identityGov);
					data.setCoordinate(gridPos);
					grid.GetGameController().getGoverment(identityGov).SetUnit (data);
					//grid.GetGameController ().getGoverment (identityGov).newUnits.Add (data);
					tr.name = tr.GetComponent<Bomber>().getNameAir() + identityGov; 
					MapInfo.current.unitMap [(int)coordinate.x, (int)coordinate.y] = identityGov * 100 + data.getTypeOfAirplane ();
					break;
				case 3: 
					transPref = grid.GetUnitPrefFinder ().GetAirPref (1);
					tr = Instantiate (transPref) as Transform;
					gridPos = coordinate;
					//Debug.Log (CalcWorldPos (gridPos));
					tr.position = grid.CalcWorldPos (gridPos);
					data = tr.GetComponent<Fighter>();
					data.setGrid (grid);
					data.setCountry (identityGov);
					data.setCoordinate(gridPos);
					grid.GetGameController().getGoverment(identityGov).SetUnit (data);
					//grid.GetGameController ().getGoverment (identityGov).newUnits.Add (data);
					tr.name = tr.GetComponent<Fighter>().getNameAir() + identityGov; 
					MapInfo.current.unitMap [(int)coordinate.x, (int)coordinate.y] = identityGov * 100 + data.getTypeOfAirplane ();
					break;
				case 4:
					//pr = new TownProject ("Разработать железо", project, 5);
					//projects.Enqueue (pr);
					foreach (Resourse res in resourses) {
						if (!res.GetActive () && (res.GetTypeRes () != -1 || res.GetTypeRes () != -4)) {
							grid.gameController.getGoverment (identityGov).SetStationIron ();
							res.Activate ();
							break;
						}
					}
					break;
				case 5:
					//pr = new TownProject ("Разработать золото", project, 5);
					//projects.Enqueue (pr);
					foreach (Resourse res in resourses) {
						if (!res.GetActive () && (res.GetTypeRes () != -2 || res.GetTypeRes () != -5)) {
							grid.gameController.getGoverment (identityGov).SetStationMoney ();
							res.Activate ();
							break;
						}
					}
					break;
				case 6:
					//pr = new TownProject ("Разработать нефть", project, 5);
					//projects.Enqueue (pr);
					foreach (Resourse res in resourses) {
						if (!res.GetActive () && (res.GetTypeRes () != -3 || res.GetTypeRes () != -6)) {
							grid.gameController.getGoverment (identityGov).SetStationOil ();
							res.Activate ();
							break;
						}
					}
					break;
				default:
					//pr = new TownProject ("Увеличить производство золота", project, 20);
					//projects.Enqueue (pr);
					break;
				}
					
			}
				
		}
	}

	public int CheckTownProject () {
		return projects.Count;
	}

	public int CheckRes() { 
		foreach (Resourse res in resourses) { 
			if (!res.GetActive ()) 
				return res.GetTypeRes(); 
		} 
		return 0; 
	}

	public Resourse GetFreeRes(){
		foreach (Resourse res in resourses) {
			if (!res.GetActive ())
				return res;
		}
		return null;
	}

	private bool HasVictim(List<Vector2> whereToGo) {
		foreach (Vector2 v in whereToGo) {
			if (MapInfo.current.unitMap [(int)v.x, (int)v.y] != 0 && MapInfo.current.unitMap [(int)v.x, (int)v.y] / 100 != identityGov) {
				grid.GetGameController ().getGoverment (MapInfo.current.unitMap [(int)v.x, (int)v.y] / 100).getUnit (v).setHeal (-damage);
				return true;
			}
		}
		return false;
	}
}
