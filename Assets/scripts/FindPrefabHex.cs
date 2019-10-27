using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindPrefabHex : MonoBehaviour {

	int rand;
	//массив префабов
	public Transform[] prefabs;

	//массив клеток вокруг выбранной
	double[] neighbors = new double[6];

	//название выбранного префаба
	string nameHex = "";

	//главный метод выбора префаба
	public Transform ChoosePrefHex(int x, int y){

		double center = MapInfo.current.prefMap [x, y];
		nameHex = "";
		SetHexs (x, y);

		return ChooseEarthPref (center);

	}


	//выбор водного префаба
	//состоит только из 0 и 1; 0 - если контактирует с другой клеткой воды, 1 - если контактирует с островом;
	Transform ChooseWaterPref (){

		//пробегаем по всему массиву соседей и записываем последоавтельность
		for (int i = 0; i < neighbors.Length; i++) {

			//0 - если вода
			if (neighbors [i] >= 0 && neighbors [i] <= MapInfo.current.waterHeigth) {
				nameHex += "0";

			} else {
				//1 - если земля
				nameHex += "1";
			}
		}

		//ищем в массиве префабов префаб с таким названием и возвращаем его
		for (int j = 0; j < prefabs.Length; j++) {
			if (prefabs [j].name == nameHex)
				return prefabs [j];
		}

		return prefabs[0];
	}

	//выбор префаба земли
	//состоит из 2-4 в начале, в зависимости от типа рельефа; вторая цифра от 0 до 3 в зависимости от наличия ресурса; далее рандомная цифра в зависимости от количества вариаций данной клетки

	public Transform ChooseEarthPref (double center){
		
		//меньше нуля - есть ресурс на гексе
		if(center < 0){

			switch ((int)center) {
			//на равнине железо
			case -1: 
				//nameHex = "21";
				rand = Random.Range (0, 2);
				if (rand == 0) {
					nameHex = "21";
				} else {
					nameHex = "73";
				}
				break;
				//на равнине нефть
			case -2:
				//nameHex = "22";
				rand = Random.Range (0, 2);
				if (rand == 0) {
					nameHex = "22";
				} else {
					nameHex = "83";
				}
				break;
				//на равнине золото
			case -3:
				//nameHex = "23";
				rand = Random.Range (0, 2);
				if (rand == 0) {
					nameHex = "23";
				} else {
					nameHex = "63";
				}

				break;
				//на холмах железо
			case -4: 
				rand = Random.Range (0, 2);
				if (rand == 0) {
					nameHex = "31";
				} else {
					nameHex = "41";
				}

				break;
				//на холмах нефть
			case -5:
					nameHex = "32";

				break;
				//на холмах золото
			case -6:
				rand = Random.Range (0, 2);
				if (rand == 0) {
					nameHex = "33";
				} else {
					nameHex = "43";
				}

				break;
				//обработанное железо
			case -7:
				nameHex = "50";
				break;
				//обработанное нефть
			case -8:
				nameHex = "60";
				break;
				//обработанное золото
			case -9:
				nameHex = "50";
				break;
			case -10:
				nameHex = "hexagonAngar";
				break;
				//вода
			default:
				nameHex = "00";
				break;
			}

		} else	if (center <= MapInfo.current.waterHeigth) {
			ChooseWaterPref ();
		} else if (center <= MapInfo.current.plainHeigth) {
			nameHex = "20";
			rand = Random.Range (0, 2);
			nameHex += rand;
		} else if (center <= MapInfo.current.hillHeigth) {
			nameHex = "30";
			rand = Random.Range (0, 3);
			nameHex += rand;
		} else {
			nameHex = "400";
		}

		/*здесь должен быть рандомайзер
		nameHex += Random.Range();
		*/

	//ищем в массиве префабов префаб с таким названием и возвращаем его
	for (int j = 0; j < prefabs.Length; j++) {
		if (prefabs [j].name == nameHex)
			return prefabs [j];
	}
		return prefabs[0];
	}

	void SetHexs(int x, int y) {

		if(x - 1 >= 0){
			neighbors [2] = MapInfo.current.prefMap [x - 1, y];
		} else {
			neighbors [2] = 0;
		}


		if(x + 1 < MapInfo.current.gridWidth){
			neighbors [5] = MapInfo.current.prefMap [x + 1, y];
		} else {
			neighbors [5] = 0;
		}


		if(y % 2 == 0) {

		if (y + 1 < MapInfo.current.gridHeigth) {
			neighbors [0] = MapInfo.current.prefMap [x, y + 1];
		} else {
			neighbors [0] = 0;
		}

		if(x - 1 >= 0 && y + 1 < MapInfo.current.gridHeigth) {
		neighbors [1] = MapInfo.current.prefMap [x - 1, y + 1];
		} else {
			neighbors [1] = 0;
		}

		
		if(x - 1 >= 0 && y - 1 >= 0) {
		neighbors [3] = MapInfo.current.prefMap [x - 1, y - 1];
		} else {
			neighbors [3] = 0;
		}

		if(y - 1 >= 0) {
		neighbors [4] = MapInfo.current.prefMap [x, y - 1];
		} else {
			neighbors [4] = 0;
		}

		

		} else {
			
			if (y + 1 < MapInfo.current.gridHeigth) {
				neighbors [1] = MapInfo.current.prefMap [x, y + 1];
			} else {
				neighbors [1] = 0;
			}

			if(y - 1 >= 0) {
				neighbors [3] = MapInfo.current.prefMap [x, y - 1];
			} else {
				neighbors [3] = 0;
			}

			if(x + 1 < MapInfo.current.gridWidth && y + 1 < MapInfo.current.gridHeigth) {
				neighbors [0] = MapInfo.current.prefMap [x + 1, y + 1];
			} else {
				neighbors [0] = 0;
			}


			if(x + 1 < MapInfo.current.gridWidth && y - 1 >= 0) {
				neighbors [4] = MapInfo.current.prefMap [x + 1, y - 1];
			} else {
				neighbors [4] = 0;
			}

		}
	}


	public Transform GetTownHex() {
		return prefabs [83];
	}


}
