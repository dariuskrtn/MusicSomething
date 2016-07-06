using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	private PlayerController player;
	private Vector3 lastPlayerPos;
	// Use this for initialization
	void Start () {
		player = FindObjectOfType<PlayerController> ();
		lastPlayerPos = player.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		float distanceX = player.transform.position.x - lastPlayerPos.x;
		float distanceY = (player.transform.position.y - transform.position.y)/15;

		transform.position = new Vector3 (transform.position.x+distanceX,
			transform.position.y+distanceY, transform.position.z);

		lastPlayerPos = player.transform.position;
	}
}
