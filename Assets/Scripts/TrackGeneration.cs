using UnityEngine;
using System.Collections;

public class TrackGeneration : MonoBehaviour, AudioProcessor.AudioCallbacks {
	public GameObject beat;
	public float moveSpeed = 15;
	public int lastObstacle = -16;

	private PlayerController player;
	AudioProcessor processor;
	Rigidbody2D myRigidBody;
	// Use this for initialization
	void Start () {
		processor = GetComponent<AudioProcessor>();
		processor.addAudioCallback(this);
		myRigidBody = GetComponent<Rigidbody2D> ();
		player = FindObjectOfType<PlayerController> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (processor.BPM () < 300)
			moveSpeed = (moveSpeed * 800 + processor.BPM ()/3) / 801;
		myRigidBody.velocity = new Vector2 (moveSpeed, 0);
		player.speedChanges.Enqueue (moveSpeed);
	}

	public void onOnbeatDetected () {
		Vector3 pos = new Vector3(transform.position.x, 0, 0);
		GameObject obj = Instantiate(beat, pos, Quaternion.identity) as GameObject;
		GameObject.Destroy (obj, 10);
	}
	public void onSpectrum(float[] spectrum)
	{
	}
}
