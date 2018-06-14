using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour {

	AreaEffector2D effector;

	public Vector2 direction;
	
	public float movementSpeed;
	public float timeToChangeDirection;
	
	void Start () {
		StartCoroutine(ChangeDirection());

		effector = GetComponentInChildren<AreaEffector2D>();
	}

	private void Update() {
		transform.Translate(direction * movementSpeed * Time.deltaTime);

		if (direction.x > 0) {
			effector.forceAngle = 360;
		}
		if (direction.x < 0) {
			effector.forceAngle = 180;
		}
	}

	IEnumerator ChangeDirection() {
		while (true) {
			yield return new WaitForSeconds(timeToChangeDirection);
			direction *= -1;
		}
	}
}
