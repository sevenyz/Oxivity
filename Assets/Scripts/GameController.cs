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
	public GameObject restartText;
	public GameObject levelCompletedText;
	public Slider loadingBar;

	public int maxLevelUnlocked;
	public bool levelCompleted;
	public bool isOnLastLevel;

	void Awake() {
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();	
	}

	void Start() {
		maxLevelUnlocked = PlayerPrefs.GetInt("UnlockedLevels", maxLevelUnlocked);
		Time.timeScale = 1;
	}

	void Update() {
		GameOver();

		if (SceneManager.GetActiveScene().buildIndex >= 3) {
			isOnLastLevel = true;
		} 

		if (levelCompleted) {
			if (Input.GetKeyDown(KeyCode.Return)) {
				Debug.Log("enter");
				StartCoroutine(Loading());
			}
		}
	}

	void GameOver() {
		if (player.isGameOver) {
			gameOverText.SetActive(true);
			Invoke("ShowRestartText", 1);
		}
	}

	void ShowRestartText() {
		restartText.SetActive(true);
		if (Input.GetKeyDown(KeyCode.R)) {
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
	}

	public void ShowLevelCompletedText() {
		Destroy(player.GetComponent<Player>());
		levelCompleted = true;
		levelCompletedText.SetActive(true);
		UnlockLevel();
	}

	public void UnlockLevel() {
		int levelToUnlock = SceneManager.GetActiveScene().buildIndex + 1;

		if (levelToUnlock > maxLevelUnlocked) {
			maxLevelUnlocked = levelToUnlock;
			PlayerPrefs.SetInt("UnlockedLevels", maxLevelUnlocked);
		}
	}

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
