using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

	Rigidbody2D rb2d;
	public bool isUpsideDown;
	bool isFacingRight = true;
	bool isGrounded;
	float oxygenLevel;
	float maxSpeed;
	float maxJumpForce;

	public GameController gameController;
	public GameObject switchTrigger;

	public Slider oxygenSlider;

	public Transform groundCheckPos;
	public LayerMask whatIsGround;

	public bool isGameOver;
	public float speed;
	public float jumpForce;
	public float oxygenMaxLevel;
	public float circleRadius;

	void Awake () {
		// Applying references
		rb2d = GetComponent<Rigidbody2D>();
	}

	void Start() {
		oxygenLevel = oxygenMaxLevel;
		oxygenSlider.maxValue = oxygenLevel;
		oxygenSlider.value = oxygenLevel;	

		maxSpeed = speed;
		maxJumpForce = jumpForce;

		StartCoroutine(DecreaseOxygen());
	}
	
	void Update () {
		GroundCheck();

		// Checking for user input and if grounded
		if (Input.GetButtonDown("Jump") && isGrounded) {
			// Checking if not upside down
			if (!isUpsideDown) {
				Jump (Vector2.up);
			}
			else {
				Jump (Vector2.down);
			}
		}
	}

	void FixedUpdate () {
		Move (Input.GetAxisRaw("Horizontal"));
	}

	void Move (float horizontalInput) {
		// Assigning rigidbody's velocity to a Vector2
		Vector2 movement = rb2d.velocity;

		// Setting the x axis of our vector
		movement.x = horizontalInput * speed * Time.fixedDeltaTime;

		// Applying the vector to the rigidbody's velocity
		rb2d.velocity = movement;

		// Checking if moving right and facing left
		if (horizontalInput > 0 && !isFacingRight) {
			// Flip the sprite on the x axis
			Flip (true, false);
		}
		// Checking if moving left and facing right
		else if (horizontalInput < 0 && isFacingRight) {
			// Flip the sprite on the x axis
			Flip (true, false);
		}
	}

	// Direction can be applyied upwards or downwards
	void Jump (Vector2 direction) {
		// Applying variables to the rigidbody's velocity
		rb2d.velocity = direction * jumpForce * Time.fixedDeltaTime;
	}

	void GravitySwitch () {
		// Saving our current gravity scale
		float gravity = rb2d.gravityScale;

		// Applying its reverse value
		rb2d.gravityScale = -gravity;

		// Switching the boolean
		isUpsideDown = !isUpsideDown;

		// Flipping the sprite on both axis
		//Flip (true, true);
	}

	void TotalGravitySwitch() {
		/* Vector2 gravity = Physics2D.gravity;

		gravity.y *= -1; */

		Physics2D.gravity = new Vector2(0, -Physics2D.gravity.y);

		isUpsideDown = !isUpsideDown;
	}

	// Taking in booleans to decide which axis to flip
	public void Flip (bool xAxis, bool yAxis) {
		// Saving our current scale
		Vector3 scale = transform.localScale;
		
		// Setting the scale to its inverse
		if (xAxis) {
			isFacingRight = !isFacingRight;
			scale.x *= -1;
		}
		if (yAxis) {
			scale.y *= -1;
		}

		// Applying the new scale
		transform.localScale = scale;
	}

	IEnumerator DecreaseOxygen() {
		while (oxygenLevel >= 0) {
			oxygenLevel = oxygenMaxLevel - Time.timeSinceLevelLoad;
			oxygenSlider.value = oxygenLevel;
			yield return null;
		}
		isGameOver = true;
		Destroy(GetComponent<BoxCollider2D>());
		Destroy(this);
	}

	void GroundCheck() {
		isGrounded = Physics2D.OverlapCircle(groundCheckPos.position, circleRadius, 1 << LayerMask.NameToLayer("Ground"));
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "GravitySwitch") {
			//GravitySwitch();
			TotalGravitySwitch();
			switchTrigger.SetActive(true);
		}

		if (other.tag == "Switch") {
			Flip(false, true);
			switchTrigger.SetActive(false);
		}
	}

	private void OnCollisionStay2D(Collision2D other) {
		if (other.gameObject.tag == "Box") {
			Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), other.collider);
		}
	}

	/* private void OnDrawGizmos() {
		Gizmos.DrawWireSphere(groundCheckPos.position, circleRadius);
	} */
}
