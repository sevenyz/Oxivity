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
	public bool test;

	FixedJoint2D joint;

	void Update() {
		if (canInteract && Input.GetKey(KeyCode.E)) {
			Grab();
		} 
		PlaceBox();
	}

	void Grab() {
		isGrabbing = true;
		joint = gameObject.AddComponent<FixedJoint2D>();
		joint.connectedBody = interactionObject.gameObject.GetComponent<Rigidbody2D>();

		//interactionObject.transform.localPosition += new Vector3(0, .2f, 0);
		interactionObject.transform.SetParent(boxPlaceHolder);
		if (isGrabbing) {
			interactionObject.transform.localPosition = boxPlaceHolder.transform.localPosition;
		}
	}

	void PlaceBox() {
		if (Input.GetKeyUp(KeyCode.E) && isGrabbing) {
			Destroy(joint);
			isGrabbing = false;

			interactionObject.transform.SetParent(null);
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Key") {
			CollectKey();
			other.gameObject.SetActive(false);
		}

		if (other.tag == "Box") {
			canInteract = true;
			interactionObject = other.gameObject;
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
	}

	void CollectKey() {
		hasKey = true;
	}
}
