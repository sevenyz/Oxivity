using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour {

	Player player;
	Vector3 originalPos;
	FixedJoint2D joint;

	bool keyPressed;
	
	public GameController gameController;
	public GameObject interactionObject;
	public Transform boxPlaceHolder;

	public bool canInteract;
	public bool hasKey;
	public bool isGrabbing;

	void Start() {
		joint = GetComponent<FixedJoint2D>();	
		player = GetComponent<Player>();
	}

	void Update() {
		TestGrab();
	}

	void TestGrab() {
		if (Input.GetButtonDown("Grab")) {
			// Controlling key state
			keyPressed =! keyPressed;

			// Grab mechanic
			if (canInteract) {
				// Impeding the player possibility to grab multiple objects at once
				canInteract = false;

				isGrabbing = true;

				// Creating a joint on the player and connecting our current interactable object to it
				joint = gameObject.AddComponent<FixedJoint2D>();
				joint.connectedBody = interactionObject.gameObject.GetComponent<Rigidbody2D>();

				// Decreasing the player's jump force
				player.jumpForce = player.jumpForceWhileGrab;

				// Setting our object's collider to trigger to avoid unwanted collisions
				interactionObject.GetComponent<BoxCollider2D>().isTrigger = true;
			}

			// Release mechanic
			if (isGrabbing && !keyPressed) {
				isGrabbing = false;
				Destroy(joint);

				// Resetting the normal player's jump force
				player.jumpForce = player.maxJumpForce;

				// Untriggering the object's collider to detect collisions again
				interactionObject.GetComponent<BoxCollider2D>().isTrigger = false;
			}
		}
	}

	/* void Grab() {
		if (canInteract && Input.GetKeyDown(KeyCode.E)) {
			canInteract = false;
			//joint.enabled = true;
			isGrabbing = false;
			joint = gameObject.AddComponent<FixedJoint2D>();
			//joint.autoConfigureConnectedAnchor = false;
			joint.connectedBody = interactionObject.gameObject.GetComponent<Rigidbody2D>();
			//joint.anchor = new Vector2(0.18f, 0.13f);
			//joint.connectedAnchor = new Vector2(-1, 0);


			//interactionObject.transform.localPosition += new Vector3(0, .2f, 0);
			//interactionObject.transform.SetParent(boxPlaceHolder);
			if (isGrabbing) {
				//interactionObject.transform.localPosition = boxPlaceHolder.transform.localPosition;
			}
		}
	}

	void PlaceBox() {
		if (isGrabbing && Input.GetKeyDown(KeyCode.E)) {
			//joint.enabled = false;
			isGrabbing = false;
			canInteract = true;
			Destroy(joint);
			//interactionObject.transform.SetParent(null);
		}
	} */

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Key") {
			CollectKey();
			other.gameObject.SetActive(false);
		}
	}	

	private void OnTriggerExit2D(Collider2D other) {
		if (other.tag == "Box") {
			canInteract = false;
		}
	}

	private void OnTriggerStay2D(Collider2D other) {
		if (other.tag == "Door" && hasKey && !gameController.isOnLastLevel) {
			if (Input.GetButtonDown("Interact")) {
				gameController.ShowLevelCompletedText();
			}
		}

		// Checking if the player is facing a box
		if (other.tag == "Box" && !isGrabbing) {
			canInteract = true;
			keyPressed = false;
			interactionObject = other.gameObject;
		}
	}

	void CollectKey() {
		hasKey = true;
	}
}
