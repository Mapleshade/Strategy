using System.Collections;
using UnityEngine;

[System.Serializable]
public class DataHex : MonoBehaviour {

	//название гекса
	[SerializeField]
	string hexName = "HexName";
	//координата X гекса
	[SerializeField]
	int corX = 0;
	//координата Y гекса
	[SerializeField]
	int corY = 0;
	//тип рельефа (0 - вода, 1 - равнина, 2 - холмы, 3 - горы)
	[SerializeField]
	int relief = -1;
	//наличие ресурса на гексе (0 - нет ресурса)
	[SerializeField]
	int resurse = 0;
	//правитель
	int owner = 0;


	//запись данных о гексе
	public void setData(string HexName, int x, int y, double number){
		hexName = HexName;
		this.corX = x;
		this.corY = y;
		changeHeigth (number);
		MapInfo.current.prefMap [x,y] = number;
		this.owner = MapInfo.current.owners [x,y];
	}
		
	// загрузка данных о гексе
	public void loadData(string HexName, int x, int y) {
		hexName = HexName;
		this.corX = x;
		this.corY = y;
		changeHeigth(MapInfo.current.prefMap [x,y]);
		this.owner = MapInfo.current.owners [x,y];
	}

	//определение рельефа и ресурсов на гексе
	void changeHeigth(double number){
		
		Renderer rend = gameObject.GetComponent<Renderer>();

		//меньше нуля - есть ресурс на гексе
		if(number < 0){
			
			switch ((int)number) {
			case -1: 
				//rend.material.color = Color.green;
				resurse = 1;
				relief = 1;
				break;
			case -2:
				//rend.material.color = Color.black;
				resurse = 2;
				relief = 1;
				break;
			case -3:
				//rend.material.color = Color.magenta;
				resurse = 3;
				relief = 1;
				break;
			case -4: 
				//rend.material.color = Color.green;
				resurse = 1;
				relief = 2;
				break;
			case -5:
				//rend.material.color = Color.black;
				resurse = 2;
				relief = 2;
				break;
			case -6:
				//rend.material.color = Color.magenta;
				resurse = 3;
				relief = 2;
				break;
			case -7: 
				//rend.material.color = Color.green;
				resurse = 1;
				relief = 1;
				break;
			case -8:
				//rend.material.color = Color.black;
				resurse = 2;
				relief = 1;
				break;
			case -9:
				//rend.material.color = Color.magenta;
				resurse = 3;
				relief = 1;
				break;
			case -10:
				resurse = 0;
				relief = -10;
				break;
			default:
				resurse = 0;
				relief = 0;
				break;
			}

		} else	if (number <= MapInfo.current.waterHeigth) {
			relief = 0;
			//rend.material.color = Color.blue;
		} else if (number <= MapInfo.current.plainHeigth) {
			//rend.material.color = Color.white;
			relief = 1;
		} else if (number <= MapInfo.current.hillHeigth) {
			//rend.material.color = Color.yellow;
			relief = 2;
		} else {
			//rend.material.color = Color.red;
			relief = 3;
		}


	}

	public int getX() {
		return corX;
	}

	public int getY() {
		return corY;
	}
}
