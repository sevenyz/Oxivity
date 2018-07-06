using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

	public GameObject menuScreen;
	public GameObject optionsScreen;

	void Start() {
		Time.timeScale = 1;
	}

	public void Options() {
		menuScreen.SetActive(false);
		optionsScreen.SetActive(true);
	}

	public void BackToMenu(GameObject currentScreen) {
		currentScreen.SetActive(false);
		menuScreen.SetActive(true);
	}

	public void QuitGame() {
		Application.Quit();
	}
}
