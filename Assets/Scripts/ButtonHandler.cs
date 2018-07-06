using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHandler : MonoBehaviour {

	public AudioSource buttonPressedSound;

	public GameObject platforms;	

	void PlatformActivation(GameObject platformsToActivate) {
		platformsToActivate = platforms;
		platforms.SetActive(true);
	}

	private void OnTriggerStay2D(Collider2D other) {
		if (other.tag == "Player") {

			if (Input.GetButtonDown("Interact")) {
				PlatformActivation(platforms);

				this.GetComponent<Animator>().enabled = true;

				buttonPressedSound.Play();
			}
		}
	}
}
