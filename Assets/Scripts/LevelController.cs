using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour {

	public GameObject continueButton;

	public int maxLevelUnlocked;

	public List<Button> unlockedLevels;

	void Start () {
		//DeleteKey("UnlockedLevels");
		maxLevelUnlocked = PlayerPrefs.GetInt("UnlockedLevels", maxLevelUnlocked);
		UnlockLevelButtons();

		if (maxLevelUnlocked > 1) {
			continueButton.SetActive(true);
		}
	}

	void UnlockLevelButtons() {
		switch (maxLevelUnlocked) {
			
			case 2:
				for (int i = 0; i < maxLevelUnlocked; i++) {
					unlockedLevels[i].interactable = true;
				}
				break;

			case 3:
				for (int i = 0; i < maxLevelUnlocked; i++) {
					unlockedLevels[i].interactable = true;
				}
				break;

			default:
				unlockedLevels[0].interactable = true;
				break;
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
