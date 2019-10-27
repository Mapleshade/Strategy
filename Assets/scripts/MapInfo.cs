using System.Collections;
using UnityEngine;

[System.Serializable]
public class MapInfo  { 

	public string nameM;
	//public string ex = "lol";
	//public static string exf = "kek";
	public static MapInfo current;
	//размер карты
	//высота
	public int gridHeigth = 100;
	//ширина
	public int gridWidth = 150;

	//количество игроков
	public int playerCount = 5; 
	//массив правительств
	public string[] govNames;
	//[номер государства, (0-деньги, 1-железо, 2-нефть)]
	public int[,] governments;
	//количестыо ходов
	public int steps;
	//какими землями, кто владеет
	public int[,] owners;

	//уровни рельефа
	//уровень воды
	public double waterHeigth = 0.6;
	//уровень равнин
	public double plainHeigth = 1;
	//уровень холмов
	public double hillHeigth = 1.4;

	//острова на карте
	//радиус островов
	//минимальный радиус
	public int minRadOfIsl = 6;
	//максимальный радиус
	public int maxRadOfIsl = 15;
	//количество островов на карте
	public int countOfIsl = 15;

	//ресурсы
	//количество типов ресурсов
	public int TypesOfResurses = 3;
	//минимальное количество на острове
	public int minCountResPerIsl = 3;
	//максимальное количество на острове
	public int maxCountResPerIsl = 6;

	//массив рельефа
	//-10 - ГОРОД
	public double[,] prefMap;
	//массив юнитов
	public int[,] unitMap;

	public MapInfo(){
		nameM = "0";
	}

}
