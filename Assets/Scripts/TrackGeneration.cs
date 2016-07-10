﻿using UnityEngine;
using System.Collections;

public class TrackGeneration : MonoBehaviour {

	public GameObject Spike, Blackground, Ground, Jump;
	public GameObject hithat;
    public GameObject kick;
    public GameObject snare;

	public float maxSpeed = 50f;
	public float minSpeed = 15f;

    public float moveSpeed = 0.1f;

	private GameObject lastObstacle;
	private GameObject lastGround;
	private GameObject lastJump;
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
		generateJumpSphere (Jump);
        switch (eventInfo.messageInfo)
        {
		case BeatDetection.EventType.HitHat:
			generateLine (hithat);
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
		lastObstacle = obj;
		lastSpeed = player.jumpForce;
		lastTime = Time.time;
	}
	private void generateObstacle(GameObject type)
	{
        // Neatsiras prie pat starto
		if (transform.position.x < 0) return;
		Vector3 pos = new Vector3(transform.position.x + (moveSpeed/5), -3.94f, 0);
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
	private float getPlayerYPrediction() {
		float totalTime = Time.time - lastTime;
		float lastPos = lastObstacle==null?-4:lastObstacle.transform.position.y;
		Rigidbody2D pl = player.GetComponent<Rigidbody2D> ();
		float prePos = lastPos + lastSpeed * totalTime + (pl.gravityScale*pl.mass*Physics2D.gravity.y*totalTime*totalTime) / 2;
		return Mathf.Max(prePos, -4f);
	}
}
