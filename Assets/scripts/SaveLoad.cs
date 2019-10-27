using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveLoad {

	public static List<MapInfo> savedGames = new List<MapInfo>();

	//методы загрузки и сохранения статические, поэтому их можно вызвать откуда угодно
	public static void Save() {
		MapInfo.current.nameM = "Ход: " + MapInfo.current.steps;
		if (SaveLoad.savedGames.Contains (MapInfo.current))
			SaveLoad.savedGames.Remove (MapInfo.current);
		SaveLoad.savedGames.Add(MapInfo.current);
		BinaryFormatter bf = new BinaryFormatter();
		//Application.persistentDataPath это строка; выведите ее в логах и вы увидите расположение файла сохранений
		//Debug.Log(Application.persistentDataPath);
		FileStream file = File.Create ("C:/Strategy/savedGames.txt");
		bf.Serialize(file, SaveLoad.savedGames);
		//Debug.Log (savedGames.Count);
		file.Close();
	}   

	public static void Load() {
		if(File.Exists("C:/Strategy/savedGames.txt")) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open("C:/Strategy/savedGames.txt", FileMode.Open);
			SaveLoad.savedGames = (List<MapInfo>)bf.Deserialize(file);
			//Debug.Log (savedGames.Count);
			file.Close();
		}
	}
}
