using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour {

	public GameObject continueButton;

	public int maxLevelUnlocked;

	void Start () {
		//DeleteKey("UnlockedLevels");
		maxLevelUnlocked = PlayerPrefs.GetInt("UnlockedLevels", maxLevelUnlocked);

		if (maxLevelUnlocked > 1) {
			continueButton.SetActive(true);
		}
	}

	void DeleteKey(string key) {
		PlayerPrefs.DeleteKey(key);
	}

	// Level Buttons
	public void JumpToLevel(int level) {
		SceneManager.LoadScene(level);
	}

	public void LoadLastLevel() {
		SceneManager.LoadScene(maxLevelUnlocked);
	}
}
