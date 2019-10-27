using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {
	
	public Transform hexPref;
	public Transform transPref;
	public GameObject cam;
	Vector3 camStart;

	float hexHeigth = 2f;
	float hexWidth = 1.732f;

	public float gap = 0.0f;

	Vector3 startPos;

	//private maxLand;
	private double[,] typeOfRelief;
	FindPrefabHex findPrefabHex;
	FindUnitPref findUnitPref;
	public GameController gameController;


	void Start(){
		
		findPrefabHex = GetComponent<FindPrefabHex> ();
		findUnitPref = GetComponent<FindUnitPref> ();
		if (MapInfo.current.nameM == "0") {
			MapInfo.current.nameM = "Ход: " + MapInfo.current.steps;
			MapInfo.current.prefMap = new double[MapInfo.current.gridWidth, MapInfo.current.gridHeigth];
			MapInfo.current.unitMap = new int[MapInfo.current.gridWidth, MapInfo.current.gridHeigth];
			MapInfo.current.owners = new int[MapInfo.current.gridWidth, MapInfo.current.gridHeigth];
			MapInfo.current.govNames = new string[MapInfo.current.playerCount + 2];
			MapInfo.current.governments = new int[MapInfo.current.playerCount + 2, 3];
			AddGap ();
			CalcStartPos ();
			CreateMassive ();
			MapInfo.current.prefMap = typeOfRelief;
			CheckPlayer ();
			CreateGrid (false);
			SaveLoad.Save ();
		} else {
			gameController.LoadGoverments ();
			AddGap ();
			CalcStartPos ();
			CreateGrid (true);
		}
		SendCoordinates ();
		SetCamera ();

	}

	void CheckPlayer() {

		if (GameObject.Find ("TransportAircraft1") == null) {
			for (int y = 0; y < MapInfo.current.gridHeigth; y++) {
				for (int x = 0; x < MapInfo.current.gridWidth; x++) {
					if (MapInfo.current.prefMap [x, y] > MapInfo.current.waterHeigth) {
						SpawnNew (1, x, y);
						return;
					}
				}
			}
	}
	}

	void SetCamera() {
		cam.transform.position = camStart;
	}

	void SpawnNew(int country, int corX, int corY) {
		transPref = findUnitPref.GetAirPref (0);
		//Debug.Log (transPref.name);
		//Debug.Log("I put " + name + " in (" + corX + ", " + corY + ") " );
		Transform tr = Instantiate (transPref) as Transform;
		//Debug.Log (tr.name);
		Vector2 gridPos = new Vector2(corX,corY);
		//Debug.Log (CalcWorldPos (gridPos));
		tr.position = CalcWorldPos (gridPos);
		AirplanBase data = tr.GetComponent<TransportAircraft>();
		//Debug.Log (data.name);
		data.setGrid (this);
		data.setCountry (country);
		data.setCoordinate(gridPos);
		//Debug.Log (country);
		//Debug.Log (MapInfo.current.governments [country]);
		//Debug.Log (MapInfo.current.governments [country].getUnits());
		gameController.getGoverment(country).SetUnit (data);
		//UnitsNasive.list.Add (tr.gameObject);
		tr.name = tr.GetComponent<TransportAircraft>().getNameAir() + country; 
		MapInfo.current.unitMap [corX, corY] = country * 100 + data.getTypeOfAirplane ();
		if(country == 1)
			camStart =  CalcWorldPos (gridPos);
	}

	void LoadSpawn(int x, int y) {
		transPref = findUnitPref.GetAirPref (MapInfo.current.unitMap[x,y]%100);
		if (MapInfo.current.unitMap [x, y] != 0) {
			Transform tr = Instantiate (transPref) as Transform;
			Vector2 gridPos = new Vector2(x,y);
			tr.position = CalcWorldPos (gridPos);
			AirplanBase data = tr.GetComponent<AirplanBase>();
			data.setCountry (MapInfo.current.unitMap[x,y] / 100);
			data.setGrid (this);
			data.setCoordinate(gridPos);
			gameController.getGoverment(MapInfo.current.unitMap[x,y] / 100).SetUnit (data);
			//UnitsNasive.list.Add (tr.gameObject);
			tr.name = tr.GetComponent<AirplanBase>().getNameAir() + data.getCountry(); 
			if(MapInfo.current.unitMap[x,y] / 100 == 1)
				camStart =  CalcWorldPos (gridPos);
		}

	}


	void CreateMassive(){
		
		typeOfRelief = new double [MapInfo.current.gridWidth, MapInfo.current.gridHeigth];

		for (int l = 0; l < MapInfo.current.playerCount; l++) {
			makeAHill (l+1, l+1);
		}
	
		for (int l = MapInfo.current.playerCount; l < MapInfo.current.countOfIsl; l++) {
			makeAHill (0, l+1);
		}
	}

	void makeAHill(int pers, int owner) {
		
		int px = Random.Range (0, MapInfo.current.gridHeigth-1);
		int py = Random.Range (0, MapInfo.current.gridWidth - 1);
		int rad = Random.Range (MapInfo.current.minRadOfIsl,MapInfo.current.maxRadOfIsl);
		int resourses = Random.Range (MapInfo.current.minCountResPerIsl, MapInfo.current.maxCountResPerIsl);
		int curRes = 0;

		for (int u = 0; u < MapInfo.current.gridHeigth; u++) {
			for(int i = 0; i < MapInfo.current.gridWidth; i++) {
				double d = rad * rad - ((px - i) * (px - i) + (py - u) * (py - u));
				if (typeOfRelief [i, u] == 0 && d > 0) {
					typeOfRelief [i, u] += d / (rad * rad) + Random.value * 0.5;
					if (typeOfRelief [i, u] > MapInfo.current.waterHeigth)
						MapInfo.current.owners [i, u] = owner;
					if (curRes < resourses && typeOfRelief [i, u] > MapInfo.current.waterHeigth && Random.Range(0,MapInfo.current.maxRadOfIsl) == 0) {
						typeOfRelief [i, u] = Random.Range (-6, 0);
						curRes++;
					}
					if (pers != 0 && typeOfRelief [i, u] > MapInfo.current.waterHeigth) {
						SpawnNew (pers, i , u);
						pers = 0;
					}
				}
			}
		}
	}


	void AddGap() {
		hexWidth += hexWidth * gap;
		hexHeigth += hexHeigth * gap;
	}

	void CalcStartPos(){
		float offset = 0;

		if (MapInfo.current.gridHeigth / 2 % 2 != 0)
			offset = hexWidth /2;


		float x = -hexWidth * (MapInfo.current.gridWidth / 2) - offset;
		float z = hexHeigth * 0.75f *(MapInfo.current.gridHeigth / 2);


		startPos = new Vector3(x,0,z);
	}

	void CreateGrid(bool isLoaded){
		for (int y = 0; y < MapInfo.current.gridHeigth; y++) {
			for (int x = 0; x < MapInfo.current.gridWidth; x++) {
				if (MapInfo.current.prefMap [x, y] == -10) {
					BuildTown (x, y, MapInfo.current.owners [x, y] / 100, MapInfo.current.owners [x, y] % 100);

				} else {
					hexPref = findPrefabHex.ChoosePrefHex (x, y);
					Transform hex = Instantiate (hexPref) as Transform;
					Vector2 gridPos = new Vector2 (x, y);
					hex.position = CalcWorldPos (gridPos);
					hex.parent = this.transform;
					hex.name = "Hexagon " + x + "|" + y; 

					DataHex data = hex.transform.GetChild (0).GetComponentInChildren<DataHex> ();

					if (data != null) {
						if (!isLoaded) {
							data.setData (hex.name, x, y, typeOfRelief [x, y]);
						} else {
							data.loadData (hex.name, x, y);
							if (MapInfo.current.unitMap [x, y] != 0)
								LoadSpawn (x, y);
						}
					}
				}
			}
		}
	}

	public Vector3 CalcWorldPos(Vector2 gridPos){
		float offset = 0f;
		if (gridPos.y % 2 != 0)
			offset = hexWidth / 2;
		float x = startPos.x + gridPos.x * hexWidth + offset;
		float z = startPos.z - gridPos.y * hexHeigth * 0.75f;

		return new Vector3 (x,0,z);
	}

	void SendCoordinates(){
		Vector3 first = CalcWorldPos (new Vector2(0,0));
		Vector3 last = CalcWorldPos (new Vector2(MapInfo.current.gridWidth-1, MapInfo.current.gridHeigth-1));
		float left = first.z;
		float right = last.z;
		float up = last.x;
		float down = first.x;
		CameraMove scr = cam.GetComponent<CameraMove> ();
		if (scr != null)
			scr.setRestrictions (left, right, up, down);
	}

	public Town BuildTown(int x, int y, int country, int cityId) {
		MapInfo.current.prefMap [x, y] = -10;
		if(transform.Find ("Hexagon " + x + "|" + y) != null)
			Destroy(transform.Find ("Hexagon " + x + "|" + y).gameObject);
		hexPref = findPrefabHex.GetTownHex();
		Transform hex = Instantiate (hexPref) as Transform;
		Vector2 gridPos = new Vector2(x,y);
		hex.position = CalcWorldPos (gridPos);
		hex.parent = this.transform;
		hex.name = "Hexagon " + x + "|" + y; 
		Town town = hex.GetChild(0).GetComponentInChildren<Town> ();
		//Debug.Log (town.name);
		DataHex data = hex.transform.GetChild(0).GetComponentInChildren<DataHex> ();
		town.setTown ("", country, cityId, x, y);
		data.setData (hex.name, x, y, -10);

		return town;
	}

	public void loadResourses() {
		Resourse res;
		for (int x = 0; x < MapInfo.current.gridWidth; x++) {
			for (int y = 0; y < MapInfo.current.gridHeigth; y++) {
				switch((int)MapInfo.current.prefMap[x,y]) {
				case -1: 
				case -2:
				case -3:
				case -4: 
				case -5:
				case -6:
					res = new Resourse ((int)MapInfo.current.prefMap[x,y], false, x, y, this);
					gameController.getGoverment (MapInfo.current.owners [x, y] / 100).getTown (MapInfo.current.owners [x, y] % 100).AddResource (res);
					break;
				case -7:
				case -8:
				case -9:
					res = new Resourse ((int)MapInfo.current.prefMap[x,y], true, x, y, this);
					gameController.getGoverment (MapInfo.current.owners [x, y] / 100).getTown (MapInfo.current.owners [x, y] % 100).AddResource (res);
					break;
					break;

				}
			}
		}

	}

	public void ActivateResource(int x, int y, int type) {
		Destroy(transform.Find ("Hexagon " + x + "|" + y).gameObject);
		hexPref = findPrefabHex.ChooseEarthPref(type);
		Transform hex = Instantiate (hexPref) as Transform;
		Vector2 gridPos = new Vector2(x,y);
		hex.position = CalcWorldPos (gridPos);
		hex.parent = this.transform;
		hex.name = "Hexagon " + x + "|" + y; 

	}

	public FindUnitPref GetUnitPrefFinder() {
		return findUnitPref;
	}

	public GameController GetGameController() {
		return gameController;
	}
}
