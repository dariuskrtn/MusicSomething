using UnityEngine;
using System.Collections;

public class TrackGeneration : MonoBehaviour {
	public GameObject hithat;
    public GameObject kick;
    public GameObject snare;
    public GameObject AudioBeat;
    public float moveSpeed = 15;
	public int lastObstacle = -16;

	private PlayerController player;
	AudioProcessor processor;
	Rigidbody2D myRigidBody;

	// Use this for initialization
	void Start () {
		myRigidBody = GetComponent<Rigidbody2D> ();
		player = FindObjectOfType<PlayerController> ();

        AudioBeat.GetComponent<BeatDetection>().CallBackFunction = onBeat;
    }
	
	// Update is called once per frame
	void Update () {
        //if (processor.BPM () < 300)
        //moveSpeed = (moveSpeed * 800 + processor.BPM ()/3) / 801;
        moveSpeed = (moveSpeed * 800 + 60 / 3) / 801;
        myRigidBody.velocity = new Vector2 (moveSpeed, 0);
		player.speedChanges.Enqueue (moveSpeed);
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
        Vector3 pos = new Vector3(transform.position.x, 0, 0);
        GameObject obj = Instantiate(type, pos, Quaternion.identity) as GameObject;
        GameObject.Destroy(obj, 10);
    }
}
