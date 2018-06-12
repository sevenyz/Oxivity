using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	GameObject player;
	//Vector3 speed;

	public float smoothTime;

	void Awake() {
		// Player reference
		player = GameObject.FindGameObjectWithTag("Player");
	}

	void LateUpdate() {	
		//Vector3 newPos = Vector3.SmoothDamp (transform.position, player.transform.position, ref speed, smoothTime);

		// Lerping to the player's position by smoothTime
		Vector3 newPos = Vector3.Lerp (transform.position, player.transform.position, smoothTime * Time.fixedDeltaTime);

		// Constraining position values
		newPos.y = 0;
		newPos.z = -10;
		newPos.x = Mathf.Clamp (newPos.x, -.8f, .8f);

		// Appliying the new position
		transform.position = newPos;
	}
}
