using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoEnd : MonoBehaviour {
	
	public GameController gameController;
	public InteractionController interaction;
	public GameObject demoEndPanel;

	/* void Update () {
		if (gameController.levelCompleted && gameController.isOnLastLevel) {
			demoEndPanel.SetActive(true);
		}
	} */

	public void QuitGame() {
		Application.Quit();
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (interaction.hasKey) {
			demoEndPanel.SetActive(true);
			Time.timeScale = 0;
		}
	}
}
