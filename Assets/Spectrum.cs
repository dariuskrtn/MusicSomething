using UnityEngine;
using System.Collections;

public class Spectrum : MonoBehaviour {
	
	public GameObject prefab;
	public int numberOfObjects = 20;
	public float radius = 5f;
	public AudioClip audi;
	GameObject[] cubes;
	public int Samples = 2048;
	public int kuris = 3;
	public float size = 1;
	// Use this for initialization
	void Start () {
		for (int i = 0; i < numberOfObjects; i++) {
			float angle = i * Mathf.PI * 2 / numberOfObjects;
			Vector3 pos = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
			Instantiate(prefab, pos, Quaternion.identity);
		}
		cubes = GameObject.FindGameObjectsWithTag ("Cube");
	}
	
	// Update is called once per frame
	void Update () {
		float[] spectrum = AudioListener.GetSpectrumData (Samples, 0, FFTWindow.Hamming);
		for (int i = 0; i < numberOfObjects; i++) {
			Vector3 previousScale = cubes [i].transform.localScale;
			previousScale.y = spectrum [i] * 20;
			cubes [i].transform.localScale = previousScale;
			if (previousScale.y > size && i==kuris) {
				bum ();
				Debug.Log (i + "");
			}
		}
	}
	void bum () {
		if (GameObject.Find ("naujas")!=null)
			Object.Destroy (GameObject.Find ("naujas"));
		Color color = new Color (Random.value, Random.value, Random.value);
		MeshRenderer mesh = prefab.GetComponent<MeshRenderer> ();

		Material mat = new Material (Shader.Find("Standard"));
		mat.color = color;
		mesh.material = mat;
		Object midd = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
		midd.name = "naujas";
	}
}