using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public float moveSpeed = 15f;
	public float jumpForce = 0.1f;
	public bool doubleJump = true;
	public KeyCode jumpKey = KeyCode.Space;
	public LayerMask groundLayer;
	public GameObject confirm;

	private Rigidbody2D myRigidbody;
	private Collider2D myCollider;
	private bool airJumped = false;

	bool running = false;
	// Use this for initialization
	void Start () {
		myRigidbody = GetComponent<Rigidbody2D> ();
		myCollider = GetComponent<Collider2D> ();

		StartCoroutine(StartRunning());
	}
	
	// Update is called once per frame
	void Update () {
		if (!running)
			return;
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
		yield return new WaitForSeconds(0);
		GetComponent<AudioSource> ().Play ();
		running = true;
	}
}
