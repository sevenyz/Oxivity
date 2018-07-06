using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour {

	Player player;
	Vector3 originalPos;
	FixedJoint2D joint;
	Rigidbody2D interObjRB;

	bool keyPressed;
	
	public GameController gameController;
	public GameObject interactionObject;
	public Transform boxPlaceHolder;
	public Animator doorAnim;

	public AudioSource boxPickupSound;
	public AudioSource keyPickupSound;

	public bool canInteract;
	public bool hasKey;
	public bool isGrabbing;

	void Start() {
		joint = GetComponent<FixedJoint2D>();	
		player = GetComponent<Player>();
	}

	void Update() {
		TestGrab();

		if (isGrabbing) {
			interactionObject.transform.position = Vector2.Lerp(interactionObject.transform.position, boxPlaceHolder.position, 20 * Time.deltaTime);
		}
	}

	void Grab() {
		if (canInteract) {
			// Impeding the player possibility to grab multiple objects at once
			canInteract = false;

			isGrabbing = true;
			boxPickupSound.Play();

			// Creating a joint on the player and connecting our current interactable object to it
			joint = gameObject.AddComponent<FixedJoint2D>();
			joint.connectedBody = interactionObject.gameObject.GetComponent<Rigidbody2D>();

			// Decreasing the player's jump force
			player.jumpForce = player.jumpForceWhileGrab;
		}	
	}

	void Release() {
		if (isGrabbing && !keyPressed) {
			isGrabbing = false;
			Destroy(joint);

			// Resetting the normal player's jump force
			player.jumpForce = player.maxJumpForce;

			// Untriggering the object's collider to detect collisions again
			interactionObject.GetComponent<BoxCollider2D>().isTrigger = false;
		}
	}

	void TestGrab() {
		if (Input.GetButtonDown("Grab")) {
			// Controlling key state
			keyPressed =! keyPressed;

			Grab();

			Release();
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Key") {
			CollectKey();
			other.gameObject.SetActive(false);
		}

		if (other.tag == "GravitySwitch" && isGrabbing) {
			interObjRB = interactionObject.GetComponent<Rigidbody2D>();
			interObjRB.gravityScale = -interObjRB.gravityScale;
		}

		if (other.tag == "BoxReleaseTrig") {
			Destroy(joint);
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
				doorAnim.SetTrigger("DoorOpenStay");
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
		keyPickupSound.Play();
	}
}
