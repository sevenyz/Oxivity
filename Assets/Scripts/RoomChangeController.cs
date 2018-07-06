using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomChangeController : MonoBehaviour {

	Rigidbody2D playerRB;
	
	public bool isTransitioning;

	public GameObject thisRoom;
	public GameObject nextRoom;

	public Player player;

	public Transform playerPlaceholderForRepositioning;

	public bool swapGravity;
	public bool repositionPlayer;
	public bool canInteractWithDoor;

	public Animator fadeAnim;
	public Animator doorAnim;

	private void Update() {
		if (canInteractWithDoor && Input.GetButtonDown("Interact") && !isTransitioning) {

			isTransitioning = true;
			fadeAnim.SetTrigger("Transition");

			Invoke("RestorePlayerMovement", 2.5f);
			
			//canInteractWithDoor = false;

			if (doorAnim != null) {
				doorAnim.SetTrigger("DoorOpen");
			}

			StartCoroutine(LoadRoom());
		}
	}

	void RestorePlayerMovement() {
		player.speed = player.maxSpeed;
		player.jumpForce = player.maxJumpForce;

		isTransitioning = false;
	}

	public IEnumerator LoadRoom() {

		player.speed = 0;
		player.jumpForce = 0;

		yield return new WaitForSeconds(1.2f);

		thisRoom.SetActive(false);
		nextRoom.SetActive(true);

		if (swapGravity) {

			player.transform.localScale = new Vector2(1, -player.transform.localScale.y);
			
			player.isUpsideDown =! player.isUpsideDown;
		
			playerRB = player.GetComponent<Rigidbody2D>();
			playerRB.gravityScale = -playerRB.gravityScale;

			if (!player.isFacingRight) {
				player.isFacingRight = true;
			}
		} 

		if (repositionPlayer) {
			player.transform.position = playerPlaceholderForRepositioning.position;
		}
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Player") {
			canInteractWithDoor = true;
		}
	}

	private void OnTriggerExit2D(Collider2D other) {
		if (other.tag == "Player") {
			canInteractWithDoor = false;
		}
	}
}
