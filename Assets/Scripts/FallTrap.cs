using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallTrap : MonoBehaviour {

	private void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.tag == "Player") {
			this.GetComponent<Animator>().enabled = true;
			this.GetComponent<BoxCollider2D>().isTrigger = true;
		}
	}
}
