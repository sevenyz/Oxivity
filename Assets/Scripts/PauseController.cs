using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour {

	public GameObject pausePanel;
	public GameObject pauseMain;
	public GameObject pauseOptions;

	void Start() {
		Time.timeScale = 1;	
	}
	
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape) && pauseMain.activeSelf == true) {
			PauseGame();
		}

		if (Input.GetKeyDown(KeyCode.Escape) && pauseOptions.activeSelf == true) {
			ShowMainPause();
		}
	}

	public void PauseGame() {
		if (Time.timeScale != 0) {
			pausePanel.SetActive(true);
			Time.timeScale = 0;
		} else {
			Time.timeScale = 1;
			pausePanel.SetActive(false);
		}
	}

	public void ShowHideOptions(GameObject panelToDisplay, GameObject panelToHide) {
		panelToDisplay.SetActive(true);
		panelToHide.SetActive(false);
	}

	public void ShowOptions() {
		pauseMain.SetActive(false);
		pauseOptions.SetActive(true);
	}

	public void ShowMainPause() {
		pauseOptions.SetActive(false);
		pauseMain.SetActive(true);
	}

	public void BackToMainMenu() {
		SceneManager.LoadScene("MainMenu");
	}
}
