using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

	public Transform groundCheck;
	public float moveSpeed = 15f;
	public float jumpForce = 0.1f;
	public KeyCode jumpKey = KeyCode.Space;
	public LayerMask groundLayer, jumpSphereLayer;
	public GameObject confirm;
    public GameObject Cloud;

	private Animator anim;
	private bool onGround = false;

	private Rigidbody2D myRigidbody;
	private Collider2D myCollider;

	private bool airJumped = false;
    public bool lookingRight = true;
    public bool doubleJump = true;
    private bool dead = false;

	bool running = false;
	public Queue speedChanges = new Queue();

	// Use this for initialization
	void Start () {
		myRigidbody = GetComponent<Rigidbody2D> ();
		myCollider = GetComponent<Collider2D> ();
		anim = GetComponent<Animator>();

		StartCoroutine(StartRunning());
	}

	void FixedUpdate()
	{
		AnimationControl ();
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(jumpKey) && canJump()) {
			Vector3 pos = new Vector3 (transform.position.x+1, transform.position.y, transform.position.z);
			Instantiate (Cloud, pos, Quaternion.identity);
			myRigidbody.velocity = new Vector2 (myRigidbody.velocity.x, Mathf.Min(myRigidbody.velocity.y+jumpForce, jumpForce));
		}
	}

	public void updateVelocity(float vel)
	{
		speedChanges.Enqueue (vel);
		if (!running)
			return;
		myRigidbody.velocity = new Vector2((float)speedChanges.Dequeue(), myRigidbody.velocity.y);
	}

	bool canJump() {
        if (dead || !running) return false;
		if (Physics2D.OverlapCircle (groundCheck.position, 0.15F, groundLayer)) {
			airJumped = false;
			return true;
		}
		if (Physics2D.IsTouchingLayers (myCollider, jumpSphereLayer)) {
			myRigidbody.velocity = new Vector2 (myRigidbody.velocity.x, 0);
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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Deadly")
        {
            if (dead) return;
            transform.FindChild("Particle System").gameObject.SetActive(false);
            ParticleSystem pr = GetComponent<ParticleSystem>();
            pr.Emit(150);
            transform.FindChild("Textures").gameObject.SetActive(false);
            dead = true;
            StartCoroutine(LoadLevelRoutine());
        }
    }

    IEnumerator LoadLevelRoutine()
    {
        yield return new WaitForSeconds(2);
		transform.FindChild ("Textures").gameObject.SetActive (true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
