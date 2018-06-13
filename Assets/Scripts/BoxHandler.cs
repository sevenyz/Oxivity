using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxHandler : MonoBehaviour {

	Rigidbody2D rb2d;

	public GameObject circlePlaceholder;
	public GameObject player;
	public float radius;
	public bool isPlayerAbove;
	public bool isUpsideDown;

	void Start () {
		rb2d = GetComponent<Rigidbody2D>();
	}
	
	void Update () {
		CheckIfPlayerAbove();

		Vector2 gravity = Physics2D.gravity;
		

		if (gravity.y > 0 && !isUpsideDown) {
			Flip();
			isUpsideDown = true;
		}

		if (gravity.y < 0 && isUpsideDown) {
			Flip();
			isUpsideDown = false;
		}
	}

	void CheckIfPlayerAbove() {
		isPlayerAbove = Physics2D.OverlapCircle(circlePlaceholder.transform.position, radius, 1 << LayerMask.NameToLayer("GroundCheck"));
		
		if (!isPlayerAbove) {
			Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), this.GetComponent<Collider2D>(), true);
			//player.GetComponent<Rigidbody2D>().gravityScale = 2;
			//rb2d.bodyType = RigidbodyType2D.Dynamic;
		}

		else {
			Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), this.GetComponent<Collider2D>(), false);
			//player.GetComponent<Rigidbody2D>().gravityScale = 0;
			//rb2d.bodyType = RigidbodyType2D.Static;
		}
	}

	void Flip() {
		transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
	}

	private void OnTriggerEnter2D(Collider2D other) {
		
	}

	void OnDrawGizmos() {
		Gizmos.DrawWireSphere(circlePlaceholder.transform.position, radius);
		
	}
}
