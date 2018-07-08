using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxHandler : MonoBehaviour {

	Rigidbody2D rb2d;
	[HideInInspector] public InteractionController interactionController;

	public GameObject circlePlaceholder;
	public GameObject player;
	public Transform placeholder;

	public AudioSource land;

	public float radius;
	[HideInInspector] public bool isPlayerAbove;
	public bool isUpsideDown;


	void Start () {
		rb2d = GetComponent<Rigidbody2D>();
		interactionController = player.GetComponent<InteractionController>();
	}
	
	void Update () {
		CheckIfPlayerAbove();

		if (rb2d.gravityScale > 0) {
			isUpsideDown = false;
		}
		else {
			isUpsideDown = true;
		}
	}

	void SwapGravityScale() {
		rb2d.gravityScale = -rb2d.gravityScale;
	}

	void CheckIfPlayerAbove() {
		isPlayerAbove = Physics2D.OverlapCircle(circlePlaceholder.transform.position, radius, 1 << LayerMask.NameToLayer("GroundCheck"));
		
		if (!isPlayerAbove) {
			Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), this.GetComponent<Collider2D>(), true);
		}

		else {
			Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), this.GetComponent<Collider2D>(), false);
			player.GetComponent<InteractionController>().canInteract = false;
		}
	}

	void Flip() {
		// Inverting the local scale
		transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
	}

	private void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.tag != "Player") {
			land.Play();
		}
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Switch") {
			Flip();
		}

		if (other.tag == "BoxReleaseTrig") {
			this.transform.position = placeholder.position;
		}
	}

	// Only for debug purposes
	void OnDrawGizmos() {
		Gizmos.DrawWireSphere(circlePlaceholder.transform.position, radius);
	}
}
