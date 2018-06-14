using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTest : MonoBehaviour {

	public GameObject laser;

	public bool isActive;
	public float angle;

	void Update () {
		if (isActive) {
			laser.GetComponent<SpriteRenderer>().enabled = true;
		} else {
			laser.GetComponent<SpriteRenderer>().enabled = false;
		}
	}
}
