using UnityEngine;
using System.Collections;

public class Spectrum : MonoBehaviour, AudioProcessor.AudioCallbacks {
	
	public GameObject prefab;
	public GameObject teksta;
	public int numberOfObjects = 20;
	public float radius = 5f;
	GameObject[] cubes;
	public int Samples = 2048;
	public int kuris = 3;
	public int size = 1;
	AudioProcessor processor;
	// Use this for initialization
	void Start () {
		for (int i = 0; i < numberOfObjects; i++) {
			float angle = i * Mathf.PI * 2 / numberOfObjects;
			Vector3 pos = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
			Instantiate(prefab, pos, Quaternion.identity);
			pos.y = 2;
			GameObject obj = Instantiate(teksta, pos, Quaternion.identity) as GameObject;
			obj.GetComponent<TextMesh> ().text = i + "";
		}
		cubes = GameObject.FindGameObjectsWithTag ("Cube");

		processor = FindObjectOfType<AudioProcessor>();
		processor.addAudioCallback(this);

		GameObject obje = Instantiate(teksta, new Vector3(0, 2, 0), Quaternion.identity) as GameObject;
		obje.GetComponent<TextMesh> ().text = "PIM";
		obje.name = "tekstas";
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log (processor.getAutoco(size).avgBpm());
		float[] spectrum = AudioListener.GetSpectrumData (Samples, 0, FFTWindow.Hamming);
		for (int i = 0; i < numberOfObjects; i++) {
			Vector3 previousScale = cubes [i].transform.localScale;
			previousScale.y = spectrum [i] * 20;
			cubes [i].transform.localScale = previousScale;
		}

	}
	public void onOnbeatDetected () {
		processor.changeCameraColor ();
		if (GameObject.Find ("naujas")!=null)
			Object.Destroy (GameObject.Find ("naujas"));
		GameObject obj = GameObject.Find ("tekstas");
		obj.GetComponent<TextMesh> ().text = obj.GetComponent<TextMesh> ().text.Equals ("PIM") ? "PAM" : "PIM";
		Color color = new Color (Random.value, Random.value, Random.value);
		MeshRenderer mesh = prefab.GetComponent<MeshRenderer> ();

		Material mat = new Material (Shader.Find("Standard"));
		mat.color = color;
		mesh.material = mat;
		Object midd = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
		midd.name = "naujas";
	}
	public void onSpectrum(float[] spectrum)
	{
		//The spectrum is logarithmically averaged
		//to 12 bands

		for (int i = 0; i < spectrum.Length; ++i)
		{
			Vector3 start = new Vector3(i, 0, 0);
			Vector3 end = new Vector3(i, spectrum[i], 0);
			Debug.DrawLine(start, end);
		}
	}
}