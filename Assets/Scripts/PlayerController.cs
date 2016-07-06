using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public float moveSpeed = 15f;
	public float jumpForce = 0.1f;
	public bool doubleJump = true;
	public KeyCode jumpKey = KeyCode.Space;
	public LayerMask groundLayer;

	private Rigidbody2D myRigidbody;
	private Collider2D myCollider;
	private bool airJumped = false;

	AudioProcessor processor;
	bool running = false;
	// Use this for initialization
	void Start () {
		myRigidbody = GetComponent<Rigidbody2D> ();
		myCollider = GetComponent<Collider2D> ();

		processor = FindObjectOfType<AudioProcessor>();
		StartCoroutine(StartRunning());
	}
	
	// Update is called once per frame
	void Update () {
		if (!running)
			return;
		
		if (processor.BPM () < 300)
			moveSpeed = (moveSpeed * 800 + processor.BPM ()/3) / 801;
		myRigidbody.velocity = new Vector2 (moveSpeed, myRigidbody.velocity.y); // Palaiko vienoda.

		if (Input.GetKeyDown(jumpKey) && canJump()) {
			myRigidbody.velocity = new Vector2 (myRigidbody.velocity.x, myRigidbody.velocity.y+jumpForce);
		}
	}
	bool canJump() {
		if (Physics2D.IsTouchingLayers (myCollider, groundLayer)) {
			airJumped = false;
			return true;
		}
		if (doubleJump && !airJumped) {
			airJumped = true;
			return true;
		}
		return false;
	}
	IEnumerator StartRunning() {
		yield return new WaitForSeconds(5);
		GetComponent<AudioSource> ().Play ();
		running = true;
	}
}
