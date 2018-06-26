using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	AsyncOperation async;
	Player player;

	public GameObject loadingScreen;
	public GameObject gameOverText;
	public GameObject restartOrQuitText;
	public GameObject levelCompletedText;
	public Slider loadingBar;

	[HideInInspector] public int maxLevelUnlocked;
	[HideInInspector] public bool levelCompleted;
	[HideInInspector] public bool isOnLastLevel;

	void Awake() {
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();	
	}

	void Start() {
		maxLevelUnlocked = PlayerPrefs.GetInt("UnlockedLevels", maxLevelUnlocked);
		Time.timeScale = 1;
	}

	void Update() {
		GameOver();

		// Checking if is last level
		if (SceneManager.GetActiveScene().buildIndex >= 4) {
			isOnLastLevel = true;
		} 

		// Loading next level
		if (levelCompleted) {
			if (Input.GetKeyDown(KeyCode.Return)) {
				StartCoroutine(Loading());
			}
		}
	}

	// Gameover actions
	void GameOver() {
		if (player.isGameOver) {
			gameOverText.SetActive(true);

			Invoke("ShowRestartText", 1);
		}
	}

	void ShowRestartText() {
		restartOrQuitText.SetActive(true);

		// Reloading current level
		if (Input.GetKeyDown(KeyCode.R)) {
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
	}

	public void ShowLevelCompletedText() {
		levelCompleted = true;
		levelCompletedText.SetActive(true);
		UnlockLevel();
	}

	// Unlocks next level if current level is cleared
	public void UnlockLevel() {
		int levelToUnlock = SceneManager.GetActiveScene().buildIndex + 1;

		// Saving
		if (levelToUnlock > maxLevelUnlocked) {
			maxLevelUnlocked = levelToUnlock;
			PlayerPrefs.SetInt("UnlockedLevels", maxLevelUnlocked);
		}
	}

	// Button
	public void BackToMenu() {
		SceneManager.LoadScene("MainMenu");
	}

	// Beta
	public IEnumerator Loading() {
		loadingScreen.SetActive(true);

		async = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
		async.allowSceneActivation = false;

		if (async.isDone == false) {
			loadingBar.value = async.progress;

			if (async.progress <= 0.9f) {
				loadingBar.value = 1;
				async.allowSceneActivation = true;
			}
			yield return null;
		}
	}
}
