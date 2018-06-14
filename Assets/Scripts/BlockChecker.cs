﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockChecker : MonoBehaviour {

	Player player;

	void Start () {
		player = GetComponentInParent<Player>();
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Block") {
			player.speed = 0;
		}

		if (other.tag == "GravitySwitch") {
			Physics2D.IgnoreCollision(this.GetComponent<BoxCollider2D>(), other);
		}
	}

	private void OnTriggerExit2D(Collider2D other) {
		if (other.tag == "Block") {
			player.speed = player.maxSpeed;
		}
	}
}
