using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindUnitPref : MonoBehaviour {

	//массив префабов
	//место в массиве - тип самолета (0,3,6 - грузовой, 1,4,7 - истребитель, 2,5,8 - бомбардировщик)
	public Transform[] prefabs;

	//название выбранного префаба
	//string nameHex = "";

	public Transform GetAirPref(int typeAir) {
		return prefabs [typeAir];
	}

}
