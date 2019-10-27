using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AirplanBase : MonoBehaviour {

	//название самолета
	protected string NameAir;
	//тип самолета (0 - грузовой, 1 - истребитель, 2 - бомбардировщик)
	protected int typeOfAirPlane; 
	//принадлежность к определенному правителю()
	protected int country;
	//уровень здоровья
	protected int heal = 100;
	//урон
	protected int damageForPlane;
	//урон
	protected int damageForTown;
	//радиус атаки
	protected int radAttack = 3;
	//количество очков действия
	protected int ActionPoints;
	protected int CurrentAction; 
	//скорость перемещения(одинаковая для всех)
	protected float speed = 5;
	//пункт назначения
	protected Vector3 destination;
	protected Vector2 coordinate;
	protected Stack<Vector2> way;

	protected Grid grid;
	//данные о повороте
	//нужно ли развернуть
	bool rotate = false;
	//конечная точка поворота
	Quaternion targ;
	//угол, на который разворачиваем
	Quaternion angle = Quaternion.Euler (0,60,0);
	//счётчик для определения текущего поворота
	private int count = 0;
	bool isCorutine;

	public Image image;

	void Start () {
		destination = transform.position;
		switch (country) {
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

	void Update () {
		//поворот самолета
		if (rotate) {
			CheckAngle();
			if (transform.rotation.y != targ.y) {
				transform.rotation = Quaternion.RotateTowards (transform.rotation, targ, Time.deltaTime * 50);
			}else {
				rotate = false;
			}
		}

			//Vector3 dir = destination - transform.position;

			//Vector3 velocity = dir.normalized * speed * Time.deltaTime;
			//velocity = Vector3.ClampMagnitude (velocity, dir.magnitude);
		//transform.Translate (dir * speed * Time.deltaTime);

		if (transform.position != destination) {
			transform.position = Vector3.Lerp(transform.position, destination,0.3f);
		}

		

	}

	public void goTo(Vector3 destination, int x, int y) {
		if (way == null || way.Count == 0) {
			//Debug.Log ("from " + (int)coordinate.x + "," + (int)coordinate.y + " to " + x + ", " + y);
			way = new Stack<Vector2> ();
			if (CouldGoThere ((int)coordinate.x, (int)coordinate.y, x, y, CurrentAction, true)) {
				StartCoroutine (Go ());
				MapInfo.current.unitMap [(int)coordinate.x, (int)coordinate.y] = 0;
				//coordinate = new Vector2 (x, y);
				MapInfo.current.unitMap [x, y] = country * 100 + getTypeOfAirplane ();
			}
		}
	}

	protected IEnumerator Go() {
		while (way.Count != 0) {
			isCorutine = true;
			Vector2 current = way.Pop();
			//Debug.Log ("going to " + current.ToString ());
			CheckCount (current);
			if (rotate)
				yield return new WaitUntil(() => !rotate);
			this.destination = grid.CalcWorldPos(current);
			coordinate = current;
			yield return new WaitForSeconds(0.5f);
			transform.position = destination;
		}
		if (way.Count == 0) {
			isCorutine = false;
			//Debug.Log ("SHIT!");
		}
	}

	bool CouldGoThere(int curX, int curY, int toX, int toY, int stepsLeft, bool rememberWay) {

		//Debug.Log("Hell here" + stepsLeft);

		bool res = false;

		if (toX == curX && toY == curY) {
			if (rememberWay) {
				way.Push (new Vector2 (curX, curY));
				CurrentAction = stepsLeft;
			}
			return true;
		}
		if(stepsLeft>0)
			foreach (Vector2 v in getNeighbour(curX, curY, rememberWay)) {
			if ((int)v.x == toX && (int)v.y == toY) {
					if (rememberWay) {
						way.Push (new Vector2 (toX, toY));
						way.Push (new Vector2 (curX, curY));
						CurrentAction = stepsLeft - 1;
					}
				return true;
			}
		}
	

		if (stepsLeft > 0) {
			foreach (Vector2 v in getNeighbour(curX, curY,rememberWay)) {
				//Debug.Log ((int)v.x + " , " + (int)v.y);
				if (rememberWay && CouldGoThere ((int)v.x, (int)v.y, toX, toY, stepsLeft - 1, true)) {
					way.Push (new Vector2 (curX, curY));
					return true;
				}
				if (!rememberWay && CouldGoThere ((int)v.x, (int)v.y, toX, toY, stepsLeft - 1, false)) {
					return true;
				}
			}
		} else {
			return false;
		}
			
		//Debug.Log (res);
		return res;
	}

	protected List<Vector2> getNeighbour(int x, int y, bool isUnit) {

		List<Vector2> list = new List<Vector2> ();

		if (y + 1 < MapInfo.current.gridHeigth && MapInfo.current.prefMap [x, y + 1] < MapInfo.current.hillHeigth &&(!isUnit || MapInfo.current.unitMap[x, y + 1] ==0))
			list.Add (new Vector2 (x, y + 1));

		if (x + 1 < MapInfo.current.gridWidth && MapInfo.current.prefMap [x + 1, y] < MapInfo.current.hillHeigth&&(!isUnit || MapInfo.current.unitMap[x + 1, y] ==0))
			list.Add (new Vector2 (x + 1, y));

		if (y - 1 >= 0 && MapInfo.current.prefMap [x, y - 1] < MapInfo.current.hillHeigth&& (!isUnit || MapInfo.current.unitMap[x, y - 1] ==0))
			list.Add (new Vector2 (x, y - 1));


		if (x - 1 >= 0 && MapInfo.current.prefMap [x - 1, y] < MapInfo.current.hillHeigth&& (!isUnit || MapInfo.current.unitMap[x - 1, y] ==0))
			list.Add (new Vector2 (x - 1, y));


		if (y % 2 == 0) {

			if (y + 1 < MapInfo.current.gridHeigth && x - 1 >= 0 && MapInfo.current.prefMap [x - 1, y + 1] < MapInfo.current.hillHeigth && (!isUnit || MapInfo.current.unitMap[x - 1, y + 1] ==0))
				list.Add (new Vector2 (x - 1, y + 1));

			if (y - 1 >= 0 && x - 1 >= 0 && MapInfo.current.prefMap [x - 1, y - 1] < MapInfo.current.hillHeigth && (!isUnit || MapInfo.current.unitMap[x - 1, y - 1] ==0))
				list.Add (new Vector2 (x - 1, y - 1));

		} else {

			if (y + 1 < MapInfo.current.gridHeigth && x + 1  < MapInfo.current.gridWidth && MapInfo.current.prefMap [x + 1, y + 1] < MapInfo.current.hillHeigth && (!isUnit || MapInfo.current.unitMap[x + 1, y + 1] ==0))
				list.Add (new Vector2 (x + 1, y + 1));

			if (y - 1 >= 0 && x + 1 < MapInfo.current.gridWidth && MapInfo.current.prefMap [x + 1, y - 1] < MapInfo.current.hillHeigth && (!isUnit || MapInfo.current.unitMap[x + 1, y - 1] ==0))
				list.Add (new Vector2 (x + 1, y - 1));
		}

		return list;
	}

	public Vector3 getDesination() {
		return destination;
	}

	public Vector2 getHex() {
		return coordinate;
	}

	public void setNameAir(string NameAir) {
		this.NameAir = NameAir;
	}

	public string getNameAir() {
		return NameAir;
	}

	public int getTypeOfAirplane() {
		return typeOfAirPlane;
	}

	public void setCountry(int country) {
		this.country = country;
		switch (country) {
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

	public int getCountry() {
		return country;
	}

	public void setHeal(int heal) {
		this.heal += heal;
		image.fillAmount = this.heal / 100f;
		CheckHeal ();
	}


	public void CheckHeal() {
		if (heal <= 0) {
			//grid.GetGameController().getGoverment (country).DeleteUnit (this);
			grid.GetGameController().getGoverment (country).newUnits.Remove(this);
			MapInfo.current.unitMap [(int)coordinate.x, (int)coordinate.y] = 0;
			Destroy (gameObject);
		}
	}

	public int getHeal() {
		return heal;
	}

	public int getPlaneDamage() {
		return damageForPlane;
	}

	public int getCurrentPoint() {
		return CurrentAction;
	}

	public int getTowmDamage() {
		return damageForTown;
	}

	public int getRadAttack() {
		return radAttack;
	}

	public int getActionPoints() {
		return ActionPoints;
	}

	public void setCoordinate(Vector2 whereIam) {
		coordinate = whereIam;
	}

	public void setGrid(Grid grid) {
		this.grid = grid;
	}

	public void NextStepUnit() {
		CurrentAction = ActionPoints;
	}

	public void Heal() {
		CurrentAction = 0;
		if (heal <= 80) {
			heal += 20;
		} else if (heal < 100) {
			heal = 100;
		}
		image.fillAmount = this.heal / 100f;
	}

	public void Attack(AirplanBase victim) {
		Vector2 toGo = victim.getHex();
		if (CurrentAction > 0) {
			if (CouldGoThere ((int)coordinate.x, (int)coordinate.y, (int)toGo.x, (int)toGo.y, radAttack, false)) {
				victim.setHeal (-damageForPlane);
				CurrentAction = 0;
				//Debug.Log (victim.getHeal ());
			}
		}
	}


	public void AttackTown(Town victim) {
		Vector2 toGo = victim.GetHex();
		if (CurrentAction > 0) {
		if (CouldGoThere ((int)coordinate.x, (int)coordinate.y, (int)toGo.x, (int)toGo.y, radAttack, false)) {
			victim.SetHeal (-damageForTown);
			victim.CheckHeal (country);
				CurrentAction = 0;
				//Debug.Log (victim.getHeal ());
			}
		}
	}


	//проверка текущего поворота самолета
	private void CheckCount(Vector2 toGo){

		int curX = (int)coordinate.x;
		int curY = (int)coordinate.y;
		int toX = (int)toGo.x;
		int toY = (int)toGo.y;

		if (curY % 2 == 0) {

				if (toX== curX-1 && toY==curY-1 && count != 4) {
							count = 4;
							rotate = true;
						}
				if (toX== curX-1 && toY==curY && count != 3) {
							count = 3;
							rotate = true;
						}
				if (toX== curX-1 && toY==curY+1 && count != 2) {
							count = 2;
							rotate = true;
						}
				if (toX== curX && toY==curY-1 && count != 5) {
							count = 5;
							rotate = true;
						}
				if (toX== curX && toY==curY+1 && count != 1) {
							count = 1;
							rotate = true;
						}
				if (toX== curX+1 && count != 0) {
						count = 0;
						rotate = true;
					}
			
		} else {
		

			if (toX== curX-1 && count != 3) {
				count = 3;
				rotate = true;
			}
			if (toX== curX && toY==curY-1 && count != 4) {
				count = 4;
				rotate = true;
			}
			if (toX== curX && toY==curY+1 && count != 2) {
				count = 2;
				rotate = true;
			}
			if (toX== curX+1 && toY==curY-1 && count != 5) {
				count = 5;
				rotate = true;
			}
			if (toX== curX+1 && toY==curY && count != 0) {
				count = 0;
				rotate = true;
			}
			if (toX== curX+1 && toY==curY+1 && count != 1) {
				count = 1;
				rotate = true;
			}
		
		}

		
	}

	//определение угла, на который нужно развернуть самолет
	void CheckAngle(){
		switch (count) {
		case 0:
			targ = Quaternion.Euler (0, 0, 0);
			break;
		case 1:
			targ = Quaternion.Euler(0, 60, 0);
			break;
		case 2:
			targ = Quaternion.Euler(0, 120, 0);
			break;
		case 3:
			targ = Quaternion.Euler(0, 180, 0);
			break;
		case 4:
			targ = Quaternion.Euler(0, -120, 0);
			break;
		case 5:
			targ = Quaternion.Euler (0, -60, 0);
			break;
		}
	}
	public void UpdateAirPlane() { 

		Transform transPref; 
		Transform tr; 
		Vector2 gridPos; 
		AirplanBase data; 

		switch (typeOfAirPlane) { 

		case 0: 
		case 1: 
		case 2: 
			if(grid.GetGameController().getGoverment(country).GetMoney() >= 100) { 
				grid.GetGameController().getGoverment(country).SetMoney(-100); 

				transPref = grid.GetUnitPrefFinder ().GetAirPref (typeOfAirPlane + 3); 
				tr = Instantiate (transPref) as Transform; 
				gridPos = coordinate; 
				//Debug.Log (CalcWorldPos (gridPos)); 
				tr.position = grid.CalcWorldPos (gridPos); 
				data = tr.GetComponent<AirplanBase>(); 
				data.setGrid (grid); 
				data.setCountry (country); 
				data.setCoordinate(gridPos); 
				data.typeOfAirPlane = typeOfAirPlane + 3;
				grid.GetGameController().getGoverment(country).SetUnit (data); 
				tr.name = tr.GetComponent<AirplanBase>().getNameAir() + country; 
				MapInfo.current.unitMap [(int)coordinate.x, (int)coordinate.y] = country * 100 + data.getTypeOfAirplane (); 
				Destroy (gameObject);
			} 
			break; 

		case 3: 
		case 4: 
		case 5: 
			if(grid.GetGameController().getGoverment(country).GetMoney() >= 300) { 
				grid.GetGameController().getGoverment(country).SetMoney(-300); 

				transPref = grid.GetUnitPrefFinder ().GetAirPref (typeOfAirPlane + 3); 
				tr = Instantiate (transPref) as Transform; 
				gridPos = coordinate; 
				//Debug.Log (CalcWorldPos (gridPos)); 
				tr.position = grid.CalcWorldPos (gridPos); 
				data = tr.GetComponent<AirplanBase>(); 
				data.setGrid (grid); 
				data.setCountry (country); 
				data.setCoordinate(gridPos); 
				data.typeOfAirPlane = typeOfAirPlane + 3;
				grid.GetGameController().getGoverment(country).SetUnit (data); 
				tr.name = tr.GetComponent<AirplanBase>().getNameAir() + country; 
				MapInfo.current.unitMap [(int)coordinate.x, (int)coordinate.y] = country * 100 + data.getTypeOfAirplane (); 
				Destroy (gameObject);
			} 
			break; 
		default: 
			break; 
		} 
	}

	public void MakeSteps() {
		List<Vector2> whereToGo;

		grid.GetGameController().getGoverment(country).newUnits.Add(this);

		while (CurrentAction != 0) {
			if (MapInfo.current.owners [(int)coordinate.x, (int)coordinate.y] < 100 && MapInfo.current.prefMap[(int)coordinate.x, (int)coordinate.y] > MapInfo.current.waterHeigth && typeOfAirPlane == 0) {
				//Debug.Log("build");
				putATown ();
				//NextStepUnit ();
				return;
			}
			//Debug.Log (NameAir + " I'm at " + coordinate.ToString ());
			way = new Stack<Vector2> ();
			whereToGo = getNeighbour ((int)coordinate.x, (int)coordinate.y, true);
			//Debug.Log (whereToGo.Count);
			if (HasVictim (whereToGo)) {
				//NextStepUnit ();
				return;
			}
			
			Vector2[] array = whereToGo.ToArray ();
			int rand = Random.Range(0,array.Length);
			//Debug.Log (array.Length);
			//way.Push (array[rand]);
			//Go ();
			destination = grid.CalcWorldPos(array[rand]);

			transform.position = destination;
			coordinate = array [rand];
			//while (isCorutine) {
			//}

			CurrentAction--;
		}
		//NextStepUnit ();


	}

	public void putATown() {

		int curOwner = MapInfo.current.owners [(int)coordinate.x, (int)coordinate.y];
		int city = grid.GetGameController().getGoverment(country).getTowns () + 1;
		List<Resourse> ress = new List<Resourse> ();

		for (int x = 0; x < MapInfo.current.gridWidth; x++) {
			for (int y = 0; y < MapInfo.current.gridHeigth; y++) {
				if (MapInfo.current.owners [x, y] == curOwner) {
					MapInfo.current.owners [x, y] = country * 100 + city;
					if (MapInfo.current.prefMap [x, y] < 0) {
						ress.Add(new Resourse ((int)MapInfo.current.prefMap [x, y], false, x, y, grid));
						Debug.Log ("country :" + country + "   town:" + city + "   res:" + MapInfo.current.prefMap [x, y]) ;
					}
				}
			}
		}

		Town town = grid.BuildTown ((int)coordinate.x, (int)coordinate.y, country, city);
		town.SetResourses (ress);

		town.SetGrid (grid);
		grid.GetGameController().getGoverment(country).newUnits.Remove (this);
		grid.GetGameController().getGoverment(country).SetTown (town);
		UnitsNasive.list.Remove (gameObject);
		//gameObject.SetActive (false);
		Destroy (gameObject);
	}


	private bool HasVictim(List<Vector2> whereToGo) {
		foreach (Vector2 v in whereToGo) {
			if (MapInfo.current.unitMap [(int)v.x, (int)v.y] != 0 && MapInfo.current.unitMap [(int)v.x, (int)v.y] / 100 != country) {
				grid.GetGameController ().getGoverment (MapInfo.current.unitMap [(int)v.x, (int)v.y] / 100).getUnit (v).setHeal (-damageForPlane);
				return true;
			}
		}
		return false;
	}

}
