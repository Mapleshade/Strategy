using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour {

	public GameObject pauseMenu;
	public GameObject ConfirmButton;
	public Text goldT;
	public Text ironT;
	public Text oilT;


	public void ContinueGame() {
		pauseMenu.SetActive (false);
	}

	public void SaveGame(){
		SaveLoadGame ();
		ConfirmButton.SetActive (true);
		ConfirmButton.GetComponentInChildren<Text> ().text = "Игра сохранена!";
	}

	public void ToMainMenu(){
		SaveLoadGame ();
		SceneManager.LoadScene ("MainMenu");
	}

	public void ExitGame(){
		SaveLoadGame ();
		Application.Quit();
	}

	void SaveLoadGame(){
		SaveLoad.Save ();
		//SaveLoad.Load ();

	}

	public void CloseconfirmButton(){
		ConfirmButton.SetActive (false);
	}

	public void OpenMenu() {
		pauseMenu.SetActive (true);
	}

	public void GetParametrs(){

		goldT.text = "Золото: " + MapInfo.current.governments[1,0];
		ironT.text = "Железо: " + MapInfo.current.governments[1,1];
		oilT.text = "Нефть: " + MapInfo.current.governments[1,2];
	}
	
}
