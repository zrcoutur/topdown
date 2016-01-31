using UnityEngine;
using System.Collections;

public class Poof : MonoBehaviour {

	float timer;

	// Use this for initialization
	void Start () {

		transform.Rotate (new Vector3 (0, 0, Random.Range (0, 360)));
		timer = 0.4f;
	
	}
	
	// Update is called once per frame
	void Update () {

		timer -= Time.deltaTime;

		if (timer <= 0) {
			Destroy (gameObject);
		}
	
	}
}
