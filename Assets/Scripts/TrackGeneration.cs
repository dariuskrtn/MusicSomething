using UnityEngine;
using System.Collections;

public class TrackGeneration : MonoBehaviour {
	public GameObject hithat;
    public GameObject kick;
    public GameObject snare;
    public float moveSpeed = 0.1f;
	public int lastObstacle = -16;

	private PlayerController player;
	Rigidbody2D myRigidbody;
	private bool running = false;
	// Use this for initialization
	void Start () {
		player = FindObjectOfType<PlayerController> ();

        GetComponent<BeatDetection>().CallBackFunction = onBeat;
		StartCoroutine(StartRunning());
		myRigidbody = GetComponent<Rigidbody2D> ();
    }

	void FixedUpdate () {
		if (!running)
			return;
        //if (processor.BPM () < 300)
        //moveSpeed = (moveSpeed * 800 + processor.BPM ()/3) / 801;
        //moveSpeed = (moveSpeed * 800 + 60 / 3) / 801;
		moveSpeed +=0.01f;
		//transform.
        myRigidbody.velocity = new Vector2 (moveSpeed, 0);
		player.updateVelocity (moveSpeed);
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

    private void generateLine(GameObject type)
    {
        Vector3 pos = new Vector3(transform.position.x, 8, 0);
        GameObject obj = Instantiate(type, pos, Quaternion.identity) as GameObject;
        GameObject.Destroy(obj, 10);
    }

	IEnumerator StartRunning() {
		yield return new WaitForSeconds(0.5f);
		GetComponent<AudioSource> ().Play ();
		running = true;
	}
}
