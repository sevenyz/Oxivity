using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHandler : MonoBehaviour {

	public GameObject platforms;

	public Sprite greenButton;
	public SpriteRenderer currentSprite;

	private void Start() {
		currentSprite = gameObject.GetComponent<SpriteRenderer>();
	}	

	void PlatformActivation(GameObject platformsToActivate) {
		platformsToActivate = platforms;
		platforms.SetActive(true);
	}

	private void OnTriggerStay2D(Collider2D other) {
		if (other.tag == "Player") {

			if (Input.GetButtonDown("Interact")) {
				PlatformActivation(platforms);
				currentSprite.sprite = greenButton;
			}
		}
	}
}
