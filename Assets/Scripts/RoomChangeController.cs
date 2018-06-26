using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomChangeController : MonoBehaviour {

	public Player player;
	Rigidbody2D playerRB;

	public GameObject thisRoom;
	public GameObject nextRoom;

	public Transform playerPlaceholderForRepositioning;

	public bool swapGravity;
	public bool repositionPlayer;
	public bool canInteractWithDoor;

	public Animator fadeAnim;
	public Animator doorAnim;

	private void Update() {
		if (canInteractWithDoor && Input.GetButtonDown("Interact")) {
			fadeAnim.SetTrigger("Transition");

			if (doorAnim != null) {
				doorAnim.SetTrigger("DoorOpen");
			}

			StartCoroutine(LoadRoom());
		}
	}

	public IEnumerator LoadRoom() {
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
