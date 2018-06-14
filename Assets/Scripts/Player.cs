using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

	[HideInInspector] public float maxSpeed;
	[HideInInspector] public float maxJumpForce;

	Animator animator;
	Rigidbody2D rb2d;
	
	bool isFacingRight = true;
	bool isGrounded;
	float oxygenLevel;
	float o2MaxTankValue;
	
	public GameController gameController;
	public GameObject switchTrigger;
	public Slider oxygenSlider;
	public Transform groundCheckPos;
	public Transform front;
	public LayerMask whatIsGround;
	public Text restoredO2;

	public AudioSource jumpSound;

	public bool isGameOver;
	public bool isUpsideDown;
	public float speed;
	public float jumpForce;
	public float jumpForceWhileGrab;
	public float oxygenMaxLevel;
	public float o2TankValue;
	public float circleRadius;

	void Awake () {
		// Applying references
		rb2d = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
	}

	void Start() {
		// Assigning placeholder variables' value
		oxygenLevel = oxygenMaxLevel;
		oxygenSlider.maxValue = oxygenLevel;
		oxygenSlider.value = oxygenLevel;

		o2MaxTankValue = o2TankValue;	

		maxSpeed = speed;
		maxJumpForce = jumpForce;

		StartCoroutine(DecreaseOxygen());
	}
	
	void Update () {
		GroundCheck();

		// Checking for user input and if grounded
		if (Input.GetButtonDown("Jump") && isGrounded) {
			jumpSound.Play();
			// Checking for current orientation
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

	// Jump can be applyied upwards or downwards setting the desired direction
	void Jump (Vector2 direction) {
		// Applying variables to the rigidbody's velocity
		rb2d.velocity = direction * jumpForce * Time.fixedDeltaTime;
	}

	// Only applies to this rigidbody
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

	// Applies to every gameobject with a dynamic rigidbody
	void TotalGravitySwitch() {
		// Reversing the current gravity scale on the y axis
		Physics2D.gravity = new Vector2(0, -Physics2D.gravity.y);

		isUpsideDown = !isUpsideDown;
	}

	// Taking in booleans to decide which axis to flip
	public void Flip (bool xAxis, bool yAxis) {
		// Saving our current scale
		Vector3 scale = transform.localScale;
		
		// Setting the axis scale to its inverse
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
			// Decreasing our current o2 level by 1 every second and updating it's visual state
			oxygenLevel = oxygenMaxLevel - Time.timeSinceLevelLoad;
			oxygenSlider.value = oxygenLevel;

			yield return null;
		}

		// Gameover effects
		isGameOver = true;
		Destroy(GetComponent<BoxCollider2D>());
		Destroy(this);
	}

	void GroundCheck() {
		isGrounded = Physics2D.OverlapCircle(groundCheckPos.position, circleRadius, 1 << LayerMask.NameToLayer("Ground"));
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "GravitySwitch") {
			TotalGravitySwitch();
			switchTrigger.SetActive(true);
		}

		if (other.tag == "Switch") {
			Flip(false, true);
			switchTrigger.SetActive(false);
		}

		if (other.tag == "O2Tank") {
			// Checking if the sum of our current o2 and the o2 tank is greater than our max o2 capacity
			if (o2TankValue + oxygenLevel > oxygenMaxLevel) {
				// Refilling only for our available o2 capacity
				o2TankValue =  Mathf.Abs(oxygenLevel - oxygenMaxLevel);
				oxygenMaxLevel += o2TankValue;
			}

			else {
				// Refilling for the full o2 tank value
				oxygenMaxLevel += o2TankValue;
			}
			// Refill text pop-up
			restoredO2.text = "+" +  o2MaxTankValue.ToString();

			restoredO2.gameObject.SetActive(true);
			Destroy(other.gameObject);
		}
	}

	// Only for debug purposes
	void OnDrawGizmos() {
		Gizmos.DrawWireSphere(groundCheckPos.position, circleRadius);
		//Gizmos.DrawWireSphere(front.position, frontRadius);
	}
}
