using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

	[HideInInspector] public float maxSpeed;
	[HideInInspector] public float maxJumpForce;

	Animator animator;
	Rigidbody2D rb2d;
	GameObject fallTrap;
	
	public bool isFacingRight = true;
	bool isGrounded;
	float oxygenLevel;
	float o2MaxTankValue;
	
	public GameController gameController;
	public List <GameObject> switchTrigger;
	public Slider oxygenSlider;
	public Transform groundCheckPos;
	public Transform front;
	public LayerMask whatIsGround;
	public Text restoredO2;

	public AudioSource jumpSound;
	public AudioSource o2TankPickUpSound;
	public AudioSource gravitySwap;
	public AudioSource step;
	public AudioSource land;

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
		
		land.volume = 0;
	}
	
	void Update () {
		GroundCheck();

		if (Time.timeScale != 0) {

			// Checking for user input and if grounded
			if (Input.GetButtonDown("Jump")) {
				// Checking for current orientation
				if (!isUpsideDown) {
					Jump (Vector2.up);
				}
				else {
					Jump (Vector2.down);
				}
			}

			if (Input.GetAxisRaw("Horizontal") != 0) {
				animator.SetBool("isMoving", true);
				ControlStepSound();
			}
			else {
				animator.SetBool("isMoving", false);
			}

			if (oxygenLevel <= 0) {
				isGameOver = true;
				animator.SetBool("isChoking", true);
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

	// Jump can be applyied upwards or downwards by setting the desired direction
	void Jump (Vector2 direction) {
		// Applying variables to the rigidbody's velocity
		if (isGrounded) {
			jumpSound.Play();
			rb2d.velocity = direction * jumpForce * Time.fixedDeltaTime;
		}
	}

	// Only applies to this rigidbody
	void GravitySwitch () {
		// Saving our current gravity scale
		float gravity = rb2d.gravityScale;

		// Applying its reverse value
		rb2d.gravityScale = -gravity;

		// Switching the boolean
		isUpsideDown = !isUpsideDown;

		gravitySwap.Play();
	}

	// Applies to every gameobject with a dynamic rigidbody
	void TotalGravitySwitch() {
		gravitySwap.Play();
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
		while (oxygenLevel >= 0 && !gameController.levelCompleted) {
			// Decreasing our current o2 level by 1 every second and updating it's visual state
			oxygenLevel = oxygenMaxLevel - Time.timeSinceLevelLoad;
			oxygenSlider.value = oxygenLevel;

			yield return null;
		}
	}

	void GroundCheck() {
		isGrounded = Physics2D.OverlapCircle(groundCheckPos.position, circleRadius, 1 << LayerMask.NameToLayer("Ground"));

		if (!isGrounded) {
			animator.SetBool("isJumping", true);
		} else {
			animator.SetBool("isJumping", false);
		}
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "GravitySwitch") {
			GravitySwitch();

			foreach (GameObject trig in switchTrigger) {
				if (trig != null) {
					trig.SetActive(true);
				}
			}
		}

		if (other.tag == "O2Tank") {
			o2TankPickUpSound.Play();

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

		if (other.tag == "Death") {
			
			isGameOver = true;
			animator.SetBool("isChoking", true);
			Destroy(this);
		}
	}

	private void OnTriggerExit2D(Collider2D other) {
		if (other.tag == "Switch") {
			Flip(false, true);
			
			foreach (GameObject trig in switchTrigger) {
				if (trig != null) {
					trig.SetActive(false);
				}
			}
		}
	}

	private void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.tag == "FallTrap") {

			fallTrap = other.gameObject;
			StartCoroutine(FallTrap());
		}
	}

	IEnumerator FallTrap() {
		yield return new WaitForSeconds(2);

		float counter = 50;
		while (counter != 0) {

			fallTrap.transform.Translate(Vector2.down * 5 * Time.deltaTime);
			counter --;
			
			if (counter == 0) {
				break;
			}

			yield return null;
		}
	}

	IEnumerator LowerVolume() {
		land.Play();

		yield return new WaitForSeconds(.1f);

		land.volume = 0;
		land.Stop();
	}

	void ControlStepSound() {
		if (isGrounded) {
			if (!step.isPlaying) {
				step.Play();
			}
		}
	}

	void ControlLandSound() {
		if (isGrounded) {
			if (!land.isPlaying) {
				StartCoroutine(LowerVolume());
			}
		}
		else {
			land.volume = .1f;
			land.Stop();
		}
	}

	// Only for debug purposes
	void OnDrawGizmos() {
		Gizmos.DrawWireSphere(groundCheckPos.position, circleRadius);
		//Gizmos.DrawWireSphere(front.position, frontRadius);
	}
}
