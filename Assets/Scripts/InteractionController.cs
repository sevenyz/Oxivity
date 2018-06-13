using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour {

	public GameController gameController;
	
	public GameObject interactionObject;
	public Transform boxPlaceHolder;

	Vector3 originalPos;

	public bool canInteract;
	public bool hasKey;
	public bool isGrabbing;

	bool keyPressed;

	Player player;

	FixedJoint2D joint;

	void Start() {
		joint = GetComponent<FixedJoint2D>();	
		player = GetComponent<Player>();
	}

	void Update() {
		//Grab();
		
		//PlaceBox();

		TestGrab();
	}

	void TestGrab() {
		if (Input.GetKeyDown(KeyCode.E)) {
			keyPressed =! keyPressed;
			if (canInteract) {
				// Grab
				canInteract = false;
				isGrabbing = true;

				joint = gameObject.AddComponent<FixedJoint2D>();
				joint.connectedBody = interactionObject.gameObject.GetComponent<Rigidbody2D>();

				player.jumpForce = player.jumpForceWhileGrab;

				interactionObject.GetComponent<BoxCollider2D>().isTrigger = true;
			}

			if (isGrabbing && !keyPressed) {
				// Release
				isGrabbing = false;
				Destroy(joint);

				player.jumpForce = player.maxJumpForce;
				interactionObject.GetComponent<BoxCollider2D>().isTrigger = false;
			}
		}
	}

	void Grab() {
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
	}

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
			if (Input.GetKeyDown(KeyCode.E)) {
				gameController.ShowLevelCompletedText();
			}
		}

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
