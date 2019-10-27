using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public class MainMenu : MonoBehaviour {

	public GameObject MainMenuObj;
	public GameObject CreateNewGameObj;
	public GameObject ContinueObj;
	public Transform button;

	//начать новую игру
	public void CreateNewGame(){
		MainMenuObj.SetActive (false);
		ContinueObj.SetActive (false);
		CreateNewGameObj.SetActive (true);
		MapInfo.current = new MapInfo();


	}

	//вернуться в главное меню
	public void BackToMenuMenu(){
		MainMenuObj.SetActive (true);
		ContinueObj.SetActive (false);
		CreateNewGameObj.SetActive (false);
	}

	//продолжить игру
	public void ContinueGame(){
		SaveLoad.Load();
		MainMenuObj.SetActive (false);
		ContinueObj.SetActive (true);
		CreateNewGameObj.SetActive (false);
		int gap = 0;

		foreach(MapInfo g in SaveLoad.savedGames) {
			
			Transform but = Instantiate (button) as Transform;
			but.SetParent (ContinueObj.transform);
			RectTransform tr = but.GetComponent<RectTransform> ();
			tr.anchoredPosition = new Vector2 (0,gap);
			gap -= -39;
			but.GetComponentInChildren<Text> ().text = g.nameM;
			Button b = but.GetComponent<Button> ();
			b.onClick.AddListener (() => onButtonClick(g));

		}

	}

	public void onButtonClick(MapInfo g){
		MapInfo.current = g;
		//Debug.Log (g.nameM);
		//Debug.Log ("clicked");
		SceneManager.LoadScene("World");

	}

	//загрузка игры
	public void LoadGame(Text text)
	{

		SceneManager.LoadScene("World");
	}

	//покинуть игру
	public void ExitGame(){
		Application.Quit();
	}

	//смена информации о карте
	public void ChangeInformation(ToggleGroup group){

		Toggle active = GetActive (group);

			
		switch (group.name) {
		case "SizeMap":
			ChangeSizeMap (active);
				break;
		case "SizeIslands": 
			ChangeSizeIslands (active);
				break;
		case "CountIslands": 
			ChangeCountIslands(active);
				break;
		case "CountRes":
			ChangeCountRes (active);
			break;
		case "CountPlayers":
			ChangeCountPlayers (active);
			break;
			default:
				break;
			}

	}

	//выбор размера карты
	void ChangeSizeMap(Toggle active){
		switch (active.name) {
		case "small":
			MapInfo.current.gridHeigth = 25;
			MapInfo.current.gridWidth = 25;
			break;
		case "normal": 
			MapInfo.current.gridHeigth = 50;
			MapInfo.current.gridWidth = 50;
			break;
		case "big": 
			MapInfo.current.gridHeigth = 150;
			MapInfo.current.gridWidth = 100;
			break;
		default:
			MapInfo.current.gridHeigth = 50;
			MapInfo.current.gridWidth = 50;
			break;
		}
	}

	//выбор размеров островов
	void ChangeSizeIslands(Toggle active){
		switch (active.name) {
		case "small":
			MapInfo.current.minRadOfIsl = 4;
			MapInfo.current.maxRadOfIsl= 9;
			break;
		case "normal": 
			MapInfo.current.minRadOfIsl = 5;
			MapInfo.current.maxRadOfIsl = 10;
			break;
		case "big": 
			MapInfo.current.minRadOfIsl = 6;
			MapInfo.current.maxRadOfIsl = 15;
			break;
		default:
			MapInfo.current.minRadOfIsl = 5;
			MapInfo.current.maxRadOfIsl = 10;
			break;
		}
	}

	//выбор количества островов
	void ChangeCountIslands(Toggle active){
		switch (active.name) {
		case "small":
			MapInfo.current.countOfIsl = 5;
			break;
		case "normal": 
			MapInfo.current.minRadOfIsl = 10;
			break;
		case "big": 
			MapInfo.current.minRadOfIsl = 15;
			break;
		default:
			MapInfo.current.minRadOfIsl = 10;
			break;
		}
	}

	//выбор количества ресурсов
	void ChangeCountRes(Toggle active){
		switch (active.name) {
		case "small":
			MapInfo.current.minCountResPerIsl = 2;
			MapInfo.current.maxCountResPerIsl = 3;
			break;
		case "normal": 
			MapInfo.current.minCountResPerIsl = 2;
			MapInfo.current.maxCountResPerIsl = 5;
			break;
		case "big": 
			MapInfo.current.minCountResPerIsl = 4;
			MapInfo.current.maxCountResPerIsl = 7;
			break;
		default:
			MapInfo.current.minCountResPerIsl = 2;
			MapInfo.current.maxCountResPerIsl = 4;
			break;
		}
	}

	//выбор количества игроков
	void ChangeCountPlayers(Toggle active){
		switch (active.name) {
		case "small":
			MapInfo.current.playerCount = 3;
			break;
		case "normal": 
			MapInfo.current.playerCount = 6;
			break;
		case "big": 
			MapInfo.current.playerCount = 9;
			break;
		default:
			MapInfo.current.playerCount = 6;
			break;
		}
	}

	//нахождение активной галочки в группе
	public static Toggle GetActive(ToggleGroup aGroup) {
		return aGroup.ActiveToggles ().FirstOrDefault ();
	}

	//загрузка сцены с параметрами игрока
	public void StartGame(){
		SaveLoad.Load ();
		//currentMenu = Menu.NewGame;
		SceneManager.LoadScene ("World");
	}
}
