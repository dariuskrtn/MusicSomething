using UnityEngine;
using System.Collections;

public class TrackGeneration : MonoBehaviour {

	public GameObject Spike, Blackground, Ground, Jump;
	public GameObject hithat;
    public GameObject kick;
    public GameObject snare;

	public int maxJumpsInARow = 5;
	public float speedDistBeforeObstacle = 5f;
	public float maxSpeed = 50f;
	public float minSpeed = 15f;

    public float moveSpeed = 0.1f;

	private float currHeight = -5f;

	private GameObject lastSpike;
	private GameObject lastBlackground;
	public GameObject lastGround;
	private GameObject lastJump;
	private int jumpStreak = 0;

	private float lastTime = 0f;
	private float lastSpeed = 25f;

	private PlayerController player;
	Rigidbody2D myRigidbody;
	AudioSource source;

	private bool running = false;

	// Use this for initialization
	void Start () {
		player = FindObjectOfType<PlayerController> ();
		source = GetComponent<AudioSource> ();
        GetComponent<BeatDetection>().CallBackFunction = onBeat;
		StartCoroutine(StartRunning());
		StartCoroutine (checkGround());
		myRigidbody = GetComponent<Rigidbody2D> ();
    }
	void Update() {
		if (moveSpeed>minSpeed)
		moveSpeed -=0.05f;
	}
	void FixedUpdate () {
		if (!running)
			return;
        myRigidbody.velocity = new Vector2 (moveSpeed, 0);
		player.updateVelocity (moveSpeed);
	}


    public void onBeat(BeatDetection.EventInfo eventInfo)
    {
		if (moveSpeed<maxSpeed)
			moveSpeed += (maxSpeed-moveSpeed)/10f;
        switch (eventInfo.messageInfo)
        {
		case BeatDetection.EventType.HitHat:
			generateLine (hithat);
			generateJumpSphere (Jump);
                break;
        case BeatDetection.EventType.Kick:
            generateLine(kick);
            break;
		case BeatDetection.EventType.Snare:
			generateLine (snare);
			generateObstacle (Spike);
                break;
        }
    }
	private void generateJumpSphere(GameObject sphere)
	{
		// Neatsiras prie pat starto
		if (transform.position.x < 0) return;
		Vector3 pos = new Vector3(transform.position.x, getPlayerYPrediction(), 0);
		GameObject obj = Instantiate(sphere, pos, Quaternion.identity) as GameObject;
		GameObject.Destroy(obj, 10);
		lastJump = obj;
		lastSpeed = player.jumpForce;
		lastTime = Time.time;
	}
	private void generateObstacle(GameObject type)
	{
        // Neatsiras prie pat starto
		if (transform.position.x < 0) return;
		Vector3 pos = new Vector3(transform.position.x + (moveSpeed/speedDistBeforeObstacle), -3.94f, 0);
		GameObject obj = Instantiate(type, pos, Quaternion.identity) as GameObject;
		GameObject.Destroy(obj, 10);
	}

    private void generateLine(GameObject type)
    {
        Vector3 pos = new Vector3(transform.position.x, -3.94f, 0);
        GameObject obj = Instantiate(type, pos, Quaternion.identity) as GameObject;
        GameObject.Destroy(obj, 10);
    }

	IEnumerator StartRunning() {
		yield return new WaitForSeconds(0.5f);
		GetComponent<AudioSource> ().Play ();
		running = true;
	}

	IEnumerator checkGround() {
		if (lastJump == null && lastBlackground == null) {
			expandGround ();
		} else {
			float obstacle = 0f;
			if (lastJump != null)
				obstacle = lastJump.transform.position.x;
			if (lastBlackground != null) {
				obstacle = Mathf.Max (obstacle, lastBlackground.transform.position.x - lastBlackground.transform.localScale.x / 2f);
			}
			if (obstacle < lastGround.transform.position.x) {
				expandGround ();
			} else {
				createGround ();
			}
		}
		yield return new WaitForSeconds(2f);
		StartCoroutine (checkGround());
	}
	private void createGround ()
	{
		Vector3 pos = new Vector3 (transform.position.x, getPlayerYPrediction()-1f, 0f);
		GameObject obj = Instantiate (Ground, pos, Quaternion.identity) as GameObject;
		lastGround = obj;
		lastSpeed = moveSpeed;
	}
	private float getPlayerYPrediction() {
		float totalTime = Time.time - lastTime;
		float lastPos = lastJump==null?-4:lastJump.transform.position.y;
		Rigidbody2D pl = player.GetComponent<Rigidbody2D> ();
		float prePos = lastPos + lastSpeed * totalTime + (pl.gravityScale*pl.mass*Physics2D.gravity.y*totalTime*totalTime) / 2;
		return Mathf.Max(prePos, -4f);
	}
	private void expandGround()
	{
			float howMuch = transform.position.x - lastGround.transform.position.x + lastGround.transform.localScale.x / 2f;
		lastGround.transform.localScale = new Vector3 (lastGround.transform.localScale.x + howMuch, 1, 1);
		lastGround.transform.position = new Vector2 (lastGround.transform.position.x + howMuch / 2f, lastGround.transform.position.y);
	}
}
