using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public Transform groundCheck;
	public float moveSpeed = 15f;
	public float jumpForce = 0.1f;
	public bool doubleJump = true;
	public KeyCode jumpKey = KeyCode.Space;
	public LayerMask groundLayer;
	public GameObject confirm;
	public bool lookingRight = true;

	private Animator cloudanim;
	public GameObject Cloud;

	private Animator anim;
	private bool onGround = false;

	private Rigidbody2D myRigidbody;
	private Collider2D myCollider;
	private bool airJumped = false;

	public GameObject hithat;
	public GameObject kick;
	public GameObject snare;

	bool running = false;
	public Queue speedChanges = new Queue();
	// Use this for initialization
	void Start () {
		myRigidbody = GetComponent<Rigidbody2D> ();
		myCollider = GetComponent<Collider2D> ();
		anim = GetComponent<Animator>();
		GetComponent<BeatDetection>().CallBackFunction = onBeat;

		StartCoroutine(StartRunning());
	}
	public void onBeat(BeatDetection.EventInfo eventInfo)
	{
		Debug.Log(eventInfo.messageInfo);
		switch (eventInfo.messageInfo)
		{
		/*
            case BeatDetection.EventType.Energy:
                StartCoroutine(showText(energy, genergy));
                break;
            */
		case BeatDetection.EventType.HitHat:
			generateLine(hithat);
			break;
		case BeatDetection.EventType.Kick:
			generateLine(kick);
			break;
		case BeatDetection.EventType.Snare:
			generateLine(snare);
			break;
		}
	}
	void FixedUpdate()
	{
		AnimationControl ();
	}
	private void generateLine(GameObject type)
	{
		Vector3 pos = new Vector3(transform.position.x, -8, 0);
		GameObject obj = Instantiate(type, pos, Quaternion.identity) as GameObject;
		GameObject.Destroy(obj, 10);
	}
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(jumpKey) && canJump()) {
			Vector3 pos = new Vector3 (transform.position.x+1, transform.position.y, transform.position.z);
			Instantiate (Cloud, pos, Quaternion.identity);
			myRigidbody.velocity = new Vector2 (myRigidbody.velocity.x, myRigidbody.velocity.y+jumpForce);
		}
	}
	public void updateVelocity(float vel)
	{
		speedChanges.Enqueue (vel);
		if (!running)
			return;
		Debug.Log (1);
		myRigidbody.velocity = new Vector2((float)speedChanges.Dequeue(), myRigidbody.velocity.y);
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

	void AnimationControl()
	{
		anim.SetFloat ("Speed", myRigidbody.velocity.x);
		onGround = Physics2D.OverlapCircle (groundCheck.position, 0.15F, groundLayer);
		anim.SetBool ("IsGrounded", onGround);
		anim.SetFloat ("vSpeed", myRigidbody.velocity.y);
	}

}
